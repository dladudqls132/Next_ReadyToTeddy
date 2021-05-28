// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Chracter Electric shock"
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0,0,0,0)
		_FresnelScale("Fresnel Scale", Float) = 0
		_FresnelPower("Fresnel Power", Float) = 0
		_FresnelColor("Fresnel Color", Color) = (0,0,0,0)
		_ElectricIns("Electric Ins", Range( 0 , 20)) = 0
		_Noise("Noise", 2D) = "white" {}
		_Characteralbedo("Character albedo", 2D) = "white" {}
		_Characternormal("Character normal", 2D) = "bump" {}
		_MappingIns("Mapping Ins", Range( 0 , 10)) = 0
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_NoiseTexture1("Noise Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Characternormal;
		uniform float4 _Characternormal_ST;
		uniform float4 _TintColor;
		uniform sampler2D _Characteralbedo;
		uniform float4 _Characteralbedo_ST;
		uniform float _MappingIns;
		uniform float _ElectricIns;
		uniform float4 _FresnelColor;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform sampler2D _Noise;
		uniform sampler2D _NoiseTexture;
		uniform sampler2D _NoiseTexture1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Characternormal = i.uv_texcoord * _Characternormal_ST.xy + _Characternormal_ST.zw;
			o.Normal = tex2D( _Characternormal, uv_Characternormal ).rgb;
			float2 uv_Characteralbedo = i.uv_texcoord * _Characteralbedo_ST.xy + _Characteralbedo_ST.zw;
			o.Albedo = ( ( _TintColor * tex2D( _Characteralbedo, uv_Characteralbedo ) ) * _MappingIns ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV2 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode2 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV2, _FresnelPower ) );
			float2 panner12 = ( 1.0 * _Time.y * float2( 1,1 ) + i.uv_texcoord);
			float2 panner15 = ( 1.0 * _Time.y * float2( -1,-1 ) + i.uv_texcoord);
			float4 temp_cast_2 = (1.32).xxxx;
			o.Emission = ( _ElectricIns * ( ( _FresnelColor * ( saturate( fresnelNode2 ) + tex2D( _Noise, ( pow( ( tex2D( _NoiseTexture, (panner12*0.32 + 0.0) ) * tex2D( _NoiseTexture1, (panner15*0.21 + 0.0) ) ) , temp_cast_2 ) * 3.6 ).rg ) ) ) * frac( ( _Time.w * 2.91 ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;348;1920;669;613.6414;939.4409;1.009134;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;-2465.407,278.1964;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;12;-2168.249,134.9073;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2141.2,362.7066;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0.32;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;15;-2185.901,479.2268;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2137.273,683.4542;Inherit;False;Constant;_Float1;Float 0;5;0;Create;True;0;0;0;False;0;False;0.21;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;44;-1909.294,149.1584;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;47;-1905.367,469.9061;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;45;-1615.595,535.9042;Inherit;True;Property;_NoiseTexture1;Noise Texture;10;0;Create;True;0;0;0;False;0;False;-1;None;e199ee8112362864f831425463b09fda;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;-1619.522,215.1565;Inherit;True;Property;_NoiseTexture;Noise Texture;9;0;Create;True;0;0;0;False;0;False;-1;None;e199ee8112362864f831425463b09fda;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-1192.039,680.4598;Inherit;False;Constant;_Float4;Float 4;10;0;Create;True;0;0;0;False;0;False;1.32;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1302.504,396.3387;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-991.5442,762.1619;Inherit;False;Constant;_Float3;Float 3;10;0;Create;True;0;0;0;False;0;False;3.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-855.1262,165.6721;Float;False;Property;_FresnelPower;Fresnel Power;2;0;Create;True;0;0;0;False;0;False;0;10.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-886.317,64.57581;Float;False;Property;_FresnelScale;Fresnel Scale;1;0;Create;True;0;0;0;False;0;False;0;3.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;51;-1071.588,450.9193;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-800.3446,448.4622;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;2;-661.544,5.799003;Inherit;True;Standard;TangentNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;5;-407.544,5.799003;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-53.47408,547.8077;Inherit;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;0;False;0;False;2.91;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;32;-33.55681,234.6641;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-535.2617,428.8039;Inherit;True;Property;_Noise;Noise;5;0;Create;True;0;0;0;False;0;False;-1;None;0d10053aab3a3684190066450b8c89ae;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;154.5503,390.6819;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-208.9553,243.3017;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;-330.5874,-228.0322;Float;False;Property;_FresnelColor;Fresnel Color;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.3915088,0.4859474,1,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;253.9779,-1018.597;Float;False;Property;_TintColor;Tint Color;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;202.8814,-839.3144;Inherit;True;Property;_Characteralbedo;Character albedo;6;0;Create;True;0;0;0;False;0;False;-1;None;81ca44e5020d18a43b8d419e0f9a868b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-75.5874,8.967773;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FractNode;34;373.6398,323.1852;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;484.291,17.78812;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;551.8813,-845.3144;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;471.9097,-600.9824;Inherit;False;Property;_MappingIns;Mapping Ins;8;0;Create;True;0;0;0;False;0;False;0;6.44;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;163.4126,-161.0322;Float;False;Property;_ElectricIns;Electric Ins;4;0;Create;True;0;0;0;False;0;False;0;8.98;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;798.2097,-755.6826;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;724.4736,-160.8077;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;35;-2526.955,117.5812;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;610.0199,-439.3571;Inherit;True;Property;_Characternormal;Character normal;7;0;Create;True;0;0;0;False;0;False;-1;None;81ca44e5020d18a43b8d419e0f9a868b;True;0;False;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1057.007,-357.1206;Float;False;True;-1;0;ASEMaterialInspector;0;0;Standard;FX/Chracter Electric shock;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;False;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;0;53;0
WireConnection;15;0;53;0
WireConnection;44;0;12;0
WireConnection;44;1;13;0
WireConnection;47;0;15;0
WireConnection;47;1;46;0
WireConnection;45;1;47;0
WireConnection;43;1;44;0
WireConnection;18;0;43;0
WireConnection;18;1;45;0
WireConnection;51;0;18;0
WireConnection;51;1;52;0
WireConnection;48;0;51;0
WireConnection;48;1;49;0
WireConnection;2;2;3;0
WireConnection;2;3;4;0
WireConnection;5;0;2;0
WireConnection;14;1;48;0
WireConnection;33;0;32;4
WireConnection;33;1;29;0
WireConnection;20;0;5;0
WireConnection;20;1;14;0
WireConnection;6;0;7;0
WireConnection;6;1;20;0
WireConnection;34;0;33;0
WireConnection;25;0;6;0
WireConnection;25;1;34;0
WireConnection;22;0;1;0
WireConnection;22;1;21;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;8;0;9;0
WireConnection;8;1;25;0
WireConnection;0;0;24;0
WireConnection;0;1;54;0
WireConnection;0;2;8;0
ASEEND*/
//CHKSM=8BD183D4E5411913E25B07CF90BD7179F92A957C