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

    private Transform mousePointer;
    private Vector2 mousePos;
    private Image icon;
    private bool oldIsPointerOverGameObject =false;
    
    void Start()
    {
        icon = gameObject.GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>(item.spritePath);
        icon.sprite = sprite;
        mousePointer = GameObject.Find("MousePointer").transform;
    }
    void Update()
    {
        bool nowIsPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
        //if (nowIsPointerOverGameObject && !oldIsPointerOverGameObject) OnPointerEnter();
        //if (!nowIsPointerOverGameObject && oldIsPointerOverGameObject) OnPointerExit();
        
        if(nowIsPointerOverGameObject){
            mousePos = Input.mousePosition;
        }
        oldIsPointerOverGameObject = nowIsPointerOverGameObject;
        if(simpleStatusPanel)simpleStatusPanelRectTrf.position = mousePos;
    }
    public void OnPointerEnter(){
        simpleStatusPanel = Instantiate(SimpleStatusPanelPrefab,mousePos,Quaternion.identity,transform);
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
        simpleStatusPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = uniqueParamsValueText;
    }
    public void OnPointerExit(){
        Destroy(simpleStatusPanel);
        simpleStatusPanelRectTrf = null;
    }
}