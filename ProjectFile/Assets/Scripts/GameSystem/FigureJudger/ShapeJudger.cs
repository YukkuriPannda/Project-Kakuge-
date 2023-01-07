using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeJudger : MonoBehaviour
{
    [Header("Input")]
    public float detectionFrequency;
    public float sinceLastAddedPoint;
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
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))inputPoints = new List<Vector2>();
        if(Input.GetMouseButton(0)){
            if(writingTime >= sinceLastAddedPoint){
                sinceLastAddedPoint = 0;
                if(inputPoints.Count > 1){
                    if(Vector2.Distance(inputPoints[inputPoints.Count-1],mousePointer.position)>=0.5f)inputPoints.Add(mousePointer.position);
                }else inputPoints.Add(mousePointer.position);
            }else{
                writingTime += Time.deltaTime;
                sinceLastAddedPoint += Time.deltaTime;
            }
        }
        if(Input.GetMouseButtonUp(0)){
            sortPoints = SortPoints(inputPoints);
        }
    }
    List<Vector2> SortPoints(List<Vector2> points){
        List<Vector2> returnPoints = points;
        for(int i = 0;i < returnPoints.Count-2;){
            if(ExtensionMethods.InteriorAngle(returnPoints[i],returnPoints[i+1],returnPoints[i+2]) >= 150 * (Mathf.PI/180)) returnPoints.RemoveAt(i+1);
            else i++;
        }
        
        return returnPoints;
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