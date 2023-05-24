using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public EntityBase entityBase;
    public RectTransform mainBarRectTrf;
    public RectTransform subBarRectTrf;
    private RectTransform my_rect_trf;
    private Camera main_camera;
    void Start()
    {
        my_rect_trf = gameObject.GetComponent<RectTransform>();
        main_camera = Camera.main;
    }
    void Update()
    {
        /*
        if(entityBase.Health >= 0)mainBar.transform.localScale = new Vector3(entityBase.Health/entityBase.MaxHealth*10,0.75f,1);
        if(subBar.transform.localScale.x > 0&&mainBar.transform.localScale.x < subBar.transform.localScale.x)subBar.transform.localScale -= new Vector3(1.5f*Time.deltaTime,0,0);
        */
        if(entityBase.Health >= 0)mainBarRectTrf.sizeDelta = new Vector2(entityBase.Health/entityBase.MaxHealth*20,2);
        if(subBarRectTrf.sizeDelta.x > 0&&mainBarRectTrf.sizeDelta.x < subBarRectTrf.sizeDelta.x)subBarRectTrf.sizeDelta -= new Vector2(5f*Time.deltaTime,0);
        my_rect_trf.position = RectTransformUtility.WorldToScreenPoint(main_camera,entityBase.transform.position);
    }
}
