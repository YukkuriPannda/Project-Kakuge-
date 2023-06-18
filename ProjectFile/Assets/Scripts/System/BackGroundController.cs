using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundController : MonoBehaviour
{
    private float length, startpos;
    private GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        // 背景画像のx座標
        startpos = transform.position.x;
        // 背景画像のx軸方向の幅
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.gameObject;
    }

    private void Update()
    {
        // 無限スクロールに使用するパラメーター
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        // 背景の視差効果に使用するパラメーター
        float dist = (cam.transform.position.x * parallaxEffect);

        // 視差効果を与える処理
        // 背景画像のx座標をdistの分移動させる
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // 無限スクロール
        // 画面外になったら背景画像を移動させる
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }

}
