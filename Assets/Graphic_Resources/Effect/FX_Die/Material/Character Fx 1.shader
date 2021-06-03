// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Character Fx"
{
	Properties
	{
		_DiffuseMap("Diffuse Map", 2D) = "white" {}
		_EmissionMap("Emission Map", 2D) = "white" {}
		_Emissionval("Emission val", Range( 0 , 3)) = 0
		_SmoothnessVal("Smoothness Val", Range( 0 , 1)) = 0
		_MetalicMap("Metalic Map", 2D) = "white" {}
		_MetalicVal("Metalic Val", Range( 0 , 1)) = 0
		_NormalMap("Normal Map", 2D) = "bump" {}
		_FresnelScale("Fresnel Scale", Range( 1 , 5)) = 1.188235
		_FresnelPow("Fresnel Pow", Range( 1 , 5)) = 5
		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		_FresnelIns("Fresnel Ins", Range( 0 , 10)) = 5.545021
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		_EdgeRange("Edge Range", Range( 0.5 , 1)) = 0.81
		_EdgeColor("Edge Color", Color) = (0,0,0,0)
		_EdgeIns("Edge Ins", Range( 1 , 10)) = 0
		_X("X", Float) = 0
		_Y("Y", Float) = 0
		_Z("Z", Float) = 0
		_VertexVal("Vertex Val", Float) = 0
		_VertexPow("Vertex Pow", Float) = 0
		_Far("Far", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite On
		AlphaToMask On
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 2.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _Far;
		uniform float _X;
		uniform float _Y;
		uniform float _Z;
		uniform float _VertexVal;
		uniform float _VertexPow;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _DiffuseMap;
		uniform float4 _DiffuseMap_ST;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float _Emissionval;
		uniform float _FresnelScale;
		uniform float _FresnelPow;
		uniform float4 _FresnelColor;
		uniform float _FresnelIns;
		uniform float _EdgeIns;
		uniform float4 _EdgeColor;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _Dissolve;
		uniform float _EdgeRange;
		uniform sampler2D _MetalicMap;
		uniform float4 _MetalicMap_ST;
		uniform float _MetalicVal;
		uniform float _SmoothnessVal;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 temp_cast_0 = (_Far).xxx;
			float3 appendResult37 = (float3(_X , _Y , _Z));
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 lerpResult46 = lerp( temp_cast_0 , ( appendResult37 - ase_vertex3Pos ) , pow( saturate( ( distance( appendResult37 , ase_vertex3Pos ) - _VertexVal ) ) , _VertexPow ));
			v.vertex.xyz += lerpResult46;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_DiffuseMap = i.uv_texcoord * _DiffuseMap_ST.xy + _DiffuseMap_ST.zw;
			o.Albedo = tex2D( _DiffuseMap, uv_DiffuseMap ).rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV4 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode4 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV4, _FresnelPow ) );
			float2 uv_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			float temp_output_18_0 = saturate( ( tex2D( _DissolveTexture, uv_DissolveTexture ).r + _Dissolve ) );
			float temp_output_20_0 = step( 0.5 , temp_output_18_0 );
			o.Emission = ( ( ( tex2D( _EmissionMap, uv_EmissionMap ) * _Emissionval ) + ( ( saturate( fresnelNode4 ) * _FresnelColor ) * _FresnelIns ) ) + ( _EdgeIns * ( _EdgeColor * saturate( ( temp_output_20_0 - step( _EdgeRange , temp_output_18_0 ) ) ) ) ) ).rgb;
			float2 uv_MetalicMap = i.uv_texcoord * _MetalicMap_ST.xy + _MetalicMap_ST.zw;
			o.Metallic = ( tex2D( _MetalicMap, uv_MetalicMap ) + _MetalicVal ).r;
			o.Smoothness = _SmoothnessVal;
			o.Alpha = temp_output_20_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1920;36;1920;983;-553.5679;269.323;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1180.754,622.0681;Inherit;False;0;15;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-932.9731,548.2155;Inherit;True;Property;_DissolveTexture;Dissolve Texture;12;0;Create;True;0;0;0;False;0;False;-1;None;bc7c2443c12109d4397562c7053fad44;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-928.4631,814.8674;Float;False;Property;_Dissolve;Dissolve;13;0;Create;True;0;0;0;False;0;False;0;0.65;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;14;-1209.9,4.800003;Inherit;False;1229.871;479;Fresnel;8;4;6;7;8;9;12;13;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-577.7538,646.0681;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;18;-364.9538,640.1682;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-234.8588,644.7414;Float;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1159.9,54.8;Float;False;Property;_FresnelScale;Fresnel Scale;8;0;Create;True;0;0;0;False;0;False;1.188235;5;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-550.6587,887.642;Float;False;Property;_EdgeRange;Edge Range;14;0;Create;True;0;0;0;False;0;False;0.81;0.5;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-29.92525,1000.807;Float;False;Property;_Y;Y;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-29.92507,905.6503;Float;False;Property;_X;X;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-19.63799,1080.533;Float;False;Property;_Z;Z;19;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1152.9,130.8;Float;False;Property;_FresnelPow;Fresnel Pow;9;0;Create;True;0;0;0;False;0;False;5;5;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;4;-878.9,62.8;Inherit;True;Standard;TangentNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;20;-163.5184,482.4044;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;40;87.27791,1183.706;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;37;208.3996,950.6654;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;24;-248.9458,889.501;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-689.8999,276.8;Float;False;Property;_FresnelColor;Fresnel Color;10;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,0.1136363,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;391.2779,1432.706;Float;False;Property;_VertexVal;Vertex Val;20;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;149.1328,708.7891;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;38;353.2779,1201.706;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;8;-625.9,61.8;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;298.5118,534.4026;Float;False;Property;_EdgeColor;Edge Color;15;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.1137254,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-444.5,69.89999;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-286.4321,-176.323;Inherit;False;Property;_Emissionval;Emission val;3;0;Create;True;0;0;0;False;0;False;0;3;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;350.5118,713.4026;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-427.9294,273.7771;Float;False;Property;_FresnelIns;Fresnel Ins;11;0;Create;True;0;0;0;False;0;False;5.545021;3.31;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;609.5479,1263.334;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-325.2015,-418.5664;Inherit;True;Property;_EmissionMap;Emission Map;2;0;Create;True;0;0;0;False;0;False;-1;None;a15835ff218a3dc428d6acaf26ebd1fe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;526.5118,540.4026;Float;False;Property;_EdgeIns;Edge Ins;16;0;Create;True;0;0;0;False;0;False;0;10;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;24.56793,-270.323;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;43;852.6391,1259.002;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-215.0294,64.9771;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;532.5118,653.4026;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;843.0688,1562.086;Float;False;Property;_VertexPow;Vertex Pow;21;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;1239.601,305.9332;Inherit;True;Property;_MetalicMap;Metalic Map;5;0;Create;True;0;0;0;False;0;False;-1;edfbc62aed2a56a4fb40782b3d32204b;edfbc62aed2a56a4fb40782b3d32204b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;1242.361,510.553;Inherit;False;Property;_MetalicVal;Metalic Val;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;1053.396,892.3422;Float;False;Property;_Far;Far;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;463.0524,47.19742;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;822.5118,313.3196;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;1001.42,1008.88;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;44;1039.454,1292.462;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-220.6075,-822.4792;Inherit;True;Property;_DiffuseMap;Diffuse Map;1;0;Create;True;0;0;0;False;0;False;-1;None;3be531dac9fc36341b56370f23845ec7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-219.6075,-632.4788;Inherit;True;Property;_NormalMap;Normal Map;7;0;Create;True;0;0;0;False;0;False;-1;None;70f6838039305994b9a22b0585c67f53;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;1489.789,619.9181;Inherit;False;Property;_SmoothnessVal;Smoothness Val;4;0;Create;True;0;0;0;False;0;False;0;0.907;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;1573.602,385.9933;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;46;1312.708,1051.275;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;952.2275,85.19289;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2011.248,82.15842;Float;False;True;-1;0;ASEMaterialInspector;0;0;Standard;FX/Character Fx;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;1;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;1;16;0
WireConnection;17;0;15;1
WireConnection;17;1;19;0
WireConnection;18;0;17;0
WireConnection;4;2;6;0
WireConnection;4;3;7;0
WireConnection;20;0;25;0
WireConnection;20;1;18;0
WireConnection;37;0;34;0
WireConnection;37;1;35;0
WireConnection;37;2;36;0
WireConnection;24;0;26;0
WireConnection;24;1;18;0
WireConnection;27;0;20;0
WireConnection;27;1;24;0
WireConnection;38;0;37;0
WireConnection;38;1;40;0
WireConnection;8;0;4;0
WireConnection;9;0;8;0
WireConnection;9;1;11;0
WireConnection;28;0;27;0
WireConnection;41;0;38;0
WireConnection;41;1;42;0
WireConnection;56;0;2;0
WireConnection;56;1;55;0
WireConnection;43;0;41;0
WireConnection;12;0;9;0
WireConnection;12;1;13;0
WireConnection;29;0;30;0
WireConnection;29;1;28;0
WireConnection;5;0;56;0
WireConnection;5;1;12;0
WireConnection;31;0;32;0
WireConnection;31;1;29;0
WireConnection;39;0;37;0
WireConnection;39;1;40;0
WireConnection;44;0;43;0
WireConnection;44;1;45;0
WireConnection;53;0;52;0
WireConnection;53;1;50;0
WireConnection;46;0;47;0
WireConnection;46;1;39;0
WireConnection;46;2;44;0
WireConnection;33;0;5;0
WireConnection;33;1;31;0
WireConnection;0;0;1;0
WireConnection;0;1;3;0
WireConnection;0;2;33;0
WireConnection;0;3;53;0
WireConnection;0;4;54;0
WireConnection;0;9;20;0
WireConnection;0;11;46;0
ASEEND*/
//CHKSM=CEB35EB325346FBC1CD5F7D1A64837A414B2BD36