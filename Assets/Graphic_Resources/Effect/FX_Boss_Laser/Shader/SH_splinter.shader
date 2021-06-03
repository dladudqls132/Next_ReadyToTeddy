// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New Amplify Shader 1"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float0("Float 0", Float) = 0.3
		_Float1("Float 1", Float) = 1.21
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_Float2("Float 2", Float) = 3.15
		_Float3("Float 3", Float) = 3.15
		_noisescale("noise scale", Float) = 2.56
		_Vector0("Vector 0", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color0;
		uniform float2 _Vector0;
		uniform float _noisescale;
		uniform float _Float0;
		uniform float _Float3;
		uniform float _Float2;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Float1;
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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			float2 uv_TexCoord1 = i.uv_texcoord * _Vector0;
			float simplePerlin2D2 = snoise( uv_TexCoord1*_noisescale );
			simplePerlin2D2 = simplePerlin2D2*0.5 + 0.5;
			float temp_output_10_0 = i.uv_tex4coord.x;
			float temp_output_18_0 = i.uv_tex4coord.y;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode24 = tex2D( _TextureSample0, uv_TextureSample0 );
			float clampResult28 = clamp( ( step( pow( ( simplePerlin2D2 * _Float0 * ( 1.0 - ( pow( ( ( 1.0 - temp_output_10_0 ) * temp_output_10_0 ) , _Float3 ) * _Float2 ) ) * ( 1.0 - ( pow( ( ( 1.0 - temp_output_18_0 ) * temp_output_18_0 ) , _Float3 ) * _Float2 ) ) ) , i.uv_tex4coord.z ) , i.uv_tex4coord.w ) * ( ( 1.0 - tex2DNode24.r ) * _Float1 ) * i.vertexColor.b ) , 0.0 , 1.0 );
			o.Alpha = clampResult28;
			clip( clampResult28 - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
61;653;1379;683;1131.044;268.3761;1.951461;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-858.2331,-265.4698;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;18;-624.0707,-228.631;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;10;-622.5493,-387.2303;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-409.1959,-506.6805;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;-409.4367,-349.3616;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-234.8063,-411.1021;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-243.8798,-323.3987;Inherit;False;Property;_Float3;Float 3;6;0;Create;True;0;0;0;False;0;False;3.15;3.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-235.0471,-253.7833;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;13;-81.60592,-409.913;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;32;-1259.657,-32.43291;Inherit;False;Property;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;26;-100.2737,-316.8775;Inherit;False;Property;_Float2;Float 2;5;0;Create;True;0;0;0;False;0;False;3.15;3.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;21;-69.04168,-248.7526;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;78.11895,-406.9556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;74.03664,-224.0267;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3.97;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-926.7671,105.7639;Inherit;False;Property;_noisescale;noise scale;7;0;Create;True;0;0;0;False;0;False;2.56;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1031.915,-58.641;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;15;246.8011,-496.6243;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;2;-718.7728,-79.42174;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-569.0118,174.8744;Inherit;False;Property;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;23;263.2069,-244.5479;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-389.9507,79.6221;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-781.4368,286.8415;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;34;-191.0626,305.2049;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-102.993,521.2252;Inherit;False;Property;_Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;1.21;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;5;-235.2795,33.60185;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;6;-7.190945,-6.393212;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;38.86768,296.2661;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;36;270.7691,339.346;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;287.8669,71.13608;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;33;-457.9116,296.3099;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;28;505.6019,114.9874;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;600.866,-101.9199;Inherit;False;Property;_Color0;Color 0;4;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;819.2921,-60.30751;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;New Amplify Shader 1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;9;2
WireConnection;10;0;9;1
WireConnection;11;0;10;0
WireConnection;19;0;18;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;20;0;19;0
WireConnection;20;1;18;0
WireConnection;13;0;12;0
WireConnection;13;1;35;0
WireConnection;21;0;20;0
WireConnection;21;1;35;0
WireConnection;14;0;13;0
WireConnection;14;1;26;0
WireConnection;22;0;21;0
WireConnection;22;1;26;0
WireConnection;1;0;32;0
WireConnection;15;0;14;0
WireConnection;2;0;1;0
WireConnection;2;1;30;0
WireConnection;23;0;22;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;3;2;15;0
WireConnection;3;3;23;0
WireConnection;34;0;24;1
WireConnection;5;0;3;0
WireConnection;5;1;9;3
WireConnection;6;0;5;0
WireConnection;6;1;9;4
WireConnection;8;0;34;0
WireConnection;8;1;25;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;7;2;36;3
WireConnection;33;0;24;1
WireConnection;28;0;7;0
WireConnection;0;0;29;0
WireConnection;0;9;28;0
WireConnection;0;10;28;0
ASEEND*/
//CHKSM=FE930C8C5F0EA222B619DE402CF9F24511A1E813