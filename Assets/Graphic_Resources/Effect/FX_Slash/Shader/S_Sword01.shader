// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Sword01"
{
	Properties
	{
		_T_trail01_clamp("T_trail01_clamp", 2D) = "white" {}
		_U_Cont("U_Cont", Range( -1 , 1)) = 0.1344167
		_Float10("Float 10", Range( -1 , 1)) = -0.1008774
		_TintColor("Tint Color", Color) = (0.8679245,0.4789961,0.4789961,1)
		_EdgeColor("Edge Color", Color) = (0.8679245,0.4789961,0.4789961,1)
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		_Ins("Ins", Range( 0 , 10)) = 10
		_EdgeIns("Edge Ins", Range( 0 , 10)) = 0.6659619
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_NoiseVal("Noise Val", Range( 0 , 5)) = 0.2544345
		_T_Trail03("T_Trail03", 2D) = "white" {}
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma shader_feature_local _USEPARTICLE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float4 vertexColor : COLOR;
		};

		uniform float4 _TintColor;
		uniform float4 _EdgeColor;
		uniform float _EdgeIns;
		uniform sampler2D _T_Trail03;
		uniform float4 _T_Trail03_ST;
		uniform float _Float10;
		uniform float _Ins;
		uniform sampler2D _T_trail01_clamp;
		uniform sampler2D _NoiseTexture;
		uniform float4 _NoiseTexture_ST;
		uniform float _NoiseVal;
		uniform float _U_Cont;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _Dissolve;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_T_Trail03 = i.uv_texcoord * _T_Trail03_ST.xy + _T_Trail03_ST.zw;
			#ifdef _USEPARTICLE_ON
				float staticSwitch98 = i.uv2_tex4coord2.x;
			#else
				float staticSwitch98 = _Float10;
			#endif
			float2 appendResult95 = (float2(( (uv_T_Trail03).x + staticSwitch98 ) , ( (uv_T_Trail03).y + 0.0 )));
			float temp_output_68_0 = ( _EdgeIns * ( tex2D( _T_Trail03, appendResult95 ).r * saturate( ( saturate( ( uv_T_Trail03.y + -0.7 ) ) * 8.0 ) ) ) );
			float4 temp_output_71_0 = ( _EdgeColor * temp_output_68_0 );
			#ifdef _USEPARTICLE_ON
				float staticSwitch88 = i.uv2_tex4coord2.y;
			#else
				float staticSwitch88 = _Ins;
			#endif
			float2 appendResult55 = (float2(-0.25 , 0.0));
			float2 uv_NoiseTexture = i.uv_texcoord * _NoiseTexture_ST.xy + _NoiseTexture_ST.zw;
			float2 panner51 = ( 1.0 * _Time.y * appendResult55 + uv_NoiseTexture);
			#ifdef _USEPARTICLE_ON
				float staticSwitch101 = i.uv2_tex4coord2.w;
			#else
				float staticSwitch101 = _NoiseVal;
			#endif
			float2 temp_output_63_0 = ( i.uv_texcoord + ( pow( tex2D( _NoiseTexture, panner51 ).r , 2.0 ) * staticSwitch101 ) );
			#ifdef _USEPARTICLE_ON
				float staticSwitch87 = i.uv2_tex4coord2.x;
			#else
				float staticSwitch87 = _U_Cont;
			#endif
			float2 appendResult13 = (float2(( (temp_output_63_0).x + staticSwitch87 ) , ( (temp_output_63_0).y + 0.0 )));
			float4 tex2DNode1 = tex2D( _T_trail01_clamp, appendResult13 );
			o.Emission = ( ( _TintColor * ( temp_output_71_0 + pow( ( staticSwitch88 * tex2DNode1.r ) , 4.0 ) ) ) * i.vertexColor ).rgb;
			float2 uv_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			#ifdef _USEPARTICLE_ON
				float staticSwitch89 = i.uv2_tex4coord2.z;
			#else
				float staticSwitch89 = _Dissolve;
			#endif
			o.Alpha = ( i.vertexColor.a * ( ( ( temp_output_71_0 + ( tex2DNode1.r * saturate( ( pow( tex2D( _DissolveTexture, uv_DissolveTexture ).r , 2.58 ) + staticSwitch89 ) ) ) ) * saturate( pow( ( ( i.uv_texcoord.x * ( 1.0 - i.uv_texcoord.x ) ) * 4.0 ) , 4.0 ) ) ) * 6.0 ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;312;1920;705;422.805;1347.05;1.095348;True;True
Node;AmplifyShaderEditor.CommentaryNode;62;-2408.695,77.79709;Inherit;False;1373.597;513.3804;Noise;11;49;50;51;54;53;55;57;56;60;101;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2357.695,476.1772;Float;False;Constant;_Float8;Float 8;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2358.695,404.1772;Float;False;Constant;_Float7;Float 7;7;0;Create;True;0;0;0;False;0;False;-0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-2349.466,127.7971;Inherit;True;0;49;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;55;-2201.695,422.1772;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;51;-2072.465,188.7971;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;86;-1899.347,-529.5619;Inherit;True;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;49;-1823.373,226.7795;Inherit;True;Property;_NoiseTexture;Noise Texture;10;0;Create;True;0;0;0;False;0;False;-1;9f929de5b036eef4b885dc47b839f226;9f929de5b036eef4b885dc47b839f226;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-1709.594,439.4948;Float;False;Constant;_Float9;Float 9;7;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;80;-1690.091,-1467.224;Inherit;False;2586.907;885.5153;Edge;23;69;70;71;68;72;67;79;66;85;77;78;76;74;73;75;95;99;94;96;97;100;98;102;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1724.101,523.1387;Float;False;Property;_NoiseVal;Noise Val;11;0;Create;True;0;0;0;False;0;False;0.2544345;0.99;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;101;-1428.205,436.0369;Float;False;Property;_UseParticle;Use Particle;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;56;-1526.064,224.2842;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-831.7359,-831.6018;Float;False;Constant;_Float12;Float 12;11;0;Create;True;0;0;0;False;0;False;-0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-1662.794,-1155.563;Inherit;True;0;66;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;99;-1568.528,-1359.882;Float;False;Property;_Float10;Float 10;3;0;Create;True;0;0;0;False;0;False;-0.1008774;0.1142042;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;44;-799.2266,685.9164;Inherit;False;1596.389;982.0499;mask;2;34;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;45;-611.9324,191.6103;Inherit;False;1387.646;459.2623;Dissolve;8;39;37;41;38;36;42;46;89;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1267.228,-334.094;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1270.095,225.1009;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;98;-1247.311,-1322.826;Float;False;Property;_UseParticle;Use Particle;12;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;96;-1299.462,-1177.585;Inherit;True;True;False;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;94;-1279.497,-948.2046;Inherit;True;False;True;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-629.0972,-978.2926;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-1010.536,-1194.431;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-432.097,-760.293;Float;False;Constant;_Float13;Float 13;11;0;Create;True;0;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;76;-433.097,-979.2926;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;-1018.947,-969.4835;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-446.5382,264.2221;Inherit;True;0;36;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-914.3259,-220.1019;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1044.393,-571.1319;Float;False;Property;_U_Cont;U_Cont;2;0;Create;True;0;0;0;False;0;False;0.1344167;0.085;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;27;-749.2267,735.9166;Inherit;False;1425.286;470;Mask01;8;16;15;23;22;25;24;17;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;58;-657.0942,-287.0718;Inherit;True;True;False;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-699.2267,785.9166;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-152.6216,240.6104;Inherit;True;Property;_DissolveTexture;Dissolve Texture;6;0;Create;True;0;0;0;False;0;False;-1;None;5bcdd617621b3814c832481889afb4b5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-184.2865,431.7311;Float;False;Constant;_Float6;Float 6;4;0;Create;True;0;0;0;False;0;False;2.58;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;95;-784.8523,-1138.362;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;87;-723.1718,-542.8766;Float;False;Property;_UseParticle;Use Particle;12;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-268.0972,-976.4845;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-242.858,532.1598;Float;False;Property;_Dissolve;Dissolve;7;0;Create;True;0;0;0;False;0;False;0;-0.07316664;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;59;-622.1293,-69.6898;Inherit;True;False;True;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;41;154.7133,270.7311;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;89;120.9753,449.0908;Float;False;Property;_UseParticle;Use Particle;11;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-369.1687,-357.9174;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;17;-359.9705,896.4087;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-320.5791,-88.96877;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;85;-34.41361,-855.0145;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;66;-461.7844,-1207.996;Inherit;True;Property;_T_Trail03;T_Trail03;12;0;Create;True;0;0;0;False;0;False;-1;19a55a836105ba84c89403ed944c640d;19a55a836105ba84c89403ed944c640d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-83.62674,-477.628;Float;False;Property;_Ins;Ins;8;0;Create;True;0;0;0;False;0;False;10;2.06;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-54.072,-1197.993;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-165.864,-1307.809;Float;False;Property;_EdgeIns;Edge Ins;9;0;Create;True;0;0;0;False;0;False;0.6659619;2.54;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-168.94,1018.418;Float;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-183.2261,800.9166;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-115.484,-236.8475;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;353.7134,256.7309;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;72;410.1071,-1417.224;Float;False;Property;_EdgeColor;Edge Color;5;0;Create;True;0;0;0;False;0;False;0.8679245,0.4789961,0.4789961,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;88;230.5649,-472.8896;Float;False;Property;_UseParticle;Use Particle;11;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;39;577.7134,251.7309;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;36.06018,798.4176;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;115.4111,-171.6456;Inherit;True;Property;_T_trail01_clamp;T_trail01_clamp;0;0;Create;True;0;0;0;False;0;False;-1;ec65d4c35162c8d45a7678a19bd96a82;ec65d4c35162c8d45a7678a19bd96a82;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;94.06025,1018.418;Float;False;Constant;_Float3;Float 3;2;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;183.5358,-1215.159;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;528.478,-229.7819;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;985.7847,30.14449;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;587.303,8.768033;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;24;255.0602,797.4177;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;656.7897,-1235.094;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;6;762.8401,-194.3795;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;492.797,795.1547;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;1244.43,-128.7333;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;1565.073,260.8895;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;1155.377,-849.4597;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;1575.427,-798.3328;Float;False;Property;_TintColor;Tint Color;4;0;Create;True;0;0;0;False;0;False;0.8679245,0.4789961,0.4789961,1;1,0.4284859,0.08962259,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;1513.646,14.85347;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;1818.35,80.84983;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;34;-425.2042,1223.488;Inherit;False;1103.688;396.0001;Mask02;6;28;30;31;33;32;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;1517.171,-459.6196;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;91;1659.668,-159.0479;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;1894.562,-31.83291;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;31;216.4838,1276.488;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-375.2043,1303.606;Float;False;Constant;_Float4;Float 4;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;103;571.5521,-368.5887;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;32;417.4836,1273.488;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;163.0015,-975.8026;Float;False;Constant;_Float11;Float 11;1;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;70;453.101,-1054.99;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-196.0725,1282.559;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;30;15.48354,1281.488;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;1808.802,-251.7561;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;220.4838,1504.489;Float;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;0;False;0;False;9.39;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;90;-1981.046,1207.235;Float;False;Property;_UseParticle;Use Particle;11;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;102;446.5475,-1175.78;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2157.862,-219.9999;Float;False;True;-1;0;ASEMaterialInspector;0;0;Unlit;FX/Sword01;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;0;53;0
WireConnection;55;1;54;0
WireConnection;51;0;50;0
WireConnection;51;2;55;0
WireConnection;49;1;51;0
WireConnection;101;1;61;0
WireConnection;101;0;86;4
WireConnection;56;0;49;1
WireConnection;56;1;57;0
WireConnection;60;0;56;0
WireConnection;60;1;101;0
WireConnection;98;1;99;0
WireConnection;98;0;86;1
WireConnection;96;0;73;0
WireConnection;94;0;73;0
WireConnection;74;0;73;2
WireConnection;74;1;75;0
WireConnection;97;0;96;0
WireConnection;97;1;98;0
WireConnection;76;0;74;0
WireConnection;100;0;94;0
WireConnection;63;0;10;0
WireConnection;63;1;60;0
WireConnection;58;0;63;0
WireConnection;36;1;46;0
WireConnection;95;0;97;0
WireConnection;95;1;100;0
WireConnection;87;1;14;0
WireConnection;87;0;86;1
WireConnection;77;0;76;0
WireConnection;77;1;78;0
WireConnection;59;0;63;0
WireConnection;41;0;36;1
WireConnection;41;1;42;0
WireConnection;89;1;38;0
WireConnection;89;0;86;3
WireConnection;11;0;58;0
WireConnection;11;1;87;0
WireConnection;17;0;16;1
WireConnection;12;0;59;0
WireConnection;85;0;77;0
WireConnection;66;1;95;0
WireConnection;79;0;66;1
WireConnection;79;1;85;0
WireConnection;15;0;16;1
WireConnection;15;1;17;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;37;0;41;0
WireConnection;37;1;89;0
WireConnection;88;1;48;0
WireConnection;88;0;86;2
WireConnection;39;0;37;0
WireConnection;22;0;15;0
WireConnection;22;1;23;0
WireConnection;1;1;13;0
WireConnection;68;0;67;0
WireConnection;68;1;79;0
WireConnection;47;0;88;0
WireConnection;47;1;1;1
WireConnection;84;0;1;1
WireConnection;84;1;39;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;71;0;72;0
WireConnection;71;1;68;0
WireConnection;6;0;47;0
WireConnection;6;1;7;0
WireConnection;26;0;24;0
WireConnection;83;0;71;0
WireConnection;83;1;84;0
WireConnection;81;0;71;0
WireConnection;81;1;6;0
WireConnection;35;0;83;0
WireConnection;35;1;26;0
WireConnection;4;0;35;0
WireConnection;4;1;2;0
WireConnection;9;0;8;0
WireConnection;9;1;81;0
WireConnection;93;0;91;4
WireConnection;93;1;4;0
WireConnection;31;0;30;0
WireConnection;103;0;88;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;28;0;16;1
WireConnection;28;1;29;0
WireConnection;30;0;28;0
WireConnection;92;0;9;0
WireConnection;92;1;91;0
WireConnection;102;0;68;0
WireConnection;0;2;92;0
WireConnection;0;9;93;0
ASEEND*/
//CHKSM=4F913547E6F187C0AF5B0662C4A0A1BDBA221E54