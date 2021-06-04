// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Practice/Crack"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_T_crack_rgb("T_crack_rgb", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Ins("Ins", Range( 1 , 20)) = 1
		_MaskRadius("Mask Radius", Range( 0 , 1)) = 0.61
		_MaskHardness("Mask Hardness", Range( 1 , 10)) = 0
		_VertexVal("Vertex Val", Range( 0 , 1)) = 1
		_HoleVal("Hole Val", Range( -1 , 1)) = -0.01
		_BaseColor("Base Color", Color) = (1,1,1,1)
		_Opacity("Opacity", Range( 0 , 1)) = 1
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 2.0
		#pragma shader_feature _USEPARTICLE_ON
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			half2 uv_texcoord;
			float4 vertexColor : COLOR;
			half4 uv_tex4coord;
		};

		uniform sampler2D _T_crack_rgb;
		uniform float4 _T_crack_rgb_ST;
		uniform half _VertexVal;
		uniform half4 _BaseColor;
		uniform half _HoleVal;
		uniform half _Ins;
		uniform half4 _TintColor;
		uniform half _Opacity;
		uniform half _MaskRadius;
		uniform half _MaskHardness;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_T_crack_rgb = v.texcoord * _T_crack_rgb_ST.xy + _T_crack_rgb_ST.zw;
			half4 tex2DNode1 = tex2Dlod( _T_crack_rgb, half4( uv_T_crack_rgb, 0, 0.0) );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( tex2DNode1.g + 0.0 ) * ase_vertexNormal ) * _VertexVal );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_T_crack_rgb = i.uv_texcoord * _T_crack_rgb_ST.xy + _T_crack_rgb_ST.zw;
			half4 tex2DNode1 = tex2D( _T_crack_rgb, uv_T_crack_rgb );
			half2 temp_cast_0 = (0.5).xx;
			float temp_output_10_0 = saturate( length( ( i.uv_texcoord - temp_cast_0 ) ) );
			o.Albedo = ( _BaseColor * ( tex2DNode1.b * saturate( pow( ( temp_output_10_0 - _HoleVal ) , 6.2 ) ) ) ).rgb;
			o.Emission = ( i.vertexColor * ( _Ins * ( _TintColor * tex2DNode1.r ) ) ).rgb;
			#ifdef _USEPARTICLE_ON
				float staticSwitch46 = i.uv_tex4coord.z;
			#else
				float staticSwitch46 = _Opacity;
			#endif
			o.Alpha = staticSwitch46;
			clip( ( pow( ( tex2DNode1.b * pow( saturate( ( ( 1.0 - temp_output_10_0 ) - _MaskRadius ) ) , _MaskHardness ) ) , 2.25 ) * 10.0 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
0;24;1920;995;-189.5796;1370.938;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;18;-1983.393,-330.2921;Float;False;1594.066;455.148;sphere;11;6;8;7;9;10;11;14;13;15;12;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1844.393,-21.29203;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1933.393,-280.2921;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-1689.393,-246.292;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;9;-1497.393,-246.292;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;10;-1332.85,-255.3139;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1322.055,-3.144071;Float;False;Property;_MaskRadius;Mask Radius;4;0;Create;True;0;0;False;0;0.61;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-1178.393,-249.4137;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-1007.455,-247.5441;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-235.4934,-994.542;Float;False;Property;_HoleVal;Hole Val;7;0;Create;True;0;0;False;0;-0.01;-0.69;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;126.8131,-1020.716;Float;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;6.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;29.70284,-1236.29;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-964.5549,9.855942;Float;False;Property;_MaskHardness;Mask Hardness;5;0;Create;True;0;0;False;0;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;-820.2551,-250.1441;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1281.753,-763.1808;Float;True;Property;_T_crack_rgb;T_crack_rgb;1;0;Create;True;0;0;False;0;800a24ddbf6ea3f40af872d147507709;800a24ddbf6ea3f40af872d147507709;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-884.4329,-916.9888;Float;False;Property;_TintColor;Tint Color;2;0;Create;True;0;0;False;0;1,1,1,1;0.6584936,0.4079299,0.9716981,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;12;-644.2402,-251.3685;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;39;246.1252,-1241.673;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;549.066,-419.6112;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;37;474.7041,-1228.997;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-120.7697,-497.019;Float;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;False;0;2.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-126.5965,-708.6119;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-562.4329,-898.9888;Float;False;Property;_Ins;Ins;3;0;Create;True;0;0;False;0;1;20;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-583.4329,-782.9888;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;516.5776,-201.1831;Float;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;870.8432,-131.2459;Float;False;Property;_VertexVal;Vertex Val;6;0;Create;True;0;0;False;0;1;0.23;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;778.1007,-1551.223;Float;False;Property;_BaseColor;Base Color;8;0;Create;True;0;0;False;0;1,1,1,1;0.5377358,0.3782093,0.2206746,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-190.5941,-871.325;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;47;1183.293,-755.5156;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;1121.489,-824.5374;Float;False;Property;_Opacity;Opacity;9;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;92.6235,-476.9939;Float;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;827.4276,-352.9218;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;733.2664,-1340.766;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;48;1207.837,-1092.806;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;20;73.00048,-712.5705;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;300.6234,-711.9936;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1067.101,-1284.223;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;46;1421.293,-767.5156;Float;False;Property;_UseParticle;Use Particle;10;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;905.018,-557.301;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;1588.646,-990.2159;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1847.222,-1035.875;Half;False;True;0;Half;ASEMaterialInspector;0;0;Standard;Practice/Crack;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;False;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;9;0;7;0
WireConnection;10;0;9;0
WireConnection;11;0;10;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;35;0;10;0
WireConnection;35;1;36;0
WireConnection;15;0;13;0
WireConnection;12;0;15;0
WireConnection;12;1;16;0
WireConnection;39;0;35;0
WireConnection;39;1;40;0
WireConnection;24;0;1;2
WireConnection;37;0;39;0
WireConnection;19;0;1;3
WireConnection;19;1;12;0
WireConnection;2;0;3;0
WireConnection;2;1;1;1
WireConnection;4;0;5;0
WireConnection;4;1;2;0
WireConnection;27;0;24;0
WireConnection;27;1;25;0
WireConnection;32;0;1;3
WireConnection;32;1;37;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;41;0;42;0
WireConnection;41;1;32;0
WireConnection;46;1;45;0
WireConnection;46;0;47;3
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;49;0;48;0
WireConnection;49;1;4;0
WireConnection;0;0;41;0
WireConnection;0;2;49;0
WireConnection;0;9;46;0
WireConnection;0;10;22;0
WireConnection;0;11;28;0
ASEEND*/
//CHKSM=D3FBAD5FDC3F386DEC9A64468B9F84997755C5C1