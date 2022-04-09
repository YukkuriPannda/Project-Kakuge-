using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carsol : MonoBehaviour
{
    TrailRenderer myTrailRenderer;
    [System.NonSerialized] public bool judging = false;
    public List<GameObject> magics = new List<GameObject>();
    public List<GameObject> candidates = new List<GameObject>();
    public Material carsolTrailMaterial;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartJudgment();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            FinishJudgment();
<<<<<<< HEAD
        }
        this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);
        
=======
        
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
>>>>>>> f7a584e0b5f0f9e5c6ff2cf24db9d10f052bae44
    }

    
    void StartJudgment()
    {
        candidates = new List<GameObject>(magics);
<<<<<<< HEAD
        myTrailRenderer = this.gameObject.AddComponent<TrailRenderer>();
        myTrailRenderer.time = 256;
        myTrailRenderer.startWidth = 2.5f;
        myTrailRenderer.material = carsolTrailMaterial;
        judgmenting = true;
=======
        judging = true;

        myTrailRenderer = this.gameObject.AddComponent<TrailRenderer>();
        myTrailRenderer.time = 256;
>>>>>>> f7a584e0b5f0f9e5c6ff2cf24db9d10f052bae44
    }
    void FinishJudgment()
    {
        judging = false;
        
        Destroy(myTrailRenderer);
    }
}
