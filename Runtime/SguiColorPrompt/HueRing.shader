Shader "Unlit/HueRing"
{
    Properties
    {
        _OuterRadius ("Outer Radius", Range(0,1)) = 0.48
        _InnerRadius ("Inner Radius", Range(0,1)) = 0.32
        _Feather     ("Edge Feather", Range(0,0.05)) = 0.01
        _Alpha       ("Ring Alpha", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            float _OuterRadius, _InnerRadius, _Feather, _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 p = i.uv * 2.0 - 1.0;   // centre = (0,0)
                float  r = length(p);

                // alpha anneau
                float aOuter = 1.0 - smoothstep(_OuterRadius - _Feather, _OuterRadius + _Feather, r);
                float aInner = smoothstep(_InnerRadius - _Feather, _InnerRadius + _Feather, r);
                float a = saturate(aOuter * aInner) * _Alpha;

                // Hue par angle
                float ang = atan2(p.y, p.x); // [-pi..pi]
                float hue = frac(ang / (2.0 * 3.14159265) + 1.0);

                float3 rgb = hsv2rgb(float3(hue, 1.0, 1.0));
                return fixed4(rgb, a); // centre transparent
            }
            ENDCG
        }
    }
}