using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carsol : MonoBehaviour
{
    TrailRenderer myTrailRenderer;
    public bool judgmenting;
    [SerializeField]
    private List<GameObject> magics = new List<GameObject>();
    public List<GameObject> candidates = new List<GameObject>();
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartJudgment();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            FinishJudgment();
        }
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    
    void StartJudgment()
    {
        candidates = magics;
        myTrailRenderer = this.gameObject.AddComponent<TrailRenderer>();
        myTrailRenderer.time = 256;
        judgmenting = true;
    }
    void FinishJudgment()
    {
        Destroy(myTrailRenderer);
        judgmenting = false;
    }
}
