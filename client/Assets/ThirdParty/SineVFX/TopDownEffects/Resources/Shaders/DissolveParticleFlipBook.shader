// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/DissolveParticleFlipBook"
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
		_FBMainMask("FB Main Mask", 2D) = "white" {}
		_FBMainMaskChannelsMultiply("FB Main Mask Channels Multiply", Vector) = (1,0,0,0)
		_FBMainMaskRotation("FB Main Mask Rotation", Range( 0 , 1)) = 0
		_FBMainMaskExp("FB Main Mask Exp", Range( 0.2 , 4)) = 1
		_FBRows("FB Rows", Float) = 8
		_FBColums("FB Colums", Float) = 8
		_FBAnimationSpeed("FB Animation Speed", Float) = 1
		_FBRandomFrame("FB Random Frame", Range( 0 , 1)) = 0
		[Toggle(_FBPARTICLEFRAMECONTROLENABLED_ON)] _FBParticleFrameControlEnabled("FB Particle Frame Control Enabled", Float) = 0
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
		_DistortionFixMask("Distortion Fix Mask", 2D) = "white" {}
		[Toggle(_EMISSIONVERTEXSTREAMENABLED_ON)] _EmissionVertexStreamEnabled("Emission Vertex Stream Enabled", Float) = 0
		[HideInInspector] _tex4coord3( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
		#pragma shader_feature _FBPARTICLEFRAMECONTROLENABLED_ON
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
		uniform sampler2D _DistortionFixMask;
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
		uniform float _FBRows;
		uniform float _FBColums;
		uniform sampler2D _FBMainMask;
		uniform float _FBAnimationSpeed;
		uniform float _FBRandomFrame;
		uniform float _FBMainMaskRotation;
		uniform float4 _FBMainMaskChannelsMultiply;
		uniform float _FBMainMaskExp;
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
			float2 uv0_Distortion = i.uv_texcoord * _Distortion_ST.xy + _Distortion_ST.zw;
			float2 appendResult556 = (float2(_DistortionScaleU , _DistortionScaleV));
			float2 appendResult209 = (float2(( _Time.y * _DistortionScrollSpeedU ) , ( _Time.y * _DistortionScrollSpeedV )));
			float2 appendResult221 = (float2(_DistortionU , _DistortionV));
			float2 uv0_DistortionMask = i.uv_texcoord * _DistortionMask_ST.xy + _DistortionMask_ST.zw;
			float2 appendResult277 = (float2(_DistortionMaskVSMoveU , _DistortionMaskVSMoveV));
			float clampResult283 = clamp( ( tex2D( _DistortionMask, ( uv0_DistortionMask + ( appendResult277 * i.uv3_tex4coord3.x ) ) ).r + _DistortionMaskNegate ) , 0.0 , 1.0 );
			float2 temp_output_217_0 = ( pow( tex2D( _Distortion, ( (( uv0_Distortion * appendResult556 )*1.0 + i.uv_tex4coord.z) + appendResult209 ) ).r , _DistortionExp ) * _DistortionPower * appendResult221 * i.uv_tex4coord.w * clampResult283 );
			float2 appendResult569 = (float2(_FBRows , _FBColums));
			float2 appendResult456 = (float2(_FBColums , _FBRows));
			float temp_output_455_0 = ( _FBColums * _FBRows );
			float2 appendResult412 = (float2(temp_output_455_0 , _FBRows));
			float temp_output_524_0 = ( i.uv_tex4coord.z * _FBRandomFrame );
			#ifdef _FBPARTICLEFRAMECONTROLENABLED_ON
				float staticSwitch468 = ( i.uv3_tex4coord3.y + temp_output_524_0 );
			#else
				float staticSwitch468 = ( ( ( i.uv_tex4coord.z + _Time.y ) * _FBAnimationSpeed ) + temp_output_524_0 );
			#endif
			float ifLocalVar477 = 0;
			if( abs( ( round( staticSwitch468 ) - staticSwitch468 ) ) >= 0.054 )
				ifLocalVar477 = staticSwitch468;
			else
				ifLocalVar477 = ( staticSwitch468 + 0.076 );
			float temp_output_407_0 = frac( ( ifLocalVar477 / temp_output_455_0 ) );
			float2 appendResult414 = (float2(temp_output_407_0 , ( 1.0 - temp_output_407_0 )));
			float2 temp_output_424_0 = ( floor( ( appendResult412 * appendResult414 ) ) / appendResult456 );
			float cos572 = cos( (0.0 + (_FBMainMaskRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0)) );
			float sin572 = sin( (0.0 + (_FBMainMaskRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0)) );
			float2 rotator572 = mul( ( ( i.uv_texcoord / appendResult456 ) + temp_output_424_0 ) - ( ( float2( 0.5,0.5 ) / appendResult456 ) + temp_output_424_0 ) , float2x2( cos572 , -sin572 , sin572 , cos572 )) + ( ( float2( 0.5,0.5 ) / appendResult456 ) + temp_output_424_0 );
			float4 tex2DNode453 = tex2D( _FBMainMask, ( rotator572 + temp_output_217_0 ) );
			float4 break613 = ( tex2DNode453 * _FBMainMaskChannelsMultiply );
			float clampResult614 = clamp( ( break613.r + break613.g + break613.b + break613.a ) , 0.0 , 1.0 );
			float temp_output_146_0 = pow( ( tex2D( _DistortionFixMask, ( ( temp_output_217_0 * ( float2( 1,1 ) * appendResult569 ) ) + i.uv_texcoord ) ).r * clampResult614 ) , _FBMainMaskExp );
			float2 uv0_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult544 = (float2(_Noise01ScaleU , _Noise01ScaleV));
			float2 appendResult178 = (float2(( _Time.y * _Noise01ScrollSpeedU ) , ( _Time.y * _Noise01ScrollSpeedV )));
			float clampResult115 = clamp( ( tex2D( _Noise01, ( (( uv0_Noise01 * appendResult544 * (_Noise01RandomMin + (i.uv_tex4coord.z - 0.0) * (_Noise01RandomMax - _Noise01RandomMin) / (120.0 - 0.0)) )*1.0 + ( i.uv_tex4coord.z * 1.4 )) + appendResult178 ) ).r + _Noise01Negate ) , 0.0 , 1.0 );
			float2 appendResult263 = (float2(_SecondaryMaskVSMoveU , _SecondayMaskVSMoveV));
			float2 uv0_SecondaryMask = i.uv_texcoord * _SecondaryMask_ST.xy + _SecondaryMask_ST.zw;
			float clampResult269 = clamp( ( tex2D( _SecondaryMask, ( ( appendResult263 * i.uv2_tex4coord2.z ) + uv0_SecondaryMask ) ).r + i.uv2_tex4coord2.w + _SecondaryMaskNegate ) , 0.0 , 1.0 );
			float clampResult66 = clamp( ( temp_output_146_0 * _FinalOpacityPower * pow( clampResult115 , _Noise01Exp ) * clampResult269 ) , 0.0 , 1.0 );
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
1920;0;1920;1018;6879.135;333.3984;1.098398;True;False
Node;AmplifyShaderEditor.CommentaryNode;609;-11578.64,-867.2494;Inherit;False;5632.583;1467.986;;45;462;459;464;523;525;465;466;458;524;521;522;468;471;472;473;402;476;474;475;400;477;455;405;407;409;414;412;415;456;421;579;417;585;577;574;424;425;426;584;580;572;452;480;453;612;FlipBook;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;459;-11511.09,-148.7835;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;462;-11517.23,-321.6763;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;523;-11469.5,114.46;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;525;-11519.58,291.2897;Inherit;False;Property;_FBRandomFrame;FB Random Frame;18;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;465;-11528.64,7.609047;Float;False;Property;_FBAnimationSpeed;FB Animation Speed;17;0;Create;True;0;0;False;0;False;1;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;464;-11252.61,-204.6496;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;524;-11226.58,227.2897;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;458;-11454.28,369.8658;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;466;-11080.45,-120.6571;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;521;-10924.65,-77.21078;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;522;-10945.45,234.7892;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;468;-10779.06,69.36995;Float;False;Property;_FBParticleFrameControlEnabled;FB Particle Frame Control Enabled;19;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;471;-10407.53,-99.41494;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;472;-10297.88,310.4069;Inherit;False;Constant;_Fix02;Fix 02;34;0;Create;True;0;0;False;0;False;0.076;0.0128;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;473;-10250.2,-45.46584;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;400;-9195.062,205.5009;Float;False;Property;_FBRows;FB Rows;15;0;Create;True;0;0;False;0;False;8;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;476;-10048.45,-83.65444;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;475;-10084.54,11.52903;Inherit;False;Constant;_Fix01;Fix 01;32;0;Create;True;0;0;False;0;False;0.054;0.0114;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;474;-10037.48,163.3595;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;402;-9214.326,-492.0804;Float;False;Property;_FBColums;FB Colums;16;0;Create;True;0;0;False;0;False;8;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;477;-9866.143,0.5620499;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;455;-8941.168,-272.3889;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;559;-9174.837,-4021.398;Inherit;False;2096.697;1802.816;;34;275;276;279;204;206;277;205;208;274;278;207;209;280;282;273;210;281;220;227;201;222;249;221;216;283;226;217;557;558;203;556;553;554;555;Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;405;-9092.84,-50.54574;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;275;-8637.024,-2595.762;Inherit;False;Property;_DistortionMaskVSMoveU;Distortion Mask VS Move U;61;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-8637.869,-2512.471;Inherit;False;Property;_DistortionMaskVSMoveV;Distortion Mask VS Move V;62;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;555;-9030.839,-3764.993;Inherit;False;Property;_DistortionScaleV;Distortion Scale V;52;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;554;-9030.839,-3846.992;Inherit;False;Property;_DistortionScaleU;Distortion Scale U;51;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;407;-8947.498,-51.20675;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;556;-8794.84,-3810.992;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;409;-8789.253,18.05854;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-9124.837,-3196.389;Inherit;False;Property;_DistortionScrollSpeedV;Distortion Scroll Speed V;58;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;279;-8407.128,-2420.582;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;204;-9116.37,-3276.03;Inherit;False;Property;_DistortionScrollSpeedU;Distortion Scroll Speed U;57;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;206;-9057.048,-3425.153;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;277;-8344.093,-2561.736;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;203;-9055.798,-3971.398;Inherit;False;0;201;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-8789.312,-3309.923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;412;-8611.499,-179.2068;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;558;-8679.584,-3718.453;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-8791.004,-3408.209;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;-8155.001,-2491.567;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;553;-8623.02,-3887.815;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;414;-8611.499,-51.20675;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;274;-8462.961,-2806.866;Inherit;False;0;273;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;557;-8374.529,-3769.549;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;415;-8467.5,-115.2067;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;295;-4312.587,3189.294;Inherit;False;3038.982;1538.66;;33;291;35;13;91;287;286;96;94;290;288;284;95;36;93;92;23;40;90;28;24;27;9;16;181;243;29;245;244;547;549;550;551;603;Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;209;-8616.466,-3359.067;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;280;-8093.116,-2652.034;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;282;-7929.735,-2629.386;Inherit;False;Property;_DistortionMaskNegate;Distortion Mask Negate;60;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-8087.773,-3379.401;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FloorOpNode;417;-8323.5,-115.2067;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;579;-7703.88,-785.8374;Inherit;False;Constant;_Vector1;Vector 1;70;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;273;-7956.918,-2832.129;Inherit;True;Property;_DistortionMask;Distortion Mask;59;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;456;-8611.499,-307.207;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;547;-4160.714,3420.244;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;34;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;421;-8468.676,-441.4471;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;549;-4162.714,3494.244;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;35;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;245;-4258.587,3949.993;Inherit;False;Property;_DissolveTextureRandomMax;Dissolve Texture Random Max;38;0;Create;True;0;0;False;0;False;1.1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-4228.392,3686.529;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;244;-4262.587,3869.993;Inherit;False;Property;_DissolveTextureRandomMin;Dissolve Texture Random Min;37;0;Create;True;0;0;False;0;False;0.9;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-7826.41,-3077.497;Inherit;False;Property;_DistortionU;Distortion U;55;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;585;-7483.553,91.83354;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;577;-7443.119,-817.2494;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;424;-8163.5,-211.2069;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;201;-7874.855,-3405.309;Inherit;True;Property;_Distortion;Distortion;50;0;Create;True;0;0;False;0;False;-1;None;0d57fcffb325773469c807d4f445589f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;227;-7866.547,-3532.465;Inherit;False;Property;_DistortionExp;Distortion Exp;54;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;281;-7640.859,-2714.639;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-7825.896,-2998.888;Inherit;False;Property;_DistortionV;Distortion V;56;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;243;-3911.39,3813.598;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;574;-7569.397,-4.931358;Inherit;False;Property;_FBMainMaskRotation;FB Main Mask Rotation;13;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;181;-4202.478,3257.343;Inherit;False;0;9;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;550;-3896.714,3452.244;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;425;-8163.5,-339.207;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;221;-7515.895,-3039.888;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-7642.198,-3194.18;Inherit;False;Property;_DistortionPower;Distortion Power;53;0;Create;True;0;0;False;0;False;-0.31;0.05;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;283;-7501.596,-2716.856;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;226;-7540.194,-3463.194;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;551;-3668.714,3385.244;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;580;-7239.023,-759.4335;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;584;-7246.508,-1.038353;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;426;-8003.5,-275.207;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;249;-7570.127,-2912.24;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;546;-3599.893,960.8034;Inherit;False;2159.318;1221.517;;24;248;247;182;246;132;131;136;179;257;183;178;144;114;112;113;174;115;173;544;142;545;542;543;177;Noise 01;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-7234.069,-3072.839;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;572;-6891.442,-383.0094;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;542;-3548.893,1179.61;Inherit;False;Property;_Noise01ScaleU;Noise 01 Scale U;25;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;182;-3401.599,1368.894;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;247;-3506.837,1572.797;Inherit;False;Property;_Noise01RandomMin;Noise 01 Random Min;31;0;Create;True;0;0;False;0;False;0.9;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-3504.837,1654.797;Inherit;False;Property;_Noise01RandomMax;Noise 01 Random Max;32;0;Create;True;0;0;False;0;False;1.1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;543;-3549.893,1258.61;Inherit;False;Property;_Noise01ScaleV;Noise 01 Scale V;26;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;16;-3487.373,3703.637;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-3141.526,2004.066;Inherit;False;Property;_Noise01ScrollSpeedU;Noise 01 Scroll Speed U;29;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-3412.404,1010.803;Inherit;False;0;112;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;296;-2983.588,117.0868;Inherit;False;1540.158;663.4843;;11;269;268;264;272;266;260;265;263;258;261;262;Secondary Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;571;-4913.34,-733.54;Inherit;False;1276.113;553.6541;;8;567;570;569;565;561;562;560;563;Distortion Fix;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;9;-3258.162,3678.127;Inherit;True;Property;_DissolveTexture;Dissolve Texture;33;0;Create;True;0;0;False;0;False;-1;None;34a3b7afc8c389e40bda6d478fb654e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;177;-3130.081,1931.934;Inherit;False;Property;_Noise01ScrollSpeedV;Noise 01 Scroll Speed V;30;0;Create;True;0;0;False;0;False;0;-0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;544;-3321.893,1206.61;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;246;-3168.838,1560.797;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;480;-6415.88,-419.7614;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TimeNode;132;-3107.275,1856.905;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;452;-6979.604,370.7367;Float;True;Property;_FBMainMask;FB Main Mask;11;0;Create;True;0;0;False;0;False;None;80ea46b40d7070a4d86f068c1b347cf0;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;453;-6267.059,107.8597;Inherit;True;Property;_TextureSample2;Texture Sample 2;14;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;612;-6270.231,306.1573;Inherit;False;Property;_FBMainMaskChannelsMultiply;FB Main Mask Channels Multiply;12;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;27;-2960.465,3854.312;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;569;-4855.603,-542.8633;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;-2852.394,2049.321;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;545;-2939.396,1112.35;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;262;-2919.464,281.9684;Inherit;False;Property;_SecondayMaskVSMoveV;Seconday Mask VS Move V;23;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-2933.588,167.0868;Inherit;False;Property;_SecondaryMaskVSMoveU;Secondary Mask VS Move U;22;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-2862.86,1929.234;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;-3128.925,1365.339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;567;-4863.34,-683.54;Inherit;False;Constant;_Vector0;Vector 0;69;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;263;-2628.706,234.8493;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;611;-5849.231,194.1573;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;258;-2868.937,389.7974;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;570;-4660.413,-586.1178;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;603;-2642.027,4276.277;Inherit;False;352;165;;1;602;If using Mask, set at "-2";1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;178;-2676.148,1978.82;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;183;-2716.299,1341.176;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2702.716,3821.418;Inherit;False;Property;_DissolveExp;Dissolve Exp;39;0;Create;True;0;0;False;0;False;6.47;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;28;-2841.465,3703.31;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;36;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;565;-4500.891,-646.4951;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-2397.213,365.0258;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;613;-5721.231,194.1573;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;144;-2466.911,1588.246;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;265;-2895.408,560.7791;Inherit;False;0;264;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;-2602.035,3433.338;Inherit;False;Property;_DissolveExpReversed;Dissolve Exp Reversed;40;0;Create;True;0;0;False;0;False;2;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;561;-4526.651,-453.8694;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;602;-2596.027,4331.277;Inherit;False;Property;_DissolveMaskRemapMin;Dissolve Mask Remap Min;46;0;Create;True;0;0;False;0;False;-1;-1;-2;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;92;-2553.708,3239.294;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;23;-2475.679,3730.362;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;291;-2144.32,4544.903;Inherit;False;307;165;;1;289;If mask is empty, keep it at 1f;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;40;-2223.491,4011.96;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;93;-2303.535,3266.37;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;112;-2305.655,1557.112;Inherit;True;Property;_Noise01;Noise 01;24;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;95;-2168.873,3516.401;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-2291.215,1756.249;Inherit;False;Property;_Noise01Negate;Noise 01 Negate;27;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;562;-4297.614,-530.5997;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;284;-2119.212,4270.872;Inherit;True;Property;_DissolveMask;Dissolve Mask;45;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;610;-5469.641,192.205;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;289;-2095.32,4610.903;Inherit;False;Property;_DissolveMaskScale;Dissolve Mask Scale;47;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;266;-2230.51,422.4701;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;36;-1993.347,4016.673;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;614;-5352.231,197.1573;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-1955.284,1647.529;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;272;-2059.833,665.571;Inherit;False;Property;_SecondaryMaskNegate;Secondary Mask Negate;21;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;264;-2053.126,383.3206;Inherit;True;Property;_SecondaryMask;Secondary Mask;20;0;Create;True;0;0;False;0;False;-1;None;724ef41e801f1e444a39a8a28816e31f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;560;-4145.019,-555.5602;Inherit;True;Property;_DistortionFixMask;Distortion Fix Mask;63;0;Create;True;0;0;False;0;False;-1;a9159bc94ccc1ad44b2359384686609a;a9159bc94ccc1ad44b2359384686609a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;290;-1772.114,4158.164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;94;-1997.159,3517.744;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;288;-1777.747,4375.007;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;286;-1600.854,4257.559;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;268;-1749.73,540.5187;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;115;-1815.532,1644.352;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;96;-1840.201,3520.427;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-1933.576,1784.643;Inherit;False;Property;_Noise01Exp;Noise 01 Exp;28;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-690.991,-174.1381;Inherit;False;Property;_FBMainMaskExp;FB Main Mask Exp;14;0;Create;True;0;0;False;0;False;1;1;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;563;-3806.228,-312.8861;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;91;-1780.529,3843.928;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1691.001,828.5112;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;173;-1620.576,1697.643;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;269;-1618.43,541.8188;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;146;-360.7907,-248.2382;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;287;-1448.017,4254.138;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;292;-1443.674,2565.81;Inherit;False;1378.789;361.9214;;10;235;236;232;234;231;240;233;242;239;238;Glowing Edges When Dissolved;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-198.7198,-90.45295;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;-1393.674,2615.81;Inherit;False;Property;_DissolveGlowOffset;Dissolve Glow Offset;43;0;Create;True;0;0;False;0;False;0.125;0.25;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1620.914,3919.683;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;239;-1118.003,2667.799;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;66;-48.97653,-88.99887;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;242;-993.16,2663.211;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;240;-833.9589,2655.147;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;-948.964,2781.714;Inherit;False;Property;_DissolveGlowPower;Dissolve Glow Power;42;0;Create;True;0;0;False;0;False;1;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;35;-1448.605,3923.387;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;231;-653.4669,2700.259;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;234;-680.7259,2812.732;Inherit;False;Property;_DissolveGlowAmount;Dissolve Glow Amount;41;0;Create;True;0;0;False;0;False;0;0;0;120;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;42.17837,484.8338;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;536;-924.0254,2234.113;Inherit;False;752.7615;296.6475;;3;537;538;534;Dissolve Glow Affects Ramp;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;236;-379.9129,2630.42;Inherit;False;Constant;_Float1;Float 1;49;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-368.0858,2713.049;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;235;-218.8838,2668.697;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;534;-892.2336,2396.377;Inherit;False;Property;_DissolveGlowAffectsRamp;Dissolve Glow Affects Ramp;44;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-3579.716,-902.5541;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;538;-762.3823,2310.023;Inherit;False;Constant;_Float6;Float 6;64;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;211.1785,487.7339;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-3327.895,-1034.334;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;9;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;151;-3264.305,-941.2837;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;537;-388.5846,2335.176;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-3034.5,-956.0848;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;510;-2880.354,-956.6819;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-2840.188,-787.6899;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;10;0;Create;True;0;0;False;0;False;1;4;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;187;-2731.791,-949.9424;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;-2579.57,-951.3734;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;188;-2429.891,-957.0424;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;158;-2278.025,-959.8229;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;39;204.9745,678.7123;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;159;-2111.833,-956.1522;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;150;-1962.946,-974.6906;Inherit;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;False;-1;None;244ec440ba2f7824dad8ce1e800567c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;468.7708,554.1188;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;191;-1879.15,-785.2544;Inherit;False;Property;_RampColorTint;Ramp Color Tint;6;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;43;625.1071,558.3958;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-1565.938,-879.1814;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;41;-1648.461,-1243.822;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;-1290.839,-1075.002;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;161;803.5771,565.9183;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;613.7361,703.9526;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;3;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-1676.163,-1409.776;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,0.7000555,0.4575472,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;189;-1006.149,-974.2191;Inherit;False;Property;_RampAffectedByVertexColor;Ramp Affected By Vertex Color;7;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1417.403,-1311.321;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;-643.7588,-1223.513;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-595.0848,-1316.655;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;162;956.6368,569.9461;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;163;1101.641,569.9461;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;55;-377.3068,-1254.09;Inherit;False;Property;_EmissionVertexStreamEnabled;Emission Vertex Stream Enabled;64;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;149;-618.728,-1030.229;Inherit;False;Property;_RampEnabled;Ramp Enabled;4;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-678.3,-650.3998;Inherit;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;4;15;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;608;-3588.564,2718.903;Inherit;False;886.4258;254.312;;4;605;607;606;604;Dissolve G From Mask (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;606;-3243.426,2789.749;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;103.6291,-694.3374;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;164;1260.071,563.2332;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;604;-3007.139,2808.397;Inherit;False;Property;_DissolveGFromMask;Dissolve G From Mask;48;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;607;-3538.564,2858.215;Inherit;False;Property;_DissolveGFromMaskExp;Dissolve G From Mask Exp;49;0;Create;True;0;0;False;0;False;4;4;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;605;-3433.542,2768.903;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1521.547,-32.13005;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/DissolveParticleFlipBook;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;539;-410.7907,-298.2382;Inherit;False;230;183;;0;Lerp 0;1,1,1,1;0;0
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
WireConnection;405;0;477;0
WireConnection;405;1;455;0
WireConnection;407;0;405;0
WireConnection;556;0;554;0
WireConnection;556;1;555;0
WireConnection;409;0;407;0
WireConnection;277;0;275;0
WireConnection;277;1;276;0
WireConnection;208;0;206;2
WireConnection;208;1;205;0
WireConnection;412;0;455;0
WireConnection;412;1;400;0
WireConnection;207;0;206;2
WireConnection;207;1;204;0
WireConnection;278;0;277;0
WireConnection;278;1;279;1
WireConnection;553;0;203;0
WireConnection;553;1;556;0
WireConnection;414;0;407;0
WireConnection;414;1;409;0
WireConnection;557;0;553;0
WireConnection;557;2;558;3
WireConnection;415;0;412;0
WireConnection;415;1;414;0
WireConnection;209;0;207;0
WireConnection;209;1;208;0
WireConnection;280;0;274;0
WireConnection;280;1;278;0
WireConnection;210;0;557;0
WireConnection;210;1;209;0
WireConnection;417;0;415;0
WireConnection;273;1;280;0
WireConnection;456;0;402;0
WireConnection;456;1;400;0
WireConnection;577;0;579;0
WireConnection;577;1;456;0
WireConnection;424;0;417;0
WireConnection;424;1;456;0
WireConnection;201;1;210;0
WireConnection;281;0;273;1
WireConnection;281;1;282;0
WireConnection;243;0;29;3
WireConnection;243;3;244;0
WireConnection;243;4;245;0
WireConnection;550;0;547;0
WireConnection;550;1;549;0
WireConnection;425;0;421;0
WireConnection;425;1;456;0
WireConnection;221;0;220;0
WireConnection;221;1;222;0
WireConnection;283;0;281;0
WireConnection;226;0;201;1
WireConnection;226;1;227;0
WireConnection;551;0;181;0
WireConnection;551;1;550;0
WireConnection;551;2;243;0
WireConnection;580;0;577;0
WireConnection;580;1;424;0
WireConnection;584;0;574;0
WireConnection;584;4;585;0
WireConnection;426;0;425;0
WireConnection;426;1;424;0
WireConnection;217;0;226;0
WireConnection;217;1;216;0
WireConnection;217;2;221;0
WireConnection;217;3;249;4
WireConnection;217;4;283;0
WireConnection;572;0;426;0
WireConnection;572;1;580;0
WireConnection;572;2;584;0
WireConnection;16;0;551;0
WireConnection;16;2;29;3
WireConnection;9;1;16;0
WireConnection;544;0;542;0
WireConnection;544;1;543;0
WireConnection;246;0;182;3
WireConnection;246;3;247;0
WireConnection;246;4;248;0
WireConnection;480;0;572;0
WireConnection;480;1;217;0
WireConnection;453;0;452;0
WireConnection;453;1;480;0
WireConnection;27;0;9;1
WireConnection;569;0;400;0
WireConnection;569;1;402;0
WireConnection;179;0;132;2
WireConnection;179;1;177;0
WireConnection;545;0;142;0
WireConnection;545;1;544;0
WireConnection;545;2;246;0
WireConnection;136;0;132;2
WireConnection;136;1;131;0
WireConnection;257;0;182;3
WireConnection;263;0;261;0
WireConnection;263;1;262;0
WireConnection;611;0;453;0
WireConnection;611;1;612;0
WireConnection;570;0;567;0
WireConnection;570;1;569;0
WireConnection;178;0;136;0
WireConnection;178;1;179;0
WireConnection;183;0;545;0
WireConnection;183;2;257;0
WireConnection;28;1;9;1
WireConnection;28;0;27;0
WireConnection;565;0;217;0
WireConnection;565;1;570;0
WireConnection;260;0;263;0
WireConnection;260;1;258;3
WireConnection;613;0;611;0
WireConnection;144;0;183;0
WireConnection;144;1;178;0
WireConnection;23;0;28;0
WireConnection;23;1;24;0
WireConnection;93;0;92;1
WireConnection;93;4;90;0
WireConnection;112;1;144;0
WireConnection;95;0;23;0
WireConnection;562;0;565;0
WireConnection;562;1;561;0
WireConnection;610;0;613;0
WireConnection;610;1;613;1
WireConnection;610;2;613;2
WireConnection;610;3;613;3
WireConnection;266;0;260;0
WireConnection;266;1;265;0
WireConnection;36;0;40;1
WireConnection;36;3;602;0
WireConnection;614;0;610;0
WireConnection;113;0;112;1
WireConnection;113;1;114;0
WireConnection;264;1;266;0
WireConnection;560;1;562;0
WireConnection;290;0;36;0
WireConnection;290;1;289;0
WireConnection;94;0;95;0
WireConnection;94;1;93;0
WireConnection;288;0;284;1
WireConnection;288;1;289;0
WireConnection;286;0;290;0
WireConnection;286;1;288;0
WireConnection;268;0;264;1
WireConnection;268;1;258;4
WireConnection;268;2;272;0
WireConnection;115;0;113;0
WireConnection;96;0;94;0
WireConnection;563;0;560;1
WireConnection;563;1;614;0
WireConnection;91;0;96;0
WireConnection;173;0;115;0
WireConnection;173;1;174;0
WireConnection;269;0;268;0
WireConnection;146;0;563;0
WireConnection;146;1;147;0
WireConnection;287;0;286;0
WireConnection;65;0;146;0
WireConnection;65;1;44;0
WireConnection;65;2;173;0
WireConnection;65;3;269;0
WireConnection;13;0;91;0
WireConnection;13;1;287;0
WireConnection;239;0;238;0
WireConnection;239;1;13;0
WireConnection;66;0;65;0
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
WireConnection;151;0;146;0
WireConnection;151;1;8;0
WireConnection;151;2;152;0
WireConnection;537;0;538;0
WireConnection;537;1;235;0
WireConnection;537;2;534;0
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
WireConnection;43;0;38;0
WireConnection;192;0;150;0
WireConnection;192;1;191;0
WireConnection;190;0;41;0
WireConnection;190;1;192;0
WireConnection;161;0;43;0
WireConnection;189;1;192;0
WireConnection;189;0;190;0
WireConnection;148;0;1;0
WireConnection;148;1;41;0
WireConnection;162;0;161;0
WireConnection;162;1;160;0
WireConnection;163;0;162;0
WireConnection;55;1;56;0
WireConnection;55;0;54;2
WireConnection;149;1;148;0
WireConnection;149;0;189;0
WireConnection;606;0;605;0
WireConnection;606;1;607;0
WireConnection;3;0;149;0
WireConnection;3;1;2;0
WireConnection;3;2;55;0
WireConnection;3;3;235;0
WireConnection;164;0;163;0
WireConnection;604;1;9;1
WireConnection;604;0;606;0
WireConnection;605;0;453;2
WireConnection;0;2;3;0
WireConnection;0;9;164;0
ASEEND*/
//CHKSM=1124A881615097C71E2BAA2844AC614800C91EF5