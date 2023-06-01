// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/DissolveParticleSimple"
{
	Properties
	{
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalPower("Final Power", Range( 0 , 60)) = 4
		_FinalOpacityPower("Final Opacity Power", Float) = 1
		[Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		[Toggle(_RAMPAFFECTEDBYVERTEXCOLOR_ON)] _RampAffectedByVertexColor("Ramp Affected By Vertex Color", Float) = 0
		_RampAffectedByDynamics("Ramp Affected By Dynamics", Range( 0 , 1)) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_MainTex("MainTex", 2D) = "white" {}
		_MainTexChannels("MainTex Channels", Vector) = (1,0,0,0)
		_MainTexRotation("MainTex Rotation", Range( 0 , 1)) = 0
		_FlipbookRows("Flipbook Rows", Float) = 1
		_FlipbookColums("Flipbook Colums", Float) = 1
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		[Toggle(_DISSOLVETEXTUREFLIP_ON)] _DissolveTextureFlip("Dissolve Texture Flip", Float) = 1
		_DissolveTextureScale("Dissolve Texture Scale", Float) = 1
		_DissolveExp("Dissolve Exp", Float) = 2
		_DissolveExpReversed("Dissolve Exp Reversed", Float) = 2
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _RAMPENABLED_ON
		#pragma shader_feature _RAMPAFFECTEDBYVERTEXCOLOR_ON
		#pragma shader_feature _DISSOLVETEXTUREFLIP_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float4 uv_tex4coord;
		};

		uniform float _FinalPower;
		uniform float4 _FinalColor;
		uniform sampler2D _Ramp;
		uniform sampler2D _MainTex;
		uniform float _FlipbookColums;
		uniform float _FlipbookRows;
		uniform float _MainTexRotation;
		uniform float4 _MainTexChannels;
		uniform sampler2D _DissolveTexture;
		uniform float _DissolveTextureScale;
		uniform float _DissolveExp;
		uniform float _DissolveExpReversed;
		uniform float _FinalOpacityPower;
		uniform float _RampAffectedByDynamics;
		uniform float _RampOffsetExp;
		uniform float4 _RampColorTint;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult102 = (float2(_FlipbookColums , _FlipbookRows));
			float temp_output_95_0 = ( _FlipbookColums * _FlipbookRows );
			float2 appendResult98 = (float2(temp_output_95_0 , _FlipbookRows));
			float ifLocalVar125 = 0;
			if( temp_output_95_0 == 1.0 )
				ifLocalVar125 = 0.0;
			else
				ifLocalVar125 = 1.0;
			float clampResult121 = clamp( i.uv2_tex4coord2.y , 0.0001 , ( temp_output_95_0 - ifLocalVar125 ) );
			float temp_output_96_0 = frac( ( clampResult121 / temp_output_95_0 ) );
			float2 appendResult99 = (float2(temp_output_96_0 , ( 1.0 - temp_output_96_0 )));
			float2 temp_output_104_0 = ( floor( ( appendResult98 * appendResult99 ) ) / appendResult102 );
			float cos111 = cos( (0.0 + (_MainTexRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0)) );
			float sin111 = sin( (0.0 + (_MainTexRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0)) );
			float2 rotator111 = mul( ( ( i.uv_texcoord / appendResult102 ) + temp_output_104_0 ) - ( ( float2( 0.5,0.5 ) / appendResult102 ) + temp_output_104_0 ) , float2x2( cos111 , -sin111 , sin111 , cos111 )) + ( ( float2( 0.5,0.5 ) / appendResult102 ) + temp_output_104_0 );
			float4 tex2DNode4 = tex2D( _MainTex, rotator111 );
			float4 break69 = ( tex2DNode4 * _MainTexChannels );
			float clampResult71 = clamp( ( break69.r + break69.g + break69.b + break69.a ) , 0.0 , 1.0 );
			float4 tex2DNode9 = tex2D( _DissolveTexture, (i.uv_texcoord*( _DissolveTextureScale * i.uv_tex4coord.w ) + i.uv_tex4coord.z) );
			#ifdef _DISSOLVETEXTUREFLIP_ON
				float staticSwitch28 = ( 1.0 - tex2DNode9.r );
			#else
				float staticSwitch28 = tex2DNode9.r;
			#endif
			float clampResult62 = clamp( ( 1.0 - pow( ( 1.0 - pow( staticSwitch28 , _DissolveExp ) ) , (1.0 + (i.uv2_tex4coord2.x - 0.0) * (_DissolveExpReversed - 1.0) / (1.0 - 0.0)) ) ) , 0.0 , 1.0 );
			float clampResult65 = clamp( ( clampResult62 + (-1.0 + (i.uv2_tex4coord2.x - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float clampResult8 = clamp( ( clampResult71 - clampResult65 ) , 0.0 , 1.0 );
			float clampResult90 = clamp( ( clampResult8 * i.vertexColor.a * _FinalOpacityPower ) , 0.0 , 1.0 );
			float lerpResult80 = lerp( clampResult71 , clampResult90 , _RampAffectedByDynamics);
			float2 appendResult78 = (float2(( 1.0 - pow( ( 1.0 - lerpResult80 ) , _RampOffsetExp ) ) , 0.0));
			float4 temp_output_89_0 = ( tex2D( _Ramp, appendResult78 ) * _RampColorTint );
			#ifdef _RAMPAFFECTEDBYVERTEXCOLOR_ON
				float4 staticSwitch93 = ( temp_output_89_0 * i.vertexColor );
			#else
				float4 staticSwitch93 = temp_output_89_0;
			#endif
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch77 = staticSwitch93;
			#else
				float4 staticSwitch77 = _FinalColor;
			#endif
			o.Emission = ( _FinalPower * i.vertexColor * staticSwitch77 ).rgb;
			o.Alpha = clampResult90;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;3008.801;1804.717;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;123;-6734.105,-634.9001;Inherit;False;3025.786;1053.338;;29;113;114;115;106;111;95;48;120;121;107;96;98;99;100;102;101;103;104;105;108;109;110;97;45;44;125;126;127;128;FlipBook;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-6632.084,260.8206;Inherit;False;Property;_FlipbookRows;Flipbook Rows;13;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-6684.105,-521.6251;Inherit;False;Property;_FlipbookColums;Flipbook Colums;14;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-6691.758,44.40624;Inherit;False;Constant;_Float1;Float 1;20;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-6696.208,122.7352;Inherit;False;Constant;_Float2;Float 2;20;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-6013.696,-89.49214;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;125;-6506.841,27.44485;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;120;-6390.262,-131.2387;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;48;-6559.293,-307.9013;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;121;-6210.141,-237.0411;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.0001;False;2;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;75;-4189.697,561.4517;Inherit;False;2907.625;864.141;;25;16;56;57;59;60;15;30;17;29;9;27;28;24;55;58;62;63;64;65;61;54;130;131;132;133;Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;107;-6126.661,216.2214;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-4093.267,1223.592;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-4139.696,1062.368;Inherit;False;Property;_DissolveTextureScale;Dissolve Texture Scale;19;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;96;-5960.517,216.8603;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-3838.414,1064.891;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;15;-4067.204,656.8408;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;97;-5808.646,308.4383;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;-5624.518,216.8603;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;98;-5624.518,88.86038;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;16;-3659.397,1029.649;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-5480.519,152.8605;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;9;-3428.677,1010.749;Inherit;True;Property;_DissolveTexture;Dissolve Texture;15;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;108;-5152.165,-553.4882;Inherit;False;Constant;_Vector1;Vector 1;70;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FloorOpNode;101;-5336.519,152.8605;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;103;-5481.694,-173.38;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;102;-5624.518,-39.13982;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;27;-3127.765,1186.81;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2835.704,1186.679;Inherit;False;Property;_DissolveExp;Dissolve Exp;21;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;28;-2960.32,1032.661;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;18;0;Create;True;0;0;False;0;False;0;1;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;104;-5176.519,56.86029;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-4674.754,77.48947;Inherit;False;Property;_MainTexRotation;MainTex Rotation;12;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;109;-4891.404,-584.9001;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;113;-4588.91,174.2545;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;105;-5176.519,-71.13982;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;115;-4351.865,81.38248;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;-4950.618,-5.39516;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;-4687.308,-527.0843;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2671.827,805.4955;Inherit;False;Property;_DissolveExpReversed;Dissolve Exp Reversed;22;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;-2623.501,611.4517;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;55;-2622.173,1098.619;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;58;-2339.466,986.1583;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;73;-2798.41,-402.6461;Inherit;False;1102.311;611.5077;;6;71;67;69;68;66;4;Main Texture or Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RotatorNode;111;-3985.32,-316.6912;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;57;-2373.328,638.5273;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;59;-2066.952,889.9014;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;66;-2642.564,-42.06155;Inherit;False;Property;_MainTexChannels;MainTex Channels;11;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-2726.631,-243.8727;Inherit;True;Property;_MainTex;MainTex;10;0;Create;True;0;0;False;0;False;-1;None;7fdf42c0388f7374eb0ac45f4a100533;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;61;-2046.967,1179.626;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;60;-1909.994,892.5834;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-2396.607,-121.8742;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;63;-1820.822,1174.864;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;69;-2258.016,-120.6142;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;62;-1749.521,894.4857;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-2002.251,-124.394;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-1589.906,970.2407;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;71;-1867.44,-126.9139;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;74;-871.7822,116.5201;Inherit;False;978.0403;487.0853;;6;90;8;38;39;7;91;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;65;-1457.071,971.8636;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-829.104,188.3096;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;39;-689.9634,313.3951;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;8;-660.1036,191.2097;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-725.457,491.9644;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;87;-2769.063,-1342.87;Inherit;False;1339.125;260.6951;;7;80;81;83;84;85;82;78;Ramp UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-407.1637,312.6948;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2719.063,-1259.702;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;7;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;90;-86.21806,320.1486;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;80;-2338.162,-1292.87;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-2184.484,-1197.175;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;8;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;82;-2071.259,-1272.841;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;83;-1915.828,-1269.514;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;84;-1768.357,-1265.031;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;78;-1596.938,-1276.787;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;86;-1216.586,-1679.771;Inherit;False;1329.745;895.6845;;10;92;41;3;2;77;89;1;79;88;93;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;88;-1092.776,-1119.171;Inherit;False;Property;_RampColorTint;Ramp Color Tint;5;0;Create;True;0;0;False;0;False;1,1,1,1;0.3584906,0.3420034,0.3365076,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;-1166.586,-1318.941;Inherit;True;Property;_Ramp;Ramp;4;0;Create;True;0;0;False;0;False;-1;None;a52fb0452b0d7d64db47d0c0ef5e325c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-835.8296,-1192.455;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;92;-1038.825,-951.5688;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-693.3994,-1029.67;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-1096.378,-1511.348;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;0.3301885,0.3301885,0.3301885,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;93;-530.3994,-1114.67;Inherit;False;Property;_RampAffectedByVertexColor;Ramp Affected By Vertex Color;6;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;76;-4577.95,-1545.478;Inherit;False;861.1541;384.4372;;5;25;26;20;18;19;Radial Dissolve Option (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-633.8163,-1629.771;Inherit;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;4;1;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;77;-811.5438,-1405.748;Inherit;False;Property;_RampEnabled;Ramp Enabled;3;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;124;-6698.743,-1190.809;Inherit;False;707.8872;408.8871;;3;49;43;42;FlipBook Old;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;41;-534.8855,-1553.068;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-6648.743,-1140.809;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-3885.795,-1423.041;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;42;-6293.856,-1022.26;Inherit;False;Flipbook;-1;;1;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-195.2846,-1446.661;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;133;-3856.354,808.8036;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;18;-4527.95,-1495.478;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-4207.796,-1276.041;Inherit;False;Property;_DissolveRadialAmount;Dissolve Radial Amount;20;0;Create;True;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-3722.828,713.4288;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-4305.651,-1494.178;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-4123.403,849.1978;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;16;0;Create;True;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;20;-4118.449,-1492.878;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-6593.993,-88.39618;Inherit;False;Constant;_Float3;Float 3;20;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-4123.403,772.8978;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;17;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;142;-2068.498,-903.7882;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;141;-2469.734,-772.5096;Inherit;False;Property;_RampUsesMainTexGandB;Ramp Uses MainTex G and B;9;0;Create;True;0;0;False;0;False;0;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;146;-2331.316,-563.748;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-2480.056,-584.9965;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-2617.103,-656.7734;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;144;-2794.231,-805.0709;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-6595.928,-896.9216;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;246,-9;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/DissolveParticleSimple;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;95;0;44;0
WireConnection;95;1;45;0
WireConnection;125;0;95;0
WireConnection;125;1;126;0
WireConnection;125;2;126;0
WireConnection;125;3;127;0
WireConnection;125;4;126;0
WireConnection;120;0;95;0
WireConnection;120;1;125;0
WireConnection;121;0;48;2
WireConnection;121;2;120;0
WireConnection;107;0;121;0
WireConnection;107;1;95;0
WireConnection;96;0;107;0
WireConnection;30;0;17;0
WireConnection;30;1;29;4
WireConnection;97;0;96;0
WireConnection;99;0;96;0
WireConnection;99;1;97;0
WireConnection;98;0;95;0
WireConnection;98;1;45;0
WireConnection;16;0;15;0
WireConnection;16;1;30;0
WireConnection;16;2;29;3
WireConnection;100;0;98;0
WireConnection;100;1;99;0
WireConnection;9;1;16;0
WireConnection;101;0;100;0
WireConnection;102;0;44;0
WireConnection;102;1;45;0
WireConnection;27;0;9;1
WireConnection;28;1;9;1
WireConnection;28;0;27;0
WireConnection;104;0;101;0
WireConnection;104;1;102;0
WireConnection;109;0;108;0
WireConnection;109;1;102;0
WireConnection;105;0;103;0
WireConnection;105;1;102;0
WireConnection;115;0;114;0
WireConnection;115;4;113;0
WireConnection;106;0;105;0
WireConnection;106;1;104;0
WireConnection;110;0;109;0
WireConnection;110;1;104;0
WireConnection;55;0;28;0
WireConnection;55;1;24;0
WireConnection;58;0;55;0
WireConnection;111;0;106;0
WireConnection;111;1;110;0
WireConnection;111;2;115;0
WireConnection;57;0;54;1
WireConnection;57;4;56;0
WireConnection;59;0;58;0
WireConnection;59;1;57;0
WireConnection;4;1;111;0
WireConnection;60;0;59;0
WireConnection;68;0;4;0
WireConnection;68;1;66;0
WireConnection;63;0;61;1
WireConnection;69;0;68;0
WireConnection;62;0;60;0
WireConnection;67;0;69;0
WireConnection;67;1;69;1
WireConnection;67;2;69;2
WireConnection;67;3;69;3
WireConnection;64;0;62;0
WireConnection;64;1;63;0
WireConnection;71;0;67;0
WireConnection;65;0;64;0
WireConnection;7;0;71;0
WireConnection;7;1;65;0
WireConnection;8;0;7;0
WireConnection;38;0;8;0
WireConnection;38;1;39;4
WireConnection;38;2;91;0
WireConnection;90;0;38;0
WireConnection;80;0;71;0
WireConnection;80;1;90;0
WireConnection;80;2;81;0
WireConnection;82;0;80;0
WireConnection;83;0;82;0
WireConnection;83;1;85;0
WireConnection;84;0;83;0
WireConnection;78;0;84;0
WireConnection;79;1;78;0
WireConnection;89;0;79;0
WireConnection;89;1;88;0
WireConnection;94;0;89;0
WireConnection;94;1;92;0
WireConnection;93;1;89;0
WireConnection;93;0;94;0
WireConnection;77;1;1;0
WireConnection;77;0;93;0
WireConnection;25;0;20;0
WireConnection;25;1;26;0
WireConnection;42;13;43;0
WireConnection;42;4;44;0
WireConnection;42;5;45;0
WireConnection;42;24;121;0
WireConnection;42;2;49;0
WireConnection;3;0;2;0
WireConnection;3;1;41;0
WireConnection;3;2;77;0
WireConnection;133;0;130;0
WireConnection;133;1;131;0
WireConnection;132;0;15;0
WireConnection;132;1;133;0
WireConnection;19;0;18;0
WireConnection;20;0;19;0
WireConnection;142;0;80;0
WireConnection;142;1;146;0
WireConnection;142;2;141;0
WireConnection;146;0;145;0
WireConnection;145;0;147;0
WireConnection;145;1;4;2
WireConnection;147;0;144;1
WireConnection;0;2;3;0
WireConnection;0;9;90;0
ASEEND*/
//CHKSM=6D939AC709AC646FFF557A3D1F11D6E44EC681BD