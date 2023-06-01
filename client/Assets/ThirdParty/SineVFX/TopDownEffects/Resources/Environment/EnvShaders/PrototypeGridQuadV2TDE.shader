// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/PrototypeGridQuadV2TDE"
{
	Properties
	{
		_SpriteTexture("Sprite Texture", 2D) = "black" {}
		_TexturePower("Texture Power", Range( 0 , 1)) = 1
		_TextureColorTint("Texture Color Tint", Color) = (1,1,1,1)
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_StretchU("Stretch U", Float) = 1
		_StretchV("Stretch V", Float) = 1
		_ColorMaskOuterColor1("Color Mask Outer Color", Color) = (0.5294118,0.5294118,0.5294118,1)
		_ColorMaskDistance1("Color Mask Distance", Float) = 25
		_ColorMaskPower1("Color Mask Power", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _ColorMaskOuterColor1;
		uniform float4 _TextureColorTint;
		uniform sampler2D _SpriteTexture;
		uniform float _StretchU;
		uniform float _StretchV;
		uniform float _TexturePower;
		uniform float _ColorMaskDistance1;
		uniform float _ColorMaskPower1;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult88 = (float2(_StretchU , _StretchV));
			float4 clampResult71 = clamp( ( _TextureColorTint + ( tex2D( _SpriteTexture, (float2( 0,0 ) + (pow( abs( (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) ) , ( 1.0 * appendResult88 ) ) - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 ))) ).r * _TexturePower ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float3 ase_worldPos = i.worldPos;
			float clampResult97 = clamp( (0.0 + (( -distance( ase_worldPos , float3(0,0,0) ) + _ColorMaskDistance1 ) - 0.0) * (1.0 - 0.0) / (_ColorMaskDistance1 - 0.0)) , 0.0 , 1.0 );
			float4 lerpResult99 = lerp( _ColorMaskOuterColor1 , clampResult71 , pow( clampResult97 , _ColorMaskPower1 ));
			o.Albedo = lerpResult99.rgb;
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
Version=18200
1920;0;1920;1018;2407.146;295.2943;1.22116;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;76;-3298.137,118.2979;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;86;-3200.125,476.0228;Inherit;False;Property;_StretchU;Stretch U;12;0;Create;True;0;0;False;0;False;1;2.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-3202.179,562.2971;Inherit;False;Property;_StretchV;Stretch V;13;0;Create;True;0;0;False;0;False;1;2.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;78;-3066.019,123.4331;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-3011.583,298.0362;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;88;-3021.414,505.8081;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;101;-2320.07,151.9641;Inherit;False;2004.07;752.1302;;12;89;90;91;92;93;94;95;96;97;98;99;100;Fix;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;90;-2270.07,482.9179;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;89;-2251.748,645.9502;Float;False;Constant;_Vector2;Vector 0;11;0;Create;True;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;83;-2849.304,49.48349;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2850.332,378.1486;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;91;-1984.087,567.0716;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;81;-2819.52,193.2746;Inherit;False;False;2;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-1899.813,693.9711;Float;False;Property;_ColorMaskDistance1;Color Mask Distance;15;0;Create;True;0;0;False;0;False;25;1000;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-2688.594,-249.1043;Float;True;Property;_SpriteTexture;Sprite Texture;0;0;Create;True;0;0;False;0;False;None;None;False;black;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NegateNode;93;-1826.445,567.4614;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;80;-2647.997,195.3287;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;-1,-1;False;2;FLOAT2;1,1;False;3;FLOAT2;0,0;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;74;-2317.615,-250.4742;Inherit;True;Property;_TextureSample0;Texture Sample 0;13;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;94;-1671.665,566.8766;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-2287.705,-48.23221;Float;False;Property;_TexturePower;Texture Power;1;0;Create;True;0;0;False;0;False;1;0.075;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-1967.905,-166.7322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;95;-1519.874,567.2386;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-2035.808,-421.4603;Float;False;Property;_TextureColorTint;Texture Color Tint;2;0;Create;True;0;0;False;0;False;1,1,1,1;0.1960782,0.1960782,0.1960782,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-1718.204,-334.7322;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1240.71,789.0943;Float;False;Property;_ColorMaskPower1;Color Mask Power;16;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;97;-1279.287,571.5674;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;71;-1568.119,-336.4858;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;98;-948.2192,578.0189;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;100;-1026.502,204.7151;Float;False;Property;_ColorMaskOuterColor1;Color Mask Outer Color;14;0;Create;True;0;0;False;0;False;0.5294118,0.5294118,0.5294118,1;0.8235294,0.8235294,0.8235294,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;67;-843.0231,1935.719;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;37;-1296.478,-1055.813;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-258.2775,911.2813;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;38;-906.3201,-1401.013;Float;False;Property;_GradientColorOne;Gradient Color One;8;0;Create;True;0;0;False;0;False;0,0,0,1;0,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;16;-914.5522,-1056.58;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;39;-896.9641,-1224.799;Float;False;Property;_GradientColorTwo;Gradient Color Two;7;0;Create;True;0;0;False;0;False;1,1,1,1;0.2352941,0.2352941,0.2352941,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;11;-2978.834,-1052.397;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;40;-470.9641,-1206.509;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NegateNode;33;-1636.97,-1059.95;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-2551.304,-1229.303;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;14;-1813.845,-1059.663;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;75;-2404.378,-497.5947;Inherit;True;Property;_MainTex;MainTex;11;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;13;-2751.52,-1052.854;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;-1,-1,-1,-1;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;36;-1103.525,-1056.518;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1174.552,-831.5797;Float;False;Property;_GradientExp;Gradient Exp;5;0;Create;True;0;0;False;0;False;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;63;-1390.181,1931.612;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;29;-2568.665,-1054.21;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;28;-1960.858,-1058.91;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-258.587,836.2813;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-1474.475,-1057.861;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-2240.301,-1128.303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenParams;27;-2746.766,-1249.311;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-1235.402,1931.027;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;65;-1083.611,1931.389;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1463.549,2058.122;Float;False;Property;_OpacityMaskDistance;Opacity Mask Distance;9;0;Create;True;0;0;False;0;False;25;1000;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;59;-1815.484,2010.101;Float;False;Constant;_Vector1;Vector 1;11;0;Create;True;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;61;-1547.823,1931.222;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;60;-1833.806,1847.068;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;68;-511.9559,1942.169;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;99;-500.0001,201.9641;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1780.602,-794.6179;Float;False;Property;_GradientLength;Gradient Length;6;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-804.446,2153.245;Float;False;Property;_OpacityMaskPower;Opacity Mask Power;10;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;147,-60;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SineVFX/PrototypeGridQuadV2TDE;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;78;0;76;0
WireConnection;88;0;86;0
WireConnection;88;1;87;0
WireConnection;83;0;78;0
WireConnection;84;0;82;0
WireConnection;84;1;88;0
WireConnection;91;0;90;0
WireConnection;91;1;89;0
WireConnection;81;0;83;0
WireConnection;81;1;84;0
WireConnection;93;0;91;0
WireConnection;80;0;81;0
WireConnection;74;0;4;0
WireConnection;74;1;80;0
WireConnection;94;0;93;0
WireConnection;94;1;92;0
WireConnection;72;0;74;1
WireConnection;72;1;73;0
WireConnection;95;0;94;0
WireConnection;95;2;92;0
WireConnection;70;0;9;0
WireConnection;70;1;72;0
WireConnection;97;0;95;0
WireConnection;71;0;70;0
WireConnection;98;0;97;0
WireConnection;98;1;96;0
WireConnection;67;0;65;0
WireConnection;37;0;34;0
WireConnection;37;2;35;0
WireConnection;16;0;36;0
WireConnection;16;1;17;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;40;2;16;0
WireConnection;33;0;14;0
WireConnection;31;0;27;1
WireConnection;31;1;27;2
WireConnection;14;0;28;0
WireConnection;13;0;11;0
WireConnection;36;0;37;0
WireConnection;63;0;61;0
WireConnection;29;0;13;0
WireConnection;28;0;30;0
WireConnection;28;1;29;1
WireConnection;34;0;33;0
WireConnection;34;1;35;0
WireConnection;30;0;31;0
WireConnection;30;1;29;0
WireConnection;64;0;63;0
WireConnection;64;1;62;0
WireConnection;65;0;64;0
WireConnection;65;2;62;0
WireConnection;61;0;60;0
WireConnection;61;1;59;0
WireConnection;68;0;67;0
WireConnection;68;1;66;0
WireConnection;99;0;100;0
WireConnection;99;1;71;0
WireConnection;99;2;98;0
WireConnection;0;0;99;0
WireConnection;0;3;7;0
WireConnection;0;4;8;0
ASEEND*/
//CHKSM=969A913D4BE4FD2B208C1F1BD5BA5D1C9E422A96