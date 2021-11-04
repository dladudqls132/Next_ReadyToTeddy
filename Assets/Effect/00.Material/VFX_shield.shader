// Upgrade NOTE: upgraded instancing buffer 'VFX_shield' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX_shield"
{
	Properties
	{
		_T_BASE("T_BASE", 2D) = "white" {}
		_T_flow("T_flow", 2D) = "white" {}
		_T_line("T_line", 2D) = "white" {}
		_T_flow02("T_flow02", 2D) = "white" {}
		[HDR]_color("color", Color) = (0,0,0,0)
		_Fresnel("Fresnel", Float) = 0
		_Fresnel_POw("Fresnel_POw", Float) = 0
		_Fresnel_brightness("Fresnel_brightness", Float) = 0
		_WPO("WPO", Float) = 0
		_Float2("Float 2", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _T_line;
		uniform sampler2D _T_BASE;
		uniform sampler2D _T_flow;
		uniform sampler2D _T_flow02;
		uniform float _WPO;
		uniform float _Fresnel;
		uniform float _Fresnel_POw;
		uniform float _Fresnel_brightness;
		uniform float _Float2;

		UNITY_INSTANCING_BUFFER_START(VFX_shield)
			UNITY_DEFINE_INSTANCED_PROP(float4, _color)
#define _color_arr VFX_shield
		UNITY_INSTANCING_BUFFER_END(VFX_shield)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 panner9 = ( 0.5 * _Time.y * float2( 0,0.1 ) + v.texcoord.xy);
			float2 uv_TexCoord22 = v.texcoord.xy * float2( 1,0.2 );
			float2 panner21 = ( 0.5 * _Time.y * float2( 0,0.035 ) + uv_TexCoord22);
			float2 temp_output_29_0 = ( v.texcoord.xy + ( ( ( (tex2Dlod( _T_BASE, float4( panner9, 0, 0.0) )).rg * 0.0 ) + ( (tex2Dlod( _T_flow, float4( panner21, 0, 0.0) )).rg * 0.0 ) ) * float2( 0,0 ) ) );
			float2 panner33 = ( 1.0 * _Time.y * float2( 0,1 ) + temp_output_29_0);
			float2 panner34 = ( 1.0 * _Time.y * float2( 0,0 ) + temp_output_29_0);
			float4 tex2DNode36 = tex2Dlod( _T_flow02, float4( panner34, 0, 0.0) );
			float temp_output_37_0 = ( tex2Dlod( _T_line, float4( panner33, 0, 0.0) ).r + tex2DNode36.r );
			float4 uv2s4_TexCoord73 = v.texcoord1;
			uv2s4_TexCoord73.xy = v.texcoord1.xy + float2( -1,0 );
			float temp_output_74_0 = pow( ( 1.0 - (( ( v.texcoord.xy + uv2s4_TexCoord73.y ) + temp_output_37_0 )).y ) , 0.0 );
			float clampResult79 = clamp( floor( ( temp_output_74_0 * 150.0 ) ) , 0.0 , 1.0 );
			float clampResult81 = clamp( temp_output_74_0 , 0.0 , 1.0 );
			v.vertex.xyz += ( ase_vertexNormal * ( ( temp_output_37_0 * _WPO ) + ( pow( ( clampResult79 - clampResult81 ) , 5.0 ) * tex2DNode36.r ) ) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _color_Instance = UNITY_ACCESS_INSTANCED_PROP(_color_arr, _color);
			float2 panner9 = ( 0.5 * _Time.y * float2( 0,0.1 ) + i.uv_texcoord);
			float2 uv_TexCoord22 = i.uv_texcoord * float2( 1,0.2 );
			float2 panner21 = ( 0.5 * _Time.y * float2( 0,0.035 ) + uv_TexCoord22);
			float2 temp_output_29_0 = ( i.uv_texcoord + ( ( ( (tex2D( _T_BASE, panner9 )).rg * 0.0 ) + ( (tex2D( _T_flow, panner21 )).rg * 0.0 ) ) * float2( 0,0 ) ) );
			float2 panner33 = ( 1.0 * _Time.y * float2( 0,1 ) + temp_output_29_0);
			float2 panner34 = ( 1.0 * _Time.y * float2( 0,0 ) + temp_output_29_0);
			float4 tex2DNode36 = tex2D( _T_flow02, panner34 );
			float temp_output_37_0 = ( tex2D( _T_line, panner33 ).r + tex2DNode36.r );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV40 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode40 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV40, _Fresnel ) );
			o.Emission = ( ( _color_Instance * temp_output_37_0 ) * ( pow( fresnelNode40 , _Fresnel_POw ) * _Fresnel_brightness ) ).rgb;
			o.Alpha = _Float2;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
Version=18912
-1361;283;1257;675;2003.931;393.3082;2.022055;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2735.646,468.3497;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2806.874,-145.7696;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;23;-2283.462,306.243;Inherit;True;Property;_T_flow;T_flow;1;0;Create;True;0;0;0;False;0;False;None;8eb77339794c1224dbcc055b45af27a2;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;9;-2525.584,-58.83268;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.1;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;21;-2455.262,467.8427;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.035;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;-2304.874,-257.7697;Inherit;True;Property;_T_BASE;T_BASE;0;0;Create;True;0;0;0;False;0;False;None;16d425ac872724e4d9b62d3e060c29c3;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;53;-2020.186,441.1854;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;-2079.67,-99.88564;Inherit;True;Property;_TextureSample4;Texture Sample 4;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;54;-1703.186,481.1854;Inherit;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1421.263,588.8428;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;51;-1742.67,-68.88565;Inherit;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1410.874,43.23035;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1401.999,-69.9949;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1392.388,451.6175;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1297.902,206.0679;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1162.792,198.3745;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-1235.725,-14.45765;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-996.3455,188.5277;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-715.6672,-180.9925;Inherit;True;Property;_T_line;T_line;2;0;Create;True;0;0;0;False;0;False;None;ae185c68fc86424499123c59d1502c24;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;33;-660.023,72.38325;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;34;-629.626,452.0316;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;35;-755.8715,247.1514;Inherit;True;Property;_T_flow02;T_flow02;3;0;Create;True;0;0;0;False;0;False;None;8eb77339794c1224dbcc055b45af27a2;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;67;-927.5781,1306.38;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-937.5349,1478.254;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-1,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-321.6671,31.00756;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-339.0149,401.9673;Inherit;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-678.5781,1372.38;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;61.74534,206.7129;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-416.6066,1375.804;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;70;-190.578,1375.38;Inherit;True;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;76;53.72823,1349.356;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-45.27188,1624.356;Inherit;False;Constant;_Wpo;Wpo;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;74;222.7285,1374.356;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;377.7284,1413.356;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;150;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;78;508.728,1405.356;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;81;468.744,1676.839;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;79;687.4559,1430.988;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;80;862.7438,1392.839;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-521.7156,1037.792;Inherit;False;Property;_Fresnel;Fresnel;5;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;82;1061.116,1429.523;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;40;-200.41,841.239;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-80.91821,1042.589;Inherit;False;Property;_Fresnel_POw;Fresnel_POw;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;1258.094,1201.47;Inherit;False;Property;_WPO;WPO;8;0;Create;True;0;0;0;False;0;False;0;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;1398.839,1098.271;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;1348.115,1311.523;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;133.0816,1041.589;Inherit;False;Property;_Fresnel_brightness;Fresnel_brightness;7;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;82.08168,917.5896;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;38;171.4049,-72.40497;Inherit;False;InstancedProperty;_color;color;4;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,5.019608,64,0.4627451;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;85;1664.092,783.4478;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;317.0817,906.5896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;511.9182,99.84399;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;1585.521,998.6066;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;2039.642,230.446;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CosTime;66;-2947.166,231.8351;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;2060.817,657.1468;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;103;2311.472,432.4461;Inherit;False;Property;_Float2;Float 2;9;0;Create;True;0;0;0;False;0;False;0;0.235;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;101;2743.408,180.843;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;VFX_shield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;13;-3291.874,-67.76962;Inherit;False;360;405;Speed(w)/ dist(t);0;;1,1,1,1;0;0
WireConnection;9;0;7;0
WireConnection;21;0;22;0
WireConnection;53;0;23;0
WireConnection;53;1;21;0
WireConnection;52;0;14;0
WireConnection;52;1;9;0
WireConnection;54;0;53;0
WireConnection;51;0;52;0
WireConnection;16;0;51;0
WireConnection;16;1;17;0
WireConnection;27;0;54;0
WireConnection;27;1;26;0
WireConnection;19;0;16;0
WireConnection;19;1;27;0
WireConnection;28;0;19;0
WireConnection;29;0;30;0
WireConnection;29;1;28;0
WireConnection;33;0;29;0
WireConnection;34;0;29;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;69;0;67;0
WireConnection;69;1;73;2
WireConnection;37;0;32;1
WireConnection;37;1;36;1
WireConnection;72;0;69;0
WireConnection;72;1;37;0
WireConnection;70;0;72;0
WireConnection;76;0;70;0
WireConnection;74;0;76;0
WireConnection;74;1;75;0
WireConnection;77;0;74;0
WireConnection;78;0;77;0
WireConnection;81;0;74;0
WireConnection;79;0;78;0
WireConnection;80;0;79;0
WireConnection;80;1;81;0
WireConnection;82;0;80;0
WireConnection;40;3;41;0
WireConnection;90;0;37;0
WireConnection;90;1;87;0
WireConnection;83;0;82;0
WireConnection;83;1;36;1
WireConnection;42;0;40;0
WireConnection;42;1;43;0
WireConnection;44;0;42;0
WireConnection;44;1;46;0
WireConnection;39;0;38;0
WireConnection;39;1;37;0
WireConnection;88;0;90;0
WireConnection;88;1;83;0
WireConnection;102;0;39;0
WireConnection;102;1;44;0
WireConnection;86;0;85;0
WireConnection;86;1;88;0
WireConnection;101;2;102;0
WireConnection;101;9;103;0
WireConnection;101;11;86;0
ASEEND*/
//CHKSM=8586622A1C116E256ED17B4CDDCBA0AC8094E365