Shader "Custom/MoonNoFog"
{
    Properties
    {
        _MainColor ("Main Red Color", Color) = (0.8, 0.05, 0.0, 1)
        _DarkColor ("Dark Shadow Color", Color) = (0.2, 0.0, 0.0, 1)
        _RimColor ("Rim Glow Color", Color) = (1.0, 0.15, 0.0, 1)
        _RimPower ("Rim Glow Size", Range(0.5, 8.0)) = 2.0
        _LightDir ("Light Direction", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma nofog
            #include "UnityCG.cginc"

            fixed4 _MainColor;
            fixed4 _DarkColor;
            fixed4 _RimColor;
            float _RimPower;
            float4 _LightDir;

            struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; };
            struct v2f { float4 vertex : SV_POSITION; float3 normal : TEXCOORD0; float3 viewDir : TEXCOORD1; };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.normal);
                float3 lightDir = normalize(_LightDir.xyz);

                // Basic diffuse shading
                float diff = saturate(dot(normal, lightDir));
                fixed4 color = lerp(_DarkColor, _MainColor, diff);

                // Rim glow around edges
                float rim = 1.0 - saturate(dot(i.viewDir, normal));
                rim = pow(rim, _RimPower);
                color += _RimColor * rim;

                return color;
            }
            ENDCG
        }
    }
}