// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Crack"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.05
		_T_crack_rgb("T_crack_rgb", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Ins("Ins", Range( 1 , 20)) = 1
		_MaskRadius("Mask Radius", Range( -1 , 1)) = 0.61
		_MaskHardness("Mask Hardness", Range( 1 , 10)) = 0
		_VertexVal("Vertex Val", Range( 0 , 1)) = 1
		_HoleRadius("Hole Radius", Range( -1 , 1)) = -0.01
		_FloorColor("Floor Color", Color) = (1,1,1,1)
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
			half4 uv_tex4coord;
		};

		uniform sampler2D _T_crack_rgb;
		uniform float4 _T_crack_rgb_ST;
		uniform half _VertexVal;
		uniform half4 _FloorColor;
		uniform half _HoleRadius;
		uniform half _Ins;
		uniform half4 _TintColor;
		uniform half _MaskRadius;
		uniform half _MaskHardness;
		uniform float _Cutoff = 0.05;

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
			o.Albedo = ( _FloorColor * ( tex2DNode1.b * saturate( pow( ( temp_output_10_0 - _HoleRadius ) , 6.2 ) ) ) ).rgb;
			o.Emission = ( _Ins * ( _TintColor * tex2DNode1.r ) ).rgb;
			o.Alpha = 1;
			#ifdef _USEPARTICLE_ON
				float staticSwitch44 = i.uv_tex4coord.z;
			#else
				float staticSwitch44 = _MaskHardness;
			#endif
			clip( ( pow( ( tex2DNode1.b * pow( saturate( ( ( 1.0 - temp_output_10_0 ) - _MaskRadius ) ) , staticSwitch44 ) ) , 2.25 ) * 10.0 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
1920;0;1920;1019;1942.139;644.0945;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;18;-2109.886,-423.3725;Float;False;1594.066;455.148;sphere;11;6;8;7;9;10;11;14;13;15;12;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1970.887,-114.3724;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-2059.887,-373.3725;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-1815.887,-339.3724;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;9;-1623.887,-339.3724;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;10;-1459.344,-348.3943;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1448.549,-96.22448;Float;False;Property;_MaskRadius;Mask Radius;4;0;Create;True;0;0;False;0;0.61;0.32;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-1304.887,-342.4941;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;43;-1047.139,18.90552;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-1091.049,-83.22446;Float;False;Property;_MaskHardness;Mask Hardness;5;0;Create;True;0;0;False;0;0;5.48;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-337.4934,-1116.542;Float;False;Property;_HoleRadius;Hole Radius;7;0;Create;True;0;0;False;0;-0.01;-0.824;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-1118.949,-352.6245;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;-32.29716,-1323.29;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;44;-818.1393,-16.09448;Float;False;Property;_UseParticle;Use Particle;9;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;-946.749,-343.2245;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;111.8131,-1019.716;Float;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;6.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1281.753,-763.1808;Float;True;Property;_T_crack_rgb;T_crack_rgb;1;0;Create;True;0;0;False;0;800a24ddbf6ea3f40af872d147507709;800a24ddbf6ea3f40af872d147507709;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;39;242.1252,-1332.673;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;12;-770.7341,-344.4489;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;326.2031,-127.8634;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-884.4329,-916.9888;Float;False;Property;_TintColor;Tint Color;2;0;Create;True;0;0;False;0;1,1,1,1;1,0.2611018,0.004716992,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;37;491.7041,-1287.997;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;293.7147,90.56467;Float;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-343.6327,-205.2712;Float;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;False;0;2.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-282.6324,-519.4915;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;20;-83.03544,-523.4501;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;742.0771,-1331.955;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;778.1007,-1551.223;Float;False;Property;_FloorColor;Floor Color;8;0;Create;True;0;0;False;0;1,1,1,1;0.6037736,0.6037736,0.6037736,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;647.9802,160.5019;Float;False;Property;_VertexVal;Vertex Val;6;0;Create;True;0;0;False;0;1;0.274;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;604.5646,-61.17403;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-583.4329,-782.9888;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-562.4329,-898.9888;Float;False;Property;_Ins;Ins;3;0;Create;True;0;0;False;0;1;20;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-130.2394,-185.2461;Float;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1067.101,-1284.223;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;144.5875,-522.8732;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-237.3059,-833.511;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;682.155,-265.5532;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1369.222,-1066.875;Half;False;True;0;Half;ASEMaterialInspector;0;0;Standard;FX/Crack;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;0;False;0;Custom;0.05;True;False;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;9;0;7;0
WireConnection;10;0;9;0
WireConnection;11;0;10;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;35;0;10;0
WireConnection;35;1;36;0
WireConnection;44;1;16;0
WireConnection;44;0;43;3
WireConnection;15;0;13;0
WireConnection;39;0;35;0
WireConnection;39;1;40;0
WireConnection;12;0;15;0
WireConnection;12;1;44;0
WireConnection;24;0;1;2
WireConnection;37;0;39;0
WireConnection;19;0;1;3
WireConnection;19;1;12;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;32;0;1;3
WireConnection;32;1;37;0
WireConnection;27;0;24;0
WireConnection;27;1;25;0
WireConnection;2;0;3;0
WireConnection;2;1;1;1
WireConnection;41;0;42;0
WireConnection;41;1;32;0
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;4;0;5;0
WireConnection;4;1;2;0
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;0;0;41;0
WireConnection;0;2;4;0
WireConnection;0;10;22;0
WireConnection;0;11;28;0
ASEEND*/
//CHKSM=D48DD0908A1EBA66433CDBD38A374C263F5E9986