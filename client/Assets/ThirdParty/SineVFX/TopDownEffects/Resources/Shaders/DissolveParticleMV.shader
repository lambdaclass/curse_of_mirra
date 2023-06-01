// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/DissolveParticleMV"
{
	Properties
	{
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalPower("Final Power", Range( 0 , 60)) = 4
		_FinalOpacityPower("Final Opacity Power", Range( 0 , 10)) = 1
		_FinalOpacityExp("Final Opacity Exp", Range( 0.2 , 8)) = 1
		[Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		[Toggle(_RAMPAFFECTEDBYVERTEXCOLOR_ON)] _RampAffectedByVertexColor("Ramp Affected By Vertex Color", Float) = 0
		_RampAffectedByDynamics("Ramp Affected By Dynamics", Range( 0 , 1)) = 1
		_RampOffsetMultiply("Ramp Offset Multiply", Float) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_MVMainMask("MV Main Mask", 2D) = "white" {}
		_MVMainMaskMotionVectors("MV Main Mask Motion Vectors", 2D) = "white" {}
		_MVMainMaskRotation("MV Main Mask Rotation", Range( 0 , 1)) = 0
		_MVMainMaskExp("MV Main Mask Exp", Range( 0.2 , 4)) = 1
		_MVRows("MV Rows", Float) = 8
		_MVColums("MV Colums", Float) = 8
		_MVDistortionFrameOffset("MV Distortion Frame Offset", Float) = 0.001
		_MVAnimationSpeed("MV Animation Speed", Float) = 1
		_MVRandomFrame("MV Random Frame", Range( 0 , 1)) = 0
		[Toggle(_MVPARTICLEFRAMECONTROLENABLED_ON)] _MVParticleFrameControlEnabled("MV Particle Frame Control Enabled", Float) = 0
		_SecondaryMask("Secondary Mask", 2D) = "white" {}
		_SecondaryMaskNegate("Secondary Mask Negate", Range( 0 , 1)) = 1
		_SecondaryMaskVSMoveU("Secondary Mask VS Move U", Range( 0 , 1)) = 0
		_SecondayMaskVSMoveV("Seconday Mask VS Move V", Range( 0 , 1)) = 0
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScaleU("Noise 01 Scale U", Float) = 1
		_Noise01ScaleV("Noise 01 Scale V", Float) = 1
		_Noise01Negate("Noise 01 Negate", Range( 0 , 1)) = 0
		_Noise01Exp("Noise 01 Exp", Range( 0.2 , 8)) = 1
		_Noise01ScrollSpeedU("Noise 01 Scroll Speed U", Float) = 0
		_Noise01ScrollSpeedV("Noise 01 Scroll Speed V", Float) = 0
		_Noise01RandomMin("Noise 01 Random Min", Range( 0.5 , 1)) = 0.9
		_Noise01RandomMax("Noise 01 Random Max", Range( 1 , 1.5)) = 1.1
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_DissolveTextureScaleU("Dissolve Texture Scale U", Float) = 1
		_DissolveTextureScaleV("Dissolve Texture Scale V", Float) = 1
		[Toggle(_DISSOLVETEXTUREFLIP_ON)] _DissolveTextureFlip("Dissolve Texture Flip", Float) = 1
		_DissolveTextureRandomMin("Dissolve Texture Random Min", Range( 0.5 , 1)) = 0.9
		_DissolveTextureRandomMax("Dissolve Texture Random Max", Range( 1 , 1.5)) = 1.1
		_DissolveExp("Dissolve Exp", Float) = 6.47
		_DissolveExpReversed("Dissolve Exp Reversed", Float) = 2
		_DissolveGlowAmount("Dissolve Glow Amount", Range( 0 , 120)) = 0
		_DissolveGlowPower("Dissolve Glow Power", Range( 0.2 , 8)) = 1
		_DissolveGlowOffset("Dissolve Glow Offset", Range( -1 , 1)) = 0.125
		_DissolveGlowAffectsRamp("Dissolve Glow Affects Ramp", Range( 0 , 1)) = 1
		_DissolveMask("Dissolve Mask", 2D) = "black" {}
		_DissolveMaskRemapMin("Dissolve Mask Remap Min", Range( -2 , -1)) = -1
		_DissolveMaskScale("Dissolve Mask Scale", Float) = 1
		_Distortion("Distortion", 2D) = "white" {}
		_DistortionScaleU("Distortion Scale U", Float) = 1
		_DistortionScaleV("Distortion Scale V", Float) = 1
		_DistortionPower("Distortion Power", Range( -1 , 1)) = -0.31
		_DistortionExp("Distortion Exp", Range( 0.2 , 8)) = 1
		_DistortionU("Distortion U", Range( 0 , 1)) = 1
		_DistortionV("Distortion V", Range( 0 , 1)) = 1
		_DistortionScrollSpeedU("Distortion Scroll Speed U", Float) = 0
		_DistortionScrollSpeedV("Distortion Scroll Speed V", Float) = 0
		_DistortionMask("Distortion Mask", 2D) = "white" {}
		_DistortionMaskNegate("Distortion Mask Negate", Range( 0 , 1)) = 0
		_DistortionMaskVSMoveU("Distortion Mask VS Move U", Range( 0 , 1)) = 0
		_DistortionMaskVSMoveV("Distortion Mask VS Move V", Range( 0 , 1)) = 0
		[Toggle(_EMISSIONVERTEXSTREAMENABLED_ON)] _EmissionVertexStreamEnabled("Emission Vertex Stream Enabled", Float) = 0
		[HideInInspector] _tex4coord3( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _RAMPENABLED_ON
		#pragma shader_feature _RAMPAFFECTEDBYVERTEXCOLOR_ON
		#pragma shader_feature _MVPARTICLEFRAMECONTROLENABLED_ON
		#pragma shader_feature _DISSOLVETEXTUREFLIP_ON
		#pragma shader_feature _EMISSIONVERTEXSTREAMENABLED_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 uv3_tex4coord3;
			float4 uv2_tex4coord2;
		};

		uniform float4 _FinalColor;
		uniform sampler2D _Ramp;
		uniform float _RampOffsetMultiply;
		uniform sampler2D _MVMainMask;
		uniform float _MVColums;
		uniform float _MVRows;
		uniform float _MVAnimationSpeed;
		uniform float _MVRandomFrame;
		uniform sampler2D _MVMainMaskMotionVectors;
		uniform float _MVDistortionFrameOffset;
		uniform float _MVMainMaskRotation;
		uniform sampler2D _Distortion;
		uniform float4 _Distortion_ST;
		uniform float _DistortionScaleU;
		uniform float _DistortionScaleV;
		uniform float _DistortionScrollSpeedU;
		uniform float _DistortionScrollSpeedV;
		uniform float _DistortionExp;
		uniform float _DistortionPower;
		uniform float _DistortionU;
		uniform float _DistortionV;
		uniform sampler2D _DistortionMask;
		uniform float4 _DistortionMask_ST;
		uniform float _DistortionMaskVSMoveU;
		uniform float _DistortionMaskVSMoveV;
		uniform float _DistortionMaskNegate;
		uniform float _MVMainMaskExp;
		uniform float _FinalOpacityPower;
		uniform sampler2D _Noise01;
		uniform float4 _Noise01_ST;
		uniform float _Noise01ScaleU;
		uniform float _Noise01ScaleV;
		uniform float _Noise01RandomMin;
		uniform float _Noise01RandomMax;
		uniform float _Noise01ScrollSpeedU;
		uniform float _Noise01ScrollSpeedV;
		uniform float _Noise01Negate;
		uniform float _Noise01Exp;
		uniform sampler2D _SecondaryMask;
		uniform float _SecondaryMaskVSMoveU;
		uniform float _SecondayMaskVSMoveV;
		uniform float4 _SecondaryMask_ST;
		uniform float _SecondaryMaskNegate;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _DissolveTextureScaleU;
		uniform float _DissolveTextureScaleV;
		uniform float _DissolveTextureRandomMin;
		uniform float _DissolveTextureRandomMax;
		uniform float _DissolveExp;
		uniform float _DissolveExpReversed;
		uniform float _DissolveMaskRemapMin;
		uniform float _DissolveMaskScale;
		uniform sampler2D _DissolveMask;
		uniform float4 _DissolveMask_ST;
		uniform float _RampAffectedByDynamics;
		uniform float _DissolveGlowOffset;
		uniform float _DissolveGlowPower;
		uniform float _DissolveGlowAmount;
		uniform float _DissolveGlowAffectsRamp;
		uniform float _RampOffsetExp;
		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform float _FinalOpacityExp;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult456 = (float2(_MVColums , _MVRows));
			float temp_output_455_0 = ( _MVColums * _MVRows );
			float2 appendResult412 = (float2(temp_output_455_0 , _MVRows));
			float temp_output_524_0 = ( i.uv_tex4coord.z * _MVRandomFrame );
			#ifdef _MVPARTICLEFRAMECONTROLENABLED_ON
				float staticSwitch468 = ( i.uv3_tex4coord3.y + temp_output_524_0 );
			#else
				float staticSwitch468 = ( ( ( i.uv_tex4coord.z + _Time.y ) * _MVAnimationSpeed ) + temp_output_524_0 );
			#endif
			float ifLocalVar477 = 0;
			if( abs( ( round( staticSwitch468 ) - staticSwitch468 ) ) >= 0.054 )
				ifLocalVar477 = staticSwitch468;
			else
				ifLocalVar477 = ( staticSwitch468 + 0.076 );
			float temp_output_407_0 = frac( ( ifLocalVar477 / temp_output_455_0 ) );
			float2 appendResult414 = (float2(temp_output_407_0 , ( 1.0 - temp_output_407_0 )));
			float2 temp_output_424_0 = ( floor( ( appendResult412 * appendResult414 ) ) / appendResult456 );
			float2 temp_output_426_0 = ( ( i.uv_texcoord / appendResult456 ) + temp_output_424_0 );
			float4 tex2DNode429 = tex2D( _MVMainMaskMotionVectors, temp_output_426_0 );
			float2 appendResult431 = (float2(tex2DNode429.r , tex2DNode429.g));
			float2 break438 = ( ( appendResult431 * float2( 2,2 ) ) - float2( 1,1 ) );
			float2 appendResult442 = (float2(break438.x , -break438.y));
			float temp_output_439_0 = frac( ifLocalVar477 );
			float2 appendResult517 = (float2(( 8.0 / _MVColums ) , ( 8.0 / _MVRows )));
			float temp_output_584_0 = (0.0 + (_MVMainMaskRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0));
			float cos572 = cos( temp_output_584_0 );
			float sin572 = sin( temp_output_584_0 );
			float2 rotator572 = mul( ( temp_output_426_0 - ( ( appendResult442 * temp_output_439_0 ) * _MVDistortionFrameOffset * appendResult517 ) ) - ( ( float2( 0.5,0.5 ) / appendResult456 ) + temp_output_424_0 ) , float2x2( cos572 , -sin572 , sin572 , cos572 )) + ( ( float2( 0.5,0.5 ) / appendResult456 ) + temp_output_424_0 );
			float2 uv0_Distortion = i.uv_texcoord * _Distortion_ST.xy + _Distortion_ST.zw;
			float2 appendResult556 = (float2(_DistortionScaleU , _DistortionScaleV));
			float2 appendResult209 = (float2(( _Time.y * _DistortionScrollSpeedU ) , ( _Time.y * _DistortionScrollSpeedV )));
			float2 appendResult221 = (float2(_DistortionU , _DistortionV));
			float2 uv0_DistortionMask = i.uv_texcoord * _DistortionMask_ST.xy + _DistortionMask_ST.zw;
			float2 appendResult277 = (float2(_DistortionMaskVSMoveU , _DistortionMaskVSMoveV));
			float clampResult283 = clamp( ( tex2D( _DistortionMask, ( uv0_DistortionMask + ( appendResult277 * i.uv3_tex4coord3.x ) ) ).r + _DistortionMaskNegate ) , 0.0 , 1.0 );
			float2 temp_output_217_0 = ( pow( tex2D( _Distortion, ( (( uv0_Distortion * appendResult556 )*1.0 + i.uv_tex4coord.z) + appendResult209 ) ).r , _DistortionExp ) * _DistortionPower * appendResult221 * i.uv_tex4coord.w * clampResult283 );
			float2 _Vector1 = float2(0,0);
			float2 temp_output_613_0 = ( float2( 1,1 ) / appendResult456 );
			float2 clampResult610 = clamp( ( rotator572 + temp_output_217_0 ) , ( _Vector1 + temp_output_424_0 ) , ( temp_output_613_0 + temp_output_424_0 ) );
			float4 tex2DNode453 = tex2D( _MVMainMask, clampResult610 );
			float2 appendResult419 = (float2(_MVColums , _MVRows));
			float temp_output_404_0 = ( _MVColums * _MVRows );
			float2 appendResult413 = (float2(temp_output_404_0 , _MVRows));
			float temp_output_408_0 = frac( ( ( ifLocalVar477 + 1.0 ) / temp_output_404_0 ) );
			float2 appendResult411 = (float2(temp_output_408_0 , ( 1.0 - temp_output_408_0 )));
			float2 temp_output_422_0 = ( floor( ( appendResult413 * appendResult411 ) ) / appendResult419 );
			float2 temp_output_428_0 = ( ( i.uv_texcoord / appendResult419 ) + temp_output_422_0 );
			float4 tex2DNode430 = tex2D( _MVMainMaskMotionVectors, temp_output_428_0 );
			float2 appendResult432 = (float2(tex2DNode430.r , tex2DNode430.g));
			float2 break437 = ( ( appendResult432 * float2( 2,2 ) ) - float2( 1,1 ) );
			float2 appendResult443 = (float2(break437.x , -break437.y));
			float cos573 = cos( temp_output_584_0 );
			float sin573 = sin( temp_output_584_0 );
			float2 rotator573 = mul( ( temp_output_428_0 + ( ( appendResult443 * ( 1.0 - temp_output_439_0 ) ) * _MVDistortionFrameOffset * appendResult517 ) ) - ( ( float2( 0.5,0.5 ) / appendResult419 ) + temp_output_422_0 ) , float2x2( cos573 , -sin573 , sin573 , cos573 )) + ( ( float2( 0.5,0.5 ) / appendResult419 ) + temp_output_422_0 );
			float2 clampResult616 = clamp( ( rotator573 + temp_output_217_0 ) , ( _Vector1 + temp_output_422_0 ) , ( temp_output_613_0 + temp_output_422_0 ) );
			float4 tex2DNode454 = tex2D( _MVMainMask, clampResult616 );
			float lerpResult512 = lerp( tex2DNode453.r , tex2DNode454.r , temp_output_439_0);
			float temp_output_146_0 = pow( lerpResult512 , _MVMainMaskExp );
			float2 uv0_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult544 = (float2(_Noise01ScaleU , _Noise01ScaleV));
			float2 appendResult178 = (float2(( _Time.y * _Noise01ScrollSpeedU ) , ( _Time.y * _Noise01ScrollSpeedV )));
			float clampResult115 = clamp( ( tex2D( _Noise01, ( (( uv0_Noise01 * appendResult544 * (_Noise01RandomMin + (i.uv_tex4coord.z - 0.0) * (_Noise01RandomMax - _Noise01RandomMin) / (120.0 - 0.0)) )*1.0 + ( i.uv_tex4coord.z * 1.4 )) + appendResult178 ) ).r + _Noise01Negate ) , 0.0 , 1.0 );
			float temp_output_173_0 = pow( clampResult115 , _Noise01Exp );
			float2 appendResult263 = (float2(_SecondaryMaskVSMoveU , _SecondayMaskVSMoveV));
			float2 uv0_SecondaryMask = i.uv_texcoord * _SecondaryMask_ST.xy + _SecondaryMask_ST.zw;
			float clampResult269 = clamp( ( tex2D( _SecondaryMask, ( ( appendResult263 * i.uv2_tex4coord2.z ) + uv0_SecondaryMask ) ).r + i.uv2_tex4coord2.w + _SecondaryMaskNegate ) , 0.0 , 1.0 );
			float clampResult66 = clamp( ( temp_output_146_0 * _FinalOpacityPower * temp_output_173_0 * clampResult269 ) , 0.0 , 1.0 );
			float2 uv0_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			float2 appendResult550 = (float2(_DissolveTextureScaleU , _DissolveTextureScaleV));
			float4 tex2DNode9 = tex2D( _DissolveTexture, (( uv0_DissolveTexture * appendResult550 * (_DissolveTextureRandomMin + (i.uv_tex4coord.z - 0.0) * (_DissolveTextureRandomMax - _DissolveTextureRandomMin) / (120.0 - 0.0)) )*1.0 + i.uv_tex4coord.z) );
			#ifdef _DISSOLVETEXTUREFLIP_ON
				float staticSwitch28 = ( 1.0 - tex2DNode9.r );
			#else
				float staticSwitch28 = tex2DNode9.r;
			#endif
			float clampResult91 = clamp( ( 1.0 - pow( ( 1.0 - pow( staticSwitch28 , _DissolveExp ) ) , (1.0 + (i.uv2_tex4coord2.x - 0.0) * (_DissolveExpReversed - 1.0) / (1.0 - 0.0)) ) ) , 0.0 , 1.0 );
			float2 uv_DissolveMask = i.uv_texcoord * _DissolveMask_ST.xy + _DissolveMask_ST.zw;
			float clampResult287 = clamp( ( ( (_DissolveMaskRemapMin + (i.uv2_tex4coord2.x - 0.0) * (1.0 - _DissolveMaskRemapMin) / (1.0 - 0.0)) * _DissolveMaskScale ) + ( tex2D( _DissolveMask, uv_DissolveMask ).r * _DissolveMaskScale ) ) , -1.0 , 1.0 );
			float temp_output_13_0 = ( clampResult91 + clampResult287 );
			float clampResult35 = clamp( temp_output_13_0 , 0.0 , 1.0 );
			float clampResult8 = clamp( ( clampResult66 - clampResult35 ) , 0.0 , 1.0 );
			float lerpResult151 = lerp( temp_output_146_0 , clampResult8 , _RampAffectedByDynamics);
			float clampResult240 = clamp( ( ( _DissolveGlowOffset + temp_output_13_0 ) - clampResult66 ) , 0.0 , 1.0 );
			float temp_output_235_0 = ( 1.0 + ( pow( clampResult240 , _DissolveGlowPower ) * _DissolveGlowAmount ) );
			float lerpResult537 = lerp( 1.0 , temp_output_235_0 , _DissolveGlowAffectsRamp);
			float clampResult510 = clamp( ( _RampOffsetMultiply * lerpResult151 * lerpResult537 ) , 0.0 , 1.0 );
			float clampResult158 = clamp( ( 1.0 - pow( ( 1.0 - clampResult510 ) , _RampOffsetExp ) ) , 0.0 , 1.0 );
			float2 appendResult159 = (float2(clampResult158 , 0.0));
			float4 temp_output_192_0 = ( tex2D( _Ramp, appendResult159 ) * _RampColorTint );
			#ifdef _RAMPAFFECTEDBYVERTEXCOLOR_ON
				float4 staticSwitch189 = ( i.vertexColor * temp_output_192_0 );
			#else
				float4 staticSwitch189 = temp_output_192_0;
			#endif
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch149 = staticSwitch189;
			#else
				float4 staticSwitch149 = ( _FinalColor * i.vertexColor );
			#endif
			#ifdef _EMISSIONVERTEXSTREAMENABLED_ON
				float staticSwitch55 = i.uv2_tex4coord2.y;
			#else
				float staticSwitch55 = 1.0;
			#endif
			o.Emission = ( staticSwitch149 * _FinalPower * staticSwitch55 * temp_output_235_0 ).rgb;
			float clampResult43 = clamp( ( clampResult8 * i.vertexColor.a ) , 0.0 , 1.0 );
			float clampResult164 = clamp( ( 1.0 - pow( ( 1.0 - clampResult43 ) , _FinalOpacityExp ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult164;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;6910.541;1811.098;3.232441;True;False
Node;AmplifyShaderEditor.CommentaryNode;482;-11289,-1496.064;Inherit;False;6030.992;3183.661;;108;529;513;530;478;531;532;479;526;512;454;453;481;452;480;451;450;448;449;445;447;517;446;519;442;518;444;443;440;520;439;441;437;438;435;436;434;433;432;431;429;430;426;428;427;424;422;423;425;421;419;420;456;417;418;415;416;411;414;412;413;409;410;408;407;406;405;455;403;404;400;402;401;477;476;474;475;473;472;471;468;522;521;458;466;524;465;525;523;464;459;462;572;573;574;579;580;577;584;585;610;612;613;614;611;615;616;617;618;Main Mask MV;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;462;-11239.29,-591.8978;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;459;-11233.15,-419.005;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;464;-10974.67,-474.8711;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;523;-11191.56,-155.7615;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;525;-11241.64,21.06818;Inherit;False;Property;_MVRandomFrame;MV Random Frame;27;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;465;-11250.7,-262.6125;Float;False;Property;_MVAnimationSpeed;MV Animation Speed;25;0;Create;True;0;0;False;0;False;1;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;524;-10948.64,-42.93182;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;466;-10802.51,-390.8786;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;458;-11176.34,99.64429;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;521;-10646.71,-347.4323;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;522;-10667.51,-35.43232;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;468;-10501.12,-200.8516;Float;False;Property;_MVParticleFrameControlEnabled;MV Particle Frame Control Enabled;28;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;471;-10129.59,-369.6365;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;473;-9972.257,-315.6874;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;472;-9979.518,159.074;Inherit;False;Constant;_Fix02;Fix 02;34;0;Create;True;0;0;False;0;False;0.076;0.0128;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;476;-9770.506,-353.876;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;474;-9759.542,-106.862;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;475;-9806.596,-258.6925;Inherit;False;Constant;_Fix01;Fix 01;32;0;Create;True;0;0;False;0;False;0.054;0.0114;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;400;-8813.89,13.71001;Float;False;Property;_MVRows;MV Rows;22;0;Create;True;0;0;False;0;False;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;401;-8981.641,579.8763;Inherit;False;Constant;_Float4;Float 4;27;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;402;-8836.564,-68.30999;Float;False;Property;_MVColums;MV Colums;23;0;Create;True;0;0;False;0;False;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;477;-9588.2,-269.6595;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;455;-8551.521,-1277.006;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;404;-8484.133,1292.415;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;403;-8769.25,516.2883;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.99;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;405;-8703.193,-1055.163;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;406;-8634.464,1513.597;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;407;-8557.852,-1055.824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;408;-8504.255,1521.871;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;409;-8399.606,-986.5587;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;410;-8330.465,1577.597;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;412;-8221.853,-1183.824;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;411;-8154.464,1513.597;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;414;-8221.853,-1055.824;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;413;-8154.464,1385.597;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;416;-8010.464,1449.597;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;415;-8077.852,-1119.824;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;420;-8011.64,1123.356;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;419;-8154.464,1257.597;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FloorOpNode;418;-7866.464,1449.597;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;456;-8221.853,-1311.824;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;421;-8079.029,-1446.064;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FloorOpNode;417;-7933.852,-1119.824;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;425;-7773.852,-1343.824;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;424;-7773.852,-1215.824;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;422;-7706.464,1353.597;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;423;-7706.464,1225.597;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;426;-7613.852,-1279.824;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;428;-7546.464,1289.597;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;427;-8368.79,3.263704;Float;True;Property;_MVMainMaskMotionVectors;MV Main Mask Motion Vectors;18;0;Create;True;0;0;False;0;False;None;a5968a90472871f45a73441be57a74b3;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;429;-7985.875,-210.86;Inherit;True;Property;_TextureSample0;Texture Sample 0;13;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;430;-7988.875,248.1398;Inherit;True;Property;_TextureSample1;Texture Sample 1;14;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;431;-7637.174,-183.86;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;432;-7647.675,274.1398;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;434;-7503.874,272.1398;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;2,2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;559;-9174.837,-4021.398;Inherit;False;2096.697;1802.816;;34;275;276;279;204;206;277;205;208;274;278;207;209;280;282;273;210;281;220;227;201;222;249;221;216;283;226;217;557;558;203;556;553;554;555;Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;433;-7495.873,-183.46;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;2,2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;295;-4312.587,3189.294;Inherit;False;3038.982;1538.66;;33;291;35;13;91;287;286;96;94;290;288;284;95;36;93;92;23;40;90;28;24;27;9;16;181;243;29;245;244;547;549;550;551;603;Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;554;-9030.839,-3846.992;Inherit;False;Property;_DistortionScaleU;Distortion Scale U;58;0;Create;True;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;555;-9030.839,-3764.993;Inherit;False;Property;_DistortionScaleV;Distortion Scale V;59;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-8637.869,-2512.471;Inherit;False;Property;_DistortionMaskVSMoveV;Distortion Mask VS Move V;69;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;435;-7326.542,-170.1977;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;436;-7352.874,274.1398;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;275;-8637.024,-2595.762;Inherit;False;Property;_DistortionMaskVSMoveU;Distortion Mask VS Move U;68;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;549;-4162.714,3494.244;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;44;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-9124.837,-3196.389;Inherit;False;Property;_DistortionScrollSpeedV;Distortion Scroll Speed V;65;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-4228.392,3686.529;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;547;-4160.714,3420.244;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;43;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;206;-9057.048,-3425.153;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;277;-8344.093,-2561.736;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-4262.587,3869.993;Inherit;False;Property;_DissolveTextureRandomMin;Dissolve Texture Random Min;46;0;Create;True;0;0;False;0;False;0.9;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;203;-9055.798,-3971.398;Inherit;False;0;201;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;279;-8407.128,-2420.582;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;204;-9116.37,-3276.03;Inherit;False;Property;_DistortionScrollSpeedU;Distortion Scroll Speed U;64;0;Create;True;0;0;False;0;False;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;438;-7444.924,-401.8254;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;437;-7476.715,578.8077;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;556;-8794.84,-3810.992;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;245;-4258.587,3949.993;Inherit;False;Property;_DissolveTextureRandomMax;Dissolve Texture Random Max;47;0;Create;True;0;0;False;0;False;1.1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;181;-4202.478,3257.343;Inherit;False;0;9;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;550;-3896.714,3452.244;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;243;-3911.39,3813.598;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-8789.312,-3309.923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-8791.004,-3408.209;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;553;-8623.02,-3887.815;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;440;-7196.152,-305.2009;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;-8155.001,-2491.567;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;520;-7772.345,-6.044775;Inherit;False;Constant;_Float3;Float 3;61;0;Create;True;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;558;-8679.584,-3718.453;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;441;-7206.312,671.1077;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;274;-8462.961,-2806.866;Inherit;False;0;273;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;439;-7995.22,737.2361;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;209;-8616.466,-3359.067;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;546;-3599.893,960.8034;Inherit;False;2159.318;1221.517;;24;248;247;182;246;132;131;136;179;257;183;178;144;114;112;113;174;115;173;544;142;545;542;543;177;Noise 01;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;442;-7059.853,-403.6006;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;551;-3668.714,3385.244;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;518;-7003.639,-28.68256;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;519;-7547.69,105.4312;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;444;-7371.65,410.7006;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;557;-8374.529,-3769.549;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;443;-7089.313,581.4076;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;280;-8093.116,-2652.034;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;517;-7380.134,43.19297;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;542;-3548.893,1179.61;Inherit;False;Property;_Noise01ScaleU;Noise 01 Scale U;34;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-3504.837,1654.797;Inherit;False;Property;_Noise01RandomMax;Noise 01 Random Max;41;0;Create;True;0;0;False;0;False;1.1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;16;-3487.373,3703.637;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;182;-3401.599,1368.894;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;282;-7929.735,-2629.386;Inherit;False;Property;_DistortionMaskNegate;Distortion Mask Negate;67;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-3506.837,1572.797;Inherit;False;Property;_Noise01RandomMin;Noise 01 Random Min;40;0;Create;True;0;0;False;0;False;0.9;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-8087.773,-3379.401;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;579;-7351.656,-1275.19;Inherit;False;Constant;_Vector2;Vector 1;70;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;273;-7956.918,-2832.129;Inherit;True;Property;_DistortionMask;Distortion Mask;66;0;Create;True;0;0;False;0;False;-1;None;a52fb0452b0d7d64db47d0c0ef5e325c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;447;-7232.34,19.57032;Float;False;Property;_MVDistortionFrameOffset;MV Distortion Frame Offset;24;0;Create;True;0;0;False;0;False;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;446;-7138.946,311.0566;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;583;-7243.506,1231.263;Inherit;False;Constant;_Vector3;Vector 1;70;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;445;-7125.64,-183.2268;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;543;-3549.893,1258.61;Inherit;False;Property;_Noise01ScaleV;Noise 01 Scale V;35;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;296;-2983.588,117.0868;Inherit;False;1540.158;663.4843;;11;269;268;264;272;266;260;265;263;258;261;262;Secondary Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;9;-3258.162,3678.127;Inherit;True;Property;_DissolveTexture;Dissolve Texture;42;0;Create;True;0;0;False;0;False;-1;None;34a3b7afc8c389e40bda6d478fb654e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;177;-3130.081,1931.934;Inherit;False;Property;_Noise01ScrollSpeedV;Noise 01 Scroll Speed V;39;0;Create;True;0;0;False;0;False;0;-0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;132;-3107.275,1856.905;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-3412.404,1010.803;Inherit;False;0;112;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;131;-3141.526,2004.066;Inherit;False;Property;_Noise01ScrollSpeedU;Noise 01 Scroll Speed U;38;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;201;-7874.855,-3405.309;Inherit;True;Property;_Distortion;Distortion;57;0;Create;True;0;0;False;0;False;-1;None;de3dab975e978d64696053ab162b6505;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;574;-7192.957,-658.0192;Inherit;False;Property;_MVMainMaskRotation;MV Main Mask Rotation;19;0;Create;True;0;0;False;0;False;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;585;-7107.113,-561.2543;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;448;-6912.208,309.5843;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;581;-6982.745,1199.851;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;544;-3321.893,1206.61;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;449;-6925.461,-182.1693;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-7826.41,-3077.497;Inherit;False;Property;_DistortionU;Distortion U;62;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;281;-7640.859,-2714.639;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;227;-7866.547,-3532.465;Inherit;False;Property;_DistortionExp;Distortion Exp;61;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;246;-3168.838,1560.797;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;577;-7125.565,-1196.938;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-7825.896,-2998.888;Inherit;False;Property;_DistortionV;Distortion V;63;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;221;-7515.895,-3039.888;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-2862.86,1929.234;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;545;-2939.396,1112.35;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;262;-2919.464,281.9684;Inherit;False;Property;_SecondayMaskVSMoveV;Seconday Mask VS Move V;32;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-2960.465,3854.312;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-2933.588,167.0868;Inherit;False;Property;_SecondaryMaskVSMoveU;Secondary Mask VS Move U;31;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;226;-7540.194,-3463.194;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;-3128.925,1365.339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;580;-6972.224,-1003.657;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;584;-6870.068,-654.1262;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-7642.198,-3194.18;Inherit;False;Property;_DistortionPower;Distortion Power;60;0;Create;True;0;0;False;0;False;-0.31;-0.05;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;612;-5848.718,-688.6447;Inherit;False;Constant;_Vector4;Vector 4;72;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;450;-6681.194,462.705;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;451;-6691.366,-70.27358;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;-2852.394,2049.321;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;582;-6778.649,1257.667;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;249;-7570.127,-2912.24;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;283;-7501.596,-2716.856;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;263;-2628.706,234.8493;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;183;-2716.299,1341.176;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;258;-2868.937,389.7974;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;603;-2642.027,4276.277;Inherit;False;352;165;;1;602;If using Mask, set at "-2";1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;178;-2676.148,1978.82;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;28;-2841.465,3703.31;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;45;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2702.716,3821.418;Inherit;False;Property;_DissolveExp;Dissolve Exp;48;0;Create;True;0;0;False;0;False;6.47;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;572;-6426.039,-656.4082;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;613;-5692.344,-685.0323;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-7234.069,-3072.839;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;611;-5854.527,-821.358;Inherit;False;Constant;_Vector1;Vector 1;72;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;573;-6608.413,794.71;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;291;-2144.32,4544.903;Inherit;False;307;165;;1;289;If mask is empty, keep it at 1f;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;92;-2553.708,3239.294;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;144;-2466.911,1588.246;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;265;-2895.408,560.7791;Inherit;False;0;264;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;40;-2223.491,4011.96;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;23;-2475.679,3730.362;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-2602.035,3433.338;Inherit;False;Property;_DissolveExpReversed;Dissolve Exp Reversed;49;0;Create;True;0;0;False;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-2397.213,365.0258;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;615;-5502.357,-842.9467;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;618;-6277.487,909.868;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;480;-6418.681,-232.1246;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;481;-6434.954,529.1091;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;614;-5503.544,-952.9405;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;617;-6276.304,806.9829;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;602;-2596.027,4331.277;Inherit;False;Property;_DissolveMaskRemapMin;Dissolve Mask Remap Min;55;0;Create;True;0;0;False;0;False;-1;-2;-2;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-2291.215,1756.249;Inherit;False;Property;_Noise01Negate;Noise 01 Negate;36;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;95;-2168.873,3516.401;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;266;-2230.51,422.4701;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;289;-2095.32,4610.903;Inherit;False;Property;_DissolveMaskScale;Dissolve Mask Scale;56;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;36;-1993.347,4016.673;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;93;-2303.535,3266.37;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;284;-2119.212,4270.872;Inherit;True;Property;_DissolveMask;Dissolve Mask;54;0;Create;True;0;0;False;0;False;-1;None;a52fb0452b0d7d64db47d0c0ef5e325c;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;112;-2305.655,1557.112;Inherit;True;Property;_Noise01;Noise 01;33;0;Create;True;0;0;False;0;False;-1;None;7a8f73b901f55d14b8a7fae4c4045efa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;452;-6701.662,100.5152;Float;True;Property;_MVMainMask;MV Main Mask;16;0;Create;True;0;0;False;0;False;None;6d578ff549a61504984d995aa9d1de7c;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ClampOpNode;616;-6086.847,652.2945;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;610;-6051.105,-511.1407;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;290;-1772.114,4158.164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;94;-1997.159,3517.744;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-1955.284,1647.529;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;288;-1777.747,4375.007;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;453;-6271.746,-64.38398;Inherit;True;Property;_TextureSample2;Texture Sample 2;14;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;454;-6269.636,296.0564;Inherit;True;Property;_TextureSample3;Texture Sample 3;15;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;539;-1323.688,-415.7617;Inherit;False;1023.463;325.5581;;2;147;146;Lerp 0;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;272;-2059.833,665.571;Inherit;False;Property;_SecondaryMaskNegate;Secondary Mask Negate;30;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;264;-2053.126,383.3206;Inherit;True;Property;_SecondaryMask;Secondary Mask;29;0;Create;True;0;0;False;0;False;-1;None;a52fb0452b0d7d64db47d0c0ef5e325c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;174;-1933.576,1784.643;Inherit;False;Property;_Noise01Exp;Noise 01 Exp;37;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;268;-1749.73,540.5187;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-1274.764,-219.4473;Inherit;False;Property;_MVMainMaskExp;MV Main Mask Exp;20;0;Create;True;0;0;False;0;False;1;1;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;96;-1840.201,3520.427;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;115;-1815.532,1644.352;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;286;-1600.854,4257.559;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;512;-5529.798,-112.5672;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;173;-1620.576,1697.643;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;269;-1618.43,541.8188;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;91;-1780.529,3843.928;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;146;-977.9521,-324.8559;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;287;-1448.017,4254.138;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1691.001,828.5112;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1.25;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;292;-1443.674,2565.81;Inherit;False;1378.789;361.9214;;10;235;236;232;234;231;240;233;242;239;238;Glowing Edges When Dissolved;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1620.914,3919.683;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;-1393.674,2615.81;Inherit;False;Property;_DissolveGlowOffset;Dissolve Glow Offset;52;0;Create;True;0;0;False;0;False;0.125;0.25;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-198.7198,-90.45295;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;66;-48.97653,-88.99887;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;239;-1118.003,2667.799;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;242;-993.16,2663.211;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;240;-833.9589,2655.147;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;-948.964,2781.714;Inherit;False;Property;_DissolveGlowPower;Dissolve Glow Power;51;0;Create;True;0;0;False;0;False;1;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;35;-1448.605,3923.387;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;231;-653.4669,2700.259;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;234;-680.7259,2812.732;Inherit;False;Property;_DissolveGlowAmount;Dissolve Glow Amount;50;0;Create;True;0;0;False;0;False;0;0;0;120;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;536;-924.0254,2234.113;Inherit;False;752.7615;296.6475;;3;537;538;534;Dissolve Glow Affects Ramp;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;236;-379.9129,2630.42;Inherit;False;Constant;_Float1;Float 1;49;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;42.17837,484.8338;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-368.0858,2713.049;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;235;-218.8838,2668.697;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;538;-762.3823,2310.023;Inherit;False;Constant;_Float6;Float 6;64;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;534;-892.2336,2396.377;Inherit;False;Property;_DissolveGlowAffectsRamp;Dissolve Glow Affects Ramp;53;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-3579.716,-902.5541;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;211.1785,487.7339;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;537;-388.5846,2335.176;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;151;-3264.305,-941.2837;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-3327.895,-1034.334;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;9;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-3034.5,-956.0848;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;510;-2880.354,-956.6819;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-2840.188,-787.6899;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;10;0;Create;True;0;0;False;0;False;1;4;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;187;-2731.791,-949.9424;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;-2579.57,-951.3734;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;188;-2429.891,-957.0424;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;158;-2278.025,-959.8229;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;159;-2111.833,-956.1522;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;39;204.9745,678.7123;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;191;-1879.15,-785.2544;Inherit;False;Property;_RampColorTint;Ramp Color Tint;6;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;150;-1962.946,-974.6906;Inherit;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;False;-1;None;244ec440ba2f7824dad8ce1e800567c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;468.7708,554.1188;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-1565.938,-879.1814;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;43;625.1071,558.3958;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;41;-1648.461,-1243.822;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-1676.163,-1409.776;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;160;613.7361,703.9526;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;3;0;Create;True;0;0;False;0;False;1;3;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;161;803.5771,565.9183;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;-1290.839,-1075.002;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1417.403,-1311.321;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;189;-1006.149,-974.2191;Inherit;False;Property;_RampAffectedByVertexColor;Ramp Affected By Vertex Color;7;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;-643.7588,-1223.513;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;162;956.6368,569.9461;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-595.0848,-1316.655;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;511;-4610.81,-3161.752;Inherit;False;2651.594;983.4888;;15;498;499;501;506;504;509;508;486;507;505;496;487;483;497;500;Custom Color Mask For Ramp (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-678.3,-650.3998;Inherit;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;4;40;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;541;-4887.402,203.0889;Inherit;False;1017.753;405.4854;;6;493;491;494;122;495;118;(Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;55;-377.3068,-1254.09;Inherit;False;Property;_EmissionVertexStreamEnabled;Emission Vertex Stream Enabled;71;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;163;1101.641,569.9461;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;149;-618.728,-1030.229;Inherit;False;Property;_RampEnabled;Ramp Enabled;4;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;608;-1134.061,-3.280465;Inherit;False;688.9867;279.9307;;4;605;607;604;606;Exp 2 (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;571;-4913.34,-733.54;Inherit;False;1276.113;553.6541;;9;567;570;569;565;561;562;560;563;619;Distortion Fix;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;532;-10608.43,-898.0307;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0.8;False;4;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;509;-2324.215,-2447.85;Inherit;False;Property;_CustomColorMaskEnabled;Custom Color Mask Enabled;11;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;493;-4837.402,253.0889;Inherit;False;Property;_MVMainMaskChannels;MV Main Mask Channels;17;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;499;-4181.541,-2986.359;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;500;-4052.956,-2986.709;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;495;-4044.649,353.1977;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;479;-5529.013,134.2619;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;526;-11216.61,-806.4544;Inherit;False;Property;_MVRandomSpeed;MV Random Speed;26;0;Create;True;0;0;False;0;False;0;0.5;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;530;-11099.15,-1010.979;Inherit;False;Constant;_Float5;Float 5;63;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;607;-1084.061,161.6502;Inherit;False;Property;_MVMainMaskExp2;MV Main Mask Exp 2;21;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;605;-786.7883,51.13997;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;103.6291,-694.3374;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;501;-3380.112,-2293.264;Inherit;False;Property;_CustomColorMaskAffectedByDynamics;Custom Color Mask Affected By Dynamics;15;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;507;-3135.705,-2952.945;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;478;-5777.945,138.6711;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;496;-4552.872,-2908.392;Inherit;False;Property;_CustomColorMaskChannels;Custom Color Mask Channels;13;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;-4555.177,349.2893;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;504;-2754.463,-2474.304;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;498;-3672.937,-2982.088;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;494;-4180.65,353.1977;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;506;-3292.51,-2956.354;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;562;-4297.614,-530.5997;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;513;-9459.57,-36.84082;Inherit;False;Constant;_Float2;Float 2;61;0;Create;True;0;0;False;0;False;0.055;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;565;-4500.891,-646.4951;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;561;-4526.651,-453.8694;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;569;-4855.603,-542.8633;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;567;-4863.34,-683.54;Inherit;False;Constant;_Vector0;Vector 0;69;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;570;-4660.413,-586.1178;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;619;-4027.499,-300.5339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;560;-4145.019,-555.5602;Inherit;True;Property;_DistortionFixMask;Distortion Fix Mask;70;0;Create;True;0;0;False;0;False;-1;a9159bc94ccc1ad44b2359384686609a;a9159bc94ccc1ad44b2359384686609a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;486;-2851.611,-2331.696;Inherit;False;Property;_CustomColorMaskSwitch;Custom Color Mask Switch;14;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;606;-632.074,51.13997;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;164;1260.071,563.2332;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;487;-2546.925,-2431.021;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;508;-2988.127,-2955.257;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;531;-10836.15,-862.9789;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;529;-10837.15,-999.9787;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;505;-3426.293,-2958.908;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;483;-4560.81,-3111.752;Inherit;True;Property;_CustomColorMask;Custom Color Mask;12;0;Create;True;0;0;False;0;False;-1;None;58298181fb411e84aa99769090d51a96;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;491;-4732.084,429.5743;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;497;-3801.094,-2985.75;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;604;-949.2383,46.71953;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;122;-4419.176,355.2893;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;563;-3806.228,-312.8861;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1521.547,-32.13005;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/DissolveParticleMV;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;540;161.1785,437.7339;Inherit;False;225;206;;0;Lerp 1;1,1,1,1;0;0
WireConnection;464;0;462;3
WireConnection;464;1;459;2
WireConnection;524;0;523;3
WireConnection;524;1;525;0
WireConnection;466;0;464;0
WireConnection;466;1;465;0
WireConnection;521;0;466;0
WireConnection;521;1;524;0
WireConnection;522;0;458;2
WireConnection;522;1;524;0
WireConnection;468;1;521;0
WireConnection;468;0;522;0
WireConnection;471;0;468;0
WireConnection;473;0;471;0
WireConnection;473;1;468;0
WireConnection;476;0;473;0
WireConnection;474;0;468;0
WireConnection;474;1;472;0
WireConnection;477;0;476;0
WireConnection;477;1;475;0
WireConnection;477;2;468;0
WireConnection;477;3;468;0
WireConnection;477;4;474;0
WireConnection;455;0;402;0
WireConnection;455;1;400;0
WireConnection;404;0;402;0
WireConnection;404;1;400;0
WireConnection;403;0;477;0
WireConnection;403;1;401;0
WireConnection;405;0;477;0
WireConnection;405;1;455;0
WireConnection;406;0;403;0
WireConnection;406;1;404;0
WireConnection;407;0;405;0
WireConnection;408;0;406;0
WireConnection;409;0;407;0
WireConnection;410;0;408;0
WireConnection;412;0;455;0
WireConnection;412;1;400;0
WireConnection;411;0;408;0
WireConnection;411;1;410;0
WireConnection;414;0;407;0
WireConnection;414;1;409;0
WireConnection;413;0;404;0
WireConnection;413;1;400;0
WireConnection;416;0;413;0
WireConnection;416;1;411;0
WireConnection;415;0;412;0
WireConnection;415;1;414;0
WireConnection;419;0;402;0
WireConnection;419;1;400;0
WireConnection;418;0;416;0
WireConnection;456;0;402;0
WireConnection;456;1;400;0
WireConnection;417;0;415;0
WireConnection;425;0;421;0
WireConnection;425;1;456;0
WireConnection;424;0;417;0
WireConnection;424;1;456;0
WireConnection;422;0;418;0
WireConnection;422;1;419;0
WireConnection;423;0;420;0
WireConnection;423;1;419;0
WireConnection;426;0;425;0
WireConnection;426;1;424;0
WireConnection;428;0;423;0
WireConnection;428;1;422;0
WireConnection;429;0;427;0
WireConnection;429;1;426;0
WireConnection;430;0;427;0
WireConnection;430;1;428;0
WireConnection;431;0;429;1
WireConnection;431;1;429;2
WireConnection;432;0;430;1
WireConnection;432;1;430;2
WireConnection;434;0;432;0
WireConnection;433;0;431;0
WireConnection;435;0;433;0
WireConnection;436;0;434;0
WireConnection;277;0;275;0
WireConnection;277;1;276;0
WireConnection;438;0;435;0
WireConnection;437;0;436;0
WireConnection;556;0;554;0
WireConnection;556;1;555;0
WireConnection;550;0;547;0
WireConnection;550;1;549;0
WireConnection;243;0;29;3
WireConnection;243;3;244;0
WireConnection;243;4;245;0
WireConnection;208;0;206;2
WireConnection;208;1;205;0
WireConnection;207;0;206;2
WireConnection;207;1;204;0
WireConnection;553;0;203;0
WireConnection;553;1;556;0
WireConnection;440;0;438;1
WireConnection;278;0;277;0
WireConnection;278;1;279;1
WireConnection;441;0;437;1
WireConnection;439;0;477;0
WireConnection;209;0;207;0
WireConnection;209;1;208;0
WireConnection;442;0;438;0
WireConnection;442;1;440;0
WireConnection;551;0;181;0
WireConnection;551;1;550;0
WireConnection;551;2;243;0
WireConnection;518;0;520;0
WireConnection;518;1;402;0
WireConnection;519;0;520;0
WireConnection;519;1;400;0
WireConnection;444;0;439;0
WireConnection;557;0;553;0
WireConnection;557;2;558;3
WireConnection;443;0;437;0
WireConnection;443;1;441;0
WireConnection;280;0;274;0
WireConnection;280;1;278;0
WireConnection;517;0;518;0
WireConnection;517;1;519;0
WireConnection;16;0;551;0
WireConnection;16;2;29;3
WireConnection;210;0;557;0
WireConnection;210;1;209;0
WireConnection;273;1;280;0
WireConnection;446;0;443;0
WireConnection;446;1;444;0
WireConnection;445;0;442;0
WireConnection;445;1;439;0
WireConnection;9;1;16;0
WireConnection;201;1;210;0
WireConnection;448;0;446;0
WireConnection;448;1;447;0
WireConnection;448;2;517;0
WireConnection;581;0;583;0
WireConnection;581;1;419;0
WireConnection;544;0;542;0
WireConnection;544;1;543;0
WireConnection;449;0;445;0
WireConnection;449;1;447;0
WireConnection;449;2;517;0
WireConnection;281;0;273;1
WireConnection;281;1;282;0
WireConnection;246;0;182;3
WireConnection;246;3;247;0
WireConnection;246;4;248;0
WireConnection;577;0;579;0
WireConnection;577;1;456;0
WireConnection;221;0;220;0
WireConnection;221;1;222;0
WireConnection;136;0;132;2
WireConnection;136;1;131;0
WireConnection;545;0;142;0
WireConnection;545;1;544;0
WireConnection;545;2;246;0
WireConnection;27;0;9;1
WireConnection;226;0;201;1
WireConnection;226;1;227;0
WireConnection;257;0;182;3
WireConnection;580;0;577;0
WireConnection;580;1;424;0
WireConnection;584;0;574;0
WireConnection;584;4;585;0
WireConnection;450;0;428;0
WireConnection;450;1;448;0
WireConnection;451;0;426;0
WireConnection;451;1;449;0
WireConnection;179;0;132;2
WireConnection;179;1;177;0
WireConnection;582;0;581;0
WireConnection;582;1;422;0
WireConnection;283;0;281;0
WireConnection;263;0;261;0
WireConnection;263;1;262;0
WireConnection;183;0;545;0
WireConnection;183;2;257;0
WireConnection;178;0;136;0
WireConnection;178;1;179;0
WireConnection;28;1;9;1
WireConnection;28;0;27;0
WireConnection;572;0;451;0
WireConnection;572;1;580;0
WireConnection;572;2;584;0
WireConnection;613;0;612;0
WireConnection;613;1;456;0
WireConnection;217;0;226;0
WireConnection;217;1;216;0
WireConnection;217;2;221;0
WireConnection;217;3;249;4
WireConnection;217;4;283;0
WireConnection;573;0;450;0
WireConnection;573;1;582;0
WireConnection;573;2;584;0
WireConnection;144;0;183;0
WireConnection;144;1;178;0
WireConnection;23;0;28;0
WireConnection;23;1;24;0
WireConnection;260;0;263;0
WireConnection;260;1;258;3
WireConnection;615;0;613;0
WireConnection;615;1;424;0
WireConnection;618;0;613;0
WireConnection;618;1;422;0
WireConnection;480;0;572;0
WireConnection;480;1;217;0
WireConnection;481;0;573;0
WireConnection;481;1;217;0
WireConnection;614;0;611;0
WireConnection;614;1;424;0
WireConnection;617;0;611;0
WireConnection;617;1;422;0
WireConnection;95;0;23;0
WireConnection;266;0;260;0
WireConnection;266;1;265;0
WireConnection;36;0;40;1
WireConnection;36;3;602;0
WireConnection;93;0;92;1
WireConnection;93;4;90;0
WireConnection;112;1;144;0
WireConnection;616;0;481;0
WireConnection;616;1;617;0
WireConnection;616;2;618;0
WireConnection;610;0;480;0
WireConnection;610;1;614;0
WireConnection;610;2;615;0
WireConnection;290;0;36;0
WireConnection;290;1;289;0
WireConnection;94;0;95;0
WireConnection;94;1;93;0
WireConnection;113;0;112;1
WireConnection;113;1;114;0
WireConnection;288;0;284;1
WireConnection;288;1;289;0
WireConnection;453;0;452;0
WireConnection;453;1;610;0
WireConnection;454;0;452;0
WireConnection;454;1;616;0
WireConnection;264;1;266;0
WireConnection;268;0;264;1
WireConnection;268;1;258;4
WireConnection;268;2;272;0
WireConnection;96;0;94;0
WireConnection;115;0;113;0
WireConnection;286;0;290;0
WireConnection;286;1;288;0
WireConnection;512;0;453;1
WireConnection;512;1;454;1
WireConnection;512;2;439;0
WireConnection;173;0;115;0
WireConnection;173;1;174;0
WireConnection;269;0;268;0
WireConnection;91;0;96;0
WireConnection;146;0;512;0
WireConnection;146;1;147;0
WireConnection;287;0;286;0
WireConnection;13;0;91;0
WireConnection;13;1;287;0
WireConnection;65;0;146;0
WireConnection;65;1;44;0
WireConnection;65;2;173;0
WireConnection;65;3;269;0
WireConnection;66;0;65;0
WireConnection;239;0;238;0
WireConnection;239;1;13;0
WireConnection;242;0;239;0
WireConnection;242;1;66;0
WireConnection;240;0;242;0
WireConnection;35;0;13;0
WireConnection;231;0;240;0
WireConnection;231;1;233;0
WireConnection;7;0;66;0
WireConnection;7;1;35;0
WireConnection;232;0;231;0
WireConnection;232;1;234;0
WireConnection;235;0;236;0
WireConnection;235;1;232;0
WireConnection;8;0;7;0
WireConnection;537;0;538;0
WireConnection;537;1;235;0
WireConnection;537;2;534;0
WireConnection;151;0;146;0
WireConnection;151;1;8;0
WireConnection;151;2;152;0
WireConnection;155;0;154;0
WireConnection;155;1;151;0
WireConnection;155;2;537;0
WireConnection;510;0;155;0
WireConnection;187;0;510;0
WireConnection;157;0;187;0
WireConnection;157;1;156;0
WireConnection;188;0;157;0
WireConnection;158;0;188;0
WireConnection;159;0;158;0
WireConnection;150;1;159;0
WireConnection;38;0;8;0
WireConnection;38;1;39;4
WireConnection;192;0;150;0
WireConnection;192;1;191;0
WireConnection;43;0;38;0
WireConnection;161;0;43;0
WireConnection;190;0;41;0
WireConnection;190;1;192;0
WireConnection;148;0;1;0
WireConnection;148;1;41;0
WireConnection;189;1;192;0
WireConnection;189;0;190;0
WireConnection;162;0;161;0
WireConnection;162;1;160;0
WireConnection;55;1;56;0
WireConnection;55;0;54;2
WireConnection;163;0;162;0
WireConnection;149;1;148;0
WireConnection;149;0;189;0
WireConnection;532;0;523;3
WireConnection;532;3;529;0
WireConnection;532;4;531;0
WireConnection;509;1;151;0
WireConnection;509;0;487;0
WireConnection;499;0;483;0
WireConnection;499;1;496;0
WireConnection;500;0;499;0
WireConnection;495;0;494;0
WireConnection;479;0;478;0
WireConnection;605;0;604;0
WireConnection;605;1;607;0
WireConnection;3;0;149;0
WireConnection;3;1;2;0
WireConnection;3;2;55;0
WireConnection;3;3;235;0
WireConnection;507;0;506;0
WireConnection;507;1;35;0
WireConnection;478;0;453;0
WireConnection;478;1;454;0
WireConnection;478;2;439;0
WireConnection;118;0;493;0
WireConnection;118;1;491;0
WireConnection;504;0;498;0
WireConnection;504;1;508;0
WireConnection;504;2;501;0
WireConnection;498;0;497;0
WireConnection;494;0;122;0
WireConnection;494;1;122;1
WireConnection;494;2;122;2
WireConnection;494;3;122;3
WireConnection;506;0;505;0
WireConnection;562;0;565;0
WireConnection;562;1;561;0
WireConnection;565;0;217;0
WireConnection;565;1;570;0
WireConnection;569;0;400;0
WireConnection;569;1;402;0
WireConnection;570;0;567;0
WireConnection;570;1;569;0
WireConnection;619;0;512;0
WireConnection;560;1;562;0
WireConnection;606;0;605;0
WireConnection;164;0;163;0
WireConnection;487;0;151;0
WireConnection;487;1;504;0
WireConnection;487;2;486;0
WireConnection;508;0;507;0
WireConnection;531;0;530;0
WireConnection;531;1;526;0
WireConnection;529;0;530;0
WireConnection;529;1;526;0
WireConnection;505;0;498;0
WireConnection;505;1;269;0
WireConnection;505;2;44;0
WireConnection;505;3;173;0
WireConnection;491;0;479;0
WireConnection;491;1;479;1
WireConnection;491;2;479;2
WireConnection;491;3;479;3
WireConnection;497;0;500;0
WireConnection;497;1;500;1
WireConnection;497;2;500;2
WireConnection;497;3;500;3
WireConnection;604;0;146;0
WireConnection;122;0;118;0
WireConnection;563;0;560;1
WireConnection;563;1;512;0
WireConnection;0;2;3;0
WireConnection;0;9;164;0
ASEEND*/
//CHKSM=9CF3ACEA381633CD99126E37F333ACAA639ED24F