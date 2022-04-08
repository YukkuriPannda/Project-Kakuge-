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
    public Material carsolTrailMaterial;
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
        this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);
        
    }

    
    void StartJudgment()
    {
        candidates = new List<GameObject>(magics);
        myTrailRenderer = this.gameObject.AddComponent<TrailRenderer>();
        myTrailRenderer.time = 256;
        myTrailRenderer.startWidth = 2.5f;
        myTrailRenderer.material = carsolTrailMaterial;
        judgmenting = true;
    }
    void FinishJudgment()
    {
        Destroy(myTrailRenderer);
        judgmenting = false;
    }
}
