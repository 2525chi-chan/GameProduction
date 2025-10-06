Shader "Custom/CharacterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowThreshold ("Shadow Threshold", Range(0, 1)) = 0.5
        _ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.8, 1)
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShadowThreshold;
            float4 _ShadowColor;
            float4 _LightColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // テクスチャカラー
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // 光方向
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                
                // ハーフランバート（0〜1の滑らかな値）
                float halfLambert = dot(normalize(i.normalDir), lightDir) * 0.5 + 0.5;
                
                // 【ここがポイント】step関数で二値化してパキッと！
                float shadowMask = step(_ShadowThreshold, halfLambert);
                
                // 影色と通常色をlerp（線形補間）で切り替え
                fixed4 finalColor = lerp(texColor * _ShadowColor, texColor * _LightColor, shadowMask);
                
                return finalColor;
            }
            ENDCG
        }
    }
}
