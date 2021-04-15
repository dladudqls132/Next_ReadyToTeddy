// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Smoke"
{
	Properties
	{
		_Ins("Ins", Range( -1 , 0)) = -0.1755842
		_SmokeTexture("Smoke Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 0)) = -0.1755842
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha One
		CGPROGRAM
		#pragma target 2.0
		#pragma shader_feature _USEPARTICLE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			half2 uv_texcoord;
			half4 uv_tex4coord;
		};

		uniform sampler2D _SmokeTexture;
		uniform float4 _SmokeTexture_ST;
		uniform half _Dissolve;
		uniform half _Ins;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_SmokeTexture = i.uv_texcoord * _SmokeTexture_ST.xy + _SmokeTexture_ST.zw;
			#ifdef _USEPARTICLE_ON
				float staticSwitch16 = i.uv_tex4coord.z;
			#else
				float staticSwitch16 = _Dissolve;
			#endif
			#ifdef _USEPARTICLE_ON
				float staticSwitch17 = i.uv_tex4coord.w;
			#else
				float staticSwitch17 = _Ins;
			#endif
			o.Emission = ( ( i.vertexColor * saturate( ( tex2D( _SmokeTexture, uv_SmokeTexture ).r + staticSwitch16 ) ) ) * staticSwitch17 ).rgb;
			o.Alpha = i.vertexColor.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
1920;0;1920;1019;1209.033;923.1661;1.3;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-831.6016,-612.7966;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;15;-775.2068,-256.0407;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-815.9306,-343.264;Float;False;Property;_Dissolve;Dissolve;3;0;Create;True;0;0;False;0;-0.1755842;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-599.2161,-602.9524;Float;True;Property;_SmokeTexture;Smoke Texture;2;0;Create;True;0;0;False;0;aadc8ef477fda3942a1b19a41bf725d0;63949db0ec1f8754a8247326d901bfa0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;16;-547.2068,-307.0407;Float;False;Property;_UseParticle;Use Particle;4;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-279.1823,-574.2941;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;12;-34.65737,-588.697;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;10;11.16722,-403.5232;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-263.153,-187.4549;Float;False;Property;_Ins;Ins;1;0;Create;True;0;0;False;0;-0.1755842;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;231.5984,-532.9526;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;17;84.1471,-164.1549;Float;False;Property;_UseParticle;Use Particle;3;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;518.6666,-323.866;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;824.443,-418.2;Half;False;True;0;Half;ASEMaterialInspector;0;0;Unlit;FX/Smoke;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;1;11;0
WireConnection;16;1;9;0
WireConnection;16;0;15;3
WireConnection;4;0;1;1
WireConnection;4;1;16;0
WireConnection;10;0;4;0
WireConnection;13;0;12;0
WireConnection;13;1;10;0
WireConnection;17;1;18;0
WireConnection;17;0;15;4
WireConnection;19;0;13;0
WireConnection;19;1;17;0
WireConnection;0;2;19;0
WireConnection;0;9;12;4
ASEEND*/
//CHKSM=6EA6C82A9934D34AC5CA685285CC697F00525F70