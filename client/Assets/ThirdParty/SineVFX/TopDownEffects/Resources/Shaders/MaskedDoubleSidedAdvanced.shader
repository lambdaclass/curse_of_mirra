// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/MaskedDoubleSidedAdvanced"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.1
		_AppearProgress("Appear Progress", Range( -1 , 1)) = 0
		_FinalPower("Final Power", Float) = 2
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_RampRemapMinOld("Ramp Remap Min Old", Range( 0 , 0.5)) = 0
		_RampRemapMaxNew("Ramp Remap Max New", Range( 1 , 1.5)) = 0
		_FrontSmokeColorOne("Front Smoke Color One", Color) = (0.2075472,0.2075472,0.2075472,1)
		_FrontSmokeColorTwo("Front Smoke Color Two", Color) = (0.2169811,0.2169811,0.2169811,1)
		_FrontSmokeExp("Front Smoke Exp", Range( 0.2 , 8)) = 1
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScaleU("Noise 01 Scale U", Float) = 1
		_Noise01ScaleV("Noise 01 Scale V", Float) = 1
		_Noise01Negate("Noise 01 Negate", Range( 0 , 1)) = 1
		_Noise01FrontAmount("Noise 01 Front Amount", Range( -1 , 1)) = 0
		_Noise01Twist("Noise 01 Twist", Range( 0 , 4)) = 0
		_Noise01ScrollSpeedU("Noise 01 Scroll Speed U", Float) = 0
		_Noise01ScrollSpeedV("Noise 01 Scroll Speed V", Float) = 0
		_Noise02("Noise 02", 2D) = "white" {}
		_Noise02TilingU("Noise 02 Tiling U", Float) = 1
		_Noise02TilingV("Noise 02 Tiling V", Float) = 1
		_Noise02Negate("Noise 02 Negate", Range( 0 , 1)) = 1
		_Noise02Amount("Noise 02 Amount", Range( -1 , 1)) = 0
		_Noise03("Noise 03", 2D) = "white" {}
		_Noise03Bias("Noise 03 Bias", Range( 0 , 0.05)) = 0
		_Noise4("Noise 4", 2D) = "white" {}
		_Noise04ScaleU("Noise 04 Scale U", Float) = 1
		_Noise04ScaleV("Noise 04 Scale V", Float) = 1
		_Noise04Amount("Noise 04 Amount", Float) = 0
		_EdgeGlowExp("Edge Glow Exp", Range( 0.2 , 8)) = 2
		_EdgeGlowPower("Edge Glow Power", Float) = 10
		_EdgeGlowSubtract("Edge Glow Subtract", Range( 0 , 1)) = 0
		_TransitionExp("Transition Exp", Range( 0.2 , 8)) = 1
		_TransitionMultiply("Transition Multiply", Float) = 0
		_TransitionFixAmount("Transition Fix Amount", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			half ASEVFace : VFACE;
		};

		uniform float4 _FrontSmokeColorOne;
		uniform float4 _FrontSmokeColorTwo;
		uniform sampler2D _Noise01;
		uniform float4 _Noise01_ST;
		uniform float _Noise01ScaleU;
		uniform float _Noise01ScaleV;
		uniform float _Noise01Twist;
		uniform float _Noise01ScrollSpeedU;
		uniform float _Noise01ScrollSpeedV;
		uniform float _Noise01FrontAmount;
		uniform sampler2D _Noise03;
		uniform float _Noise03Bias;
		uniform float _FrontSmokeExp;
		uniform float _AppearProgress;
		uniform float _TransitionExp;
		uniform float _TransitionMultiply;
		uniform sampler2D _Noise02;
		uniform float4 _Noise02_ST;
		uniform float _Noise02TilingU;
		uniform float _Noise02TilingV;
		uniform float _Noise02Negate;
		uniform float _Noise02Amount;
		uniform float _Noise01Negate;
		uniform float _EdgeGlowSubtract;
		uniform float _EdgeGlowExp;
		uniform sampler2D _Ramp;
		uniform float _RampRemapMinOld;
		uniform float _RampRemapMaxNew;
		uniform float _RampOffsetExp;
		uniform float _TransitionFixAmount;
		uniform float _Noise04Amount;
		uniform sampler2D _Noise4;
		uniform float _Noise04ScaleU;
		uniform float _Noise04ScaleV;
		uniform float4 _RampColorTint;
		uniform float _EdgeGlowPower;
		uniform float _FinalPower;
		uniform float _Cutoff = 0.1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult412 = (float2(_Noise01ScaleU , _Noise01ScaleV));
			float2 break413 = ( uv0_Noise01 * appendResult412 );
			float2 appendResult346 = (float2(( break413.x + ( break413.y * _Noise01Twist ) ) , break413.y));
			float2 appendResult341 = (float2(_Noise01ScrollSpeedU , _Noise01ScrollSpeedV));
			float2 temp_output_342_0 = ( appendResult346 + ( appendResult341 * _Time.y ) );
			float4 tex2DNode329 = tex2D( _Noise01, temp_output_342_0 );
			float clampResult552 = clamp( ( tex2DNode329.r + _Noise01FrontAmount ) , 0.0 , 1.0 );
			float4 lerpResult420 = lerp( _FrontSmokeColorOne , _FrontSmokeColorTwo , ( 1.0 - pow( ( 1.0 - ( clampResult552 - tex2D( _Noise03, ( _Noise03Bias + temp_output_342_0 ) ).r ) ) , _FrontSmokeExp ) ));
			float clampResult545 = clamp( i.vertexColor.r , 0.0 , 1.0 );
			float temp_output_481_0 = ( pow( clampResult545 , _TransitionExp ) * _TransitionMultiply );
			float clampResult354 = clamp( ( ( ( 1.0 - i.uv_texcoord.y ) + _AppearProgress ) - temp_output_481_0 ) , 0.0 , 1.0 );
			float2 uv0_Noise02 = i.uv_texcoord * _Noise02_ST.xy + _Noise02_ST.zw;
			float2 appendResult535 = (float2(_Noise02TilingU , _Noise02TilingV));
			float clampResult358 = clamp( ( tex2D( _Noise02, ( uv0_Noise02 * appendResult535 ) ).r + _Noise02Negate ) , 0.0 , 1.0 );
			float clampResult504 = clamp( ( clampResult354 + ( clampResult358 * _Noise02Amount ) ) , 0.0 , 1.0 );
			float clampResult334 = clamp( ( tex2DNode329.r + _Noise01Negate ) , 0.0 , 1.0 );
			float clampResult393 = clamp( ( clampResult504 * clampResult334 ) , 0.0 , 1.0 );
			float temp_output_394_0 = ( 1.0 - clampResult393 );
			float clampResult396 = clamp( ( temp_output_394_0 - _EdgeGlowSubtract ) , 0.0 , 1.0 );
			float temp_output_382_0 = pow( clampResult396 , _EdgeGlowExp );
			float clampResult468 = clamp( (0.0 + (temp_output_394_0 - _RampRemapMinOld) * (_RampRemapMaxNew - 0.0) / (1.0 - _RampRemapMinOld)) , 0.0 , 1.0 );
			float2 appendResult506 = (float2(_Noise04ScaleU , _Noise04ScaleV));
			float clampResult494 = clamp( ( ( pow( clampResult468 , _RampOffsetExp ) - ( temp_output_481_0 * _TransitionFixAmount ) ) + ( _Noise04Amount * tex2D( _Noise4, ( appendResult506 * temp_output_342_0 ) ).r ) ) , 0.0 , 1.0 );
			float2 appendResult461 = (float2(clampResult494 , 0.0));
			float4 temp_output_473_0 = ( tex2D( _Ramp, appendResult461 ) * _RampColorTint );
			float clampResult450 = clamp( ( temp_output_382_0 * _EdgeGlowPower ) , 0.0 , 1.0 );
			float4 lerpResult449 = lerp( lerpResult420 , ( temp_output_382_0 * temp_output_473_0 * _EdgeGlowPower * _FinalPower ) , clampResult450);
			float4 temp_output_442_0 = ( temp_output_473_0 * clampResult334 * _FinalPower );
			float clampResult546 = clamp( 0.0 , i.vertexColor.g , 1.0 );
			float4 lerpResult530 = lerp( lerpResult449 , temp_output_442_0 , clampResult546);
			float4 switchResult326 = (((i.ASEVFace>0)?(lerpResult530):(temp_output_442_0)));
			o.Emission = switchResult326.rgb;
			o.Alpha = 1;
			clip( clampResult393 - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;1834.978;1574.803;1.214178;True;False
Node;AmplifyShaderEditor.CommentaryNode;414;-3906.513,-478.708;Inherit;False;1551.222;832.1194;;16;345;344;342;343;346;340;339;341;337;338;412;409;410;413;335;411;Noise 01 UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;410;-3847.501,-305.7329;Inherit;False;Property;_Noise01ScaleU;Noise 01 Scale U;13;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;411;-3847.09,-221.128;Inherit;False;Property;_Noise01ScaleV;Noise 01 Scale V;14;0;Create;True;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;536;-2158.841,80.11774;Inherit;False;1380.622;512.8989;;11;355;356;357;358;459;458;533;534;535;508;532;Noise 02;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;335;-3733.907,-428.7078;Inherit;False;0;329;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;412;-3630.784,-279.0352;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;507;-1960.675,907.1509;Inherit;False;948.6281;428.3939;;7;481;483;482;478;484;487;486;Transition Mesh Part;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;533;-2108.841,283.9098;Inherit;False;Property;_Noise02TilingU;Noise 02 Tiling U;21;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;534;-2108.841,362.9098;Inherit;False;Property;_Noise02TilingV;Noise 02 Tiling V;22;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;-3415.088,-384.5966;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;539;-968.0027,1508.559;Inherit;False;908.7591;359.0823;;6;399;353;400;352;480;354;Appear Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;413;-3281.012,-384.5904;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;535;-1889.842,311.9098;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;345;-3830.938,5.475483;Inherit;False;Property;_Noise01Twist;Noise 01 Twist;17;0;Create;True;0;0;False;0;False;0;1.5;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;508;-1999.627,130.1178;Inherit;False;0;355;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;478;-1814.894,957.1509;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;532;-1688.842,221.9098;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;399;-918.0026,1558.559;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;545;-1673.034,738.8835;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;344;-3039.415,-220.3048;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;340;-3133.703,93.41138;Inherit;False;Property;_Noise01ScrollSpeedV;Noise 01 Scroll Speed V;19;0;Create;True;0;0;False;0;False;0;-0.125;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;339;-3132.703,16.41144;Inherit;False;Property;_Noise01ScrollSpeedU;Noise 01 Scroll Speed U;18;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;484;-1910.675,1119.582;Inherit;False;Property;_TransitionExp;Transition Exp;37;0;Create;True;0;0;False;0;False;1;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;353;-834.2858,1752.642;Inherit;False;Property;_AppearProgress;Appear Progress;1;0;Create;True;0;0;False;0;False;0;-0.125;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;483;-1599.029,1037.19;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;337;-3097.703,174.4115;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;356;-1533.834,401.2068;Inherit;False;Property;_Noise02Negate;Noise 02 Negate;23;0;Create;True;0;0;False;0;False;1;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;482;-1663.562,1208.545;Inherit;False;Property;_TransitionMultiply;Transition Multiply;38;0;Create;True;0;0;False;0;False;0;0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;341;-2860.24,55.67113;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;400;-708.8277,1603.063;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;343;-2833.489,-382.9856;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;355;-1555.914,192.3609;Inherit;True;Property;_Noise02;Noise 02;20;0;Create;True;0;0;False;0;False;-1;None;165e54ca813c80b429e4e3e6c6f04a59;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;338;-2716.239,103.6711;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;357;-1229.834,343.2068;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;481;-1446.047,1131.233;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;352;-519.9582,1668.169;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;346;-2703.892,-183.4938;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;480;-389.2034,1665.564;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;358;-1106.834,346.2068;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;459;-1241.939,478.0169;Inherit;False;Property;_Noise02Amount;Noise 02 Amount;24;0;Create;True;0;0;False;0;False;0;-0.075;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;540;-1459.266,-813.1494;Inherit;False;673.9996;363;;4;329;331;333;334;Noise 01;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;342;-2509.291,-27.39073;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;331;-1387.266,-565.149;Inherit;False;Property;_Noise01Negate;Noise 01 Negate;15;0;Create;True;0;0;False;0;False;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;329;-1409.266,-763.1492;Inherit;True;Property;_Noise01;Noise 01;12;0;Create;True;0;0;False;0;False;-1;None;91cbca9d8dd9ec246b38ecfc817c635a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;354;-234.2443,1663.149;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;458;-947.2197,358.4458;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;541;563.0737,524.5881;Inherit;False;701.2361;221.3861;;4;503;504;332;393;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;503;613.0737,574.5881;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;333;-1083.266,-623.1491;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;334;-959.266,-620.1491;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;504;732.4697,580.0776;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;332;938.6008,586.7394;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;393;1089.31,589.9742;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;544;896.1288,-1573.512;Inherit;False;2162.199;1023.161;;18;394;397;395;383;396;398;382;451;379;450;449;531;530;326;325;442;377;546;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;542;-1912.286,-349.8779;Inherit;False;1161.728;373.7446;;7;496;505;506;495;490;493;492;Noise 04;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;543;-419.2531,-455.4106;Inherit;False;1926.376;578.6915;;13;470;469;465;468;463;485;491;494;461;460;472;471;473;Ramp;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;394;1069.42,-1382.126;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;470;-365.8093,-400.1541;Inherit;False;Property;_RampRemapMinOld;Ramp Remap Min Old;6;0;Create;True;0;0;False;0;False;0;0.5;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;471;-369.253,-306.5337;Inherit;False;Property;_RampRemapMaxNew;Ramp Remap Max New;7;0;Create;True;0;0;False;0;False;0;1.25;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;505;-1862.286,-186.1474;Inherit;False;Property;_Noise04ScaleV;Noise 04 Scale V;29;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;496;-1860.845,-266.4478;Inherit;False;Property;_Noise04ScaleU;Noise 04 Scale U;28;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;469;-80.8212,-405.4108;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.35;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;506;-1577.178,-232.2518;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;495;-1396.318,-176.8215;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;457;-1830.411,-1239.341;Inherit;False;1006.91;322.9088;;4;453;452;455;454;Noise 03 Trick;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;468;111.1974,-399.7629;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;465;-13.56275,-244.9618;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;5;0;Create;True;0;0;False;0;False;1;8;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;487;-1439.062,1256.163;Inherit;False;Property;_TransitionFixAmount;Transition Fix Amount;39;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;551;-866.7546,-880.2722;Inherit;False;Property;_Noise01FrontAmount;Noise 01 Front Amount;16;0;Create;True;0;0;False;0;False;0;-0.916;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;490;-1245.841,-206.1332;Inherit;True;Property;_Noise4;Noise 4;27;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;463;272.2335,-321.3603;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;453;-1780.411,-1189.341;Inherit;False;Property;_Noise03Bias;Noise 03 Bias;26;0;Create;True;0;0;False;0;False;0;0.0225;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;486;-1154.807,1099.825;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;493;-1170.252,-299.8779;Inherit;False;Property;_Noise04Amount;Noise 04 Amount;30;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;485;433.6656,-301.3748;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;550;-490.4218,-1014.921;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;452;-1491.574,-1136.814;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;492;-919.56,-223.1778;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;491;576.1779,-224.3536;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;397;946.1288,-1293.417;Inherit;False;Property;_EdgeGlowSubtract;Edge Glow Subtract;36;0;Create;True;0;0;False;0;False;0;0.85;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;455;-1319.166,-1146.433;Inherit;True;Property;_Noise03;Noise 03;25;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;552;-325.7548,-1073.348;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;395;1253.398,-1383.179;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;538;-308.4432,-2121.83;Inherit;False;1059.161;649.4969;;7;423;422;421;324;424;419;420;Front Smoke;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;454;-997.5005,-1134.761;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;494;691.078,-226.5536;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;422;-258.4432,-1587.333;Inherit;False;Property;_FrontSmokeExp;Front Smoke Exp;11;0;Create;True;0;0;False;0;False;1;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;396;1433.129,-1372.416;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;461;833.2949,-229.345;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;423;-134.0596,-1697.896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;383;1301.692,-1243.342;Inherit;False;Property;_EdgeGlowExp;Edge Glow Exp;34;0;Create;True;0;0;False;0;False;2;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;460;968.6899,-269.7646;Inherit;True;Property;_Ramp;Ramp;3;0;Create;True;0;0;False;0;False;-1;None;f886c45d44a459d4194fe337b3e90660;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;421;45.31733,-1701.392;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;398;1565.199,-982.5079;Inherit;False;Property;_EdgeGlowPower;Edge Glow Power;35;0;Create;True;0;0;False;0;False;10;17.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;472;1057.124,-83.71918;Inherit;False;Property;_RampColorTint;Ramp Color Tint;4;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;382;1608.773,-1332.437;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;377;1825.011,-816.5046;Inherit;False;Property;_FinalPower;Final Power;2;0;Create;True;0;0;False;0;False;2;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;451;1817.182,-1466.239;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;324;117.3175,-2071.83;Inherit;False;Property;_FrontSmokeColorOne;Front Smoke Color One;9;0;Create;True;0;0;False;0;False;0.2075472,0.2075472,0.2075472,1;0.1094694,0.1720034,0.1886791,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;419;115.5641,-1901.156;Inherit;False;Property;_FrontSmokeColorTwo;Front Smoke Color Two;10;0;Create;True;0;0;False;0;False;0.2169811,0.2169811,0.2169811,1;0.05384467,0.08624051,0.09433947,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;473;1338.124,-152.7191;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;424;197.9761,-1699.896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;531;2303.2,-1180.677;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;379;1859.76,-1215.9;Inherit;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;450;1967.139,-1465.799;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;420;566.7181,-1921.585;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;449;2279.091,-1523.512;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;546;2313.438,-968.6119;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;442;2345.167,-706.3507;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;537;713.5363,1260.033;Inherit;False;1021.724;815.587;;11;444;445;447;375;365;374;364;371;373;372;446;Offset (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;530;2558.947,-1252.669;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;446;763.5363,1445.804;Inherit;False;Property;_OffsetAdjust;Offset Adjust;33;0;Create;True;0;0;False;0;False;0;0.075;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;365;1295.282,1661.187;Inherit;False;Property;_OffsetPower;Offset Power;31;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;374;1302.321,1461.123;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;371;1093.318,1745.336;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;447;1122.298,1389.751;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;375;1001.615,1549.297;Inherit;False;Property;_OffsetExp;Offset Exp;32;0;Create;True;0;0;False;0;False;1;16;0.2;16;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;372;1336.32,1848.098;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;364;1566.26,1641.908;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;325;1566.575,-1167.165;Inherit;False;Property;_BackColor;Back Color;8;0;Create;True;0;0;False;0;False;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwitchByFaceNode;326;2837.328,-1033.852;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;445;991.0187,1384.718;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;373;1109.035,1891.62;Inherit;False;Constant;_Vector0;Vector 0;14;0;Create;True;0;0;False;0;False;1,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;444;791.6315,1310.033;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3832.827,17.58198;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/MaskedDoubleSidedAdvanced;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.1;True;False;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;412;0;410;0
WireConnection;412;1;411;0
WireConnection;409;0;335;0
WireConnection;409;1;412;0
WireConnection;413;0;409;0
WireConnection;535;0;533;0
WireConnection;535;1;534;0
WireConnection;532;0;508;0
WireConnection;532;1;535;0
WireConnection;545;0;478;1
WireConnection;344;0;413;1
WireConnection;344;1;345;0
WireConnection;483;0;545;0
WireConnection;483;1;484;0
WireConnection;341;0;339;0
WireConnection;341;1;340;0
WireConnection;400;0;399;2
WireConnection;343;0;413;0
WireConnection;343;1;344;0
WireConnection;355;1;532;0
WireConnection;338;0;341;0
WireConnection;338;1;337;2
WireConnection;357;0;355;1
WireConnection;357;1;356;0
WireConnection;481;0;483;0
WireConnection;481;1;482;0
WireConnection;352;0;400;0
WireConnection;352;1;353;0
WireConnection;346;0;343;0
WireConnection;346;1;413;1
WireConnection;480;0;352;0
WireConnection;480;1;481;0
WireConnection;358;0;357;0
WireConnection;342;0;346;0
WireConnection;342;1;338;0
WireConnection;329;1;342;0
WireConnection;354;0;480;0
WireConnection;458;0;358;0
WireConnection;458;1;459;0
WireConnection;503;0;354;0
WireConnection;503;1;458;0
WireConnection;333;0;329;1
WireConnection;333;1;331;0
WireConnection;334;0;333;0
WireConnection;504;0;503;0
WireConnection;332;0;504;0
WireConnection;332;1;334;0
WireConnection;393;0;332;0
WireConnection;394;0;393;0
WireConnection;469;0;394;0
WireConnection;469;1;470;0
WireConnection;469;4;471;0
WireConnection;506;0;496;0
WireConnection;506;1;505;0
WireConnection;495;0;506;0
WireConnection;495;1;342;0
WireConnection;468;0;469;0
WireConnection;490;1;495;0
WireConnection;463;0;468;0
WireConnection;463;1;465;0
WireConnection;486;0;481;0
WireConnection;486;1;487;0
WireConnection;485;0;463;0
WireConnection;485;1;486;0
WireConnection;550;0;329;1
WireConnection;550;1;551;0
WireConnection;452;0;453;0
WireConnection;452;1;342;0
WireConnection;492;0;493;0
WireConnection;492;1;490;1
WireConnection;491;0;485;0
WireConnection;491;1;492;0
WireConnection;455;1;452;0
WireConnection;552;0;550;0
WireConnection;395;0;394;0
WireConnection;395;1;397;0
WireConnection;454;0;552;0
WireConnection;454;1;455;1
WireConnection;494;0;491;0
WireConnection;396;0;395;0
WireConnection;461;0;494;0
WireConnection;423;0;454;0
WireConnection;460;1;461;0
WireConnection;421;0;423;0
WireConnection;421;1;422;0
WireConnection;382;0;396;0
WireConnection;382;1;383;0
WireConnection;451;0;382;0
WireConnection;451;1;398;0
WireConnection;473;0;460;0
WireConnection;473;1;472;0
WireConnection;424;0;421;0
WireConnection;379;0;382;0
WireConnection;379;1;473;0
WireConnection;379;2;398;0
WireConnection;379;3;377;0
WireConnection;450;0;451;0
WireConnection;420;0;324;0
WireConnection;420;1;419;0
WireConnection;420;2;424;0
WireConnection;449;0;420;0
WireConnection;449;1;379;0
WireConnection;449;2;450;0
WireConnection;546;1;531;2
WireConnection;442;0;473;0
WireConnection;442;1;334;0
WireConnection;442;2;377;0
WireConnection;530;0;449;0
WireConnection;530;1;442;0
WireConnection;530;2;546;0
WireConnection;374;0;447;0
WireConnection;374;1;375;0
WireConnection;447;0;445;0
WireConnection;372;0;371;0
WireConnection;372;1;373;0
WireConnection;364;0;374;0
WireConnection;364;1;365;0
WireConnection;364;2;372;0
WireConnection;326;0;530;0
WireConnection;326;1;442;0
WireConnection;445;0;444;0
WireConnection;445;1;446;0
WireConnection;444;0;354;0
WireConnection;0;2;326;0
WireConnection;0;10;393;0
ASEEND*/
//CHKSM=6F550A86793EB531DF7F508B69A9C1239E7E51C1