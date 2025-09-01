Shader "Unlit/GridShader"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (0, 1, 1, 1) // cyan
        _GridThickness ("Grid Thickness", Range(0, 0.1)) = 0.01
        _GridSpacing ("Grid Spacing", Float) = 1.0
        _GlowIntensity ("Glow Intensity", Float) = 1.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            fixed4 _LineColor;
            float _GridThickness;
            float _GridSpacing;
            float _GlowIntensity;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 grid = abs(frac(i.worldPos.xz / _GridSpacing) - 0.5);
                float lineFact = min(grid.x, grid.y);
                float onLine = 1.0 - smoothstep(0.0, _GridThickness, lineFact);

                return _LineColor * onLine * _GlowIntensity;
            }
            ENDCG
        }
    }
}
