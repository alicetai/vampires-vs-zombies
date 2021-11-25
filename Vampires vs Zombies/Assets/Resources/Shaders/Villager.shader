// Contains lighting code adapted from Workshop 7, by Jeremy Nicholson, Chris Ewin, & Alex Zable
// https://github.com/Graphics-and-Interaction-COMP30019-2021/Workshop-7-Solution/blob/master/Assets/PhongShaderUnityLights.shader

Shader "Dames & Co/Villager_Shader"
{
	Properties
	{
		
		_SKINSMOOTHNESS("SKIN SMOOTHNESS", Range( 0 , 1)) = 0.3
		[HDR]_EYESCOLOR("EYES COLOR", Color) = (0.0734529,0.1320755,0.05046281,1)
		_EYESSMOOTHNESS("EYES SMOOTHNESS", Range( 0 , 1)) = 0.7
		[HDR]_HAIRCOLOR("HAIR COLOR", Color) = (1,0,0,0)
		_HAIRSMOOTHNESS("HAIR SMOOTHNESS", Range( 0 , 1)) = 0.1
		[HDR]_SCLERACOLOR("SCLERA COLOR", Color) = (0.9056604,0.8159487,0.8159487,0)
		_SCLERASMOOTHNESS("SCLERA SMOOTHNESS", Range( 0 , 1)) = 0.5
		[HDR]_LIPSCOLOR("LIPS COLOR", Color) = (0.8301887,0.3185886,0.2780349,0)
		_LIPSSMOOTHNESS("LIPS SMOOTHNESS", Range( 0 , 1)) = 0.4
		[HDR]_OTHERCOLOR("OTHER COLOR", Color) = (0.5188679,0.4637216,0.3206212,0)
		_OTHERSMOOTHNESS("OTHER SMOOTHNESS", Range( 0 , 1)) = 0.3
		[HDR]_METAL1COLOR("METAL 1 COLOR", Color) = (0.8792791,0.9922886,1.007606,0)
		_METAL1METALLIC("METAL 1 METALLIC", Range( 0 , 1)) = 0.65
		_METAL1SMOOTHNESS("METAL 1 SMOOTHNESS", Range( 0 , 1)) = 0.7
		[HDR]_METAL2COLOR("METAL 2 COLOR", Color) = (0.4674706,0.4677705,0.5188679,0)
		_METAL2METALLIC("METAL 2 METALLIC", Range( 0 , 1)) = 0.65
		_METAL2SMOOTHNESS("METAL 2 SMOOTHNESS", Range( 0 , 1)) = 0.7
		[HDR]_METAL3COLOR("METAL 3 COLOR", Color) = (0.4383232,0.4383232,0.4716981,0)
		_METAL3METALLIC("METAL 3 METALLIC", Range( 0 , 1)) = 0.65
		_METAL3SMOOTHNESS("METAL 3 SMOOTHNESS", Range( 0 , 1)) = 0.7
		[HDR]_METAL4COLOR("METAL 4 COLOR", Color) = (0.4383232,0.4383232,0.4716981,0)
		_METAL4METALLIC("METAL 4 METALLIC", Range( 0 , 1)) = 0.65
		_METAL4SMOOTHNESS("METAL 4 SMOOTHNESS", Range( 0 , 1)) = 0.7
		[HDR]_LEATHER1COLOR("LEATHER 1 COLOR", Color) = (0.4811321,0.2041155,0.08851016,1)
		_LEATHER1SMOOTHNESS("LEATHER 1 SMOOTHNESS", Range( 0 , 1)) = 0.3
		[HDR]_LEATHER2COLOR("LEATHER 2 COLOR", Color) = (0.4245283,0.190437,0.09011215,1)
		_LEATHER2SMOOTHNESS("LEATHER 2 SMOOTHNESS", Range( 0 , 1)) = 0.3
		[HDR]_LEATHER3COLOR("LEATHER 3 COLOR", Color) = (0.1698113,0.04637412,0.02963688,1)
		_LEATHER3SMOOTHNESS("LEATHER 3 SMOOTHNESS", Range( 0 , 1)) = 0.3
		[HDR]_LEATHER4COLOR("LEATHER 4 COLOR", Color) = (0.1698113,0.04637412,0.02963688,1)
		_LEATHER4SMOOTHNESS("LEATHER 4 SMOOTHNESS", Range( 0 , 1)) = 0.3
		[HDR]_CLOTH1COLOR("CLOTH 1 COLOR", Color) = (0,0.1792453,0.05062231,0)
		[HDR]_CLOTH2COLOR("CLOTH 2 COLOR", Color) = (1,0,0,0)
		[HDR]_CLOTH3COLOR("CLOTH 3 COLOR", Color) = (0.3962264,0.3391397,0.2710039,0)
		[HDR]_CLOTH4COLOR("CLOTH 4 COLOR", Color) = (0.2011392,0.3773585,0.3739074,0)
		[HDR]_FEATHERS1COLOR("FEATHERS 1 COLOR", Color) = (0.7735849,0.492613,0.492613,0)
		[HDR]_FEATHERS2COLOR("FEATHERS 2 COLOR", Color) = (0.6792453,0,0,0)
		_OCCLUSION("OCCLUSION", Range( 0 , 1)) = 0.5
		[Toggle]_MetalicOn("Metalic On", Float) = 1
		[Toggle]_SmoothnessOn("Smoothness On", Float) = 1
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector]_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1

		_TRANSITIONSTART("Start of transition", Float) = -1
		_TRANSITIONLENGTH("Length of transition", Float) = 3.0

		_SKINSTATE("0-Villager,1-Vampire, 2-Zombie", Int) = 0

		[HideInInspector][HDR]_LIPSCOLORTRANS("Lips trans colour", Color) = (0,0,0,1)
		[HideInInspector][HDR]_EYESCOLORTRANS("Eyes trans colour", Color) = (0,0,0,1)
		[HideInInspector][HDR]_CLOTH3COLORTRANS("Cloth3 trans colour", Color) = (0,0,0,1)
		[HideInInspector][HDR]_SKINCOLORTRANS("Skin trans colour", Color) = (0,0,0,1)
		[HideInInspector][HDR]_HAIRCOLORTRANS("Hair trans colour", Color) = (0,0,0,1)


		_ZombieBump ("Zombie bump map", 2D) = "bump" {}


	}

	SubShader
	{

		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Back
		
		// GrabPass {
		// 	"_GrabTexture"
		// }

	
		pass {

			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma exclude_renderers d3d11 gles
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			uniform float _TRANSITIONSTART;
			uniform float _TRANSITIONLENGTH;

			uniform float4 _SKINCOLOR;
			uniform float4 _SKINCOLORTRANS;

			uniform int _SKINSTATE;

			uniform float _SPARKLESEEDS[900];


			sampler2D _ZombieBump;

			//sampler2D _GrabTexture;

            struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float4 modelPos : POSITION1;
				float4 uv : TEXCOORD2;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));


				o.vertex = UnityObjectToClipPos(v.vertex);


				// use as makeshift uv-map
				o.uv = ComputeGrabScreenPos(o.vertex);

				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;
				

				return o;
			}

			float wrap(float val) {
				return val - floor(val);
			}

			float rand(float seed) {
				return wrap(sin(seed * 100000.0));
			}

			fixed4 frag(vertOut v) : SV_Target
			{

				// how far the transition is in progress, between 0 and 1. Used for lerping.
				// TransitionStart and TransitionLength are set in scripts, allows an exact start time to be specified
				float transWeight = saturate(_TRANSITIONSTART < 0 ? 0 : (_Time.y - _TRANSITIONSTART) / _TRANSITIONLENGTH);

				// Sparkle effect
			
				// float2 sparkle_centre = float2(wrap(_Time.y), wrap(_Time.z));

				// Vampire sparkles
				float3 sparkle = (0,0,0);

				if (_SKINSTATE == 1) {
					// Multiply uv coords by 5 to increase density
					float2 t = float2(wrap(v.uv.x * 5), wrap(v.uv.y * 5));


					// Convert the 2d uv into an index in [0, 900].
					uint index = min(899, floor(t.x * 30) * 30 + floor(t.y * 30));
					
					// Use this index to retrieve a random seed in [0, 2pi] passed by the scripts. Add the current time.
					// To make this sparkle pixel on only part of the time, take 0.9 so only the peak of the sin wave will be 
					// above 0, saturate, then take this 0.1 peak and multiply it by 10 to give the range [0, 1].
					// sparkle will thus pulse in and out
					// Multiplying by transWeight means the sparkles fade in as the transformation happens
					float intensity = transWeight * saturate((sin((_Time.y + _SPARKLESEEDS[index])) - 0.8) * 5);
					
					// Random state used to determine the colour of the sparkles
					float state = rand(_Time.w);

					if (state < 0.4) {
						// 40% chance to use a random shade of yellow (full red, full green, less blue)
						sparkle = float3(1, 1, rand(_Time.y * _SPARKLESEEDS[index]));
					} else if (state < 0.8) {
						// 40% chance to use a random shade of red (full red, others same but lower);
						sparkle = float3(1, rand(_Time.y * _SPARKLESEEDS[index]), rand(_Time.y * _SPARKLESEEDS[index]));
					} else {
						// 20% change to be pure white
						sparkle = float3(1,1,1);
					}

					// Indexes change at every 30th of t step, so the centre of the region with this index will be the index
					// coords + 1/60
					float2 centre = float2(floor(t.x * 30) + 0.5, floor(t.y * 30) + 0.5) / 30;
					
					//Fall off with distance from centre, to make sparkles round
					// squares are 1/30th wide, so max dist from centre to edge is 1/60
					// We want this to be 0 in cos function, so multiply length by 60 * pi/2
					float dist_falloff = saturate(cos(94.248 * length(t - centre)));

					sparkle *= intensity * dist_falloff;
				}

				//return float4(sparkle, 1);


				// Illumination + Bump map effect

				float3 normal = v.worldNormal;
				// If zombie, decode a normal from the bump map and average it with the calculated world normal
				if (_SKINSTATE == 2) {
					// Divide the UV coords by the transition weight
					float3 bump = normalize(UnpackNormal(tex2D(_ZombieBump, v.uv / transWeight)));
				
					normal = normalize((v.worldNormal + (transWeight * bump)));
				}

				// Skin colour is lerped between the current and the target
				float4 skinColour = lerp(_SKINCOLOR, _SKINCOLORTRANS, transWeight);

				// Lighting code adapted from workshop 7
				
				float Ka = 1;
				float3 amb = skinColour.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				float fAtt = 1;
				float Kd = 1;
				float3 L = _WorldSpaceLightPos0; 
				float LdotN;
				// Directional light
				if (_WorldSpaceLightPos0.w == 0)
					LdotN = dot(L, normal);
				else
					// Point light
					LdotN = dot(L - v.worldVertex.xyz, normal);
				

				float3 dif = fAtt * _LightColor0 * Kd * skinColour * saturate(LdotN); 
				
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb;
				returnColor.a = 1;

				// Add sparkle to the colour, saturating
				returnColor.xyz = saturate(returnColor.xyz + sparkle);

				return returnColor;
			}
			ENDCG
		}


		// Surface shader, adapted from the resource pack to render the clothes / non-skin features on the model
		// Adapted by us to lerp between desired colours

		CGPROGRAM

		#pragma target 3.5
		#pragma surface surf Standard alpha addshadow fullforwardshadows exclude_path:deferred 

		// provided code from resource pack

		struct Input
		{
			float2 uv_texcoord;
		};


		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float4 _FEATHERS2COLOR;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _FEATHERS1COLOR;
		uniform float4 _CLOTH4COLOR;
		uniform float4 _CLOTH2COLOR;
		uniform float4 _CLOTH1COLOR;
		uniform float4 _LEATHER4COLOR;
		uniform float4 _LEATHER3COLOR;
		uniform float4 _LEATHER2COLOR;
		uniform float4 _LEATHER1COLOR;
		uniform float4 _METAL4COLOR;
		uniform float4 _METAL3COLOR;
		uniform float4 _METAL2COLOR;
		uniform float4 _METAL1COLOR;
		uniform float4 _OTHERCOLOR;
		uniform float4 _SCLERACOLOR;
		
		uniform float _MetalicOn;
		uniform float _METAL4METALLIC;
		uniform float _METAL3METALLIC;
		uniform float _METAL2METALLIC;
		uniform float _METAL1METALLIC;
		uniform float _SmoothnessOn;
		uniform float _LEATHER4SMOOTHNESS;
		uniform float _LEATHER3SMOOTHNESS;
		uniform float _LEATHER2SMOOTHNESS;
		uniform float _LEATHER1SMOOTHNESS;
		uniform float _METAL4SMOOTHNESS;
		uniform float _METAL3SMOOTHNESS;
		uniform float _METAL2SMOOTHNESS;
		uniform float _METAL1SMOOTHNESS;
		uniform float _OTHERSMOOTHNESS;
		uniform float _LIPSSMOOTHNESS;
		uniform float _SCLERASMOOTHNESS;
		uniform float _EYESSMOOTHNESS;
		uniform float _HAIRSMOOTHNESS;
		uniform float _SKINSMOOTHNESS;
		uniform float _OCCLUSION;


		// Parts we modify
		uniform float4 _EYESCOLOR;
		uniform float4 _EYESCOLORTRANS;
		uniform float4 _HAIRCOLOR;
		uniform float4 _HAIRCOLORTRANS;
		uniform float4 _CLOTH3COLOR;
		uniform float4 _CLOTH3COLORTRANS;
		uniform float4 _LIPSCOLOR;
		uniform float4 _LIPSCOLORTRANS;

		uniform float _TRANSITIONSTART;
		uniform float _TRANSITIONLENGTH;


		void surf( Input i , inout SurfaceOutputStandard o )
		{

			// This was autogenerated by some software the resource pack maker used, we're modifying
			float transWeight = _TRANSITIONSTART < -0.2 ? 0 : (_Time.y - _TRANSITIONSTART) / _TRANSITIONLENGTH;

			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode199 = tex2D( _TextureSample2, uv_TextureSample2 );
			float4 color638 = IsGammaSpace() ? float4(0.4980392,1,1,1) : float4(0.2122307,1,1,1);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode617 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 lerpResult189 = lerp( float4( 0,0,0,0 ) , ( tex2DNode199 * _FEATHERS2COLOR ) , saturate( ( 1.0 - ( ( distance( color638.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) ));
			float4 color637 = IsGammaSpace() ? float4(0.4980392,1,0.4980392,1) : float4(0.2122307,1,0.2122307,1);
			float4 lerpResult184 = lerp( lerpResult189 , ( tex2DNode199 * _FEATHERS1COLOR ) , saturate( ( 1.0 - ( ( distance( color637.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) ));
			float4 color636 = IsGammaSpace() ? float4(0,0,1,1) : float4(0,0,1,1);
			float4 lerpResult598 = lerp( lerpResult184 , ( tex2DNode199 * _CLOTH4COLOR ) , saturate( ( 1.0 - ( ( distance( color636.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) ));
			float4 color635 = IsGammaSpace() ? float4(0,1,1,1) : float4(0,1,1,1);

			float4 cloth3 = lerp(_CLOTH3COLOR, _CLOTH3COLORTRANS, saturate(transWeight));
			float4 lerpResult171 = lerp( lerpResult598 , ( tex2DNode199 * cloth3 ) , saturate( ( 1.0 - ( ( distance( color635.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) ));

			float4 color634 = IsGammaSpace() ? float4(0,1,0,1) : float4(0,1,0,1);
			float4 lerpResult178 = lerp( lerpResult171 , ( tex2DNode199 * _CLOTH2COLOR ) , saturate( ( 1.0 - ( ( distance( color634.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) ));
			float4 color633 = IsGammaSpace() ? float4(0,0.4980392,0,1) : float4(0,0.2122307,0,1);
			float4 lerpResult173 = lerp( lerpResult178 , ( tex2DNode199 * _CLOTH1COLOR ) , saturate( ( 1.0 - ( ( distance( color633.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) ));
			float4 color632 = IsGammaSpace() ? float4(1,0.4980392,0.4980392,1) : float4(1,0.2122307,0.2122307,1);
			float temp_output_599_0 = saturate( ( 1.0 - ( ( distance( color632.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult602 = lerp( lerpResult173 , ( tex2DNode199 * _LEATHER4COLOR ) , temp_output_599_0);
			float4 color631 = IsGammaSpace() ? float4(1,1,0.4980392,1) : float4(1,1,0.2122307,1);
			float temp_output_165_0 = saturate( ( 1.0 - ( ( distance( color631.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult160 = lerp( lerpResult602 , ( tex2DNode199 * _LEATHER3COLOR ) , temp_output_165_0);
			float4 color630 = IsGammaSpace() ? float4(1,0,1,1) : float4(1,0,1,1);
			float temp_output_158_0 = saturate( ( 1.0 - ( ( distance( color630.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult167 = lerp( lerpResult160 , ( tex2DNode199 * _LEATHER2COLOR ) , temp_output_158_0);
			float4 color629 = IsGammaSpace() ? float4(1,0.4980392,1,1) : float4(1,0.2122307,1,1);
			float temp_output_157_0 = saturate( ( 1.0 - ( ( distance( color629.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult162 = lerp( lerpResult167 , ( tex2DNode199 * _LEATHER1COLOR ) , temp_output_157_0);
			float4 color628 = IsGammaSpace() ? float4(0.4980392,0.4980392,1,1) : float4(0.2122307,0.2122307,1,1);
			float temp_output_603_0 = saturate( ( 1.0 - ( ( distance( color628.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult606 = lerp( lerpResult162 , ( tex2DNode199 * _METAL4COLOR ) , temp_output_603_0);
			float4 color627 = IsGammaSpace() ? float4(0,0.4980392,0.4980392,1) : float4(0,0.2122307,0.2122307,1);
			float temp_output_117_0 = saturate( ( 1.0 - ( ( distance( color627.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult118 = lerp( lerpResult606 , ( tex2DNode199 * _METAL3COLOR ) , temp_output_117_0);
			float4 color625 = IsGammaSpace() ? float4(0,0,0.4980392,1) : float4(0,0,0.2122307,1);
			float temp_output_127_0 = saturate( ( 1.0 - ( ( distance( color625.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult128 = lerp( lerpResult118 , ( tex2DNode199 * _METAL2COLOR ) , temp_output_127_0);
			float4 color624 = IsGammaSpace() ? float4(0.4980392,0,0.4980392,1) : float4(0.2122307,0,0.2122307,1);
			float temp_output_123_0 = saturate( ( 1.0 - ( ( distance( color624.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult122 = lerp( lerpResult128 , ( tex2DNode199 * _METAL1COLOR ) , temp_output_123_0);
			float4 color623 = IsGammaSpace() ? float4(1,1,0,1) : float4(1,1,0,1);
			float temp_output_145_0 = saturate( ( 1.0 - ( ( distance( color623.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult148 = lerp( lerpResult122 , ( tex2DNode199 * _OTHERCOLOR ) , temp_output_145_0);
			float4 color622 = IsGammaSpace() ? float4(0.4980392,0.4980392,0,1) : float4(0.2122307,0.2122307,0,1);
			float temp_output_149_0 = saturate( ( 1.0 - ( ( distance( color622.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );

			float4 lips = lerp(_LIPSCOLOR, _LIPSCOLORTRANS, saturate(transWeight));
			float4 lerpResult151 = lerp( lerpResult148 , ( tex2DNode199 * lips ) , temp_output_149_0);

			float4 color621 = IsGammaSpace() ? float4(0.4980392,0.4980392,0.4980392,1) : float4(0.2122307,0.2122307,0.2122307,1);
			float temp_output_150_0 = saturate( ( 1.0 - ( ( distance( color621.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			float4 lerpResult153 = lerp( lerpResult151 , ( tex2DNode199 * _SCLERACOLOR ) , temp_output_150_0);
			float4 color618 = IsGammaSpace() ? float4(1,0,0,1) : float4(1,0,0,1);
			float temp_output_71_0 = saturate( ( 1.0 - ( ( distance( color618.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );

			float4 eyes = lerp(_EYESCOLOR, _EYESCOLORTRANS, saturate(transWeight));
			float4 lerpResult73 = lerp( lerpResult153 , ( tex2DNode199 * eyes ) , temp_output_71_0);

			float4 color619 = IsGammaSpace() ? float4(1,0.4980392,0,1) : float4(1,0.2122307,0,1);
			float temp_output_67_0 = saturate( ( 1.0 - ( ( distance( color619.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );

			float4 hair = lerp(_HAIRCOLOR, _HAIRCOLORTRANS, saturate(transWeight));
			float4 lerpResult69 = lerp( lerpResult73 , ( tex2DNode199 * hair ) , temp_output_67_0);

			float4 color620 = IsGammaSpace() ? float4(0.4980392,0,0,1) : float4(0.2122307,0,0,1);
			float temp_output_63_0 = saturate( ( 1.0 - ( ( distance( color620.rgb , tex2DNode617.rgb ) - 0.1 ) / max( 0.0 , 1E-05 ) ) ) );
			


			float lerpResult610 = lerp( 0.0 , _METAL4METALLIC , temp_output_603_0);
			float lerpResult315 = lerp( lerpResult610 , _METAL3METALLIC , temp_output_117_0);
			float lerpResult319 = lerp( lerpResult315 , _METAL2METALLIC , temp_output_127_0);
			float lerpResult316 = lerp( lerpResult319 , _METAL1METALLIC , temp_output_123_0);
			o.Metallic = (( _MetalicOn )?( lerpResult316 ):( 0.0 ));
			float lerpResult612 = lerp( 0.0 , _LEATHER4SMOOTHNESS , temp_output_599_0);
			float lerpResult336 = lerp( lerpResult612 , _LEATHER3SMOOTHNESS , temp_output_165_0);
			float lerpResult332 = lerp( lerpResult336 , _LEATHER2SMOOTHNESS , temp_output_158_0);
			float lerpResult334 = lerp( lerpResult332 , _LEATHER1SMOOTHNESS , temp_output_157_0);
			float lerpResult608 = lerp( lerpResult334 , _METAL4SMOOTHNESS , temp_output_603_0);
			float lerpResult327 = lerp( lerpResult608 , _METAL3SMOOTHNESS , temp_output_117_0);
			float lerpResult331 = lerp( lerpResult327 , _METAL2SMOOTHNESS , temp_output_127_0);
			float lerpResult328 = lerp( lerpResult331 , _METAL1SMOOTHNESS , temp_output_123_0);
			float lerpResult321 = lerp( lerpResult328 , _OTHERSMOOTHNESS , temp_output_145_0);
			float lerpResult325 = lerp( lerpResult321 , _LIPSSMOOTHNESS , temp_output_149_0);
			float lerpResult322 = lerp( lerpResult325 , _SCLERASMOOTHNESS , temp_output_150_0);
			float lerpResult306 = lerp( lerpResult322 , _EYESSMOOTHNESS , temp_output_71_0);
			float lerpResult304 = lerp( lerpResult306 , _HAIRSMOOTHNESS , temp_output_67_0);
			float lerpResult302 = lerp( lerpResult304 , _SKINSMOOTHNESS , temp_output_63_0);
			o.Smoothness = (( _SmoothnessOn )?( lerpResult302 ):( 0.0 ));
			o.Occlusion = (1.0 + (_OCCLUSION - 0.0) * (0.5 - 1.0) / (1.0 - 0.0));
			

			// Dames & Co Added

			//Set the transparency of everything else to 1, so the skin doesn't show through
			lerpResult69.a = 1;			
			// Set the skin to completely transparent so our first pass shows through
			float4 transparentSkin = lerp( lerpResult69 , ( tex2DNode199 * float4(0,0,0,0) ) , temp_output_63_0);
			
			// float4 villagerSkin = lerp( lerpResult69 , ( tex2DNode199 * _VILLAGERSKIN ) , temp_output_63_0);
			

			o.Albedo = transparentSkin.rgb;
			o.Alpha = transparentSkin.a;
		}

		ENDCG

	}
	Fallback "Diffuse"

	
}
