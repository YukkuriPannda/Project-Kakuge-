using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeJudger : MonoBehaviour
{
    [Header("Input")]
    public float detectionFrequency;
    public float sinceLastAddedPoint = 0;
    [SerializeField] Transform mousePointer;
    [SerializeField] PlayerController plc;
    [Space(5)]

    [Header("Output")]
    public string result ="None";
    public float writingTime;
    public float accuracy = 0;
    public bool OutPutLog = false;
    [Space(20)]

    [Header("Info")]
    [SerializeField,ReadOnly]private bool clicking;
    [SerializeField,ReadOnly]private List<Vector2> inputPoints = new List<Vector2>();
    [SerializeField,ReadOnly]private List<Vector2> sortPoints = new List<Vector2>();
    [SerializeField,ReadOnly]private float CircleAccuracyValue = 0;
    [SerializeField,ReadOnly]private float RegularTriangleAccuracyValue = 0;
    [SerializeField,ReadOnly]private float InvertedTriangleAccuracyValue = 0;
    [SerializeField,ReadOnly]private float thunderAccuracyValue = 0;
    [SerializeField,ReadOnly]private float grassAccuracyValue = 0;
    [SerializeField,ReadOnly]private Vector2 center;
    [SerializeField,ReadOnly]private float radius;
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){//初期化
            inputPoints = new List<Vector2>();
            CircleAccuracyValue = 0;
            RegularTriangleAccuracyValue = 0;
            InvertedTriangleAccuracyValue = 0;
            thunderAccuracyValue = 0;
            grassAccuracyValue = 0;
            accuracy =0;
        }
        if(Input.GetMouseButton(0)){
            if(detectionFrequency >= sinceLastAddedPoint){
                sinceLastAddedPoint = 0;
                if(inputPoints.Count > 0){
                    if(Vector2.Distance(inputPoints[inputPoints.Count-1],mousePointer.position)>=0.3f)inputPoints.Add(mousePointer.position);
                }else inputPoints.Add(mousePointer.position);
            }else{
                writingTime += Time.deltaTime;
                sinceLastAddedPoint += Time.deltaTime;
            }
        }
        if(Input.GetMouseButtonUp(0)){
            CircleAccuracyValue = CheckCircle(inputPoints);
            if(CircleAccuracyValue < 0.67f){
                sortPoints = SortPoints(inputPoints);
                RegularTriangleAccuracyValue = CheckRegularTriangle(sortPoints);
                InvertedTriangleAccuracyValue = CheckInvertedTriangle(sortPoints);
                thunderAccuracyValue = CheckThunder(sortPoints);
                grassAccuracyValue = CheckGrass(sortPoints);
                if(RegularTriangleAccuracyValue > 0 && RegularTriangleAccuracyValue > CircleAccuracyValue){
                    result = "RegularTriangle";
                    accuracy = RegularTriangleAccuracyValue;
                }
                if(InvertedTriangleAccuracyValue > 0 && InvertedTriangleAccuracyValue > CircleAccuracyValue) {
                    result = "InvertedTriangle";
                    accuracy = InvertedTriangleAccuracyValue;
                }
                if(thunderAccuracyValue > 0 && thunderAccuracyValue > CircleAccuracyValue){
                    result = "Thunder";
                    accuracy = thunderAccuracyValue;
                }
                if(grassAccuracyValue > 0 && grassAccuracyValue > CircleAccuracyValue){
                    result = "Grass";
                    accuracy = grassAccuracyValue;
                }
                if(result == null){
                    result = "Circle";
                    accuracy = CircleAccuracyValue;
                }
            }else {
                result = "Circle";
                accuracy = CircleAccuracyValue;
            }

            if(sortPoints.Count < 3 && CircleAccuracyValue < 0.67f){
                float drawingAngle = Mathf.Atan2(inputPoints[0].y-inputPoints[inputPoints.Count-1].y,inputPoints[0].x-inputPoints[inputPoints.Count-1].x)+Mathf.PI;
                Debug.Log($"drawingAngle{drawingAngle}");
                if(drawingAngle>Mathf.PI/4 && drawingAngle<Mathf.PI*3/4)result = "StraightToUp";
                if(drawingAngle>Mathf.PI*3/4 && drawingAngle<Mathf.PI*5/4)result = "StraightToLeft";
                if(drawingAngle>Mathf.PI*5/4 && drawingAngle<Mathf.PI*7/4)result = "StraightToDown";
                if(drawingAngle>Mathf.PI*7/4 || drawingAngle<Mathf.PI/4)result = "StraightToRight";
                accuracy = 1;
            }
            if(radius < 0.15f){
                result = "tap";
                accuracy = 1; 
            }
            plc.drawShapeName = result;
            plc.drawShapePos = center;
        }
        DrawLine();
    }
    List<Vector2> SortPoints(List<Vector2> points){
        List<Vector2> returnPoints = points;
        for(int i = 0;i < returnPoints.Count-2;){
            if(ExtensionMethods.InteriorAngle(returnPoints[i],returnPoints[i+1],returnPoints[i+2]) >= 100 * (Mathf.PI/180)) returnPoints.RemoveAt(i+1);
            else i++;
        }
        
        return returnPoints;
    }
    float CheckCircle(List<Vector2> points){
        Vector2 sumPos = new Vector2(0,0);
        foreach(Vector2 point in points){
            sumPos += point;
        }
        center = sumPos/points.Count;
        Debug.Log(center);
        float sumDis = 0;
        float MaxDis = 0;
        float MinDis = Vector2.Distance(center,points[0]);
        foreach(Vector2 point in points){
            float Dis = Vector2.Distance(center,point);
            sumDis += Dis;
            if(Dis > MaxDis)MaxDis = Dis;
            if(Dis < MinDis)MinDis = Dis;
        }
        radius = sumDis / points.Count;
        float sumAccuracy = 0;
        if(OutPutLog)Debug.Log($"{radius} min{MinDis} max{MaxDis}");
        foreach(Vector2 point in points){
            float Dis = Vector2.Distance(center,point);
            float accuracy = ExtensionMethods.AccuraryCheckFromNum(Dis,radius/2,radius);
            sumAccuracy += accuracy;
            if(OutPutLog)Debug.Log($"Pos:{point} Dis:{Dis} accuracy:{accuracy}");
        }

        return sumAccuracy/points.Count;
    }
    float CheckRegularTriangle(List<Vector2> points){
       if(OutPutLog) Debug.Log("Start Triangle Check");
        float accuracyValue = 0;
        if(points.Count == 4 && points[1].y < points[3].y && points[1].y < points[0].y){
            float A = ExtensionMethods.InteriorAngle(points[2],points[0],points[1]);
            float B = ExtensionMethods.InteriorAngle(points[0],points[1],points[2]);
            float C = ExtensionMethods.InteriorAngle(points[1],points[2],points[0]);
            if(OutPutLog)Debug.Log($"Angle is A:{A} B:{B} C:{C}");
            accuracyValue = (AnAngleAccurary(A,Mathf.PI/3) + AnAngleAccurary(B,Mathf.PI/3) + AnAngleAccurary(C,Mathf.PI/3))/3;
            if(OutPutLog)Debug.Log($"Angle Accuray is {accuracyValue} (A:{AnAngleAccurary(A,Mathf.PI/3)} B:{AnAngleAccurary(B,Mathf.PI/3)} C:{AnAngleAccurary(C,Mathf.PI/3)})");
        } 
        if(OutPutLog)Debug.Log(accuracyValue);
        return accuracyValue;
    }
    float CheckInvertedTriangle(List<Vector2> points){
        if(OutPutLog)Debug.Log("Start Triangle Check");
        float accuracyValue = 0;
        if(points.Count == 4 && points[1].y > points[3].y && points[1].y > points[0].y){
            float A = ExtensionMethods.InteriorAngle(points[2],points[0],points[1]);
            float B = ExtensionMethods.InteriorAngle(points[0],points[1],points[2]);
            float C = ExtensionMethods.InteriorAngle(points[1],points[2],points[0]);
            if(OutPutLog)Debug.Log($"Angle is A:{A} B:{B} C:{C}");
            accuracyValue = (AnAngleAccurary(A,Mathf.PI/3) + AnAngleAccurary(B,Mathf.PI/3) + AnAngleAccurary(C,Mathf.PI/3))/3;
            if(OutPutLog)Debug.Log($"Angle Accuray is {accuracyValue} (A:{AnAngleAccurary(A,Mathf.PI/3)} B:{AnAngleAccurary(B,Mathf.PI/3)} C:{AnAngleAccurary(C,Mathf.PI/3)})");
        } 
        if(OutPutLog)Debug.Log(accuracyValue);
        return accuracyValue;
    }
    float CheckThunder(List<Vector2> points){
        if(OutPutLog)Debug.Log("Start Thunder Check");
        float accuracyValue = 0;
        if(points.Count == 4 && points[1].y > points[3].y && points[0].y > points[1].y){
            float A = ExtensionMethods.InteriorAngle(points[2],points[0],points[1]);
            float B = ExtensionMethods.InteriorAngle(points[0],points[1],points[2]);
            float C = ExtensionMethods.InteriorAngle(points[1],points[2],points[0]);
            if(OutPutLog)Debug.Log($"Angle is A:{A} B:{B} C:{C}");
            accuracyValue = (AnAngleAccurary(A,Mathf.PI/3) + AnAngleAccurary(B,Mathf.PI/3) + AnAngleAccurary(C,Mathf.PI/3))/3;
            if(OutPutLog)Debug.Log($"Angle Accuray is {accuracyValue} (A:{AnAngleAccurary(A,Mathf.PI/3)} B:{AnAngleAccurary(B,Mathf.PI/3)} C:{AnAngleAccurary(C,Mathf.PI/3)})");
        } 
        if(OutPutLog)Debug.Log(accuracyValue);
        return accuracyValue;
    }
    float CheckGrass(List<Vector2> points){
        if(OutPutLog)Debug.Log("Start Grass Check");
        float accuracyValue = 0;
        if(points.Count == 5){
            float A = ExtensionMethods.InteriorAngle(points[0],points[1],points[2]);
            float B = ExtensionMethods.InteriorAngle(points[1],points[2],points[3]);
            float C = ExtensionMethods.InteriorAngle(points[2],points[3],points[4]);
            if(OutPutLog)Debug.Log($"Angle is A:{A} B:{B} C:{C}");
            accuracyValue = (AnAngleAccurary(A,Mathf.PI/4) + AnAngleAccurary(B,Mathf.PI/4) + AnAngleAccurary(C,Mathf.PI/4))/3;
            if(OutPutLog)Debug.Log($"Angle Accuray is {accuracyValue} (A:{AnAngleAccurary(A,Mathf.PI/4)} B:{AnAngleAccurary(B,Mathf.PI/4)} C:{AnAngleAccurary(C,Mathf.PI/4)})");
        } 
        if(OutPutLog)Debug.Log(accuracyValue);
        return accuracyValue;
    }
    
    float AnAngleAccurary(float angle,float targetAngle){
        if(angle >= (targetAngle*1)/2 && angle <= (targetAngle*3)/2)
        {
            return -Mathf.Cos(angle*(Mathf.PI/targetAngle))/2 + 0.5f;
        }else return 0;
    }
    void DrawLine(){
        if(inputPoints.Count>0){
            Vector3[] poses = new Vector3[inputPoints.Count];
            lineRenderer.positionCount = inputPoints.Count;
            for(int i = 0;i < inputPoints.Count;i++){
                poses[i] = new Vector3(inputPoints[i].x,inputPoints[i].y,-5);
            }
            lineRenderer.SetPositions(poses);
        }
    }
    void OnDrawGizmos()
    {
        if(inputPoints.Count >= 2){
            for(int i = 0;i < inputPoints.Count-1;i ++){
                Gizmos.color = Color.white;
                Gizmos.DrawLine(inputPoints[i],inputPoints[i+1]);
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(inputPoints[i],0.1f);
            }
        }
        if(sortPoints.Count >= 2){
            for(int i = 0;i < sortPoints.Count-1;i ++){
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(sortPoints[i],sortPoints[i+1]);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(sortPoints[i],0.1f);
            }
        }
        if(center != null){
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(center,0.1f);
        }
        if(radius != 0){
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center,radius);
        }
    }
}
public class ExtensionMethods{
    public static float InteriorAngle(Vector2 point1,Vector2 point2,Vector2 point3){
        Vector2 a =  point1 - point2;
        Vector2 b = point3 - point2;
        return Mathf.Acos((a.x*b.x+a.y*b.y)/(Mathf.Sqrt(a.x * a.x + a.y * a.y)*Mathf.Sqrt(b.x * b.x + b.y * b.y)));
    }
    public static float AccuraryCheckFromNum(float num,float numToMin,float numToMax){
        if(num> numToMax +numToMin || num < numToMax - numToMin*2)return 0;
        return Mathf.Cos((num*Mathf.PI-numToMax*Mathf.PI)/numToMin)/2 + 0.5f;
    }
}