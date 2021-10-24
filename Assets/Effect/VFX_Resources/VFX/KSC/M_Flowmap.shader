// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/flowmap"
{
	Properties
	{
		_speed("speed", Float) = 0.2
		_Flowmap("Flow map", 2D) = "white" {}
		//_Tiling("Tiling", Float) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_strength("strength", Float) = 1
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_emissive("emissive", 2D) = "white" {}
		_normal("normal", 2D) = "white" {}
		_diffuse("diffuse", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty("", Int) = 1
		[ShowAsVector2] _Tiling("Tiling", Vector) = (1, 1, 0, 0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _normal;
		uniform sampler2D _Flowmap;
		uniform float4 _Flowmap_ST;
		uniform float _strength;
		uniform float _speed;
		uniform float4 _Tiling;
		uniform sampler2D _diffuse;
		uniform sampler2D _emissive;
		uniform float4 _Color0;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Flowmap = i.uv_texcoord * _Flowmap_ST.xy + _Flowmap_ST.zw;
			float2 blendOpSrc24 = i.uv_texcoord;
			float2 blendOpDest24 = (tex2D( _Flowmap, uv_Flowmap )).rg;
			float2 temp_output_24_0 = ( saturate( (( blendOpDest24 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest24 ) * ( 1.0 - blendOpSrc24 ) ) : ( 2.0 * blendOpDest24 * blendOpSrc24 ) ) ));
			float temp_output_30_0 = ( _Time.y * _speed );
			float temp_output_1_0_g3 = temp_output_30_0;
			float temp_output_32_0 = (0.0 + (( ( temp_output_1_0_g3 - floor( ( temp_output_1_0_g3 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float TimeA34 = ( _strength * -temp_output_32_0 );
			float2 lerpResult25 = lerp( i.uv_texcoord , temp_output_24_0 , TimeA34);
			float2 temp_cast_0 = float2(_Tiling.x, _Tiling.y);
			float2 uv_TexCoord43 = i.uv_texcoord * temp_cast_0;
			float2 Tiling44 = uv_TexCoord43;
			float2 FlowA26 = ( lerpResult25 + Tiling44 );
			float temp_output_1_0_g4 = (temp_output_30_0*1.0 + 0.5);
			float TimeB51 = ( _strength * -(0.0 + (( ( temp_output_1_0_g4 - floor( ( temp_output_1_0_g4 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) );
			float2 lerpResult54 = lerp( i.uv_texcoord , temp_output_24_0 , TimeB51);
			float2 FlowB57 = ( lerpResult54 + Tiling44 );
			float Blendtime68 = saturate( abs( ( 1.0 - ( temp_output_32_0 / 0.5 ) ) ) );
			float4 lerpResult75 = lerp( tex2D( _normal, FlowA26 ) , tex2D( _normal, FlowB57 ) , Blendtime68);
			float4 Normal73 = lerpResult75;
			o.Normal = Normal73.rgb;
			float4 lerpResult61 = lerp( tex2D( _diffuse, FlowA26 ) , tex2D( _diffuse, FlowB57 ) , Blendtime68);
			float4 Diffuse39 = lerpResult61;
			o.Albedo = Diffuse39.rgb;
			float4 lerpResult90 = lerp( ( tex2D( _emissive, FlowA26 ) * _Color0 ) , ( tex2D( _emissive, FlowB57 ) * _Color0 ) , Blendtime68);
			float4 Emissive91 = lerpResult90;
			o.Emission = Emissive91.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
-50;128;1906;763;2129.891;486.4071;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;38;-3435.113,-574.4626;Inherit;False;1756.729;1027.07;;20;34;33;32;31;30;29;28;48;49;50;51;52;64;65;66;67;68;82;83;84;Time;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-3389.713,-309.0626;Inherit;True;Property;_speed;speed;0;0;Create;True;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;28;-3385.113,-524.4626;Inherit;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-3206.113,-392.6627;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;48;-3073.236,-221.9649;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;52;-2824.753,-232.1171;Inherit;True;Sawtooth Wave;-1;;4;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;31;-3059.913,-478.0627;Inherit;True;Sawtooth Wave;-1;;3;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;32;-2843.838,-462.7;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;49;-2641.929,-221.8341;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;45;-3132.993,-1934.102;Inherit;False;726.8071;209;;3;43;44;42;Diffuse tiling;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;36;-3464.309,-1608.99;Inherit;False;1768.356;899.1131;;13;35;25;26;24;23;22;21;47;46;55;54;56;57;Flow map UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.NegateNode;50;-2471.55,-188.7766;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;33;-2638.912,-422.5818;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2542.662,-355.4175;Inherit;False;Property;_strength;strength;6;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-2345.662,-424.4175;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2333.662,-279.4175;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-3413.161,-1245.423;Inherit;True;Property;_Flowmap;Flow map;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-3054.993,-1859.204;Inherit;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;22;-3103.602,-1252.079;Inherit;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-2213.918,-456.3586;Inherit;False;TimeA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-2883.293,-1884.102;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-2147.645,-186.0948;Inherit;False;TimeB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-3094.951,-1554.403;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;55;-2756.651,-967.9402;Inherit;False;51;TimeB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;24;-2851.075,-1282.292;Inherit;True;Overlay;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-2781.317,-1477.837;Inherit;False;34;TimeA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-2630.186,-1872.492;Inherit;False;Tiling;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;25;-2561.267,-1530.477;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;64;-2762.018,37.4306;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;54;-2551.375,-1227.564;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-2486.835,-1308.281;Inherit;False;44;Tiling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-2266.003,-1229.419;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-2279.935,-1475.38;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;65;-2649.045,47.55452;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-2101.667,-1455.077;Inherit;False;FlowA;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;66;-2490.36,51.45351;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-2096.003,-1244.419;Inherit;False;FlowB;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;100;-1568.37,-447.4457;Inherit;True;Property;_emissive;emissive;8;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-1564.8,45.33701;Inherit;True;57;FlowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1566.951,-195.0649;Inherit;True;26;FlowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;67;-2367.214,57.43126;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;63;-1623.854,-1808.631;Inherit;False;1039.1;601.4016;Comment;8;41;58;40;39;62;61;69;97;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;99;-1356.37,14.55426;Inherit;True;Property;_ems1;ems;3;0;Create;True;0;0;False;0;False;-1;cb175b802e4b3e54aa97c17e2e7dfc9d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1660.717,-2027.516;Inherit;True;Property;_diffuse;diffuse;10;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-1553.973,-733.9903;Inherit;True;57;FlowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-1573.854,-1753.631;Inherit;True;26;FlowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;87;-1346.951,-200.0649;Inherit;True;Property;_ems;ems;3;0;Create;True;0;0;False;0;False;-1;cb175b802e4b3e54aa97c17e2e7dfc9d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;102;-1584.88,-1211.253;Inherit;True;Property;_normal;normal;9;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;-2210.53,44.47575;Inherit;False;Blendtime;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-1571.703,-1513.229;Inherit;True;57;FlowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;95;-1297.072,275.8892;Inherit;False;Property;_Color0;Color 0;7;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;70;-1556.124,-974.3922;Inherit;True;26;FlowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-1004.721,-127.3145;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-999.7209,99.68549;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;76;-1334.973,-741.9903;Inherit;True;Property;_Nomal;Nomal;5;0;Create;True;0;0;False;0;False;-1;cb175b802e4b3e54aa97c17e2e7dfc9d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;77;-945.2689,-521.0693;Inherit;False;68;Blendtime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-930.2316,-6.632385;Inherit;False;68;Blendtime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;97;-1359.852,-1537.428;Inherit;True;Property;_Diffuse5;Diffuse;1;0;Create;True;0;0;False;0;False;-1;cb175b802e4b3e54aa97c17e2e7dfc9d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;69;-962.9987,-1300.308;Inherit;False;68;Blendtime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-1336.124,-979.3922;Inherit;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;False;-1;cb175b802e4b3e54aa97c17e2e7dfc9d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;40;-1353.854,-1758.631;Inherit;True;Property;_Diffuse;Diffuse;1;0;Create;True;0;0;False;0;False;-1;cb175b802e4b3e54aa97c17e2e7dfc9d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;90;-699.8003,-107.6631;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;61;-995.7035,-1610.229;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;75;-977.9736,-830.9904;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;-479.8509,-138.6641;Inherit;True;Emissive;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-808.754,-1714.23;Inherit;True;Diffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-791.0242,-934.9914;Inherit;True;Normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-591.7045,-524.8071;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1274.703,-1323.229;Inherit;False;Property;_Temporary;Temporary;3;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-576.4319,-764.5919;Inherit;False;91;Emissive;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;-561.3971,-864.389;Inherit;False;73;Normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;-562.3662,-945.7633;Inherit;False;39;Diffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-592.6367,-620.5173;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-302.2686,-942.7712;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;VFX/flowmap;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;48;0;30;0
WireConnection;52;1;48;0
WireConnection;31;1;30;0
WireConnection;32;0;31;0
WireConnection;49;0;52;0
WireConnection;50;0;49;0
WireConnection;33;0;32;0
WireConnection;82;0;83;0
WireConnection;82;1;33;0
WireConnection;84;0;83;0
WireConnection;84;1;50;0
WireConnection;22;0;21;0
WireConnection;34;0;82;0
WireConnection;43;0;42;0
WireConnection;51;0;84;0
WireConnection;24;0;23;0
WireConnection;24;1;22;0
WireConnection;44;0;43;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;25;2;35;0
WireConnection;64;0;32;0
WireConnection;54;0;23;0
WireConnection;54;1;24;0
WireConnection;54;2;55;0
WireConnection;56;0;54;0
WireConnection;56;1;46;0
WireConnection;47;0;25;0
WireConnection;47;1;46;0
WireConnection;65;0;64;0
WireConnection;26;0;47;0
WireConnection;66;0;65;0
WireConnection;57;0;56;0
WireConnection;67;0;66;0
WireConnection;99;0;100;0
WireConnection;99;1;86;0
WireConnection;87;0;100;0
WireConnection;87;1;85;0
WireConnection;68;0;67;0
WireConnection;93;0;87;0
WireConnection;93;1;95;0
WireConnection;92;0;99;0
WireConnection;92;1;95;0
WireConnection;76;0;102;0
WireConnection;76;1;71;0
WireConnection;97;0;103;0
WireConnection;97;1;58;0
WireConnection;72;0;102;0
WireConnection;72;1;70;0
WireConnection;40;0;103;0
WireConnection;40;1;41;0
WireConnection;90;0;93;0
WireConnection;90;1;92;0
WireConnection;90;2;89;0
WireConnection;61;0;40;0
WireConnection;61;1;97;0
WireConnection;61;2;69;0
WireConnection;75;0;72;0
WireConnection;75;1;76;0
WireConnection;75;2;77;0
WireConnection;91;0;90;0
WireConnection;39;0;61;0
WireConnection;73;0;75;0
WireConnection;0;0;78;0
WireConnection;0;1;79;0
WireConnection;0;2;96;0
WireConnection;0;3;80;0
WireConnection;0;4;81;0
ASEEND*/
//CHKSM=2654B7B22D35168BECBEB9E3188F5C1779B12AD2