Shader "Unlit/LineRepeated"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DotColor ("DotColor", Color) = (1.0, 1.0, 1.0, 1.0)
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _DotColor;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 objectOrigin = UnityObjectToClipPos(float4(0, 0, 0, 1));
                float d = distance(objectOrigin, i.vertex);
                // sample the texture
                fixed4 col = fixed4(255, 255, 255, 255);
                return col;
            }
            ENDCG
        }
    }
}
