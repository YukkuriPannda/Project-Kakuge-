using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class Bloom : AbstractImageEffect  
{
    // Bloomの強度  
    [Range(0,1f)] public float strength = 0.3f;  
    [Range(1,12)] public int samplerCnt = 6;  
    // ブラーの強度  
    [Range(1,64)] public int blur = 20;  
    // 明るさのしきい値  
    [Range(0,1f)] public float threshold = 0.3f;  
    // RenderTextureサイズの分母  
    [Range(1,12)] public int ratio = 1;  

    protected override void _OnRenderImage (RenderTexture src, RenderTexture dest)  
    {
        RenderTexture tmp = RenderTexture.GetTemporary (src.width / ratio, src.height / ratio, 0, RenderTextureFormat.ARGB32);  
        tmp.filterMode = FilterMode.Bilinear;  

        m_mat.SetFloat ("_SamplerCnt", samplerCnt);  
        m_mat.SetFloat ("_Strength", strength);  
        m_mat.SetFloat ("_Threshold", threshold);  
        m_mat.SetFloat ("_Blur", blur);  
        m_mat.SetTexture ("_Tmp", tmp);  
        Graphics.Blit (src, tmp, m_mat, 0);  

        // ぼかし + 合成  
        Graphics.Blit (src, dest, m_mat, 1);  

        RenderTexture.ReleaseTemporary (tmp);  
    }
}