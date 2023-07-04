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
    
    void Start()
    {
        icon = gameObject.GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>(item.spritePath);
        icon.sprite = sprite;
    }
    void Update()
    {
        if(simpleStatusPanel){
            mousePos = Input.mousePosition;
            simpleStatusPanelRectTrf.position = mousePos;
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
    }
    public void OnPointerExit(){
        Destroy(simpleStatusPanel);
        simpleStatusPanel = null;
        simpleStatusPanelRectTrf = null;
    }
}