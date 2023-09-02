using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : ButtonBaseEX
{
    private Image image;
    private Color normalColor;
    public Color onHoverColor;
    public override void OnStart()
    {
        image = gameObject.GetComponent<Image>();
        normalColor = image.color;
    }
    public override void OnPointerEnter(){
        image.color = onHoverColor;
    }
    public override void OnPointerExit()
    {
        image.color = normalColor;
    }
    public override void OnClickDown()
    {
        Debug.Log("MenuButtonClick");
        base.OnClickDown();
        CloseMenuPanel();
    }
    public void CloseMenuPanel(){
        Destroy(transform.parent.gameObject);
    }
}
