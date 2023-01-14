using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeJudger : MonoBehaviour
{
    [Header("Input")]
    public float detectionFrequency;
    public float sinceLastAddedPoint = 0;
    [SerializeField] Transform mousePointer;
    [Space(5)]

    [Header("Output")]
    public string result ="";
    public float writingTime;
    [Space(20)]

    [Header("Info")]
    [SerializeField]private bool clicking;
    [SerializeField]private List<Vector2> inputPoints = new List<Vector2>();
    [SerializeField]private List<Vector2> sortPoints = new List<Vector2>();
    [SerializeField]private float triangleAccuracyValue = 0;
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))inputPoints = new List<Vector2>();
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
            sortPoints = SortPoints(inputPoints);
            triangleAccuracyValue = CheckTriangle(sortPoints);
        }
    }
    List<Vector2> SortPoints(List<Vector2> points){
        List<Vector2> returnPoints = points;
        for(int i = 0;i < returnPoints.Count-2;){
            if(ExtensionMethods.InteriorAngle(returnPoints[i],returnPoints[i+1],returnPoints[i+2]) >= 100 * (Mathf.PI/180)) returnPoints.RemoveAt(i+1);
            else i++;
        }
        
        return returnPoints;
    }
    float CheckTriangle(List<Vector2> points){
        float accuracyValue = 0;
        if(points.Count == 4 && points[1].y < points[3].y){
            float A = ExtensionMethods.InteriorAngle(points[2],points[0],points[1]);
            float B = ExtensionMethods.InteriorAngle(points[0],points[1],points[2]);
            float C = ExtensionMethods.InteriorAngle(points[1],points[2],points[0]);
            Debug.Log($"Angle is A:{A} B:{B} C:{C}");
            accuracyValue = (AnAngleAccurary(A,Mathf.PI/3) + AnAngleAccurary(B,Mathf.PI/3) + AnAngleAccurary(C,Mathf.PI/3))/3;
            Debug.Log($"Angle Accuray is {accuracyValue} (A:{AnAngleAccurary(A,Mathf.PI/3)} B:{AnAngleAccurary(B,Mathf.PI/3)} C:{AnAngleAccurary(C,Mathf.PI/3)})");
            float a = Vector2.Distance(points[1],points[2]);
            float b = Vector2.Distance(points[2],points[0]);
            float c = Vector2.Distance(points[0],points[1]);
        } 
        Debug.Log(accuracyValue);
        return accuracyValue;
    }
    float AnAngleAccurary(float angle,float targetAngle){
        if(angle >= (targetAngle*1)/2 && angle <= (targetAngle*3)/2)
        {
            return -Mathf.Cos(angle*(Mathf.PI/targetAngle))/2 + 0.5f;
        }else return 0;
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
    }
}
public class ExtensionMethods{
    public static float InteriorAngle(Vector2 point1,Vector2 point2,Vector2 point3){
        Vector2 a =  point1 - point2;
        Vector2 b = point3 - point2;
        return Mathf.Acos((a.x*b.x+a.y*b.y)/(Mathf.Sqrt(a.x * a.x + a.y * a.y)*Mathf.Sqrt(b.x * b.x + b.y * b.y)));
    }
}