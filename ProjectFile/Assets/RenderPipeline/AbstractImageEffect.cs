using UnityEngine;  
/// <summary>  
/// イメージエフェクトをサクッと試す用ベースクラス  
/// </summary>  
[ExecuteInEditMode]  
public abstract class AbstractImageEffect : MonoBehaviour  
{
    Shader m_shader;  
    protected Material m_mat;  
    public string shaderName;  

    void OnRenderImage(RenderTexture src, RenderTexture dest)  
    {
        if (m_mat == null)  
        {
            m_shader = Shader.Find(shaderName);  
            m_mat = new Material(m_shader);  
            m_mat.hideFlags = HideFlags.DontSave;  
        }
        _OnRenderImage(src, dest);  
    }
    protected virtual void _OnRenderImage (RenderTexture src, RenderTexture dest){Graphics.Blit(src, dest, m_mat);}  
}