Shader "Custom/ChromaKeyGreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _KeyColor ("Key Color", Color) = (0,1,0,1)
        _Threshold ("Threshold", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _KeyColor;
            float _Threshold;

            v2f vert (appdata v) { v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.uv, _MainTex); return o; }

         fixed4 frag (v2f i) : SV_Target
{
    fixed4 col = tex2D(_MainTex, i.uv);
    float dist = distance(col.rgb, _KeyColor.rgb);

    float alpha = saturate((dist - 0.0) / _Threshold);
    col.a = alpha;

    // 透明に近いほど「緑 → 他の色」に寄せる
    float edge = 1.0 - alpha;
    float avg = (col.r + col.b) * 0.5;
    col.g = lerp(col.g, avg, edge);  // edge が大きいほど緑を減らす

    return col;
}


            ENDCG
        }
    }
}
