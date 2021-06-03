// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/smoke dissolve"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 1
		_Pow("Pow", Range( 1 , 6)) = 2.647059
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_DissolveTextureRotator("Dissolve Texture Rotator", Float) = 12.86
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		_Opacity("Opacity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 2.0
		#pragma shader_feature_local _USEPARTICLE_ON
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
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
		uniform float _Pow;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _DissolveTextureRotator;
		uniform float _Dissolve;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			half4 temp_cast_0 = (_Pow).xxxx;
			half4 temp_output_6_0 = pow( tex2D( _MainTexture, uv_MainTexture ) , temp_cast_0 );
			float2 uv_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			#ifdef _USEPARTICLE_ON
				float staticSwitch16 = i.uv_tex4coord.w;
			#else
				float staticSwitch16 = _DissolveTextureRotator;
			#endif
			float cos12 = cos( staticSwitch16 );
			float sin12 = sin( staticSwitch16 );
			half2 rotator12 = mul( uv_DissolveTexture - float2( 0.5,0.5 ) , float2x2( cos12 , -sin12 , sin12 , cos12 )) + float2( 0.5,0.5 );
			#ifdef _USEPARTICLE_ON
				float staticSwitch15 = i.uv_tex4coord.z;
			#else
				float staticSwitch15 = _Dissolve;
			#endif
			half temp_output_22_0 = saturate( ( tex2D( _DissolveTexture, rotator12 ).r + staticSwitch15 ) );
			o.Albedo = ( i.vertexColor * ( ( _TintColor * temp_output_6_0 ) * temp_output_22_0 ) ).rgb;
			o.Alpha = ( i.vertexColor.a * saturate( ( ( temp_output_6_0 * temp_output_22_0 ) * _Opacity ) ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;6;1920;1013;295.1531;1042.003;1.141233;True;True
Node;AmplifyShaderEditor.TexCoordVertexDataNode;17;-2277.025,-120.2307;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-2326.275,-211.257;Float;False;Property;_DissolveTextureRotator;Dissolve Texture Rotator;5;0;Create;True;0;0;0;False;0;False;12.86;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;16;-2032.498,-212.3571;Float;False;Property;_UseParticle;Use Particle;6;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2323.747,-467.0629;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;13;-2233.275,-332.257;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;12;-1821.275,-338.257;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1867.397,-127.5327;Float;False;Property;_Dissolve;Dissolve;2;0;Create;True;0;0;0;False;0;False;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1642.412,-312.8379;Inherit;True;Property;_DissolveTexture;Dissolve Texture;1;0;Create;True;0;0;0;False;0;False;-1;9f929de5b036eef4b885dc47b839f226;bc7c2443c12109d4397562c7053fad44;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;15;-1537.992,-86.6323;Float;False;Property;_UseParticle;Use Particle;6;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1892.747,-602.663;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-1585.084,-396.7663;Float;False;Property;_Pow;Pow;3;0;Create;True;0;0;0;False;0;False;2.647059;1.5;1;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1144.531,-190.8835;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1664.793,-597.6742;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;0;False;0;False;-1;aadc8ef477fda3942a1b19a41bf725d0;63949db0ec1f8754a8247326d901bfa0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;6;-1265.091,-609.3079;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;22;-122.3637,-360.0282;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-480.0266,-732.0339;Float;False;Property;_TintColor;Tint Color;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;141.8882,-4.254578;Float;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;125.036,-375.7281;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;337.8882,-173.2546;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-176.6303,-731.8486;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;199.9037,-621.1147;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;25;567.8882,-183.2546;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;19;100.8459,-833.5338;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;560.153,-739.4146;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;576.5482,-461.1862;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;990.369,-606.3621;Half;False;True;-1;0;ASEMaterialInspector;0;0;Standard;FX/smoke dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;1;14;0
WireConnection;16;0;17;4
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;12;2;16;0
WireConnection;2;1;12;0
WireConnection;15;1;4;0
WireConnection;15;0;17;3
WireConnection;3;0;2;1
WireConnection;3;1;15;0
WireConnection;1;1;10;0
WireConnection;6;0;1;0
WireConnection;6;1;7;0
WireConnection;22;0;3;0
WireConnection;23;0;6;0
WireConnection;23;1;22;0
WireConnection;24;0;23;0
WireConnection;24;1;26;0
WireConnection;9;0;8;0
WireConnection;9;1;6;0
WireConnection;5;0;9;0
WireConnection;5;1;22;0
WireConnection;25;0;24;0
WireConnection;20;0;19;0
WireConnection;20;1;5;0
WireConnection;21;0;19;4
WireConnection;21;1;25;0
WireConnection;0;0;20;0
WireConnection;0;9;21;0
ASEEND*/
//CHKSM=15EFC4F02549C1F0FD5DFF0F411FA125064ABC2E