Shader "Unlit/SpriteChromaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ChromaColor ("Chroma Color", Color) = (1, 1, 1, 1)
        _ChromaWidth ("Chroma Tolerance", Float) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
		"Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _ChromaColor;
            float _ChromaWidth;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 diff = col - _ChromaColor;
                fixed4 transparent = {0, 0, 0, 0};
                float sum = diff.r + diff.g + diff.b + diff.a;
                if(abs(sum) <= _ChromaWidth) {
                    return transparent;
                }
                return col * i.color;
            }
            ENDCG
        }
    }
}
