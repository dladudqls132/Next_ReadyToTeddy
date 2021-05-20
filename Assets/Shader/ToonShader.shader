// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ToonShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_BumpTex("NormalMap", 2D) = "bump" {}
	// Ambient light is applied uniformly to all surfaces on the object.
	[HDR]
	_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		//[HDR]
		//_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
			// Controls the size of the specular reflection.
			_Glossiness("Glossiness", Float) = 32


	}
		SubShader
	{
		  

				Tags
		{
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

		Pass
		{
		// Setup our pass to use Forward rendering, and only receive
		// data on the main directional light and ambient light.


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		// Compile multiple versions of this shader depending on lighting settings.
		#pragma multi_compile_fwdbase

		#include "UnityCG.cginc"
		// Files below include macros and functions to assist
		// with lighting and shadows.
		#include "Lighting.cginc"
		#include "AutoLight.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float4 uv : TEXCOORD0;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 worldNormal : NORMAL;
			float2 uv : TEXCOORD0;
			float3 T : TEXCOORD1;
			float3 B : TEXCOORD2;
			float3 N : TEXCOORD3;
			float3 lightDir : TEXCOORD4;
			float3 viewDir : TEXCOORD5;
			// Macro found in Autolight.cginc. Declares a vector4
			// into the TEXCOORD2 semantic with varying precision 
			// depending on platform target.
			//SHADOW_COORDS(2)
				//UNITY_FOG_COORDS(6)
				LIGHTING_COORDS(6, 7) // LIGHTING COORDS HERE ---------------------------------------
		};

		sampler2D _MainTex;
		sampler2D _BumpTex;
		float4 _MainTex_ST;
		void Fuc_LocalNormal2TBN(half3 localnormal, float4 tangent, inout half3 T, inout half3  B, inout half3 N)
		{
			half fTangentSign = tangent.w * unity_WorldTransformParams.w;
			N = normalize(UnityObjectToWorldNormal(localnormal));
			T = normalize(UnityObjectToWorldDir(tangent.xyz));
			B = normalize(cross(N, T) * fTangentSign);
		}

		half3 Fuc_TangentNormal2WorldNormal(half3 fTangnetNormal, half3 T, half3  B, half3 N)
		{
			float3x3 TBN = float3x3(T, B, N);
			TBN = transpose(TBN);
			return mul(TBN, fTangnetNormal);
		}
		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.lightDir = WorldSpaceLightDir(v.vertex);
			o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			// Defined in Autolight.cginc. Assigns the above shadow coordinate
			// by transforming the vertex from world space to shadow-map space.
			//TRANSFER_SHADOW(o)
			Fuc_LocalNormal2TBN(v.normal, v.tangent, o.T, o.B, o.N);
				UNITY_TRANSFER_FOG(o, o.vertex);
			TRANSFER_VERTEX_TO_FRAGMENT(o) // TRANSFER DONE HERE -------------------------------
			return o;
		}

		float4 _Color;

		float4 _AmbientColor;

		float4 _SpecularColor;
		float _Glossiness;

		float4 frag(v2f i) : SV_Target
		{
			//float3 normal = normalize(i.worldNormal);
			float3 viewDir = normalize(i.viewDir);

			half3 fTangnetNormal = UnpackNormal(tex2D(_BumpTex, i.uv));
			fTangnetNormal.xy *= 1.0f; // 노말강도 조절
			float3 worldNormal = Fuc_TangentNormal2WorldNormal(fTangnetNormal, i.T, i.B, i.N);
			float NdotL = dot(_WorldSpaceLightPos0, worldNormal);


			float shadow = LIGHT_ATTENUATION(i);

			float lightIntensity = lerp(0, 1, NdotL * shadow);

			float4 light = lightIntensity * _LightColor0;


			//float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
			//float NdotH = dot(normal, halfVector);

			float4 sample = tex2D(_MainTex, i.uv);

			//NdotL = dot(_WorldSpaceLightPos0, worldNormal);
			//float3 outLine = dot(viewDir, normal);
			fixed last = sample * NdotL;
			float3 diffuse = last + lightIntensity;
			diffuse = clamp(diffuse, 0.1f, 1.0f);
			diffuse = ceil((diffuse * 3)) / 3;

			float4 result = float4(diffuse.x, diffuse.y, diffuse.z, 1) + _AmbientColor;


		/*	if (outLine.x < 0.15f)
			{
				result = float4(0.0, 0.0, 0.0, result.w);

			}
			else*/
				result *= _Color;

			return (result) * sample;
		}
		ENDCG

	}



		// Shadow casting support.
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
