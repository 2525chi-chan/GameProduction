Shader "Custom/SwampSurfaceNoise"
{
    Properties
    {
        _LowColor("Low Color", Color) = (0.05, 0.2, 0.05, 1)
        _HighColor("High Color", Color) = (0.2, 0.6, 0.2, 1)
        _NoiseMap("Noise Map", 2D) = "white" {}
        _Shininess("Shininess", Range(1,100)) = 20
        _Gloss("Gloss", Range(0,1)) = 0.3
        _Threshold("Threshold", Range(0,1)) = 0.5
        _Speed("Speed", float)=20
         _NoiseScale("Noise Scale", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
             Cull Off
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
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };
            sampler2D _NoiseMap;
            fixed4 _LowColor;
            fixed4 _HighColor;
          
           float _Speed;
            float _Shininess;
            float _Gloss;
            float _Threshold;
          float _NoiseScale;

           

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                 o.uv = v.uv * _NoiseScale + float2(_Time.y * _Speed, _Time.y*_Speed);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // --- ノイズ値を取得 ---
              
                float4 color=tex2D(_NoiseMap, i.uv);
                float3 baseCol=float3(0,0,0);
                float height=(color.r+color.g+color.b)/3;

                baseCol=lerp(_LowColor.rgb,_HighColor.rgb,height);
              
              
                

                // --- 簡単なライティング ---
                float NdotL = max(0, dot(normal, lightDir));
                fixed3 diffuse = baseCol * NdotL;

                float3 halfDir = normalize(lightDir + viewDir);
                float NdotH = max(0, dot(normal, halfDir));
                float spec = pow(NdotH, _Shininess) * _Gloss;

                float fres = pow(1 - saturate(dot(viewDir, normal)), 6.0);

                fixed3 col = diffuse + spec + fres * 0.05;
                     i.uv.xy+=_Time.y*_Speed;
                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
