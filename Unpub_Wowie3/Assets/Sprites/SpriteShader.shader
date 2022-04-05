Shader "Unlit/SpriteShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_local _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				float3 rgb_to_hsv(float3 RGB)
				{
				float3 HSV;

				float minChannel, maxChannel;
				if (RGB.x > RGB.y) {
				maxChannel = RGB.x;
				minChannel = RGB.y;
				}
				else {
				maxChannel = RGB.y;
				minChannel = RGB.x;
				}

				if (RGB.z > maxChannel) maxChannel = RGB.z;
				if (RGB.z < minChannel) minChannel = RGB.z;

				HSV.xy = 0;
				HSV.z = maxChannel;
				float delta = maxChannel - minChannel;             //Delta RGB value
				if (delta != 0) {                    // If gray, leave H  S at zero
				HSV.y = delta / HSV.z;
				float3 delRGB;
				delRGB = (HSV.zzz - RGB + 3 * delta) / (6.0*delta);
				if (RGB.x == HSV.z) HSV.x = delRGB.z - delRGB.y;
				else if (RGB.y == HSV.z) HSV.x = (1.0 / 3.0) + delRGB.x - delRGB.z;
				else if (RGB.z == HSV.z) HSV.x = (2.0 / 3.0) + delRGB.y - delRGB.x;
				}
				return (HSV);
				}

				float3 hsv_to_rgb(float3 HSV)
				{
				float3 RGB = HSV.z;

				float var_h = HSV.x * 6;
				float var_i = floor(var_h);   // Or ... var_i = floor( var_h )
				float var_1 = HSV.z * (1.0 - HSV.y);
				float var_2 = HSV.z * (1.0 - HSV.y * (var_h - var_i));
				float var_3 = HSV.z * (1.0 - HSV.y * (1 - (var_h - var_i)));
				if (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
				else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
				else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
				else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
				else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
				else { RGB = float3(HSV.z, var_1, var_2); }

				return (RGB);
				}

				v2f vert(appdata_t IN)
				{
					v2f OUT;

					UNITY_SETUP_INSTANCE_ID(IN);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

					OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
					OUT.vertex = UnityObjectToClipPos(OUT.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color;

					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord);
					float hue = rgb_to_hsv(IN.color).r;
					float3 textureHSV = rgb_to_hsv(c.rgb);
					textureHSV.r = hue;
					c.a *= IN.color.a;
					c.rgb = hsv_to_rgb(textureHSV) * c.a;
					return c;
				}
			ENDCG
			}
		}
}