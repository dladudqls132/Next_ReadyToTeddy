// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Additive Dissolve teddy"
{
	Properties
	{
		_MainTexture("Main Texture ", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Pow("Pow", Float) = 4
		_Ins("Ins", Range( 1 , 50)) = 1
		_Opacity("Opacity", Range( 0 , 1)) = 1
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		_DissoveTexture("Dissove Texture", 2D) = "white" {}
		[Toggle(_KEYWORD0_ON)] _Keyword0("Keyword 0", Float) = 0
		_Dissolve("Dissolve", Range( -1 , 1)) = 1
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha One
		
		CGPROGRAM
		#pragma target 2.0
		#pragma shader_feature_local _KEYWORD0_ON
		#pragma shader_feature_local _USEPARTICLE_ON
		#pragma surface surf Unlit keepalpha noshadow nofog 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
			float2 uv_texcoord;
		};

		uniform float _Ins;
		uniform float4 _TintColor;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float _Pow;
		uniform sampler2D _DissoveTexture;
		uniform float4 _DissoveTexture_ST;
		uniform float _Dissolve;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			#ifdef _KEYWORD0_ON
				float staticSwitch30 = i.uv_tex4coord.w;
			#else
				float staticSwitch30 = _Ins;
			#endif
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			half4 tex2DNode1 = tex2D( _MainTexture, uv_MainTexture );
			o.Emission = ( i.vertexColor * ( staticSwitch30 * ( _TintColor * pow( tex2DNode1.r , _Pow ) ) ) ).rgb;
			float2 uv_DissoveTexture = i.uv_texcoord * _DissoveTexture_ST.xy + _DissoveTexture_ST.zw;
			half2 temp_cast_1 = (0.5).xx;
			float cos25 = cos( 0.0 );
			float sin25 = sin( 0.0 );
			half2 rotator25 = mul( uv_DissoveTexture - temp_cast_1 , float2x2( cos25 , -sin25 , sin25 , cos25 )) + temp_cast_1;
			#ifdef _USEPARTICLE_ON
				float staticSwitch23 = i.uv_tex4coord.z;
			#else
				float staticSwitch23 = _Dissolve;
			#endif
			o.Alpha = ( i.vertexColor.a * saturate( ( ( tex2DNode1.r * saturate( ( tex2D( _DissoveTexture, rotator25 ).r + staticSwitch23 ) ) ) * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1920;0;1920;1019;1233.287;663.8391;1.522884;True;True
Node;AmplifyShaderEditor.RangedFloatNode;28;-1151.136,674.5719;Float;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1079.136,557.5719;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1058,429.5;Inherit;False;0;16;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-1233,854.5;Float;False;Property;_Dissolve;Dissolve;9;0;Create;True;0;0;0;False;0;False;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-873.7434,918.6567;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;25;-805.1357,533.5719;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;16;-601,449.5;Inherit;True;Property;_DissoveTexture;Dissove Texture;7;0;Create;True;0;0;0;False;0;False;-1;ed2cf0efcc6b5224e8fd3ac550dc00a5;9f929de5b036eef4b885dc47b839f226;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;23;-579,820.5;Float;False;Property;_UseParticle;Use Particle;6;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-220,462.5;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-817,-48.5;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-394,187.5;Float;False;Property;_Pow;Pow;3;0;Create;True;0;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-495,-25.5;Inherit;True;Property;_MainTexture;Main Texture ;1;0;Create;True;0;0;0;False;0;False;-1;1e0ea9ef7c0c52b4996aff9ea5c91e01;50664bed1cbc4da40afed73768649e44;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;21;21,473.5;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;341,293.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-123,-153.5;Float;False;Property;_TintColor;Tint Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,0.2512522,0.09905647,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-333.8321,-436.6581;Float;False;Property;_Ins;Ins;4;0;Create;True;0;0;0;False;0;False;1;23.3;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;234,531.5;Float;False;Property;_Opacity;Opacity;5;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;2;-124,164.5;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-278.4385,-324.2361;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;131,-78.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;30;62.68728,-365.354;Float;False;Property;_Keyword0;Keyword 0;8;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;565,293.5;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;12;354,-302.5;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;11;812,189.5;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;325,-121.5;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;688,81.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;659,-153.5;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;867,-150;Half;False;True;-1;0;ASEMaterialInspector;0;0;Unlit;FX/Additive Dissolve teddy;False;False;False;False;False;False;False;False;False;True;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;18;0
WireConnection;25;1;26;0
WireConnection;25;2;28;0
WireConnection;16;1;25;0
WireConnection;23;1;20;0
WireConnection;23;0;24;3
WireConnection;19;0;16;1
WireConnection;19;1;23;0
WireConnection;1;1;17;0
WireConnection;21;0;19;0
WireConnection;22;0;1;1
WireConnection;22;1;21;0
WireConnection;2;0;1;1
WireConnection;2;1;3;0
WireConnection;5;0;6;0
WireConnection;5;1;2;0
WireConnection;30;1;8;0
WireConnection;30;0;29;4
WireConnection;9;0;22;0
WireConnection;9;1;10;0
WireConnection;11;0;9;0
WireConnection;7;0;30;0
WireConnection;7;1;5;0
WireConnection;14;0;12;4
WireConnection;14;1;11;0
WireConnection;13;0;12;0
WireConnection;13;1;7;0
WireConnection;0;2;13;0
WireConnection;0;9;14;0
ASEEND*/
//CHKSM=CCB6A9784FDE436C2C1BA8CC006CE76F5079B36C