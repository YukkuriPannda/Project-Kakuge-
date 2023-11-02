using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : ButtonBaseEX
{
    Animator animator;
    public string sceneName;
    public Image fade;
    public GameObject Loading;
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
        StartCoroutine(IEOnClickDown());
    }
    IEnumerator IEOnClickDown(){
        for(float t = 0;t <= 0.5f;t +=Time.deltaTime){
            fade.color+= new Color(0,0,0,Time.deltaTime * 2);
            yield return null;
        }
        Loading.SetActive(true);
        SceneManager.LoadScene(sceneName);
        yield break;
    }
}
