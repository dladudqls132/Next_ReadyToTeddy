// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_dust"
{
	Properties
	{
		[HDR]_HDRColour("HDR Colour", Color) = (0,0,0,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.7
		_MainTex("ZeldaSmoke", 2D) = "white" {}
		_JiggleNormals("Jiggle Normals", 2D) = "bump" {}
		_JiggleStrength("Jiggle Strength", Float) = 0.7
		_JiggleSpeed("Jiggle Speed", Float) = 0
		_AlphaClip("AlphaClip", Float) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _HDRColour;
		uniform sampler2D _MainTex;
		uniform sampler2D _JiggleNormals;
		uniform float _JiggleSpeed;
		uniform float _JiggleStrength;
		uniform float _AlphaClip;
		uniform float _Cutoff = 0.7;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner68 = ( ( _Time.y * _JiggleSpeed ) * float2( 1,1 ) + i.uv_texcoord);
			float2 _distortUV97 = ( ( (UnpackNormal( tex2D( _JiggleNormals, panner68 ) )).xy * _JiggleStrength ) + i.uv_texcoord );
			float4 tex2DNode1 = tex2D( _MainTex, _distortUV97 );
			float4 temp_output_24_0 = ( _HDRColour * tex2DNode1.r );
			o.Emission = ( temp_output_24_0 * i.vertexColor ).rgb;
			o.Alpha = _AlphaClip;
			float _alpha56 = tex2DNode1.b;
			clip( saturate( ( ( i.vertexColor.a * _alpha56 ) * 4.0 ) ) - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows 

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
				half4 color : COLOR0;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
0;116;1920;943;1707.926;824.9917;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;96;-3870.286,-1228.854;Inherit;False;2091.938;823.894;Comment;12;97;66;95;61;62;59;58;68;73;71;70;63;Distort UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;70;-3819.813,-977.7352;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-3820.286,-876.1149;Inherit;False;Property;_JiggleSpeed;Jiggle Speed;5;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-3679.109,-1178.854;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-3610.231,-958.6226;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;68;-3419.159,-1040.06;Inherit;False;3;0;FLOAT2;1,1;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;58;-3192.007,-1066.681;Inherit;True;Property;_JiggleNormals;Jiggle Normals;3;0;Create;True;0;0;0;False;0;False;-1;73ad01cf98af75f46880d7d88b3ce234;73ad01cf98af75f46880d7d88b3ce234;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;59;-2832.566,-937.821;Inherit;False;True;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-2826.082,-779.7556;Inherit;False;Property;_JiggleStrength;Jiggle Strength;4;0;Create;True;0;0;0;False;0;False;0.7;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;95;-2568.571,-580.5738;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2588.956,-882.2843;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-2273.619,-682.4016;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-1989.205,-683.9243;Inherit;False;_distortUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;98;-1574.595,-486.6406;Inherit;False;97;_distortUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1393.571,-513.1729;Inherit;True;Property;_MainTex;ZeldaSmoke;2;0;Create;False;0;0;0;False;0;False;-1;None;46dcd935af2aac24ba76a82cd0a132e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-1030.96,-347.0045;Inherit;False;_alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;-617.6978,269.5466;Inherit;False;56;_alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;10;-574.7632,-126.5847;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-1034.703,-895.2349;Inherit;False;Property;_HDRColour;HDR Colour;0;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;6.954251,5.228476,3.772353,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-414.6847,185.429;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-763.3842,-730.2408;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-240.2664,185.2086;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;47;-2307.174,-273.195;Inherit;False;534.3999;273.8172;Controls Lerp from fire to smoke;2;43;26;CustomDataVector2;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;74;11.205,188.0587;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-2259.174,-208.4591;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-1993.83,-144.8709;Inherit;False;X;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;-622.2779,-394.4317;Inherit;False;43;X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2;-406.2484,-537.6776;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-55.34239,-142.2072;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-28.9345,46.3386;Inherit;False;Property;_AlphaClip;AlphaClip;6;0;Create;True;0;0;0;False;0;False;0.2;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;99;217.1584,-46.55753;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;S_dust;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.7;True;True;0;True;Opaque;;AlphaTest;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;71;0;70;0
WireConnection;71;1;63;0
WireConnection;68;0;73;0
WireConnection;68;1;71;0
WireConnection;58;1;68;0
WireConnection;59;0;58;0
WireConnection;61;0;59;0
WireConnection;61;1;62;0
WireConnection;66;0;61;0
WireConnection;66;1;95;0
WireConnection;97;0;66;0
WireConnection;1;1;98;0
WireConnection;56;0;1;3
WireConnection;22;0;10;4
WireConnection;22;1;57;0
WireConnection;24;0;23;0
WireConnection;24;1;1;1
WireConnection;75;0;22;0
WireConnection;74;0;75;0
WireConnection;43;0;26;3
WireConnection;2;0;24;0
WireConnection;2;1;1;1
WireConnection;2;2;44;0
WireConnection;12;0;24;0
WireConnection;12;1;10;0
WireConnection;99;2;12;0
WireConnection;99;9;82;0
WireConnection;99;10;74;0
ASEEND*/
//CHKSM=8FF44998E5A7C10AEB216931C517C9DBCEEE97E2