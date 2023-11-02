using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : ButtonBaseEX
{
    Animator animator;
    public string sceneName;
    public override void OnStart()
    {
        base.OnStart();
        animator = gameObject.GetComponent<Animator>();
    }
    public override void OnPointerEnter()
    {
        base.OnPointerEnter();
        animator.Play("Enter");
    }
    public override void OnPointerExit()
    {
        base.OnPointerEnter();
        animator.Play("Exit");
    }
    public override void OnClickDown()
    {
        base.OnClickDown();
        SceneManager.LoadScene(sceneName);
    }
}
