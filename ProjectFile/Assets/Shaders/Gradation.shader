Shader "Gradation/Gradation" {
    Properties{
        _Color1("Color 1", Color) = (1,1,1,1)
        _Color2("Color 2", Color) = (1,1,1,1)
        _Color1Pos("Top Color Pos", Range(0, 1)) = 1 //初期値は1
        _Color1Amount("Top Color Amount", Range(0, 1)) = 0.5 //初期値は0.5
    }
        SubShader{
            Tags { 
        "RenderType" = "Opaque" 
        "IgnoreProjector" = "True"
        "Queue" = "Transparent"
         }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
            LOD 100

            Pass{
            CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

            fixed4 _Color1;
            fixed4 _Color2;
            fixed _Color1Pos;
            fixed _Color1Amount;

            struct appdata {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
            };
            struct v2f {
                half4 vertex : POSITION;
                fixed4 color : COLOR;
                half2 uv : TEXCOORD0;
            };

            v2f vert(appdata v) {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }
            fixed4 frag(v2f i) : COLOR{
                fixed amount = clamp(abs(_Color1Pos - i.uv.y) + (0.5 - _Color1Amount), 0, 1);
                i.color = lerp(_Color1, _Color2, amount);

                return i.color;
            }
            ENDCG
        }
    }
}