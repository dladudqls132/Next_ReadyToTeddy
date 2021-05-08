// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Particle Alpha blend"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Power("Power", Range( 0 , 10)) = 4.632798
		_Ins("Ins", Range( 0 , 20)) = 10
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		_Opacity("Opacity", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 2.0
		#pragma shader_feature _USEPARTICLE_ON
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
		};

		uniform float4 _TintColor;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float _Power;
		uniform float _Ins;
		uniform float _Opacity;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTexture, uv_MainTexture );
			float4 temp_cast_0 = (_Power).xxxx;
			#ifdef _USEPARTICLE_ON
				float staticSwitch17 = i.uv_tex4coord.z;
			#else
				float staticSwitch17 = _Ins;
			#endif
			o.Emission = ( i.vertexColor * ( pow( ( _TintColor * tex2DNode1 ) , temp_cast_0 ) * staticSwitch17 ) ).rgb;
			#ifdef _USEPARTICLE_ON
				float staticSwitch15 = i.uv_tex4coord.w;
			#else
				float staticSwitch15 = _Opacity;
			#endif
			o.Alpha = ( i.vertexColor.a * ( tex2DNode1.r * staticSwitch15 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
0;0;1920;1019;1372.574;846.715;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-858.9243,-235.8246;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-589.4368,-240.5917;Float;True;Property;_MainTexture;Main Texture;1;0;Create;True;0;0;False;0;None;1c6088deee102c4469441bde13e6e5b1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-377.4492,-510.0036;Float;False;Property;_TintColor;Tint Color;2;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-13.77794,99.27599;Float;False;Property;_Opacity;Opacity;5;0;Create;True;0;0;False;0;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-225.3207,-223.4114;Float;False;Property;_Power;Power;3;0;Create;True;0;0;False;0;4.632798;4.632798;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-274.7329,-57.41805;Float;False;Property;_Ins;Ins;4;0;Create;True;0;0;False;0;10;5.319912;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;16;-312.0444,62.96594;Float;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-178.3087,-444.0945;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;6;55.64308,-363.453;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;15;281.8079,128.8494;Float;False;Property;_UseParticle;Use Particle;6;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;17;38.25557,-111.2341;Float;False;Property;_UseParticle;Use Particle;5;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;365.2618,-313.2893;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;10;347.9003,-561.0453;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;518.3051,-73.4837;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;654.6848,-512.2248;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;763.5781,-262.8821;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;993.3244,-531.2192;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;FX/Particle Alpha blend;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;2;False;-1;7;False;-1;False;0;0;False;0;Custom;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;1;9;0
WireConnection;2;0;3;0
WireConnection;2;1;1;0
WireConnection;6;0;2;0
WireConnection;6;1;7;0
WireConnection;15;1;13;0
WireConnection;15;0;16;4
WireConnection;17;1;8;0
WireConnection;17;0;16;3
WireConnection;4;0;6;0
WireConnection;4;1;17;0
WireConnection;12;0;1;1
WireConnection;12;1;15;0
WireConnection;11;0;10;0
WireConnection;11;1;4;0
WireConnection;14;0;10;4
WireConnection;14;1;12;0
WireConnection;0;2;11;0
WireConnection;0;9;14;0
ASEEND*/
//CHKSM=13119CFC401090CC5AE3677EB70990EA62974985