using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPBarController : MonoBehaviour
{
    public EntityBase entityBase;
    public RectTransform mainBarRectTrf;
    public RectTransform subBarRectTrf;
    public GameObject DMGTexPrefab;
    private RectTransform my_rect_trf;
    private Camera main_camera;
    private float oldHP = 0;

    void Start()
    {
        my_rect_trf = gameObject.GetComponent<RectTransform>();
        main_camera = Camera.main;
    }
    void Update()
    {
        if(!entityBase)Destroy(gameObject);
        else{
            mainBarRectTrf.sizeDelta = new Vector2(Mathf.Clamp(entityBase.Health/entityBase.MaxHealth*50,0,50),5);
            if(subBarRectTrf.sizeDelta.x > 0&&mainBarRectTrf.sizeDelta.x < subBarRectTrf.sizeDelta.x)subBarRectTrf.sizeDelta -= new Vector2(5f*Time.deltaTime,0);
            my_rect_trf.position = RectTransformUtility.WorldToScreenPoint(main_camera,entityBase.transform.position);
            if(oldHP > entityBase.Health){
                GameObject DMGTexObject = Instantiate(DMGTexPrefab,transform.position + new Vector3(Random.Range(-10,10),0,0),Quaternion.identity,transform);
                TextMeshProUGUI DMGTex = DMGTexObject.GetComponent<TextMeshProUGUI>();
                DMGTex.text = (oldHP-entityBase.Health).ToString();
                DMGTex.color = MagicColorManager.GetColorFromMagicArticle(entityBase.hurtMagicAtttribute);
                Destroy(DMGTexObject,1);
            }
            oldHP = entityBase.Health;
        }
    }
}
