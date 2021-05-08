// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX / Additive Dissolve"
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
		[Toggle(_USEPARTICLE_ON)] _Useparticle("Use particle", Float) = 0
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma shader_feature_local _USEPARTICLE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
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
		uniform sampler2D _MaskTexture;
		uniform float4 _MaskTexture_ST;
		uniform float _MaskPow;
		uniform sampler2D _MainTexture;
		uniform float _UPanner;
		uniform float _VPanner;
		uniform float4 _MainTexture_ST;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _Dissolve;
		uniform float _Ceil;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			#ifdef _USEPARTICLE_ON
				float staticSwitch71 = i.uv_tex4coord.w;
			#else
				float staticSwitch71 = _Ins;
			#endif
			float2 uv_MaskTexture = i.uv_texcoord * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
			half2 appendResult24 = (half2(_UPanner , _VPanner));
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			half2 panner21 = ( 1.0 * _Time.y * appendResult24 + uv_MainTexture);
			float2 uv_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			#ifdef _USEPARTICLE_ON
				float staticSwitch68 = i.uv_tex4coord.z;
			#else
				float staticSwitch68 = _Dissolve;
			#endif
			half temp_output_14_0 = ( ( ( saturate( pow( tex2D( _MaskTexture, uv_MaskTexture ).r , _MaskPow ) ) * tex2D( _MainTexture, panner21 ).r ) * saturate( ( tex2D( _DissolveTexture, uv_DissolveTexture ).r + staticSwitch68 ) ) ) * _Ceil );
			half temp_output_16_0 = ( temp_output_14_0 / _Ceil );
			o.Emission = ( i.vertexColor * ( staticSwitch71 * ( _TintColor * temp_output_16_0 ) ) ).rgb;
			o.Alpha = ( i.vertexColor.a * temp_output_16_0 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;0;1920;1019;847.9709;840.0593;1.29783;True;True
Node;AmplifyShaderEditor.CommentaryNode;66;-1085.881,-259.4439;Inherit;False;883.0001;405;main;6;23;22;24;20;21;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;65;-1249.798,-682.3749;Inherit;False;1044.705;411.592;mask;5;33;50;52;51;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;67;-1197.491,157.2534;Inherit;False;1117.395;659.2087;dissolve;7;68;58;57;56;54;55;70;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1019.881,30.55606;Float;False;Property;_VPanner;VPanner;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1019.881,-49.44392;Float;False;Property;_UPanner;UPanner;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1199.798,-632.3749;Inherit;True;0;50;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;58;-938.0955,440.4622;Float;False;Property;_Dissolve;Dissolve;10;0;Create;True;0;0;0;False;0;False;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;55;-1147.491,207.7305;Inherit;True;0;54;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-1035.881,-209.4439;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;50;-931.0929,-621.7828;Inherit;True;Property;_MaskTexture;Mask Texture;7;0;Create;True;0;0;0;False;0;False;-1;d2c7af2b651ea9741bf039787f5f36bd;d2c7af2b651ea9741bf039787f5f36bd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-864.0929,-385.7826;Float;False;Property;_MaskPow;Mask Pow;8;0;Create;True;0;0;0;False;0;False;2.33;-0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-843.8804,-33.44393;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;70;-923.5181,569.8014;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;-906.7015,207.2534;Inherit;True;Property;_DissolveTexture;Dissolve Texture ;9;0;Create;True;0;0;0;False;0;False;-1;ed2cf0efcc6b5224e8fd3ac550dc00a5;ed2cf0efcc6b5224e8fd3ac550dc00a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;68;-674.575,552.2419;Float;True;Property;_Useparticle;Use particle;11;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;21;-715.8804,-145.444;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;51;-619.0934,-592.7828;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-403.0934,-594.7828;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-617.0958,216.462;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-523.8807,-193.4439;Inherit;True;Property;_MainTexture;Main Texture;1;0;Create;True;0;0;0;False;0;False;-1;ed2cf0efcc6b5224e8fd3ac550dc00a5;ed2cf0efcc6b5224e8fd3ac550dc00a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;57;-403.0958,216.462;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-177.2301,-308.1499;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;129.1136,-2.879163;Float;False;Property;_Ceil;Ceil;2;0;Create;True;0;0;0;False;0;False;5.140479;4.82;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-56.74129,-228.0456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;448.5083,-255.3289;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;813.2158,-100.0491;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;667.803,-309.3137;Float;False;Property;_TintColor;Tint Color;5;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-98.93929,-451.0015;Float;False;Property;_Ins;Ins;6;0;Create;True;0;0;0;False;0;False;1;6.5;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;71;656.4361,-451.7928;Float;False;Property;_Useparticle;Use particle;10;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;877.1434,-271.9216;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;30;1096.079,-583.7477;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;951.073,-468.9072;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;1213.408,-401.362;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FloorOpNode;48;627.8312,-143.5989;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;1233.095,-220.4382;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1349.046,-431.5976;Half;False;True;-1;0;ASEMaterialInspector;0;0;Unlit;FX / Additive Dissolve;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;1;33;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;54;1;55;0
WireConnection;68;1;58;0
WireConnection;68;0;70;3
WireConnection;21;0;20;0
WireConnection;21;2;24;0
WireConnection;51;0;50;1
WireConnection;51;1;52;0
WireConnection;53;0;51;0
WireConnection;56;0;54;1
WireConnection;56;1;68;0
WireConnection;13;1;21;0
WireConnection;57;0;56;0
WireConnection;47;0;53;0
WireConnection;47;1;13;1
WireConnection;64;0;47;0
WireConnection;64;1;57;0
WireConnection;14;0;64;0
WireConnection;14;1;17;0
WireConnection;16;0;14;0
WireConnection;16;1;17;0
WireConnection;71;1;29;0
WireConnection;71;0;70;4
WireConnection;25;0;26;0
WireConnection;25;1;16;0
WireConnection;27;0;71;0
WireConnection;27;1;25;0
WireConnection;31;0;30;0
WireConnection;31;1;27;0
WireConnection;48;0;14;0
WireConnection;49;0;30;4
WireConnection;49;1;16;0
WireConnection;0;2;31;0
WireConnection;0;9;49;0
ASEEND*/
//CHKSM=A02898B8BB9249A4FEC83F86CC26EE3D38E25159