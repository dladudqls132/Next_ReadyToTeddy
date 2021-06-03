// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield"
{
	Properties
	{
		_MainIntensity("Main Intensity", Range( 0 , 2)) = 1
		_TintColor("Tint Color", Color) = (1,1,1,0)
		_main_tex("main_tex", 2D) = "white" {}
		_LineIntensity("Line Intensity", Float) = 1
		_LineColor("Line Color", Color) = (1,1,1,0)
		_LineTexture("Line Texture", 2D) = "white" {}
		_Lerp_con("Lerp_con", Range( 0 , 1)) = 0.06469498
		_FresnelColor("Fresnel Color", Color) = (1,1,1,0)
		_FresnelScale("Fresnel Scale", Float) = 1
		_FresnelPower("Fresnel Power", Float) = 1
		_LineNormal("LineNormal", Float) = 0
		_VertexNormal("Vertex Normal", Float) = 0
		_LineSpeed("Line Speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow nofog vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
		};

		uniform float _VertexNormal;
		uniform sampler2D _LineTexture;
		uniform float _LineSpeed;
		uniform float _LineNormal;
		uniform float _MainIntensity;
		uniform float4 _TintColor;
		uniform sampler2D _main_tex;
		uniform float4 _main_tex_ST;
		uniform float _LineIntensity;
		uniform float4 _LineColor;
		uniform float4 _FresnelColor;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _Lerp_con;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float2 temp_cast_0 = (_LineSpeed).xx;
			float2 panner26 = ( 1.0 * _Time.y * temp_cast_0 + ase_worldNormal.xy);
			float temp_output_35_0 = saturate( ( pow( tex2Dlod( _LineTexture, float4( ( ase_worldNormal + float3( ( panner26 * 0.08 ) ,  0.0 ) ).xy, 0, 0.0) ).r , 1.68 ) * 17.33 ) );
			float vertexOffset133 = temp_output_35_0;
			v.vertex.xyz += ( ( ase_vertexNormal * _VertexNormal ) + ( ( ase_vertexNormal * vertexOffset133 ) * _LineNormal ) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_main_tex = i.uv_texcoord * _main_tex_ST.xy + _main_tex_ST.zw;
			float3 ase_worldNormal = i.worldNormal;
			float2 temp_cast_0 = (_LineSpeed).xx;
			float2 panner26 = ( 1.0 * _Time.y * temp_cast_0 + ase_worldNormal.xy);
			float temp_output_35_0 = saturate( ( pow( tex2D( _LineTexture, ( ase_worldNormal + float3( ( panner26 * 0.08 ) ,  0.0 ) ).xy ).r , 1.68 ) * 17.33 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV2 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode2 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV2, _FresnelPower ) );
			float4 temp_output_6_0 = ( _FresnelColor * fresnelNode2 );
			float4 lerpResult15 = lerp( ( ( _TintColor * tex2D( _main_tex, uv_main_tex ).r ) + ( _LineIntensity * _LineColor * temp_output_35_0 ) ) , temp_output_6_0 , _Lerp_con);
			float4 temp_output_79_0 = ( _MainIntensity * ( lerpResult15 + temp_output_6_0 ) );
			o.Emission = temp_output_79_0.rgb;
			o.Alpha = temp_output_79_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1920;6;1920;1013;473.6627;1145.901;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;39;-3845.72,-1085.11;Inherit;False;2518.063;835.0038;아우라 텍스쳐;16;38;36;37;133;35;33;34;30;21;24;27;26;28;116;130;146;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-3761.441,-560.2765;Inherit;False;Property;_LineSpeed;Line Speed;13;0;Create;True;0;0;0;False;0;False;0;9.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;146;-3802.744,-959.2747;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;28;-3472.016,-475.3165;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0.08;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;26;-3575.862,-652.3822;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-3303.208,-617.7151;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-3167.252,-764.5635;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;21;-2990.521,-786.4269;Inherit;True;Property;_LineTexture;Line Texture;6;0;Create;True;0;0;0;False;0;False;-1;0d10053aab3a3684190066450b8c89ae;0d10053aab3a3684190066450b8c89ae;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;34;-2547.551,-534.2261;Inherit;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;0;False;0;False;17.33;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;30;-2675.987,-786.8108;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1.68;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2394.15,-768.2263;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;-2148.614,-757.7973;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2118.024,-1035.11;Inherit;False;Property;_LineIntensity;Line Intensity;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;-1603.168,-1487.01;Inherit;False;Property;_TintColor;Tint Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.772549,0.0117737,0.00392159,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;37;-2172.596,-953.042;Inherit;False;Property;_LineColor;Line Color;5;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,0,0.01667595,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1294.301,-187.4205;Inherit;False;Property;_FresnelPower;Fresnel Power;10;0;Create;True;0;0;0;False;0;False;1;2.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1663.746,-1310.113;Inherit;True;Property;_main_tex;main_tex;3;0;Create;True;0;0;0;False;0;False;-1;db3579e0c54a9b84890fbcbc31d30612;db3579e0c54a9b84890fbcbc31d30612;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-1298.44,-270.1007;Inherit;False;Property;_FresnelScale;Fresnel Scale;9;0;Create;True;0;0;0;False;0;False;1;2.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1903.677,-716.4256;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;133;-1915.304,-464.2967;Inherit;False;vertexOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1299.986,-1481.602;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;2;-1125.688,-325.0183;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1044.269,-584.4319;Inherit;False;Property;_FresnelColor;Fresnel Color;8;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,0.4031561,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;134;243.2169,339.1539;Inherit;True;133;vertexOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-579.4272,-497.0061;Inherit;False;Property;_Lerp_con;Lerp_con;7;0;Create;True;0;0;0;False;0;False;0.06469498;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;152;227.2969,104.8007;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-969.0179,-1067.319;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-658.2993,-393.8801;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;115;484.6928,13.32006;Inherit;False;Property;_VertexNormal;Vertex Normal;12;0;Create;True;0;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;136;319.5082,-273.9383;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;140;594.9235,426.3816;Inherit;False;Property;_LineNormal;LineNormal;11;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;568.4459,178.673;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;15;-273.9002,-686.2292;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;752.4485,-93.35628;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;788.9236,290.3812;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;24.53606,-390.8074;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;203.1449,-683.3446;Inherit;False;Property;_MainIntensity;Main Intensity;1;0;Create;True;0;0;0;False;0;False;1;1.74;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;648.5214,-523.6353;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;138;1158.514,99.62717;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;130;-3788.132,-812.518;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1442.997,-450.4529;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shield;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;146;0
WireConnection;26;2;116;0
WireConnection;27;0;26;0
WireConnection;27;1;28;0
WireConnection;24;0;146;0
WireConnection;24;1;27;0
WireConnection;21;1;24;0
WireConnection;30;0;21;1
WireConnection;33;0;30;0
WireConnection;33;1;34;0
WireConnection;35;0;33;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;38;2;35;0
WireConnection;133;0;35;0
WireConnection;20;0;18;0
WireConnection;20;1;1;1
WireConnection;2;2;3;0
WireConnection;2;3;13;0
WireConnection;86;0;20;0
WireConnection;86;1;38;0
WireConnection;6;0;5;0
WireConnection;6;1;2;0
WireConnection;135;0;152;0
WireConnection;135;1;134;0
WireConnection;15;0;86;0
WireConnection;15;1;6;0
WireConnection;15;2;16;0
WireConnection;143;0;136;0
WireConnection;143;1;115;0
WireConnection;141;0;135;0
WireConnection;141;1;140;0
WireConnection;17;0;15;0
WireConnection;17;1;6;0
WireConnection;79;0;19;0
WireConnection;79;1;17;0
WireConnection;138;0;143;0
WireConnection;138;1;141;0
WireConnection;0;2;79;0
WireConnection;0;9;79;0
WireConnection;0;11;138;0
ASEEND*/
//CHKSM=3CF6EF70155C7B8443786A85FC1D499747CFA88F