Shader "Custom/URPLitDistanceEmission"
{
    Properties
    {
        _BaseMap ("BaseMap", 2D) = "white" {}
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Float) = 1.0
        _MaxDistance ("Max Emission Distance", Float) = 20.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        Pass
        {
            Name "ForwardLit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BaseMap);
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionStrength;
            float _MaxDistance;
            float _Smoothness;
            float _Metallic;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            struct SurfaceData
            {
                float3 albedo;
                float3 emission;
                float metallic;
                float perceptualRoughness;
                float3 normalWS;
                float occlusion;
            };

            half4 frag(Varyings IN) : SV_Target
            {
                SurfaceData surf;
                float3 camPos = _WorldSpaceCameraPos;
                float dist = distance(IN.worldPos, camPos);

                // 距離でエミッション減衰
                float atten = saturate(1.0 - dist / _MaxDistance);

                float4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                surf.albedo = texColor.rgb * _BaseColor.rgb;
                surf.metallic = _Metallic;
                surf.perceptualRoughness = 1.0 - _Smoothness;
                surf.normalWS = float3(0, 0, 1);
                surf.occlusion = 1.0;
                surf.emission = _EmissionColor.rgb * _EmissionStrength * atten;

                // PBR出力（ShaderにはLightingモデルを自前で追加する必要あり）

                // 簡易的なUnlit的出力（エミッションのみ反映する場合）
                float3 color = surf.albedo + surf.emission;

                // 通常はUniversalFragmentPBR(surf, ...)などPBR関数を呼ぶ
                return half4(color, 1);
            }
            ENDHLSL
        }
    }
}
