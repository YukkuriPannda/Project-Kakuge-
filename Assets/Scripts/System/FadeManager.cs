using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    private bool? FadeSwitch = null;
    private float alpha = 0.0f;

    private float fadeSpeed = 1.0f;
    private string sceneName;

    public void fadeStart(string sceneN, float fadeS)
    { 
        DontDestroyOnLoad(this);
        FadeSwitch = true;
        fadeSpeed = fadeS;
        sceneName = sceneN;
    }

    void Update()
    {
        if (FadeSwitch == true)
        {
            alpha += Time.deltaTime / fadeSpeed;
            if (alpha >= 1.0f)
            {
                alpha = 1.0f;
                SceneManager.LoadScene(sceneName);
                FadeSwitch = false;
            }
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        else if (FadeSwitch == false)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            if (alpha <= 0.0f)
            {
                FadeSwitch = null;
                alpha = 0.0f;
                Destroy(gameObject);
            }
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }
}