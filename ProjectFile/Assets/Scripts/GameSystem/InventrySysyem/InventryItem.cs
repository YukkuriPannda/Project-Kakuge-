using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventryItem : ButtonBaseEX
{
    public ItemBase item;
    public InventrySystem inventrySystem;
    public GameObject menuPrefab;

    public GameObject SimpleStatusPanelPrefab;
    private GameObject simpleStatusPanel;
    private RectTransform simpleStatusPanelRectTrf;

    public GameObject BarPrefab;

    private Image icon;
    private LanguageManager languageManager;
    
    public override void OnStart()
    {
        icon = gameObject.GetComponent<Image>();
        if(item.category != ItemCategory.Blank){
            Sprite sprite = Resources.Load<Sprite>(item.spritePath);
            icon.sprite = sprite;
        }else icon.enabled = false;
        myRectTrf = gameObject.GetComponent<RectTransform>();
        languageManager = (LanguageManager)Resources.Load(LanguageManager.textDatabasePath);
    }
    public override void OnPointerEnter(){
        if(item.category != ItemCategory.Blank){
            simpleStatusPanel = Instantiate(SimpleStatusPanelPrefab,mousePos,Quaternion.identity,transform.parent.parent);
            simpleStatusPanelRectTrf = simpleStatusPanel.GetComponent<RectTransform>();
            simpleStatusPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = languageManager.GetTextFromText(item.name);

            string uniqueParamsNameText = "";
            foreach(UniqueParameter uniqueParameter in item.uniqueParameters){
                if(uniqueParameter.maxValue > 0)uniqueParamsNameText += languageManager.GetTextFromText(uniqueParameter.name) + "\n";
            }
            simpleStatusPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = uniqueParamsNameText;
            
            string uniqueParamsValueText = "";
            foreach(UniqueParameter uniqueParameter in item.uniqueParameters){
                if(uniqueParameter.maxValue > 0)uniqueParamsValueText += uniqueParameter.value + "\n";
            }
            simpleStatusPanelRectTrf.sizeDelta = new Vector2(256,40+15*item.uniqueParameters.Length);
            simpleStatusPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = uniqueParamsValueText;
            
            for(int i = 0;i < item.uniqueParameters.Length;i++){
                if(item.uniqueParameters[i].maxValue > 0){
                    GameObject bar = Instantiate(BarPrefab,simpleStatusPanel.transform);
                    bar.GetComponent<RectTransform>().sizeDelta = new Vector2((item.uniqueParameters[i].value/item.uniqueParameters[i].maxValue) * 125,1);
                    bar.GetComponent<RectTransform>().localPosition = new Vector2(125,-35-15*i);
                }
            }
            StartCoroutine(ExpansionItemIcon());
        }
    }
    public override void OnUpdate()
    {
        if(simpleStatusPanel)simpleStatusPanelRectTrf.position = mousePos;
    }
    public override void OnClickDown(){
        if(!inventrySystem.menu){
            if(item.category != ItemCategory.Blank){
                Destroy(simpleStatusPanel);
                inventrySystem.menu = Instantiate(menuPrefab,mousePos,Quaternion.identity,transform.parent.parent);
                inventrySystem.menu.GetComponentInChildren<DetailButton>().item = item;
                if(inventrySystem.menu.GetComponentInChildren<EquipMagicBookButton>())inventrySystem.menu.GetComponentInChildren<EquipMagicBookButton>().id = item.id;
                if(inventrySystem.menu.GetComponentInChildren<EquipWeaponButton>())inventrySystem.menu.GetComponentInChildren<EquipWeaponButton>().id = item.id;
                if(inventrySystem.menu.GetComponentInChildren<UnequipMagicBook>())inventrySystem.menu.GetComponentInChildren<UnequipMagicBook>().id = item.id;
                if(inventrySystem.menu.GetComponentInChildren<UnequipWeapon>())inventrySystem.menu.GetComponentInChildren<UnequipWeapon>().id = item.id;
            }
        }else {
            ButtonBaseEX[] buttonBaseExes = inventrySystem.menu.GetComponentsInChildren<ButtonBaseEX>();
            int i = 0;
            for(;i < buttonBaseExes.Length; i ++){
                if(buttonBaseExes[i].pointerStay){
                    break;
                }
            }
            if(i == buttonBaseExes.Length) Destroy(inventrySystem.menu);
        }
    }
    public override void OnPointerExit(){
        StartCoroutine(ShrinkItemIcon());
        Destroy(simpleStatusPanel);
        simpleStatusPanel = null;
        simpleStatusPanelRectTrf = null;
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