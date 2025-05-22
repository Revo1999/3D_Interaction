Shader "Custom/GrassWiggle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WiggleSpeed ("Wiggle Speed", Float) = 2
        _WiggleStrength ("Wiggle Strength", Float) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        Lighting Off
        ZWrite On
        Cull Off
        AlphaTest Greater 0.6

        Pass
        {
            Name "WiggleGrass"
            Tags { "LightMode"="Always" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WiggleSpeed;
            float _WiggleStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Add per-instance phase offset based on world XZ position
                float phaseOffset = (worldPos.x + worldPos.z) * 3.1415;

                float wave = sin(_Time.y * _WiggleSpeed + phaseOffset + worldPos.y * 4.0);

                float3 pos = v.vertex.xyz;
                pos.x += wave * _WiggleStrength * saturate(pos.y); // Wiggle in X axis

                o.vertex = UnityObjectToClipPos(float4(pos, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.6);
                return col;
            }
            ENDCG
        }
    }

    FallBack Off
}
