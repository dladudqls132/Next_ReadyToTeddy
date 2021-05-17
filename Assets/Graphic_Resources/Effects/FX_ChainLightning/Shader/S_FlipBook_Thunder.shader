// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FlipBook_Thunder"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_Speed("Speed", Float) = 4.17
		_Ins("Ins", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows nofog 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTexture;
		uniform float _Speed;
		uniform float _Ins;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color10 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			// *** BEGIN Flipbook UV Animation vars ***
			// Total tiles of Flipbook Texture
			float fbtotaltiles18 = 1.0 * 4.0;
			// Offsets for cols and rows of Flipbook Texture
			float fbcolsoffset18 = 1.0f / 1.0;
			float fbrowsoffset18 = 1.0f / 4.0;
			// Speed of animation
			float fbspeed18 = ( _Time.w * _Speed ) * 1.0;
			// UV Tiling (col and row offset)
			float2 fbtiling18 = float2(fbcolsoffset18, fbrowsoffset18);
			// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
			// Calculate current tile linear index
			float fbcurrenttileindex18 = round( fmod( fbspeed18 + 0.0, fbtotaltiles18) );
			fbcurrenttileindex18 += ( fbcurrenttileindex18 < 0) ? fbtotaltiles18 : 0;
			// Obtain Offset X coordinate from current tile linear index
			float fblinearindextox18 = round ( fmod ( fbcurrenttileindex18, 1.0 ) );
			// Multiply Offset X by coloffset
			float fboffsetx18 = fblinearindextox18 * fbcolsoffset18;
			// Obtain Offset Y coordinate from current tile linear index
			float fblinearindextoy18 = round( fmod( ( fbcurrenttileindex18 - fblinearindextox18 ) / 1.0, 4.0 ) );
			// Reverse Y to get tiles from Top to Bottom
			fblinearindextoy18 = (int)(4.0-1) - fblinearindextoy18;
			// Multiply Offset Y by rowoffset
			float fboffsety18 = fblinearindextoy18 * fbrowsoffset18;
			// UV Offset
			float2 fboffset18 = float2(fboffsetx18, fboffsety18);
			// Flipbook UV
			half2 fbuv18 = i.uv_texcoord * fbtiling18 + fboffset18;
			// *** END Flipbook UV Animation vars ***
			o.Emission = ( ( i.vertexColor * ( color10 * tex2D( _MainTexture, fbuv18 ) ) ) * _Ins ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1920;0;1920;1019;1765.915;320.3686;1;True;True
Node;AmplifyShaderEditor.TimeNode;4;-1341,279.5;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-1330,490.5;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;0;False;0;False;4.17;5.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1395.281,-137.8109;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1134,406.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1351.281,166.1891;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1212,104.5;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;18;-917.2815,75.18915;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;2;-522,57.5;Inherit;True;Property;_MainTexture;MainTexture;1;0;Create;True;0;0;0;False;0;False;-1;3d5fa62b047209b4dbfb2a1ec3efd472;4b5dc216e19e8ed4fb6b29be07c9d670;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-470,-152.5;Inherit;False;Constant;_TintColor;Tint Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-223,-27.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;11;-217,-217.5;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-47,-97.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-125,95.5;Inherit;False;Property;_Ins;Ins;3;0;Create;True;0;0;0;False;0;False;0;15;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;151,-19.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;477,-25;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FlipBook_Thunder;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;4;4
WireConnection;6;1;7;0
WireConnection;18;0;16;0
WireConnection;18;1;19;0
WireConnection;18;2;5;0
WireConnection;18;3;19;0
WireConnection;18;5;6;0
WireConnection;2;1;18;0
WireConnection;8;0;10;0
WireConnection;8;1;2;0
WireConnection;12;0;11;0
WireConnection;12;1;8;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;0;2;13;0
ASEEND*/
//CHKSM=CF0415628518C670A53BFD1FC9EA45516CFBCD71