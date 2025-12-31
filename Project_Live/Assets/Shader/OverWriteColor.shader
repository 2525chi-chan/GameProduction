Shader "Custom/OverWriteColor"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite", 2D) = "white" {}
        [MainColor]   _Color1   ("Gradient Color 1", Color) = (1,1,1,1)
        [MainColor]   _Color2   ("Gradient Color 2", Color) = (0,1,1,1)
        _GradientSpeed ("Gradient Speed", Range(0, 5)) = 1.0
        _GradientDirX ("Gradient Direction X", Range(-1, 1)) = 1.0
        _GradientDirY ("Gradient Direction Y", Range(-1, 1)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color1;
                float4 _Color2;
                float _GradientSpeed;
                float _GradientDirX;
                float _GradientDirY;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos    : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos   = TransformObjectToHClip(v.vertex.xyz);
                o.uv    = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                // UVベースのグラデーション位置
                float2 gradientUV = i.uv;
                
                // 時間で移動するグラデーション
                float timeOffset = _Time.y * _GradientSpeed;
                float gradientPos = dot(gradientUV, float2(_GradientDirX, _GradientDirY)) + timeOffset;
                
                // 0-1のグラデーション値
                float gradientFactor = frac(gradientPos);
                
                // 2色間Lerp
                float3 gradientColor = lerp(_Color1.rgb, _Color2.rgb, gradientFactor);
                
                return half4(gradientColor, texColor.a * _Color1.a);
            }
            ENDHLSL
        }
    }
}
