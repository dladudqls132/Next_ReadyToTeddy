// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_flame01"
{
	Properties
	{
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_vornoiintensity("vornoi intensity", Float) = 2.61
		_voronoipower("voronoi power", Float) = 0.52
		_voronoiscale("voronoi scale", Float) = 1.1
		_stepamount("step amount", Float) = 0.87
		_remap_min("remap_min", Float) = 0
		_remap_max("remap_max", Float) = 1
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
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
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample1;
		uniform float _voronoiscale;
		uniform float _vornoiintensity;
		uniform float _voronoipower;
		uniform float _remap_min;
		uniform float _remap_max;
		uniform float _stepamount;
		uniform float4 _Color0;


		float2 voronoihash49( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi49( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash49( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		float2 voronoihash48( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi48( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash48( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float time49 = 10.0;
			float2 panner4 = ( 1.0 * _Time.y * float2( -0.2,-0.2 ) + i.uv_texcoord);
			float2 coords49 = panner4 * _voronoiscale;
			float2 id49 = 0;
			float2 uv49 = 0;
			float voroi49 = voronoi49( coords49, time49, id49, uv49, 0 );
			float time48 = 10.0;
			float2 panner5 = ( 1.0 * _Time.y * float2( 0.2,-0.2 ) + i.uv_texcoord);
			float2 coords48 = panner5 * _voronoiscale;
			float2 id48 = 0;
			float2 uv48 = 0;
			float voroi48 = voronoi48( coords48, time48, id48, uv48, 0 );
			float clampResult22 = clamp( pow( ( ( voroi49 * voroi48 ) * _vornoiintensity ) , _voronoipower ) , 0.0 , 1.0 );
			float4 appendResult19 = (float4(i.uv_tex4coord.z , i.uv_tex4coord.w , 0.0 , 0.0));
			float temp_output_28_0 = ( tex2D( _TextureSample2, uv_TextureSample2 ).r * (_remap_min + (tex2D( _TextureSample1, (( clampResult22 + i.uv_texcoord )*1.0 + appendResult19.xy) ).r - 0.0) * (_remap_max - _remap_min) / (1.0 - 0.0)) );
			float clampResult35 = clamp( ( temp_output_28_0 + step( _stepamount , temp_output_28_0 ) ) , 0.0 , 1.0 );
			o.Emission = ( clampResult35 * i.vertexColor * _Color0 ).rgb;
			o.Alpha = ( i.vertexColor.a * clampResult35 );
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
-85;644;1440;574;1391.124;-145.168;1.3;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1423.676,57.48264;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1413.019,265.5805;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;4;-1188.353,62.49075;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.2,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;5;-1186.4,263.4445;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1191.748,184.9864;Inherit;False;Property;_voronoiscale;voronoi scale;5;0;Create;True;0;0;0;False;0;False;1.1;2.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;49;-998.8698,58.33371;Inherit;False;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;10;False;2;FLOAT;4;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.VoronoiNode;48;-987.7823,255.3787;Inherit;False;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;10;False;2;FLOAT;4;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;10;-889.3387,404.1323;Inherit;False;Property;_vornoiintensity;vornoi intensity;3;0;Create;True;0;0;0;False;0;False;2.61;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-807.4732,146.862;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-731.3881,274.6777;Inherit;False;Property;_voronoipower;voronoi power;4;0;Create;True;0;0;0;False;0;False;0.52;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-687.9379,147.9208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;39;-536.6821,154.5826;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-553.7463,307.9318;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-697.7799,575.8302;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;22;-390.936,166.8771;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-483.1502,587.3967;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-335.3317,314.6746;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;14;-347.6663,446.6677;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;53;-144.5142,447.0689;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;f3687277be8851e4d80fda9b642b2210;f3687277be8851e4d80fda9b642b2210;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-17.30082,650.444;Inherit;False;Property;_remap_min;remap_min;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-15.48961,725.4028;Inherit;False;Property;_remap_max;remap_max;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;165.7466,449.2878;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-2;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;52.80664,227.7441;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;0;False;0;False;-1;8c4a7fca2884fab419769ccc0355c0c1;8c4a7fca2884fab419769ccc0355c0c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;374.5057,350.7992;Inherit;False;Property;_stepamount;step amount;7;0;Create;True;0;0;0;False;0;False;0.87;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;342.2356,448.9193;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;43;489.5698,450.7946;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.17;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;653.8672,494.3817;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;842.7719,719.4904;Inherit;False;Property;_Color0;Color 0;10;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;31;840.3324,232.663;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;35;809.2325,465.9327;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;17;-522.0952,441.0643;Inherit;False;Property;_dissolvescale;dissolve scale;6;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.8,0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1090.253,273.7038;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;1089.839,541.1014;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1246.456,277.8914;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SH_flame01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;0
WireConnection;5;0;3;0
WireConnection;49;0;4;0
WireConnection;49;2;50;0
WireConnection;48;0;5;0
WireConnection;48;2;50;0
WireConnection;7;0;49;0
WireConnection;7;1;48;0
WireConnection;9;0;7;0
WireConnection;9;1;10;0
WireConnection;39;0;9;0
WireConnection;39;1;40;0
WireConnection;22;0;39;0
WireConnection;19;0;18;3
WireConnection;19;1;18;4
WireConnection;13;0;22;0
WireConnection;13;1;11;0
WireConnection;14;0;13;0
WireConnection;14;2;19;0
WireConnection;53;1;14;0
WireConnection;27;0;53;1
WireConnection;27;3;36;0
WireConnection;27;4;37;0
WireConnection;28;0;29;1
WireConnection;28;1;27;0
WireConnection;43;0;47;0
WireConnection;43;1;28;0
WireConnection;46;0;28;0
WireConnection;46;1;43;0
WireConnection;35;0;46;0
WireConnection;30;0;35;0
WireConnection;30;1;31;0
WireConnection;30;2;33;0
WireConnection;32;0;31;4
WireConnection;32;1;35;0
WireConnection;0;2;30;0
WireConnection;0;9;32;0
ASEEND*/
//CHKSM=FF97CB38B946A7EF58B176D3E3AD03EDF158810F