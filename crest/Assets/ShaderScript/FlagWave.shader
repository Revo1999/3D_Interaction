Shader "Custom/FlagWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 2.0
        _WaveSpeed ("Wave Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        float _WaveAmplitude;
        float _WaveFrequency;
        float _WaveSpeed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v)
        {
            // Animate wave along Z axis based on x-position
            float weight = saturate(v.vertex.x); // 0 on the left, 1 on the right
            float wave = sin((v.vertex.x + _Time.x * _WaveSpeed) * _WaveFrequency);
            v.vertex.z += wave * _WaveAmplitude * weight;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
