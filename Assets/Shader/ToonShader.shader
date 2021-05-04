// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ToonShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
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
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 worldNormal : NORMAL;
			float2 uv : TEXCOORD0;
			float3 viewDir : TEXCOORD1;
			// Macro found in Autolight.cginc. Declares a vector4
			// into the TEXCOORD2 semantic with varying precision 
			// depending on platform target.
			//SHADOW_COORDS(2)
				UNITY_FOG_COORDS(2)
				LIGHTING_COORDS(3, 4) // LIGHTING COORDS HERE ---------------------------------------
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.viewDir = WorldSpaceViewDir(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			// Defined in Autolight.cginc. Assigns the above shadow coordinate
			// by transforming the vertex from world space to shadow-map space.
			//TRANSFER_SHADOW(o)
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
			float3 normal = normalize(i.worldNormal);
			float3 viewDir = normalize(i.viewDir);

			float NdotL = dot(_WorldSpaceLightPos0, normal);


			float shadow = LIGHT_ATTENUATION(i);

			float lightIntensity = lerp(0, 1, NdotL * shadow);

			float4 light = lightIntensity * _LightColor0;


			float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
			float NdotH = dot(normal, halfVector);

			float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
			float specularIntensitySmooth = specularIntensity > 0.5 ? 1 : 0;
			float4 specular = specularIntensitySmooth * _SpecularColor;

			float4 sample = tex2D(_MainTex, i.uv);

			float3 lightDir = normalize(_WorldSpaceLightPos0 - i.pos);

			float3 outLine = dot(viewDir, normal);
			float3 diffuse = lightIntensity;
			diffuse = ceil((diffuse * 2)) / 2;

			float4 result = float4(diffuse.x, diffuse.y, diffuse.z, 1) + _AmbientColor;


			if (outLine.x < 0.15f)
			{
				result = float4(0.0, 0.0, 0.0, result.w);

			}
			else
				result *= _Color;

			return (result) * sample;
		}
		ENDCG
	}

		// Shadow casting support.
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
