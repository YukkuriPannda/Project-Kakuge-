using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchingScene : MonoBehaviour
{
    private float d_time;
    // Start is called before the first frame update
    void Start()
    {
        d_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        d_time += Time.deltaTime;
 
        // 3秒後に画面遷移（scene2へ移動）
        if (d_time >= 3.0f)
        {
            SceneManager.LoadSceneAsync("Scenes/InGameTest2");
        }
    }
}
