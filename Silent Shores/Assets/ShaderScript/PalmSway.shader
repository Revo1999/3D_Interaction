Shader "Custom/PalmSway"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SwayStrength ("Sway Strength", Float) = 0.1
        _SwaySpeed ("Sway Speed", Float) = 1.0
        _BaseHeight ("Base Height", Float) = 0.0
        _TopHeight ("Top Height", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        sampler2D _MainTex;
        float _SwayStrength;
        float _SwaySpeed;
        float _BaseHeight;
        float _TopHeight;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v)
        {
            float time = _Time.y * _SwaySpeed;

            // Normalized height between base and top (shared across trunk/leaves)
            float heightNormalized = saturate((v.vertex.y - _BaseHeight) / (_TopHeight - _BaseHeight));

            float sway = sin(time + v.vertex.x * 0.5) * _SwayStrength * heightNormalized;

            v.vertex.x += sway;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
