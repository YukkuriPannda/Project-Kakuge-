using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTrailRenderer : MonoBehaviour
{
    public bool record;
    public bool display;
    [ReadOnly]public List<Vector3> linePos = new List<Vector3>();
    [HideInInspector]public LineRenderer lineRenderer;
    private void Start() {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        Clear();
    }
    void FixedUpdate()
    {
        if(record){
            lineRenderer.positionCount ++;
            linePos.Add(transform.position);
            lineRenderer.SetPositions(linePos.ToArray());
        }
        lineRenderer.enabled = display;
    }
    public IEnumerator Clear(float time = 0){
        yield return new WaitForSeconds(time);
        linePos = new List<Vector3>();
        lineRenderer.positionCount = 0;
    }
}
