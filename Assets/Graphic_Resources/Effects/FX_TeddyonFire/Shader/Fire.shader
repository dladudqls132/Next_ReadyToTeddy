// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Fire"
{
	Properties
	{
		_Speed("Speed", Float) = 5
		_DistortionStrength("DistortionStrength", Float) = 2.38
		_FireBottomOffset("FireBottomOffset", Float) = 4
		[HDR]_FireColor("Fire Color", Color) = (1,0.4809354,0.3254717,0)
		_NoiseScale("Noise Scale", Float) = 0
		_Width("Width", Float) = 0
		_AlphaWidth("Alpha Width", Float) = 0.6
		_Height("Height", Float) = 0
		_AlphaHeight("Alpha Height", Float) = 0.6
		_FireOffset("Fire Offset", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _FireColor;
		uniform float _FireBottomOffset;
		uniform float _DistortionStrength;
		uniform float _Speed;
		uniform float _NoiseScale;
		uniform float _FireOffset;
		uniform float _Width;
		uniform float _Height;
		uniform float _AlphaWidth;
		uniform float _AlphaHeight;


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
			float2 uv_TexCoord50 = i.uv_texcoord * float2( 2,1 ) + float2( 0.1,0.03 );
			float2 panner29 = ( ( _Time.y * _Speed ) * float2( 0,-0.1 ) + uv_TexCoord50);
			float simplePerlin2D110 = snoise( panner29*_NoiseScale );
			simplePerlin2D110 = simplePerlin2D110*0.5 + 0.5;
			float2 temp_cast_0 = (( pow( i.uv_texcoord.y , _FireBottomOffset ) * ( _DistortionStrength * ( simplePerlin2D110 - _FireOffset ) ) )).xx;
			float2 uv_TexCoord64 = i.uv_texcoord * float2( 0.91,0.63 ) + temp_cast_0;
			float2 break175 = uv_TexCoord64;
			float temp_output_92_0 = pow( ( 1.0 - break175.y ) , 2.0 );
			float2 appendResult201 = (float2(break175.x , temp_output_92_0));
			float2 appendResult11_g18 = (float2(_Width , _Height));
			float temp_output_17_0_g18 = length( ( (appendResult201*2.0 + -1.0) / appendResult11_g18 ) );
			float4 temp_cast_1 = (( ( temp_output_92_0 + 0.4 ) * 1.5 )).xxxx;
			float div97=256.0/float(3);
			float4 posterize97 = ( floor( temp_cast_1 * div97 ) / div97 );
			o.Emission = ( _FireColor * ( saturate( ( ( 1.0 - temp_output_17_0_g18 ) / fwidth( temp_output_17_0_g18 ) ) ) + posterize97 ) ).rgb;
			float2 appendResult11_g19 = (float2(_AlphaWidth , _AlphaHeight));
			float temp_output_17_0_g19 = length( ( (appendResult201*2.0 + -1.0) / appendResult11_g19 ) );
			o.Alpha = saturate( ( ( 1.0 - temp_output_17_0_g19 ) / fwidth( temp_output_17_0_g19 ) ) );
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
2045;24;1920;1019;-1215.464;1208.539;1;True;True
Node;AmplifyShaderEditor.TimeNode;51;-1400.225,-110.0277;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;-1357.049,56.48449;Inherit;False;Property;_Speed;Speed;1;0;Create;True;0;0;0;False;0;False;5;5.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;199;-1469.953,-234.3109;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;0.1,0.03;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-1334.817,-317.7186;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-1178.049,-78.51551;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;29;-1006.209,-199.7877;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-957.2976,-11.79919;Inherit;False;Property;_NoiseScale;Noise Scale;5;0;Create;True;0;0;0;False;0;False;0;2.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;110;-777.8159,-115.7902;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-668.655,137.3418;Inherit;False;Property;_FireOffset;Fire Offset;10;0;Create;True;0;0;0;False;0;False;0;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-734.3492,-615.5155;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;54;-473.3492,-169.5155;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;-0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-460.3492,-296.5155;Inherit;False;Property;_DistortionStrength;DistortionStrength;2;0;Create;True;0;0;0;False;0;False;2.38;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-438.3492,-416.5155;Inherit;False;Property;_FireBottomOffset;FireBottomOffset;3;0;Create;True;0;0;0;False;0;False;4;1.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;59;-440.3492,-534.5155;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-193.3492,-223.5155;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;61;-223.3492,-547.5155;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;65;299.1353,-506.4142;Inherit;False;Constant;_YStretch;YStretch;2;0;Create;True;0;0;0;False;0;False;0.91,0.63;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;96.65076,-315.5155;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;507.99,-513.1667;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;175;753.6088,-576.5021;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OneMinusNode;140;1018.803,-402.5392;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;92;1210.153,-405.0999;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;1735.99,-303.6915;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;201;1467.05,-618.7316;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;1937.99,-280.6915;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;143;1808.351,-735.2297;Inherit;False;Property;_Height;Height;8;0;Create;True;0;0;0;False;0;False;0;0.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;1824.164,-830.1064;Inherit;False;Property;_Width;Width;6;0;Create;True;0;0;0;False;0;False;0;0.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;141;2006.576,-786.1108;Inherit;True;Ellipse;-1;;18;3ba94b7b3cfd5f447befde8107c04d52;0;3;2;FLOAT2;0,0;False;7;FLOAT;0.5;False;9;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosterizeNode;97;2151.99,-308.6915;Inherit;False;3;2;1;COLOR;0,0,0,0;False;0;INT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;84;2365.668,-1116.324;Inherit;False;Property;_FireColor;Fire Color;4;1;[HDR];Create;True;0;0;0;False;0;False;1,0.4809354,0.3254717,0;2.270603,0.4612937,0.2020955,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;98;2318.39,-757.1912;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;152;1858.253,-482.5909;Inherit;False;Property;_AlphaWidth;Alpha Width;7;0;Create;True;0;0;0;False;0;False;0.6;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;1845.44,-385.7142;Inherit;False;Property;_AlphaHeight;Alpha Height;9;0;Create;True;0;0;0;False;0;False;0.6;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;131;2053.973,-542.9872;Inherit;True;Ellipse;-1;;19;3ba94b7b3cfd5f447befde8107c04d52;0;3;2;FLOAT2;0,0;False;7;FLOAT;0.5;False;9;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;2527.172,-910.2245;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2979.005,-714.5027;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FX/Fire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;1;199;0
WireConnection;52;0;51;2
WireConnection;52;1;53;0
WireConnection;29;0;50;0
WireConnection;29;1;52;0
WireConnection;110;0;29;0
WireConnection;110;1;111;0
WireConnection;54;0;110;0
WireConnection;54;1;203;0
WireConnection;59;0;58;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;61;0;59;1
WireConnection;61;1;62;0
WireConnection;63;0;61;0
WireConnection;63;1;57;0
WireConnection;64;0;65;0
WireConnection;64;1;63;0
WireConnection;175;0;64;0
WireConnection;140;0;175;1
WireConnection;92;0;140;0
WireConnection;95;0;92;0
WireConnection;201;0;175;0
WireConnection;201;1;92;0
WireConnection;96;0;95;0
WireConnection;141;2;201;0
WireConnection;141;7;142;0
WireConnection;141;9;143;0
WireConnection;97;1;96;0
WireConnection;98;0;141;0
WireConnection;98;1;97;0
WireConnection;131;2;201;0
WireConnection;131;7;152;0
WireConnection;131;9;153;0
WireConnection;83;0;84;0
WireConnection;83;1;98;0
WireConnection;0;2;83;0
WireConnection;0;9;131;0
ASEEND*/
//CHKSM=E3269593502FE18233F2AF6D3E629A4F0121F509