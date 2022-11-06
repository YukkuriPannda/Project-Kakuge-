using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{
    TrailRenderer myTrailRenderer;
    public Material carsolTrailMaterial;
    
    public void start()
    {
        myTrailRenderer = this.gameObject.AddComponent<TrailRenderer>();
        myTrailRenderer.time = 256;
        myTrailRenderer.startWidth = 0.2f;
        myTrailRenderer.material = carsolTrailMaterial; 
    }
    public void end()
    {
        Destroy(myTrailRenderer);
    }
        
    void Update(){
         this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
}
