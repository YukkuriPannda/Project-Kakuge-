using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPBarController : MonoBehaviour
{
    public EntityBase entityBase;
    public RectTransform mainBarRectTrf;
    public RectTransform subBarRectTrf;
    public RectTransform HeatBar;
    public GameObject DMGTexPrefab;
    private RectTransform my_rect_trf;
    private RectTransform myParentRectTrf;
    private Camera main_camera;
    private float oldHP = 0;
    private Vector2 offsetPos;

    void Start()
    {
        my_rect_trf = gameObject.GetComponent<RectTransform>();
        myParentRectTrf = transform.parent.gameObject.GetComponent<RectTransform>();
        main_camera = Camera.main;
        Debug.Log(main_camera.name);
        offsetPos = new Vector2(main_camera.pixelWidth/2,main_camera.pixelHeight/2);
    }
    void Update()
    {
        if(!entityBase)Destroy(gameObject);
        else{
            mainBarRectTrf.sizeDelta = new Vector2(Mathf.Clamp(entityBase.Health/entityBase.MaxHealth*50,0,50),5);
            HeatBar.sizeDelta = new Vector2(Mathf.Clamp(entityBase.Heat/entityBase.HeatCapacity*50,0,50),5);

            if(subBarRectTrf.sizeDelta.x > 0&&mainBarRectTrf.sizeDelta.x < subBarRectTrf.sizeDelta.x)subBarRectTrf.sizeDelta -= new Vector2(5f*Time.deltaTime,0);
            my_rect_trf.position =  GetHealthBarPostion();
            if(oldHP > entityBase.Health){
                Debug.Log("a"+mainBarRectTrf.sizeDelta.x);
                GameObject DMGTexObject = Instantiate(DMGTexPrefab,transform.position + new Vector3(Random.Range(-50,50),0,0),Quaternion.identity,transform);
                TextMeshProUGUI DMGTex = DMGTexObject.GetComponent<TextMeshProUGUI>();
                DMGTex.text = (oldHP-entityBase.Health).ToString();
                DMGTex.color = MagicColorManager.GetColorFromMagicArticle(entityBase.hurtMagicAtttribute);
                Destroy(DMGTexObject,1);
            }
            oldHP = entityBase.Health;
        }

    }
    Vector2 GetHealthBarPostion(){
        Vector2 screenPos =  RectTransformUtility.WorldToScreenPoint(main_camera,entityBase.transform.position);
        Vector2 uiloaclPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            myParentRectTrf,screenPos, main_camera, out uiloaclPos
        );
        return screenPos;

        
    }

}
