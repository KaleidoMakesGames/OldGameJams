Shader "Unlit/SpriteChromaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 0)
        [HideInInspector] _LineLength ("Line Length", float) = 1
        [HideInInspector] _LineWidth ("Line Width", float) = 1
        _ScrollSpeed ("Scroll Speed", float) = 0
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 objectPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _ScrollSpeed;
            float _LineWidth;
            fixed4 _Color;
            float _LineLength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.objectPos = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float distance = i.objectPos.y + _LineLength/2;

                float aspect = _MainTex_TexelSize.z/_MainTex_TexelSize.w;
                float imageWidth = _LineWidth;
                float imageHeight = imageWidth / aspect;
                int numArrows =  floor(_LineLength / imageHeight);

                fixed4 transparent = {0, 0, 0, 0};

                float cap = numArrows * imageHeight;
                if(distance > cap) {
                    return transparent;
                }

                float relativeX = (i.objectPos.x/_LineWidth + 0.5);
                // sample the texture
                float2 uv = {relativeX, distance/imageHeight + _Time.y * _ScrollSpeed};
                fixed4 col = tex2D(_MainTex, uv);

                //if(projectedDistance % _DistanceRepeat >= _DistanceRepeat/2) {
                //    return transparent;
                //}
                return col * _Color;
            }
            ENDCG
        }
    }
}
