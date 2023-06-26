Shader "Hidden/Bloom"  
{
    Properties  
    {
        _MainTex ("Texture", 2D) = "white" {}  
    }

    SubShader  
    {
        Cull Off ZWrite Off ZTest Always  

        CGINCLUDE  
        #include "UnityCG.cginc"  
        sampler2D _MainTex;  
        sampler2D _Tmp;  
        float _Strength;  
        float _SamplerCnt;  
        float _Blur;  
        float _Threshold;  

        fixed4 frag0 (v2f_img i) : SV_Target  
        {
            fixed4 col = tex2D(_MainTex, i.uv);  
            // ピクセルの明るさ  
            float bright = (col.r + col.g + col.b)/3;  
            // 0 or 1  
            float tmp = step(_Threshold, bright);  
            return tex2D(_MainTex, i.uv) * tmp * _Strength;  
        }

        fixed4 fragBlur (v2f_img i) : SV_Target  
        {
            float u = 1 / _ScreenParams.x;  
            float v = 1 / _ScreenParams.y;  

            fixed4 result;  
            // ぼかし  
            for (float x = 0; x < _Blur; x++)  
            {
                float xx = i.uv.x + (x - _Blur/2) * u;  

                for (float y = 0; y < _Blur; y++)  
                {
                    float yy = i.uv.y + (y - _Blur/2) * v;  
                    fixed4 smp = tex2D(_Tmp, float2(xx, yy));  
                    result += smp;  
                }
            }

            result /= _Blur * _Blur;  
            return tex2D(_MainTex, i.uv) + result;  
        }

        ENDCG  

        // Pass 0 bright sampling  
        Pass  
        {
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment frag0  
            ENDCG  
        }

        // Pass 1 blur & 合成  
        Pass  
        {
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment fragBlur  
            ENDCG  
        }
    }
}