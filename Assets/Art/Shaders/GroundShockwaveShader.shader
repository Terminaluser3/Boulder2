Shader "Unlit/GroundShockwaveShader"
{
    Properties{
        _Color ("Color", Color) = (1,1,1,1)
        _RingThickness ("Ring Thickness", Range(0, 0.5)) = 0.1
        _FadeAmount ("Fade Amount", Range(0, 1)) = 0
    }

    SubShader{
        Tags{ "Que"="Transparent" "RenderType"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float _RingThickness;
            float _FadeAmount;

            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                // Ring and fade logic
                float dist = distance(i.uv, float2(0.5, 0.5));
                float ring = smoothstep(0.5 - _RingThickness, 0.5, dist) - smoothstep(0.5, 0.5 + _RingThickness, dist);

                fixed4 col = _Color * ring;
                col.a *= (1.0 - _FadeAmount);

                return col;
            }
            ENDCG
        }
    }
}