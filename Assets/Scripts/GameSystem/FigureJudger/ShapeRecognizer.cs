/*
The MIT License (MIT)
*/

#define DEBUG

using UnityEngine;
using System.Collections.Generic;

public class ShapeRecognizer : MonoBehaviour
{
    public enum ShapeType { Circle, Triangle, Rectangle, Star }
    public struct Shape
    {
        public ShapeType     type;
        public Vector3       position;
        public Vector3       normal;
        public Vector3       up;
        public Quaternion    rotation;
        public Vector3       scale;
        public List<Vector3> trail;
    }

    #region [Public Parameters]
    public float limitDrawTime             = 3.0f;    // 図形のタイマー（秒）

    public float minCircleError            = 0.2f;  // 円の誤差の総和の許容値（大きいほど適当な図形でも反応する）
    public float minCircleRadius           = 0.12f;  // 円と認識する最小半径

    public float minPolygonalSideLength    = 0.2f;   // 多角形の辺の最小値
    public int   stopPointJudgePointNum    = 4;      // 停止点と判断するための速度を算出する点数
    public float stopPointMaxVelocity      = 0.006f; // これ以下の速度ならば停止点と認識する

    public int   sharpAngleJudgePointNum   = 6;      // 鋭角認識のための辺と判断する点数（実際はこの４倍の点を見る）
    public float sharpAngleJudgeSideLength = 0.05f;

    public float closeJudgeDistance        = 0.2f;   // 図形が終端したと判断する誤差
    public float adjacentDistanceThreshold = 0.05f;  // 前回の距離との差が小さい時は円の判定を除外（軽量化のため）

    public Transform target;
    public mouse mouse;
    #endregion


    #region [EventHandlers]
    public delegate void ShapeRecognizedEventHandler(Shape shape);
    public delegate void VertexDetectedEventHandler(Vector3 position);

    public event ShapeRecognizedEventHandler onShapeDetected  = shape => {};
    public event VertexDetectedEventHandler  onVertexDetected = pos   => {};
    #endregion


    #region [Private Parameters]
    private List<Vector2> positions_    = new List<Vector2>();
    private List<Vector3> vertexPoints_ = new List<Vector3>();
    private int skipCountForSharpAngleDetection_ = 0;
    public bool IsJudging = false;
    public float drawingTimer = 0.0f;
    #endregion


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            IsJudging = true;
            mouse.start();
        }
        if (Input.GetKey(KeyCode.Mouse0)){
            drawingTimer += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
	        if (DetectCircleGesture()/* || 他の検出()*/) {
	        }
            Reset();
        }
	}

    void FixedUpdate()
    {
        if (IsJudging && drawingTimer < limitDrawTime) {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            positions_.Insert(0, new Vector2(mousePos.x , mousePos.y));
        }
    }

    void Reset()
    {
        IsJudging = false;
        drawingTimer = 0.0f;
        positions_.Clear();
        vertexPoints_.Clear();
        mouse.end();
    }


    // 円の検出
    // 円はすべての点の平均点（= 中心）から各点の距離（= 半径）が一定という特性を使って検出
    bool DetectCircleGesture()
    {

        // NOTE: 軽量化のためにスキップしても良いかも
        int i = positions_.Count;
        // 図形の終端検出
        var distanceBetweenFirstAndLastPoint = Vector2.Distance(positions_[i == 0 ? 0 : i - 1], positions_[0]);
        if (distanceBetweenFirstAndLastPoint > closeJudgeDistance) return false;
        
        // すべての点の位置の平均（円であれば円の中心点）
        var positionSum = Vector2.zero;
        for (int j = 0; j < i; ++j) {
            positionSum += positions_[j];
        }
        var meanPosition = positionSum / (i + 1);
        Log(positions_[1] + " " + positions_[0]);

        // すべての点それぞれの点と上記平均点との距離の平均（円であれば半径）
        var meanDistanceSum = 0f;
        for (int j = 0; j < i; ++j) {
            meanDistanceSum += Vector2.Distance(positions_[j], meanPosition);
        }
        var meanDistance = meanDistanceSum / i;
        //[Debug] 中心点を表示
        target.position = new Vector3(meanPosition.x , meanPosition.y , 0);


        // 各平均点との距離の誤差を正規化して足し合わせた総和
        var error = 0f;
        for (int j = 0; j < i; ++j) {
            error += Mathf.Abs(Vector2.Distance(positions_[j], meanPosition) - meanDistance) / meanDistance;
        }
        error /= i;

        // 誤差の総和が許容値以下で、半径が最低サイズよりも大きかったら円として認識
        if (error < minCircleError && meanDistance > minCircleRadius) {
            Log("精度: "+error+ " 半径: "+meanDistance);

            // イベントハンドラを呼ぶ

            // 過去の点履歴を消去
            Reset();

            return true;
        }
        
        return false;
    }

    // 多角形認識
    // 停止点と鋭角点を頂点としてつないだ辺がなす角度を見ることで判断
    bool DetectPolygonGesture()
    {
        // 頂点の追加
        if (DetectSharpAnglePoint() || DetectStopPoint()) {
            // いずれかの図形に当てはまったら過去の点の履歴を消去して認識終了
            if (DetectTriangleGesture() || DetectRectangleGesture() || DetectStarGesture()) {
                Reset();
                return true;
            }
        }

        return false;
    }


    // 過去の点を見てある範囲の最大変化角度が閾値を超えて且つ最大になる場所を見つける
    bool DetectSharpAnglePoint()
    {
        if (--skipCountForSharpAngleDetection_ >= 0) return false;

        var n = sharpAngleJudgePointNum;
        if (positions_.Count >= n * 4) {
            // 適当な間隔を開けた２辺
            var v0 = positions_[n - 1]     - positions_[n * 2 - 1];
            var v1 = positions_[n * 3 - 1] - positions_[n * 2];

            // それぞれの辺が閾値よりも長く、かつ直線でないと判断（なす角が最大角以下）したらそこを頂点とみなす
            if (v0.magnitude > sharpAngleJudgeSideLength && 
                v1.magnitude > sharpAngleJudgeSideLength &&
                Mathf.Abs(Vector3.Angle(v0, v1)) < 110) {
                // 閾値を超えた点の前後で最小となる角を探す
                var minAngle = 180f;
                var sharpestAnglePosition = Vector3.zero;
                for (var i = 0; i < sharpAngleJudgePointNum - 1; ++i) {
                    var v2 = positions_[i]             - positions_[i + n];
                    var v3 = positions_[i + n * 2 + 1] - positions_[i + n + 1];
                    var ang = Mathf.Abs(Vector3.Angle(v2, v3));
                    if (ang < minAngle) {
                        minAngle = ang;
                        sharpestAnglePosition = (positions_[i + n] + positions_[i + n + 1]) / 2;
                    }
                }

                if (AddVertex(sharpestAnglePosition)) {
                    skipCountForSharpAngleDetection_ = n;
                    return true;
                }
            }
        }

        return false;
    }


    // 停止点認識
    // 単純に速度が閾値以下の場所を探しているだけ
    bool DetectStopPoint()
    {
        if (positions_.Count < stopPointJudgePointNum) return false;

        // 直近の点の移動距離が閾値以下であれば頂点とみなす
        var p0 = positions_[0];
        var p1 = positions_[stopPointJudgePointNum - 1];
        var v = (p0 - p1).magnitude / stopPointJudgePointNum;
        if (v < stopPointMaxVelocity) {
            var stopPosition = (p0 + p1) / 2;
            return AddVertex(stopPosition);
        }
        return false;
    }


    // 新しい頂点が直前に保存された頂点と一定距離離れた点であれば保存する
    bool AddVertex(Vector3 position)
    {
        if (vertexPoints_.Count == 0 || Vector3.Distance(vertexPoints_[0], position) > 0.1f) {
            onVertexDetected(position);
            vertexPoints_.Insert(0, position);
            return true;
        }
        return false;
    }
    
    // 三角形認識
    // MEMO: サイズは円に内接する三角形の一辺の長さを返す
    bool DetectTriangleGesture()
    {
        if (vertexPoints_.Count < 4) return false;
        var p1 = vertexPoints_[0];
        var p2 = vertexPoints_[1];
        var p3 = vertexPoints_[2];
        var p4 = vertexPoints_[3];

        // 始点と終点が大体同じ位置
        if (Vector3.Distance(p1, p4) < closeJudgeDistance) {
            // 各頂点の角度が大体 60 度付近
            const float minError = 15f;
            var ang1 = Vector3.Angle(p2 - p1, p2 - p3);
            var ang2 = Vector3.Angle(p3 - p2, p3 - p1);
            var ang3 = Vector3.Angle(p1 - p3, p1 - p2);
            var length = (p2 - p1).magnitude;
            if (Mathf.Abs(ang1 - 60) < minError &&
                Mathf.Abs(ang2 - 60) < minError && 
                Mathf.Abs(ang3 - 60) < minError &&
                length > minPolygonalSideLength) {
                var center = (p1 + p2 + p3) / 3;
                var normal = Vector3.Cross(p1 - p3, p3 - p2);
                if (Vector3.Dot(normal, Camera.main.transform.forward) < 0) {
                    normal *= -1;
                }
                var scale = ((p2 - p1).magnitude + (p3 - p2).magnitude + (p1 - p3).magnitude) / 3 * 2 / Mathf.Sqrt(3) * Vector3.one;
                var up = FindBestFitUpAxis(3, (p1 - center).normalized, (p2 - center).normalized, normal);
                var trail = new List<Vector3>() {p1, p2, p3};
                onShapeDetected(new Shape() {
                    type = ShapeType.Triangle,
                    position = center,
                    rotation = Quaternion.LookRotation(normal, up),
                    normal = normal,
                    up = up,
                    scale = scale,
                    trail = trail
                });
                return true;
            }
        }

        return false;
    }

    // 四角形認識
    // MEMO: サイズは描いたスケールをそのまま返す
    bool DetectRectangleGesture()
    {
        if (vertexPoints_.Count < 5) return false;
        var p1 = vertexPoints_[0];
        var p2 = vertexPoints_[1];
        var p3 = vertexPoints_[2];
        var p4 = vertexPoints_[3];
        var p5 = vertexPoints_[4];

        // 始点と終点が大体同じ位置
        if (Vector3.Distance(p1, p5) < closeJudgeDistance) {
            // 各頂点の角度が大体 90 度付近
            const float minError = 20f;
            var ang1 = Vector3.Angle(p2 - p1, p2 - p3);
            var ang2 = Vector3.Angle(p3 - p2, p3 - p4);
            var ang3 = Vector3.Angle(p4 - p3, p4 - p1);
            var ang4 = Vector3.Angle(p1 - p4, p1 - p2);
            var length = (p2 - p1).magnitude;
            if (Mathf.Abs(ang1 - 90) < minError &&
                Mathf.Abs(ang2 - 90) < minError && 
                Mathf.Abs(ang3 - 90) < minError && 
                Mathf.Abs(ang4 - 90) < minError &&
                length > minPolygonalSideLength) {
                var center = (p1 + p2 + p3 + p4) / 4;
                var normal = Vector3.Cross(p4 - p3, p3 - p2).normalized;
                if (Vector3.Dot(normal, Camera.main.transform.forward) < 0) {
                    normal *= -1;
                }
                var scale = ((p2 - p1).magnitude + (p3 - p2).magnitude + (p4 - p3).magnitude + (p1 - p4).magnitude) / 4 / 2 * Vector3.one * 2;
                var trail = new List<Vector3>() {p1, p2, p3, p4};
                var up = FindBestFitUpAxis(4, ((p1 + p2) / 2 - center).normalized, ((p2 + p3) / 2 - center).normalized, normal);
                onShapeDetected(new Shape() {
                    type = ShapeType.Rectangle,
                    position = center,
                    rotation = Quaternion.LookRotation(normal, up),
                    normal = normal,
                    up = up,
                    scale = scale,
                    trail = trail
                });
                return true;
            }
        }

        return false;
    }


    // 五芒星認識
    bool DetectStarGesture()
    {
        if (vertexPoints_.Count < 6) return false;

        var p1 = vertexPoints_[0];
        var p2 = vertexPoints_[1];
        var p3 = vertexPoints_[2];
        var p4 = vertexPoints_[3];
        var p5 = vertexPoints_[4];
        var p6 = vertexPoints_[5];

        // 始点と終点が大体同じ位置
        if (Vector3.Distance(p1, p6) < closeJudgeDistance) {
            // 各頂点の角度が大体 36 度付近
            const float minError = 10f;
            var ang1 = Vector3.Angle(p2 - p1, p2 - p3);
            var ang2 = Vector3.Angle(p3 - p2, p3 - p4);
            var ang3 = Vector3.Angle(p4 - p3, p4 - p5);
            var ang4 = Vector3.Angle(p5 - p4, p5 - p1);
            var ang5 = Vector3.Angle(p1 - p2, p1 - p5);
            var length = (p2 - p1).magnitude;
            if (Mathf.Abs(ang1 - 36) < minError &&
                Mathf.Abs(ang2 - 36) < minError && 
                Mathf.Abs(ang3 - 36) < minError && 
                Mathf.Abs(ang4 - 36) < minError && 
                Mathf.Abs(ang5 - 36) < minError &&
                length > minPolygonalSideLength) {
                var center = (p1 + p2 + p3 + p4 + p5) / 5;
                var normal = Vector3.Cross(p4 - p3, p3 - p2);
                if (Vector3.Dot(normal, Camera.main.transform.forward) < 0) {
                    normal *= -1;
                }
                var scale = ((p2 - p1).magnitude + (p3 - p2).magnitude + (p4 - p3).magnitude 
                    + (p5 - p4).magnitude + (p1 - p5).magnitude) / 5 * (1 / (2 * Mathf.Cos(18f * Mathf.Deg2Rad))) * Vector3.one * 2;
                var trail = new List<Vector3>() {p1, p2, p3, p4, p5};
                var up = FindBestFitUpAxis(5, (p1 - center).normalized, (p3 - center).normalized, normal);
                onShapeDetected(new Shape() {
                    type = ShapeType.Star,
                    position = center,
                    rotation = Quaternion.LookRotation(normal, up),
                    normal = normal,
                    up = up,
                    scale = scale,
                    trail = trail
                });
                return true;
            }
        }

        return false;
    }


    // 空間上の上に最も近い中心から頂点へ向かうベクトルを調べる
    // 引数は、図形のポリゴンの数と認識した図形平面上の異なる２つのベクトルおよび法線ベクトルを指定
    Vector3 FindBestFitUpAxis(int polygon, Vector3 firstVertexVector, Vector3 secondVertexVector, Vector3 normal)
    {
        // 基準となる上方向ベクトル
        var up = Vector3.up;

        // 基準となる上方向ベクトルの認識した図形への正射影ベクトル
        var upAxisOnShape = (Vector3.Dot(firstVertexVector,  up) * firstVertexVector +
                             Vector3.Dot(secondVertexVector, up) * secondVertexVector).normalized;
        var axis = firstVertexVector;
        var bestFitUpAxis = upAxisOnShape;
        var maxInnerProduct = 0f;
        for (var i = 0; i < polygon; ++i) {
            var innerProduct = Vector3.Dot(axis, upAxisOnShape);
            if (innerProduct > maxInnerProduct) {
                bestFitUpAxis = axis;
                maxInnerProduct = innerProduct;
            }
            axis = Quaternion.AngleAxis(360 / polygon, normal) * axis;
        }
        return bestFitUpAxis;
    }


    [System.Diagnostics.Conditional("DEBUG")]
    void Log(string msg)
    {
        Debug.Log(msg);
    }
}