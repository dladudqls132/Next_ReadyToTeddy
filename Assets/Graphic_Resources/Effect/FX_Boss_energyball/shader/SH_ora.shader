// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_ora"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_T_ora("T_ora", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0.7100961,3.482202,0.9558986,0)
		_noisescale("noise scale", Float) = 0.79
		_noiseintensity("noise intensity", Float) = 0.34
		_noisepower("noise power", Float) = 0.95
		_oraspeed("ora speed", Vector) = (1,0,0,0)
		_noisespeed("noise speed", Vector) = (1,0,0,0)
		_remap("remap", Vector) = (-0.17,3.08,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
		};

		uniform float4 _Color0;
		uniform sampler2D _T_ora;
		uniform float2 _noisespeed;
		uniform float _noisescale;
		uniform float _noiseintensity;
		uniform float _noisepower;
		uniform float2 _oraspeed;
		uniform float2 _remap;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner9 = ( 1.0 * _Time.y * _noisespeed + i.uv_texcoord);
			float2 temp_output_14_0 = (panner9*_noisescale + 0.0);
			float simplePerlin2D47 = snoise( temp_output_14_0 );
			simplePerlin2D47 = simplePerlin2D47*0.5 + 0.5;
			float myVarName25 = simplePerlin2D47;
			float2 panner2 = ( 1.0 * _Time.y * _oraspeed + i.uv_texcoord);
			float4 tex2DNode1 = tex2D( _T_ora, ( pow( ( myVarName25 * _noiseintensity ) , _noisepower ) + panner2 ) );
			float clampResult49 = clamp( tex2DNode1.r , 0.0 , 1.0 );
			o.Emission = ( _Color0 * clampResult49 * i.vertexColor ).rgb;
			float temp_output_29_0 = i.uv_tex4coord.x;
			float temp_output_37_0 = pow( ( pow( ( temp_output_29_0 * 4.1 ) , 1.34 ) * pow( ( 1.0 - temp_output_29_0 ) , 5.09 ) * i.uv_tex4coord.z ) , 0.4 );
			float temp_output_48_0 = ( myVarName25 * temp_output_37_0 );
			float clampResult40 = clamp( (_remap.x + (temp_output_48_0 - 0.0) * (_remap.y - _remap.x) / (1.0 - 0.0)) , 0.0 , 1.0 );
			o.Alpha = ( tex2DNode1.r * i.vertexColor.a * clampResult40 );
			float clampResult56 = clamp( temp_output_48_0 , 0.0 , 1.0 );
			clip( ( clampResult56 * i.uv_tex4coord.w ) - _Cutoff );
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
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
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
				o.customPack2.xyzw = customInputData.uv_tex4coord;
				o.customPack2.xyzw = v.texcoord;
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
				surfIN.uv_tex4coord = IN.customPack2.xyzw;
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
Version=18900
205;673;1641;278;3693.127;-499.562;3.313014;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-2356.992,-291.4531;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;27;-2325.261,-116.7513;Inherit;False;Property;_noisespeed;noise speed;8;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;9;-2133.715,-285.5386;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-2465.013,826.6267;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-2083.352,-110.7637;Inherit;False;Property;_noisescale;noise scale;4;0;Create;True;0;0;0;False;0;False;0.79;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;14;-1935.585,-291.029;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;29;-2227.474,802.2275;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;47;-1752.123,-364.3975;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;30;-1988.146,1023.921;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2007.6,792.1103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;4.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;33;-1789.824,1031.671;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;5.09;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;34;-1836.927,808.9518;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.34;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1383.492,-236.8585;Inherit;False;myVarName;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1366.603,14.31201;Inherit;False;Property;_noiseintensity;noise intensity;5;0;Create;True;0;0;0;False;0;False;0.34;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1174.203,-236.5881;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1170.303,15.61202;Inherit;False;Property;_noisepower;noise power;6;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1323.23,149.6743;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1488.271,827.9105;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;22;-1282.277,298.3002;Inherit;False;Property;_oraspeed;ora speed;7;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;2;-1089.16,151.9828;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;3.14,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;17;-1004.146,-252.7974;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;37;-1251.16,845.7723;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1263.179,557.1275;Inherit;False;25;myVarName;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-716.8552,-165.4641;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-940.4429,663.0144;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;41;-887.9274,909.9545;Inherit;False;Property;_remap;remap;9;0;Create;True;0;0;0;False;0;False;-0.17,3.08;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;39;-621.0421,666.9719;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.43;False;4;FLOAT;0.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-479.0424,111.116;Inherit;True;Property;_T_ora;T_ora;1;0;Create;True;0;0;0;False;0;False;-1;d5efca0d961331b43a40f777ea60b88d;95bcc9797068bc641971373f50656263;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-102.8746,-162.9748;Inherit;False;Property;_Color0;Color 0;2;1;[HDR];Create;True;0;0;0;False;0;False;0.7100961,3.482202,0.9558986,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;49;-125.2485,97.50372;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;23;-97.27567,408.6237;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;56;-524.3845,945.2086;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;-293.3228,657.5367;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;42;-1415.626,-609.6332;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-963.6234,403.8775;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;97.89521,979.1373;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1719.604,-118.9324;Inherit;True;Property;_cloud_noise;cloud_noise;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;227.7096,-4.984207;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;224.1134,422.5727;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;617.4913,112.7863;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SH_ora;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;8;0
WireConnection;9;2;27;0
WireConnection;14;0;9;0
WireConnection;14;1;15;0
WireConnection;29;0;28;1
WireConnection;47;0;14;0
WireConnection;30;0;29;0
WireConnection;31;0;29;0
WireConnection;33;0;30;0
WireConnection;34;0;31;0
WireConnection;25;0;47;0
WireConnection;16;0;25;0
WireConnection;16;1;18;0
WireConnection;32;0;34;0
WireConnection;32;1;33;0
WireConnection;32;2;28;3
WireConnection;2;0;3;0
WireConnection;2;2;22;0
WireConnection;17;0;16;0
WireConnection;17;1;19;0
WireConnection;37;0;32;0
WireConnection;10;0;17;0
WireConnection;10;1;2;0
WireConnection;48;0;26;0
WireConnection;48;1;37;0
WireConnection;39;0;48;0
WireConnection;39;3;41;1
WireConnection;39;4;41;2
WireConnection;1;1;10;0
WireConnection;49;0;1;1
WireConnection;56;0;48;0
WireConnection;40;0;39;0
WireConnection;42;0;14;0
WireConnection;38;0;26;0
WireConnection;38;1;37;0
WireConnection;53;0;56;0
WireConnection;53;1;28;4
WireConnection;7;1;14;0
WireConnection;4;0;6;0
WireConnection;4;1;49;0
WireConnection;4;2;23;0
WireConnection;24;0;1;1
WireConnection;24;1;23;4
WireConnection;24;2;40;0
WireConnection;0;2;4;0
WireConnection;0;9;24;0
WireConnection;0;10;53;0
ASEEND*/
//CHKSM=0DAEFAED6229D5CEB60B34E825E204732F93E186