// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Particle Additive Thunder"
{
	Properties
	{
		_NoiseTexture01("Noise Texture01", 2D) = "white" {}
		_NoiseTexture02("Noise Texture02", 2D) = "white" {}
		_Ins("Ins", Range( 0 , 50)) = 5.319912
		[Toggle(_USEPARTICLE_ON)] _Useparticle("Use particle", Float) = 0
		_TintColor("Tint Color", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma shader_feature_local _USEPARTICLE_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
		};

		uniform float4 _TintColor;
		uniform sampler2D _NoiseTexture01;
		uniform float4 _NoiseTexture01_ST;
		uniform sampler2D _NoiseTexture02;
		uniform float _Ins;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			half2 appendResult11_g2 = (half2(0.73 , 0.73));
			half temp_output_17_0_g2 = length( ( (i.uv_texcoord*2.0 + -1.0) / appendResult11_g2 ) );
			float2 uv_NoiseTexture01 = i.uv_texcoord * _NoiseTexture01_ST.xy + _NoiseTexture01_ST.zw;
			half2 panner11 = ( 1.0 * _Time.y * float2( 1,1 ) + uv_NoiseTexture01);
			half2 panner12 = ( 1.0 * _Time.y * float2( -0.5,-0.5 ) + uv_NoiseTexture01);
			half4 temp_cast_0 = (0.18).xxxx;
			half4 temp_cast_1 = (0.18).xxxx;
			half2 appendResult10_g1 = (half2(0.5 , 0.73));
			half2 temp_output_11_0_g1 = ( abs( ((float4( 0,0,0,0 ) + (( tex2D( _NoiseTexture01, panner11 ) * tex2D( _NoiseTexture02, panner12 ) ) - temp_cast_0) * (float4( 1,1,1,1 ) - float4( 0,0,0,0 )) / (temp_cast_1 - temp_cast_0)).rg*2.0 + -1.0) ) - appendResult10_g1 );
			half2 break16_g1 = ( 1.0 - ( temp_output_11_0_g1 / fwidth( temp_output_11_0_g1 ) ) );
			#ifdef _USEPARTICLE_ON
				float staticSwitch4 = i.uv_tex4coord.z;
			#else
				float staticSwitch4 = _Ins;
			#endif
			o.Emission = ( i.vertexColor * ( ( _TintColor * ( saturate( ( ( 1.0 - temp_output_17_0_g2 ) / fwidth( temp_output_17_0_g2 ) ) ) * saturate( min( break16_g1.x , break16_g1.y ) ) ) ) * staticSwitch4 ) ).rgb;
			o.Alpha = i.vertexColor.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
7;6;1906;1013;1790.147;981.0472;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-3453.151,-459.0349;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;12;-3108.771,-323.1485;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;11;-3108.906,-582.6748;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-2753.512,-330.1458;Inherit;True;Property;_NoiseTexture02;Noise Texture02;1;0;Create;True;0;0;0;False;0;False;-1;e199ee8112362864f831425463b09fda;e199ee8112362864f831425463b09fda;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2758.985,-552.1928;Inherit;True;Property;_NoiseTexture01;Noise Texture01;0;0;Create;True;0;0;0;False;0;False;-1;e199ee8112362864f831425463b09fda;e199ee8112362864f831425463b09fda;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2173.826,-216.3963;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0.18;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-2348.515,-432.2555;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;15;-1985.678,-439.1389;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1807.868,-181.9285;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;0.73;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-1606.868,-612.9285;Inherit;True;Ellipse;-1;;2;3ba94b7b3cfd5f447befde8107c04d52;0;3;2;FLOAT2;0,0;False;7;FLOAT;0.5;False;9;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;18;-1607.286,-364.4856;Inherit;True;Rectangle;-1;;1;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;0.5;False;3;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-1177.169,-775.809;Float;False;Property;_TintColor;Tint Color;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1280.868,-493.9285;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1332.006,-263.7639;Float;False;Property;_Ins;Ins;2;0;Create;True;0;0;0;False;0;False;5.319912;13.8;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;5;-1246.319,-179.2359;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-836.8408,-541.934;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;4;-981.3188,-263.936;Float;False;Property;_Useparticle;Use particle;3;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;8;-455.5078,-599.8093;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-637.1059,-394.1634;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-136.2319,-509.8104;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;171.4001,-419.1;Half;False;True;-1;0;ASEMaterialInspector;0;0;Unlit;FX/Particle Additive Thunder;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Off;2;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;0;10;0
WireConnection;11;0;10;0
WireConnection;13;1;12;0
WireConnection;1;1;11;0
WireConnection;14;0;1;0
WireConnection;14;1;13;0
WireConnection;15;0;14;0
WireConnection;15;1;16;0
WireConnection;15;2;16;0
WireConnection;20;7;19;0
WireConnection;20;9;19;0
WireConnection;18;1;15;0
WireConnection;18;3;19;0
WireConnection;21;0;20;0
WireConnection;21;1;18;0
WireConnection;6;0;7;0
WireConnection;6;1;21;0
WireConnection;4;1;3;0
WireConnection;4;0;5;3
WireConnection;2;0;6;0
WireConnection;2;1;4;0
WireConnection;9;0;8;0
WireConnection;9;1;2;0
WireConnection;0;2;9;0
WireConnection;0;9;8;4
ASEEND*/
//CHKSM=EAF74CFF87338E16C045DFD3C5CE1B723DD0BE25