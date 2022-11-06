using UnityEngine;

public class LaunchingScene : MonoBehaviour
{
    public GameObject fade;
    public string scene;
    public float speed = 1.0f;

    private bool loading = false;
    private float d_time = 0.0f;

    void Update()
    {
        d_time += Time.deltaTime;
 
        if (d_time >= 2.0f && !loading)
        {
            GameObject fadeCanvas = Instantiate(fade);
            fadeCanvas.GetComponent<FadeManager>().fadeStart(scene,speed);
            loading = true;
        }
    }
}
