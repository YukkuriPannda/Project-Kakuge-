using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carsol : MonoBehaviour
{
    TrailRenderer myTrailRenderer;
    [System.NonSerialized] public bool judging = false;
    public List<GameObject> magics = new List<GameObject>();
    public List<GameObject> candidates = new List<GameObject>();
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartJudgment();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            FinishJudgment();
        
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    
    void StartJudgment()
    {
        candidates = new List<GameObject>(magics);
        judging = true;

        myTrailRenderer = this.gameObject.AddComponent<TrailRenderer>();
        myTrailRenderer.time = 256;
    }
    void FinishJudgment()
    {
        judging = false;
        
        Destroy(myTrailRenderer);
    }
}
