// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Fire"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Ceil("Ceil", Range( 1 , 20)) = 5.140479
		_UPanner("UPanner", Float) = 0
		_VPanner("VPanner", Float) = 0
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Ins("Ins", Range( 1 , 50)) = 1
		_MaskTexture("Mask Texture", 2D) = "white" {}
		_MaskPow("Mask Pow", Float) = 2.33
		_DissolveTexture("Dissolve Texture ", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		[Toggle(_USEPARTICLE_ON)] _Useparticle("Use particle", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma shader_feature _USEPARTICLE_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			half4 uv_tex4coord;
			half2 uv_texcoord;
		};

		uniform half _Ins;
		uniform half4 _TintColor;
		uniform sampler2D _MaskTexture;
		uniform float4 _MaskTexture_ST;
		uniform half _MaskPow;
		uniform sampler2D _MainTexture;
		uniform half _UPanner;
		uniform half _VPanner;
		uniform float4 _MainTexture_ST;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform half _Dissolve;
		uniform half _Ceil;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			#ifdef _USEPARTICLE_ON
				float staticSwitch71 = i.uv_tex4coord.w;
			#else
				float staticSwitch71 = _Ins;
			#endif
			float2 uv_MaskTexture = i.uv_texcoord * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
			float2 appendResult24 = (half2(_UPanner , _VPanner));
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float2 panner21 = ( 1.0 * _Time.y * appendResult24 + uv_MainTexture);
			float2 uv_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			#ifdef _USEPARTICLE_ON
				float staticSwitch68 = i.uv_tex4coord.z;
			#else
				float staticSwitch68 = _Dissolve;
			#endif
			float temp_output_16_0 = ( floor( ( ( ( saturate( pow( tex2D( _MaskTexture, uv_MaskTexture ).r , _MaskPow ) ) * tex2D( _MainTexture, panner21 ).r ) * saturate( ( tex2D( _DissolveTexture, uv_DissolveTexture ).r + staticSwitch68 ) ) ) * _Ceil ) ) / _Ceil );
			o.Emission = ( i.vertexColor * ( staticSwitch71 * ( _TintColor * temp_output_16_0 ) ) ).rgb;
			o.Alpha = ( i.vertexColor.a * temp_output_16_0 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
-126;102;1920;1012;2962.599;983.4713;2.893115;True;False
Node;AmplifyShaderEditor.CommentaryNode;66;-630.2454,-278.0957;Float;False;883.0001;405;main;6;23;22;24;20;21;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;65;-794.1624,-701.0267;Float;False;1044.705;411.592;mask;5;33;50;52;51;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;67;-741.8552,138.6017;Float;False;1117.395;659.2087;dissolve;7;68;58;57;56;54;55;70;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-744.1625,-651.0267;Float;True;0;50;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-564.2454,-68.09566;Float;False;Property;_UPanner;UPanner;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-564.2454,11.90432;Float;False;Property;_VPanner;VPanner;3;0;Create;True;0;0;False;0;0;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-475.4576,-640.4346;Float;True;Property;_MaskTexture;Mask Texture;6;0;Create;True;0;0;False;0;d2c7af2b651ea9741bf039787f5f36bd;42dd93a3ebe498047964b647b5284de8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;24;-388.2451,-52.09567;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;55;-691.8552,189.0788;Float;True;0;54;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-408.4576,-404.4344;Float;False;Property;_MaskPow;Mask Pow;7;0;Create;True;0;0;False;0;2.33;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-482.4602,421.8104;Float;False;Property;_Dissolve;Dissolve;9;0;Create;True;0;0;False;0;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;70;-467.8828,551.1497;Float;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-580.2454,-228.0957;Float;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;21;-260.2451,-164.0957;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;68;-218.9396,533.59;Float;True;Property;_Useparticle;Use particle;10;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;51;-163.4581,-611.4346;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;-451.0662,188.6017;Float;True;Property;_DissolveTexture;Dissolve Texture ;8;0;Create;True;0;0;False;0;ed2cf0efcc6b5224e8fd3ac550dc00a5;ed2cf0efcc6b5224e8fd3ac550dc00a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-68.24532,-212.0957;Float;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;ed2cf0efcc6b5224e8fd3ac550dc00a5;ed2cf0efcc6b5224e8fd3ac550dc00a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-161.4605,197.8103;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;52.54187,-613.4346;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;278.4052,-326.8017;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;52.53954,197.8103;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;398.894,-246.6974;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;371.9049,-83.82674;Float;False;Property;_Ceil;Ceil;1;0;Create;True;0;0;False;0;5.140479;8;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;539.3564,-252.7332;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;48;627.8312,-143.5989;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;356.696,-469.6533;Float;False;Property;_Ins;Ins;5;0;Create;True;0;0;False;0;1;12.2;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;667.803,-309.3137;Float;False;Property;_TintColor;Tint Color;4;0;Create;True;0;0;False;0;1,1,1,1;0.4103774,1,0.9632521,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;748.3242,-148.0688;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;71;656.4361,-451.7928;Float;False;Property;_Useparticle;Use particle;10;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;877.1434,-271.9216;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;30;1096.079,-583.7477;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;951.073,-468.9072;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;1233.095,-220.4382;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;1213.408,-401.362;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1349.046,-431.5976;Half;False;True;0;Half;ASEMaterialInspector;0;0;Unlit;Atents/Fx Toon shader;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;1;33;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;21;0;20;0
WireConnection;21;2;24;0
WireConnection;68;1;58;0
WireConnection;68;0;70;3
WireConnection;51;0;50;1
WireConnection;51;1;52;0
WireConnection;54;1;55;0
WireConnection;13;1;21;0
WireConnection;56;0;54;1
WireConnection;56;1;68;0
WireConnection;53;0;51;0
WireConnection;47;0;53;0
WireConnection;47;1;13;1
WireConnection;57;0;56;0
WireConnection;64;0;47;0
WireConnection;64;1;57;0
WireConnection;14;0;64;0
WireConnection;14;1;17;0
WireConnection;48;0;14;0
WireConnection;16;0;48;0
WireConnection;16;1;17;0
WireConnection;71;1;29;0
WireConnection;71;0;70;4
WireConnection;25;0;26;0
WireConnection;25;1;16;0
WireConnection;27;0;71;0
WireConnection;27;1;25;0
WireConnection;49;0;30;4
WireConnection;49;1;16;0
WireConnection;31;0;30;0
WireConnection;31;1;27;0
WireConnection;0;2;31;0
WireConnection;0;9;49;0
ASEEND*/
//CHKSM=16459FA361A8B9A39812DA19C596F5686C70B504