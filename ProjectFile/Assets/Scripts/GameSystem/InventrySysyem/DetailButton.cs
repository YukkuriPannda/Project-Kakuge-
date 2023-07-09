using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailButton : MenuButton
{
    public ItemBase item;
    public override void OnClickDown()
    {
        base.OnClickDown();
        Debug.Log("On Click Detail Button");
    }
}
