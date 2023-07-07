using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventryItem : MonoBehaviour
{
    public ItemBase item;

    public GameObject SimpleStatusPanelPrefab;
    private GameObject simpleStatusPanel;
    private RectTransform simpleStatusPanelRectTrf;

    public GameObject BarPrefab;

    private Vector2 mousePos;
    private Image icon;
    private bool oldIsPointerOverGameObject =false;
    private RectTransform myRectTrf;
    
    void Start()
    {
        icon = gameObject.GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>(item.spritePath);
        icon.sprite = sprite;
        myRectTrf = gameObject.GetComponent<RectTransform>();
    }
    void Update()
    {
        mousePos = Input.mousePosition;
        if(simpleStatusPanel){
            simpleStatusPanelRectTrf.position = mousePos; 
            if(myRectTrf.sizeDelta.x < mousePos.x - myRectTrf.position.x
            || -myRectTrf.sizeDelta.x > mousePos.x - myRectTrf.position.x
            || myRectTrf.sizeDelta.y < mousePos.y - myRectTrf.position.y
            || -myRectTrf.sizeDelta.y > mousePos.y - myRectTrf.position.y
            ){
                OnPointerExit();
            }
        }else {
            if (!(myRectTrf.sizeDelta.x < mousePos.x - myRectTrf.position.x
            || -myRectTrf.sizeDelta.x > mousePos.x - myRectTrf.position.x
            || myRectTrf.sizeDelta.y < mousePos.y - myRectTrf.position.y
            || -myRectTrf.sizeDelta.y > mousePos.y - myRectTrf.position.y
            )){
                OnPointerEnter();
            }
        }
       
    }
    public void OnPointerEnter(){
        simpleStatusPanel = Instantiate(SimpleStatusPanelPrefab,mousePos,Quaternion.identity,transform.parent.parent);
        simpleStatusPanelRectTrf = simpleStatusPanel.GetComponent<RectTransform>();
        simpleStatusPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.name;

        string uniqueParamsNameText = "";
        foreach(UniqueParameter uniqueParameter in item.uniqueParameters){
            uniqueParamsNameText += uniqueParameter.name + "\n";
        }
        simpleStatusPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = uniqueParamsNameText;
        
        string uniqueParamsValueText = "";
        foreach(UniqueParameter uniqueParameter in item.uniqueParameters){
            uniqueParamsValueText += uniqueParameter.value + "\n";
        }
        simpleStatusPanelRectTrf.sizeDelta = new Vector2(256,40+15*item.uniqueParameters.Length);
        simpleStatusPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = uniqueParamsValueText;
        
        for(int i = 0;i < item.uniqueParameters.Length;i++){
            GameObject bar = Instantiate(BarPrefab,simpleStatusPanel.transform);
            bar.GetComponent<RectTransform>().sizeDelta = new Vector2((item.uniqueParameters[i].value/item.uniqueParameters[i].maxValue) * (150/100) * 100,1);
            bar.GetComponent<RectTransform>().localPosition = new Vector2(80,-35-15*i);
        }
        Debug.Log("Enter");
        StartCoroutine(ExpansionItemIcon());
    }
    public void OnPointerExit(){
        StartCoroutine(ShrinkItemIcon());
        Destroy(simpleStatusPanel);
        simpleStatusPanel = null;
        simpleStatusPanelRectTrf = null;
        Debug.Log("Exit");
    }
    IEnumerator ExpansionItemIcon()
    {
        Vector2 startScale = myRectTrf.localScale;
        float sizeOnHover = 1.2f;
        for(float t = 0;t <= 0.1f;t += Time.deltaTime){
            myRectTrf.localScale = new Vector3(10 * (sizeOnHover - startScale.x)* t +startScale.x
            ,10* (sizeOnHover - startScale.y) * t+startScale.y,1);
            yield return null;
        }
        yield break;
    }
    IEnumerator ShrinkItemIcon()
    {
        Vector2 startScale = myRectTrf.localScale;
        float sizeOnHover = 1f;
        for(float t = 0;t <= 0.1f;t += Time.deltaTime){
            myRectTrf.localScale = new Vector3(10 * (sizeOnHover - startScale.x)* t +startScale.x
            ,10* (sizeOnHover - startScale.y) * t+startScale.y,1);
            yield return null;
        }
        yield break;
    }
}