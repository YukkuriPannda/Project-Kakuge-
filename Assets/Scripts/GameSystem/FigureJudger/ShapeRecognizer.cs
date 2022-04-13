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
    public List<Vector2> positions_    = new List<Vector2>();
    private List<Vector3> vertexPoints_ = new List<Vector3>();
    //private int skipCountForSharpAngleDetection_ = 0;
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

    [System.Diagnostics.Conditional("DEBUG")]
    void Log(string msg)
    {
        Debug.Log(msg);
    }
}