// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/DissolveParticleAdvanced"
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
		_RampAffectedByDynamics("Ramp Affected By Dynamics", Range( 0 , 1)) = 1
		_RampOffsetMultiply("Ramp Offset Multiply", Float) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_RampIgnoreVertexColor("Ramp Ignore Vertex Color", Range( 0 , 1)) = 0
		_CustomColorMask("Custom Color Mask", 2D) = "white" {}
		_CustomColorMaskChannels("Custom Color Mask Channels", Vector) = (1,0,0,0)
		_CustomColorMaskSwitch("Custom Color Mask Switch", Range( 0 , 1)) = 0
		_CustomColorMaskAffectedByDynamics("Custom Color Mask Affected By Dynamics", Range( 0 , 1)) = 1
		_MainMask("Main Mask", 2D) = "white" {}
		_MainMaskScaleU("Main Mask Scale U", Float) = 1
		_MainMaskScaleV("Main Mask Scale V", Float) = 1
		_MainMaskChannelsMultiply("Main Mask Channels Multiply", Vector) = (1,0,0,0)
		_MainMaskScrollSpeedU("Main Mask Scroll Speed U", Float) = 0
		_MainMaskScrollSpeedV("Main Mask Scroll Speed V", Float) = 0
		_MainMaskExp("Main Mask Exp", Range( 0.2 , 4)) = 1
		_SecondMask("Second Mask", 2D) = "white" {}
		_SecondMaskScaleU("Second Mask Scale U", Float) = 1
		_SecondMaskScaleV("Second Mask Scale V", Float) = 1
		_SecondMaskProfile("Second Mask Profile", 2D) = "white" {}
		_SecondMaskNegate("Second Mask Negate", Range( 0 , 1)) = 1
		_SecondMaskVSMoveU("Second Mask VS Move U", Range( 0 , 1)) = 0
		_SecondMaskVSMoveV("Second Mask VS Move V", Range( 0 , 1)) = 0
		_SecondMaskAffectsDistortion("Second Mask Affects Distortion", Range( 0 , 1)) = 0
		_SecondMaskAffectsNoise01Negate("Second Mask Affects Noise 01 Negate", Range( 0 , 1)) = 0
		_SecondMaskAffectsRamp("Second Mask Affects Ramp", Range( 0 , 1)) = 0
		_SecondMaskBoostsEmission("Second Mask Boosts Emission", Range( 0 , 40)) = 0
		_SecondMaskFractSwitch("Second Mask Fract Switch", Range( 0 , 1)) = 0
		_SecondMaskFractShrink("Second Mask Fract Shrink", Float) = 1
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScaleU("Noise 01 Scale U", Float) = 1
		_Noise01ScaleV("Noise 01 Scale V", Float) = 1
		_Noise01Negate("Noise 01 Negate", Range( 0 , 1)) = 0
		_Noise01Exp("Noise 01 Exp", Range( 0.2 , 8)) = 1
		_Noise01ScrollSpeedU("Noise 01 Scroll Speed U", Float) = 0
		_Noise01ScrollSpeedV("Noise 01 Scroll Speed V", Float) = 0
		_Noise01RandomMin("Noise 01 Random Min", Range( 0.5 , 1)) = 0.9
		_Noise01RandomMax("Noise 01 Random Max", Range( 1 , 1.5)) = 1.1
		[Toggle(_NOISE01RADIAL_ON)] _Noise01Radial("Noise 01 Radial", Float) = 0
		_Noise01RadialScaleU("Noise 01 Radial Scale U", Float) = 1
		_Noise01RadialScaleV("Noise 01 Radial Scale V", Float) = 1
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_DissolveTextureFlipSwitch("Dissolve Texture Flip Switch", Range( 0 , 1)) = 1
		_DissolveTextureScaleU("Dissolve Texture Scale U", Float) = 1
		_DissolveTextureScaleV("Dissolve Texture Scale V", Float) = 1
		_DissolveTextureRandomMin("Dissolve Texture Random Min", Range( 0.5 , 1)) = 0.9
		_DissolveTextureRandomMax("Dissolve Texture Random Max", Range( 1 , 1.5)) = 1.1
		[Toggle(_DISSOLVETEXTURERADIAL_ON)] _DissolveTextureRadial("Dissolve Texture Radial", Float) = 0
		_DissolveTextureRadialScaleU("Dissolve Texture Radial Scale U", Float) = 1
		_DissolveTextureRadialScaleV("Dissolve Texture Radial Scale V", Float) = 1
		_DissolveExp("Dissolve Exp", Float) = 2
		_DissolveExpReversed("Dissolve Exp Reversed", Float) = 2
		_DissolveGlowAmount("Dissolve Glow Amount", Range( 0 , 120)) = 0
		_DissolveGlowPower("Dissolve Glow Power", Range( 0.2 , 8)) = 1
		_DissolveGlowOffset("Dissolve Glow Offset", Range( -1 , 1)) = 0.125
		_DissolveGlowAffectsRamp("Dissolve Glow Affects Ramp", Range( 0 , 1)) = 1
		_DissolveMask("Dissolve Mask", 2D) = "black" {}
		_DissolveMaskSteepness("Dissolve Mask Steepness", Range( 0 , 10)) = 0
		[Toggle(_DISTORTIONENABLED_ON)] _DistortionEnabled("Distortion Enabled", Float) = 1
		_Distortion("Distortion", 2D) = "white" {}
		_DistortionScaleU("Distortion Scale U", Float) = 1
		_DistortionScaleV("Distortion Scale V", Float) = 1
		_DistortionPower("Distortion Power", Range( -1 , 1)) = 0
		_DistortionRemapMin("Distortion Remap Min", Range( -1 , 0)) = 0
		_DistortionRemapMax("Distortion Remap Max", Range( 1 , 2)) = 1
		_DistortionExp("Distortion Exp", Range( 0.2 , 8)) = 1
		[Toggle(_DISTORTIONUVORSPHERICAL_ON)] _DistortionUVorSpherical("Distortion UV or Spherical", Float) = 0
		_DistortionSpheticalStyle("Distortion Sphetical Style", Range( 0 , 1)) = 0
		_DistortionU("Distortion U", Range( 0 , 1)) = 1
		_DistortionV("Distortion V", Range( 0 , 1)) = 1
		_DistortionScrollSpeedU("Distortion Scroll Speed U", Float) = 0
		_DistortionScrollSpeedV("Distortion Scroll Speed V", Float) = 0
		[Toggle(_DISTORTIONRADIAL_ON)] _DistortionRadial("Distortion Radial", Float) = 0
		_DistortionRadialScaleU("Distortion Radial Scale U", Float) = 1
		_DistortionRadialScaleV("Distortion Radial Scale V", Float) = 1
		_DistortionRadialOldMethodSwitch("Distortion Radial Old Method Switch", Range( 0 , 1)) = 0
		_DistortionMask("Distortion Mask", 2D) = "white" {}
		_DistortionMaskNegate("Distortion Mask Negate", Range( 0 , 1)) = 0
		_DistortionMaskVSMoveU("Distortion Mask VS Move U", Range( 0 , 1)) = 0
		_DistortionMaskVSMoveV("Distortion Mask VS Move V", Range( 0 , 1)) = 0
		[Toggle(_EMISSIONVERTEXSTREAMENABLED_ON)] _EmissionVertexStreamEnabled("Emission Vertex Stream Enabled", Float) = 0
		[Toggle(_SOFTPARTICLESENABLED_ON)] _SoftParticlesEnabled("Soft Particles Enabled", Float) = 0
		_SoftParticlesDistance("Soft Particles Distance", Float) = 0.2
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
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _RAMPENABLED_ON
		#pragma shader_feature _DISTORTIONENABLED_ON
		#pragma shader_feature _DISTORTIONRADIAL_ON
		#pragma shader_feature _DISTORTIONUVORSPHERICAL_ON
		#pragma shader_feature _NOISE01RADIAL_ON
		#pragma shader_feature _DISSOLVETEXTURERADIAL_ON
		#pragma shader_feature _EMISSIONVERTEXSTREAMENABLED_ON
		#pragma shader_feature _SOFTPARTICLESENABLED_ON
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
			float4 screenPos;
		};

		uniform float4 _FinalColor;
		uniform float _RampIgnoreVertexColor;
		uniform sampler2D _Ramp;
		uniform float _RampOffsetMultiply;
		uniform float4 _MainMaskChannelsMultiply;
		uniform sampler2D _MainMask;
		uniform sampler2D _Distortion;
		uniform float4 _Distortion_ST;
		uniform float _DistortionScaleU;
		uniform float _DistortionScaleV;
		uniform float _DistortionScrollSpeedU;
		uniform float _DistortionScrollSpeedV;
		uniform float _DistortionRadialScaleU;
		uniform float _DistortionRadialScaleV;
		uniform float _DistortionRadialOldMethodSwitch;
		uniform float _DistortionExp;
		uniform float _DistortionRemapMin;
		uniform float _DistortionRemapMax;
		uniform float _DistortionPower;
		uniform float _DistortionU;
		uniform float _DistortionV;
		uniform float _DistortionSpheticalStyle;
		uniform sampler2D _DistortionMask;
		uniform float4 _DistortionMask_ST;
		uniform float _DistortionMaskVSMoveU;
		uniform float _DistortionMaskVSMoveV;
		uniform float _DistortionMaskNegate;
		uniform sampler2D _SecondMaskProfile;
		uniform float _SecondMaskNegate;
		uniform float _SecondMaskFractShrink;
		uniform sampler2D _SecondMask;
		uniform float _SecondMaskVSMoveU;
		uniform float _SecondMaskVSMoveV;
		uniform float4 _SecondMask_ST;
		uniform float _SecondMaskScaleU;
		uniform float _SecondMaskScaleV;
		uniform float _SecondMaskFractSwitch;
		uniform float _SecondMaskAffectsDistortion;
		uniform float _MainMaskScaleU;
		uniform float _MainMaskScaleV;
		uniform float4 _MainMask_ST;
		uniform float _MainMaskScrollSpeedU;
		uniform float _MainMaskScrollSpeedV;
		uniform float _MainMaskExp;
		uniform float _FinalOpacityPower;
		uniform sampler2D _Noise01;
		uniform float4 _Noise01_ST;
		uniform float _Noise01ScaleU;
		uniform float _Noise01ScaleV;
		uniform float _Noise01RandomMin;
		uniform float _Noise01RandomMax;
		uniform float _Noise01ScrollSpeedU;
		uniform float _Noise01ScrollSpeedV;
		uniform float _Noise01RadialScaleU;
		uniform float _Noise01RadialScaleV;
		uniform float _Noise01Negate;
		uniform float _SecondMaskAffectsNoise01Negate;
		uniform float _Noise01Exp;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _DissolveTextureScaleU;
		uniform float _DissolveTextureScaleV;
		uniform float _DissolveTextureRandomMin;
		uniform float _DissolveTextureRandomMax;
		uniform float _DissolveTextureRadialScaleU;
		uniform float _DissolveTextureRadialScaleV;
		uniform float _DissolveTextureFlipSwitch;
		uniform float _DissolveExp;
		uniform float _DissolveExpReversed;
		uniform float _DissolveMaskSteepness;
		uniform sampler2D _DissolveMask;
		uniform float4 _DissolveMask_ST;
		uniform float _RampAffectedByDynamics;
		uniform sampler2D _CustomColorMask;
		uniform float4 _CustomColorMask_ST;
		uniform float4 _CustomColorMaskChannels;
		uniform float _CustomColorMaskAffectedByDynamics;
		uniform float _CustomColorMaskSwitch;
		uniform float _DissolveGlowOffset;
		uniform float _DissolveGlowPower;
		uniform float _DissolveGlowAmount;
		uniform float _DissolveGlowAffectsRamp;
		uniform float _RampOffsetExp;
		uniform float _SecondMaskAffectsRamp;
		uniform float4 _RampColorTint;
		uniform float _FinalPower;
		uniform float _SecondMaskBoostsEmission;
		uniform float _FinalOpacityExp;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _SoftParticlesDistance;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 clampResult431 = clamp( ( i.vertexColor + _RampIgnoreVertexColor ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float2 uv0_Distortion = i.uv_texcoord * _Distortion_ST.xy + _Distortion_ST.zw;
			float2 appendResult351 = (float2(_DistortionScaleU , _DistortionScaleV));
			float2 appendResult209 = (float2(( _Time.y * _DistortionScrollSpeedU ) , ( _Time.y * _DistortionScrollSpeedV )));
			float2 temp_output_210_0 = ( ( uv0_Distortion * appendResult351 ) + appendResult209 + i.uv_tex4coord.z );
			float2 temp_output_19_0 = (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float temp_output_58_0 = length( temp_output_19_0 );
			float2 break195 = temp_output_19_0;
			float2 appendResult199 = (float2(( temp_output_58_0 * _DistortionRadialScaleU ) , ( _DistortionRadialScaleV * (0.0 + (atan2( break195.x , break195.y ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI ))) )));
			float2 temp_output_211_0 = ( appendResult209 + appendResult199 );
			float2 normalizeResult57 = normalize( temp_output_19_0 );
			float2 lerpResult333 = lerp( temp_output_211_0 , normalizeResult57 , round( _DistortionRadialOldMethodSwitch ));
			#ifdef _DISTORTIONRADIAL_ON
				float2 staticSwitch202 = lerpResult333;
			#else
				float2 staticSwitch202 = temp_output_210_0;
			#endif
			float2 appendResult221 = (float2(_DistortionU , _DistortionV));
			float2 lerpResult383 = lerp( ( temp_output_58_0 * normalizeResult57 ) , normalizeResult57 , _DistortionSpheticalStyle);
			#ifdef _DISTORTIONUVORSPHERICAL_ON
				float2 staticSwitch218 = lerpResult383;
			#else
				float2 staticSwitch218 = appendResult221;
			#endif
			float2 uv0_DistortionMask = i.uv_texcoord * _DistortionMask_ST.xy + _DistortionMask_ST.zw;
			float2 appendResult277 = (float2(_DistortionMaskVSMoveU , _DistortionMaskVSMoveV));
			float clampResult283 = clamp( ( tex2D( _DistortionMask, ( uv0_DistortionMask + ( appendResult277 * i.uv3_tex4coord3.x ) ) ).r + _DistortionMaskNegate ) , 0.0 , 1.0 );
			float2 appendResult263 = (float2(_SecondMaskVSMoveU , _SecondMaskVSMoveV));
			float2 uv0_SecondMask = i.uv_texcoord * _SecondMask_ST.xy + _SecondMask_ST.zw;
			float2 appendResult412 = (float2(_SecondMaskScaleU , _SecondMaskScaleV));
			float temp_output_268_0 = ( ( _SecondMaskFractShrink * tex2D( _SecondMask, ( ( appendResult263 * i.uv2_tex4coord2.z ) + ( uv0_SecondMask * appendResult412 ) ) ).r ) + i.uv2_tex4coord2.w );
			float lerpResult396 = lerp( temp_output_268_0 , frac( temp_output_268_0 ) , round( _SecondMaskFractSwitch ));
			float clampResult269 = clamp( ( _SecondMaskNegate + lerpResult396 ) , 0.0 , 1.0 );
			float2 appendResult366 = (float2(clampResult269 , 0.0));
			float4 tex2DNode365 = tex2D( _SecondMaskProfile, appendResult366 );
			float lerpResult352 = lerp( clampResult283 , tex2DNode365.r , round( _SecondMaskAffectsDistortion ));
			#ifdef _DISTORTIONENABLED_ON
				float2 staticSwitch403 = ( (_DistortionRemapMin + (pow( tex2D( _Distortion, staticSwitch202 ).r , _DistortionExp ) - 0.0) * (_DistortionRemapMax - _DistortionRemapMin) / (1.0 - 0.0)) * _DistortionPower * staticSwitch218 * i.uv_tex4coord.w * lerpResult352 );
			#else
				float2 staticSwitch403 = float2( 0,0 );
			#endif
			float2 appendResult454 = (float2(_MainMaskScaleU , _MainMaskScaleV));
			float2 uv0_MainMask = i.uv_texcoord * _MainMask_ST.xy + _MainMask_ST.zw;
			float2 appendResult256 = (float2(( _Time.y * _MainMaskScrollSpeedU ) , ( _Time.y * _MainMaskScrollSpeedV )));
			float4 tex2DNode4 = tex2D( _MainMask, ( staticSwitch403 + ( ( appendResult454 * uv0_MainMask ) + appendResult256 ) ) );
			float4 appendResult298 = (float4(tex2DNode4.r , tex2DNode4.g , tex2DNode4.b , tex2DNode4.a));
			float4 clampResult292 = clamp( ( _MainMaskChannelsMultiply * appendResult298 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 break122 = clampResult292;
			float clampResult301 = clamp( ( break122.x + break122.y + break122.z + break122.w ) , 0.0 , 1.0 );
			float temp_output_146_0 = pow( clampResult301 , _MainMaskExp );
			float2 uv0_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult340 = (float2(_Noise01ScaleU , _Noise01ScaleV));
			float temp_output_246_0 = (_Noise01RandomMin + (i.uv_tex4coord.z - 0.0) * (_Noise01RandomMax - _Noise01RandomMin) / (120.0 - 0.0));
			float2 appendResult178 = (float2(( _Time.y * _Noise01ScrollSpeedU ) , ( _Time.y * _Noise01ScrollSpeedV )));
			float2 temp_output_127_0 = (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float2 break128 = temp_output_127_0;
			float2 appendResult143 = (float2(( length( temp_output_127_0 ) * _Noise01RadialScaleU ) , ( (0.0 + (atan2( break128.x , break128.y ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI ))) * _Noise01RadialScaleV )));
			#ifdef _NOISE01RADIAL_ON
				float2 staticSwitch145 = ( appendResult143 + appendResult178 + i.uv_tex4coord.z );
			#else
				float2 staticSwitch145 = ( (( uv0_Noise01 * appendResult340 * temp_output_246_0 )*1.0 + ( i.uv_tex4coord.z * 1.4 )) + appendResult178 );
			#endif
			float lerpResult357 = lerp( 0.0 , tex2DNode365.r , _SecondMaskAffectsNoise01Negate);
			float clampResult115 = clamp( ( tex2D( _Noise01, staticSwitch145 ).r + _Noise01Negate + lerpResult357 ) , 0.0 , 1.0 );
			float temp_output_173_0 = pow( clampResult115 , _Noise01Exp );
			float clampResult66 = clamp( ( temp_output_146_0 * _FinalOpacityPower * temp_output_173_0 * tex2DNode365.g ) , 0.0 , 1.0 );
			float2 uv0_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			float2 appendResult344 = (float2(_DissolveTextureScaleU , _DissolveTextureScaleV));
			float2 temp_output_98_0 = (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float2 break102 = temp_output_98_0;
			float2 appendResult107 = (float2(( length( temp_output_98_0 ) * _DissolveTextureRadialScaleU ) , ( (0.0 + (atan2( break102.x , break102.y ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI ))) * _DissolveTextureRadialScaleV )));
			#ifdef _DISSOLVETEXTURERADIAL_ON
				float2 staticSwitch97 = appendResult107;
			#else
				float2 staticSwitch97 = (( uv0_DissolveTexture * appendResult344 * (_DissolveTextureRandomMin + (i.uv_tex4coord.z - 0.0) * (_DissolveTextureRandomMax - _DissolveTextureRandomMin) / (120.0 - 0.0)) )*1.0 + i.uv_tex4coord.z);
			#endif
			float4 tex2DNode9 = tex2D( _DissolveTexture, staticSwitch97 );
			float temp_output_27_0 = ( 1.0 - tex2DNode9.r );
			float lerpResult423 = lerp( tex2DNode9.r , temp_output_27_0 , round( _DissolveTextureFlipSwitch ));
			float clampResult426 = clamp( lerpResult423 , 0.0 , 1.0 );
			float clampResult91 = clamp( ( 1.0 - pow( ( 1.0 - pow( clampResult426 , _DissolveExp ) ) , (1.0 + (i.uv2_tex4coord2.x - 0.0) * (_DissolveExpReversed - 1.0) / (1.0 - 0.0)) ) ) , 0.0 , 1.0 );
			float temp_output_389_0 = ( -1.0 - _DissolveMaskSteepness );
			float temp_output_36_0 = (temp_output_389_0 + (i.uv2_tex4coord2.x - 0.0) * (-temp_output_389_0 - temp_output_389_0) / (1.0 - 0.0));
			float2 uv_DissolveMask = i.uv_texcoord * _DissolveMask_ST.xy + _DissolveMask_ST.zw;
			float clampResult287 = clamp( ( temp_output_36_0 + (-_DissolveMaskSteepness + (tex2D( _DissolveMask, uv_DissolveMask ).r - 0.0) * (_DissolveMaskSteepness - -_DissolveMaskSteepness) / (1.0 - 0.0)) ) , -1.0 , 1.0 );
			float temp_output_13_0 = ( clampResult91 + clampResult287 );
			float clampResult35 = clamp( temp_output_13_0 , 0.0 , 1.0 );
			float clampResult8 = clamp( ( clampResult66 - clampResult35 ) , 0.0 , 1.0 );
			float lerpResult151 = lerp( temp_output_146_0 , clampResult8 , _RampAffectedByDynamics);
			float2 uv_CustomColorMask = i.uv_texcoord * _CustomColorMask_ST.xy + _CustomColorMask_ST.zw;
			float4 break323 = ( tex2D( _CustomColorMask, uv_CustomColorMask ) * _CustomColorMaskChannels );
			float clampResult326 = clamp( ( break323.r + break323.g + break323.b + break323.a ) , 0.0 , 1.0 );
			float clampResult327 = clamp( ( clampResult326 * temp_output_173_0 * tex2DNode365.g * _FinalOpacityPower ) , 0.0 , 1.0 );
			float clampResult318 = clamp( ( clampResult327 - clampResult35 ) , 0.0 , 1.0 );
			float lerpResult325 = lerp( clampResult326 , clampResult318 , _CustomColorMaskAffectedByDynamics);
			float lerpResult320 = lerp( lerpResult151 , lerpResult325 , _CustomColorMaskSwitch);
			float clampResult240 = clamp( ( ( _DissolveGlowOffset + temp_output_13_0 ) - clampResult66 ) , 0.0 , 1.0 );
			float temp_output_235_0 = ( 1.0 + ( pow( clampResult240 , _DissolveGlowPower ) * _DissolveGlowAmount ) );
			float lerpResult314 = lerp( 1.0 , temp_output_235_0 , _DissolveGlowAffectsRamp);
			float clampResult414 = clamp( ( _RampOffsetMultiply * lerpResult320 * lerpResult314 ) , 0.0 , 1.0 );
			float clampResult158 = clamp( ( ( 1.0 - pow( ( 1.0 - clampResult414 ) , _RampOffsetExp ) ) + ( _SecondMaskAffectsRamp * tex2DNode365.r ) ) , 0.0 , 1.0 );
			float2 appendResult159 = (float2(clampResult158 , 0.0));
			float4 temp_output_192_0 = ( tex2D( _Ramp, appendResult159 ) * _RampColorTint );
			float4 temp_output_190_0 = ( clampResult431 * temp_output_192_0 );
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch149 = temp_output_190_0;
			#else
				float4 staticSwitch149 = ( _FinalColor * i.vertexColor );
			#endif
			#ifdef _EMISSIONVERTEXSTREAMENABLED_ON
				float staticSwitch55 = i.uv2_tex4coord2.y;
			#else
				float staticSwitch55 = 1.0;
			#endif
			o.Emission = ( staticSwitch149 * _FinalPower * staticSwitch55 * temp_output_235_0 * ( ( tex2DNode365.r * _SecondMaskBoostsEmission ) + 1.0 ) ).rgb;
			float clampResult43 = clamp( ( clampResult8 * i.vertexColor.a ) , 0.0 , 1.0 );
			float clampResult164 = clamp( ( 1.0 - pow( ( 1.0 - clampResult43 ) , _FinalOpacityExp ) ) , 0.0 , 1.0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth434 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth434 = abs( ( screenDepth434 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _SoftParticlesDistance ) );
			float clampResult450 = clamp( distanceDepth434 , 0.0 , 1.0 );
			#ifdef _SOFTPARTICLESENABLED_ON
				float staticSwitch448 = clampResult450;
			#else
				float staticSwitch448 = 1.0;
			#endif
			float clampResult447 = clamp( ( clampResult164 * staticSwitch448 ) , 0.0 , 1.0 );
			o.Alpha = clampResult447;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;7473.188;496.5515;1.498824;True;False
Node;AmplifyShaderEditor.CommentaryNode;305;-2518.603,3739.392;Inherit;False;1962.238;702.8895;;21;396;397;269;268;399;398;272;264;266;265;260;263;258;262;261;400;402;401;410;411;412;Second Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;303;-7856.203,-3395.007;Inherit;False;4155.188;2546.407;;68;293;203;210;335;217;283;216;249;294;218;281;296;295;219;221;226;201;282;273;227;222;220;280;202;274;333;278;279;332;57;277;211;331;276;209;275;199;215;207;214;208;205;212;196;58;213;204;206;198;194;197;195;19;18;351;350;348;349;352;353;354;355;384;383;405;416;417;418;Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;410;-2446.558,4277.628;Inherit;False;Property;_SecondMaskScaleU;Second Mask Scale U;25;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;262;-2465.155,3868.69;Inherit;False;Property;_SecondMaskVSMoveV;Second Mask VS Move V;30;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;411;-2445.377,4357.904;Inherit;False;Property;_SecondMaskScaleV;Second Mask Scale V;26;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-2468.603,3789.392;Inherit;False;Property;_SecondMaskVSMoveU;Second Mask VS Move U;29;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;412;-2196.284,4297.697;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;258;-2414.629,3976.52;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;263;-2174.398,3821.572;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;18;-7725.208,-2253.206;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;265;-2441.1,4147.502;Inherit;False;0;264;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;413;-1999.135,4213.879;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-1942.904,3951.747;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-7502.91,-2251.906;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;195;-7293.519,-2035.337;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;266;-1776.201,4009.192;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;197;-7099.789,-2190.759;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;198;-7099.789,-2110.86;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;402;-1626.079,3828.569;Inherit;False;Property;_SecondMaskFractShrink;Second Mask Fract Shrink;36;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;264;-1633.097,3983.226;Inherit;True;Property;_SecondMask;Second Mask;24;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ATan2OpNode;194;-7048.348,-2037.526;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;196;-6865.559,-2130.558;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;58;-7104.782,-2457.588;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-7451.172,-2778.49;Inherit;False;Property;_DistortionScrollSpeedU;Distortion Scroll Speed U;84;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;212;-7576.125,-2048.721;Inherit;False;Property;_DistortionRadialScaleU;Distortion Radial Scale U;88;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;213;-7577.5,-1969.577;Inherit;False;Property;_DistortionRadialScaleV;Distortion Radial Scale V;89;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-7459.641,-2698.848;Inherit;False;Property;_DistortionScrollSpeedV;Distortion Scroll Speed V;85;0;Create;True;0;0;False;0;False;0;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;206;-7391.852,-2927.613;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;401;-1318.36,3868.716;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;398;-1404.161,4338.84;Inherit;False;Property;_SecondMaskFractSwitch;Second Mask Fract Switch;35;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;275;-5876.075,-1263.352;Inherit;False;Property;_DistortionMaskVSMoveU;Distortion Mask VS Move U;94;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-6658.849,-2119.297;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;268;-1310.845,4137.304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;350;-6881.813,-3116.152;Inherit;False;Property;_DistortionScaleV;Distortion Scale V;75;0;Create;True;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-7124.117,-2812.383;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-5876.918,-1180.062;Inherit;False;Property;_DistortionMaskVSMoveV;Distortion Mask VS Move V;95;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-7125.81,-2910.668;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;349;-6878.813,-3191.152;Inherit;False;Property;_DistortionScaleU;Distortion Scale U;74;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-6862.496,-2454.532;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;399;-1141.561,4344.041;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;397;-1227.554,4264.29;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;277;-5583.143,-1229.327;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;203;-6767.462,-3333.649;Inherit;False;0;201;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;279;-5653.542,-1074.9;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;351;-6672.813,-3162.152;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;331;-6281.557,-2713.5;Inherit;False;Property;_DistortionRadialOldMethodSwitch;Distortion Radial Old Method Switch;91;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;199;-6396.309,-2374.093;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;209;-6951.271,-2861.526;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;306;-7507.722,2782.165;Inherit;False;4580.409;1828.732;;55;102;29;244;104;245;101;105;106;243;103;111;110;181;109;108;107;16;97;9;27;28;24;23;90;92;95;284;93;290;94;286;96;91;287;13;35;36;40;99;98;342;343;344;391;289;382;392;393;394;421;422;423;424;425;426;Dissolve For Subtraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;293;-6551.582,-2904.871;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;99;-7457.722,2835.548;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;272;-1222.402,4020.658;Inherit;False;Property;_SecondMaskNegate;Second Mask Negate;28;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;348;-6472.813,-3235.152;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;274;-5702.013,-1474.456;Inherit;False;0;273;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;211;-6157.782,-2545.473;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;-5394.053,-1159.156;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RoundOpNode;332;-5996.592,-2710.404;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;396;-1088.239,4143.175;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;57;-6471.055,-1972.6;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;333;-5726.627,-2662.09;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-6312.58,-3066.861;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;280;-5332.167,-1319.624;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;98;-7235.419,2836.848;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;400;-915.1631,4077.805;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;269;-777.4026,4136.635;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;373;-366.5303,4058.715;Inherit;False;493.1639;299.6899;;2;365;366;Second Mask Profile;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;282;-5168.786,-1296.976;Inherit;False;Property;_DistortionMaskNegate;Distortion Mask Negate;93;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;273;-5195.97,-1499.719;Inherit;True;Property;_DistortionMask;Distortion Mask;92;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;202;-5521.471,-2882.73;Inherit;False;Property;_DistortionRadial;Distortion Radial;86;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;102;-6986.435,3036.564;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;308;-3328.59,-2340.664;Inherit;False;2525.475;892.0974;;23;255;254;253;251;252;256;186;250;62;297;118;292;122;147;146;4;298;300;301;451;452;454;455;Main Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-6200.187,4052.138;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;342;-6508.435,3901.462;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;52;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;245;-6230.383,4315.603;Inherit;False;Property;_DissolveTextureRandomMax;Dissolve Texture Random Max;55;0;Create;True;0;0;False;0;False;1.1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;302;-7280.134,-281.0562;Inherit;False;3776.28;1203.096;;44;126;127;128;247;182;130;248;133;129;134;185;131;139;138;246;177;135;132;137;184;140;142;136;257;179;143;178;183;144;180;145;114;112;113;174;115;173;339;340;341;356;357;358;359;Noise 01;1,1,1,1;0;0
Node;AmplifyShaderEditor.ATan2OpNode;101;-6695.232,2966.366;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;105;-6677.041,3239.365;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;104;-6667.956,3174.365;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;384;-6065.735,-1649.833;Inherit;False;Property;_DistortionSpheticalStyle;Distortion Sphetical Style;81;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-6234.383,4235.603;Inherit;False;Property;_DissolveTextureRandomMin;Dissolve Texture Random Min;54;0;Create;True;0;0;False;0;False;0.9;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;343;-6509.435,3980.462;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;53;0;Create;True;0;0;False;0;False;1;0.125;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;-5833.399,-2057.684;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-5823.25,-2306.599;Inherit;False;Property;_DistortionV;Distortion V;83;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;353;-4586.753,-1139.898;Inherit;False;Property;_SecondMaskAffectsDistortion;Second Mask Affects Distortion;31;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;201;-4967.271,-2194.762;Inherit;True;Property;_Distortion;Distortion;73;0;Create;True;0;0;False;0;False;-1;None;cac42f4ff44b0ae4e9da61044d764346;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;220;-5823.765,-2385.209;Inherit;False;Property;_DistortionU;Distortion U;82;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;366;-326.6075,4146.951;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;227;-4958.961,-2321.918;Inherit;False;Property;_DistortionExp;Distortion Exp;79;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;281;-4879.911,-1382.23;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;106;-6427.44,2832.165;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;126;-7230.134,-86.14961;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;344;-6193.435,3933.462;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-6779.727,3409.363;Inherit;False;Property;_DissolveTextureRadialScaleV;Dissolve Texture Radial Scale V;61;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;226;-4633.394,-2304.407;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;251;-3278.59,-1638.906;Inherit;False;Property;_MainMaskScrollSpeedU;Main Mask Scroll Speed U;21;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;452;-3085.425,-2104.643;Inherit;False;Property;_MainMaskScaleV;Main Mask Scale V;19;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;181;-6283.275,3475.951;Inherit;False;0;9;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;252;-3275.876,-1563.566;Inherit;False;Property;_MainMaskScrollSpeedV;Main Mask Scroll Speed V;22;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;253;-3213.327,-1784.963;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;103;-6441.746,3075.564;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;243;-5897.186,4158.208;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;354;-4289.072,-1136.602;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;221;-5513.249,-2347.599;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;296;-4981.326,-2482.253;Inherit;False;Property;_DistortionRemapMax;Distortion Remap Max;78;0;Create;True;0;0;False;0;False;1;1;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;295;-4981.326,-2565.253;Inherit;False;Property;_DistortionRemapMin;Distortion Remap Min;77;0;Create;True;0;0;False;0;False;0;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;383;-5342.858,-1734.561;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;365;-193.6075,4118.951;Inherit;True;Property;_SecondMaskProfile;Second Mask Profile;27;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;283;-4740.648,-1384.446;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;451;-3085.425,-2183.643;Inherit;False;Property;_MainMaskScaleU;Main Mask Scale U;18;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-6781.02,3323.564;Inherit;False;Property;_DissolveTextureRadialScaleU;Dissolve Texture Radial Scale U;59;0;Create;True;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;294;-4411.014,-2496.531;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;218;-4944.424,-1867.426;Inherit;False;Property;_DistortionUVorSpherical;Distortion UV or Spherical;80;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;347;-5909.648,3515.844;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-6184.329,2849.065;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-6179.125,3150.664;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;249;-4816.414,-1750.535;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;186;-2912.515,-1894.28;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;216;-4951.616,-1992.633;Inherit;False;Property;_DistortionPower;Distortion Power;76;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;127;-7007.834,-84.84956;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;255;-2974.258,-1663.611;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;352;-4038.649,-1300.325;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;454;-2846.425,-2153.643;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;254;-2979.846,-1761.405;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;256;-2818.374,-1730.008;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;16;-5773.172,3769.246;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;128;-6800.391,-26.65703;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.Vector2Node;405;-3895.348,-2159.362;Inherit;False;Constant;_Vector0;Vector 0;87;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-3874.066,-2026.693;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;455;-2672.425,-1963.643;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;107;-5735.84,2977.766;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;185;-5805.517,367.1943;Inherit;False;Property;_Noise01ScaleU;Noise 01 Scale U;38;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;403;-3645.668,-2095.471;Inherit;False;Property;_DistortionEnabled;Distortion Enabled;72;0;Create;True;0;0;False;0;False;0;1;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;97;-5504.933,3387.819;Inherit;False;Property;_DissolveTextureRadial;Dissolve Texture Radial;57;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;339;-5804.289,445.526;Inherit;False;Property;_Noise01ScaleV;Noise 01 Scale V;39;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;130;-6481.912,111.1435;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;182;-5800.364,536.4592;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;129;-6490.999,176.1434;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;250;-2644.04,-1800.376;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ATan2OpNode;133;-6509.19,-96.85542;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-5809.962,807.0394;Inherit;False;Property;_Noise01RandomMax;Noise 01 Random Max;45;0;Create;True;0;0;False;0;False;1.1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-5811.962,725.0395;Inherit;False;Property;_Noise01RandomMin;Noise 01 Random Min;44;0;Create;True;0;0;False;0;False;0.9;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-2442.683,-1971.812;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;421;-5229,3880.542;Inherit;False;Property;_DissolveTextureFlipSwitch;Dissolve Texture Flip Switch;51;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;340;-5572.289,388.526;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;9;-5131.683,3361.013;Inherit;True;Property;_DissolveTexture;Dissolve Texture;49;0;Create;True;0;0;False;0;False;-1;None;34a3b7afc8c389e40bda6d478fb654e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-5832.752,152.6372;Inherit;False;0;112;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;246;-5493.962,693.0395;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;132;-6604.541,528.6579;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;135;-6594.977,260.3424;Inherit;False;Property;_Noise01RadialScaleU;Noise 01 Radial Scale U;47;0;Create;True;0;0;False;0;False;1;0.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;139;-6255.702,12.34262;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;134;-6241.398,-231.0562;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-6593.683,346.1413;Inherit;False;Property;_Noise01RadialScaleV;Noise 01 Radial Scale V;48;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-6639.584,765.83;Inherit;False;Property;_Noise01ScrollSpeedV;Noise 01 Scroll Speed V;43;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-6638.793,675.8188;Inherit;False;Property;_Noise01ScrollSpeedU;Noise 01 Scroll Speed U;42;0;Create;True;0;0;False;0;False;0;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-6360.126,600.987;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;422;-4935,3883.542;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-5993.079,87.44242;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;341;-5382.289,189.526;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-5998.281,-214.1567;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2287.094,-2007.827;Inherit;True;Property;_MainMask;Main Mask;17;0;Create;True;0;0;False;0;False;-1;None;2bc344f77a919034c93cdae977e870d6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;-6349.66,721.0742;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;-5356.724,563.1734;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-5147.984,3676.197;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;178;-6173.414,650.5728;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;297;-2240.098,-2261.141;Inherit;False;Property;_MainMaskChannelsMultiply;Main Mask Channels Multiply;20;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;298;-1969.3,-1982.44;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;143;-5549.794,-85.45601;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;423;-4765,3779.542;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;395;-4856.523,4407.398;Inherit;False;352;165;;1;390;If no mask, set to 0;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;183;-5196.755,352.3616;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;180;-5315.483,3.753217;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;426;-4612.772,3778.325;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-4576.235,3504.304;Inherit;False;Property;_DissolveExp;Dissolve Exp;62;0;Create;True;0;0;False;0;False;2;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;390;-4806.523,4457.398;Inherit;False;Property;_DissolveMaskSteepness;Dissolve Mask Steepness;71;0;Create;True;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;391;-4591.523,4075.398;Inherit;False;Constant;_Float4;Float 4;84;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;-1820.626,-2132.091;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;144;-5085.367,619.431;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;145;-4705.28,264.8167;Inherit;False;Property;_Noise01Radial;Noise 01 Radial;46;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;389;-4425.159,4173.983;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-4475.555,3116.224;Inherit;False;Property;_DissolveExpReversed;Dissolve Exp Reversed;63;0;Create;True;0;0;False;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;359;-4444.257,794.3589;Inherit;False;Property;_SecondMaskAffectsNoise01Negate;Second Mask Affects Noise 01 Negate;32;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;92;-4427.228,2922.179;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;358;-4305.579,598.6541;Inherit;False;Constant;_Float2;Float 2;77;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;23;-4349.199,3413.248;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;292;-1734.905,-2290.664;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;112;-4407.658,245.5681;Inherit;True;Property;_Noise01;Noise 01;37;0;Create;True;0;0;False;0;False;-1;None;34a3b7afc8c389e40bda6d478fb654e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;40;-4484.97,3828.897;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;284;-4257.921,4238.685;Inherit;True;Property;_DissolveMask;Dissolve Mask;68;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;357;-4093.579,671.6541;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-4393.218,444.7051;Inherit;False;Property;_Noise01Negate;Noise 01 Negate;40;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;394;-4220.412,4158.824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;315;-2533.914,-4410.97;Inherit;False;2651.594;983.4888;;15;330;329;328;327;326;325;323;322;321;320;319;318;317;316;334;Custom Color Mask For Ramp;1,1,1,1;0;0
Node;AmplifyShaderEditor.NegateNode;393;-4100.412,4427.824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;122;-1648.428,-2130.091;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OneMinusNode;95;-4042.394,3199.287;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;93;-4177.055,2949.256;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;36;-4243.825,3838.711;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;300;-1382.067,-2127.005;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;94;-3870.679,3200.63;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;321;-2483.914,-4360.971;Inherit;True;Property;_CustomColorMask;Custom Color Mask;13;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;392;-3934.141,4365.062;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;329;-2475.976,-4157.611;Inherit;False;Property;_CustomColorMaskChannels;Custom Color Mask Channels;14;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-4057.288,335.9846;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-4035.58,473.0988;Inherit;False;Property;_Noise01Exp;Noise 01 Exp;41;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;115;-3878.81,328.9355;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;286;-3746.326,4140.499;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;96;-3713.721,3203.312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;309;-264.8423,158.1672;Inherit;False;2120.434;472.0852;;14;65;66;7;8;39;38;43;160;161;162;163;164;44;374;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-1353.316,-1897.055;Inherit;False;Property;_MainMaskExp;Main Mask Exp;23;0;Create;True;0;0;False;0;False;1;4;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;301;-1228.489,-2130.872;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;330;-2104.645,-4235.578;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;91;-3550.66,3206.426;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;304;-3818.429,1770.619;Inherit;False;1465.482;441.1868;;10;239;240;238;242;233;231;232;234;236;235;Dissolve Edges Glow;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;323;-1976.06,-4235.928;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PowerNode;173;-3683.854,382.2263;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;287;-3607.226,4140.499;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-214.8423,228.3851;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;146;-983.1156,-1977.155;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;319;-1724.198,-4234.969;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-3274.622,3710.381;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;106.6964,209.5086;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;-3768.428,1842.05;Inherit;False;Property;_DissolveGlowOffset;Dissolve Glow Offset;66;0;Create;True;0;0;False;0;False;0.125;0.25;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;66;256.4397,210.9627;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;326;-1596.041,-4231.307;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;239;-3447.336,1927.241;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;322;-1349.397,-4208.127;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;242;-3315.394,1923.14;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;35;-3102.313,3714.085;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;240;-3129.783,1891.009;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;327;-1215.614,-4205.573;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;-3263.227,2042.949;Inherit;False;Property;_DissolveGlowPower;Dissolve Glow Power;65;0;Create;True;0;0;False;0;False;1;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;234;-3002.459,2096.806;Inherit;False;Property;_DissolveGlowAmount;Dissolve Glow Amount;64;0;Create;True;0;0;False;0;False;0;0;0;120;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;231;-2947.187,1912.126;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;310;-257.8628,-1556.072;Inherit;False;1519.673;597.7856;;13;379;376;159;158;380;188;157;187;156;155;154;151;152;Ramp UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;316;-1058.809,-4202.164;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;412.4047,208.1672;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;236;-2705.242,1820.619;Inherit;False;Constant;_Float1;Float 1;49;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;581.4049,211.0672;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;311;-3114.16,1402.361;Inherit;False;752.7615;296.6475;;3;314;313;312;Dissolve Glow Affects Ramp;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-2700.757,1911.595;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-207.8628,-1361.194;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;318;-911.2308,-4204.476;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;328;-1303.216,-3542.482;Inherit;False;Property;_CustomColorMaskAffectedByDynamics;Custom Color Mask Affected By Dynamics;16;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;312;-2952.517,1478.271;Inherit;False;Constant;_Float6;Float 6;64;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;151;87.61806,-1421.344;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;313;-3082.369,1564.625;Inherit;False;Property;_DissolveGlowAffectsRamp;Dissolve Glow Affects Ramp;67;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;317;-774.7148,-3580.915;Inherit;False;Property;_CustomColorMaskSwitch;Custom Color Mask Switch;15;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;235;-2506.946,1851.199;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;325;-677.5665,-3723.522;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;320;-470.0286,-3680.24;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;32.32595,-1506.072;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;9;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;314;-2578.72,1503.424;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;267.2708,-1469.495;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;415;301.9096,-1827.294;Inherit;False;225;206;;1;414;BugCheck01;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;414;351.9096,-1777.294;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;156;214.3455,-1326.866;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;10;0;Create;True;0;0;False;0;False;1;6;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;187;412.3101,-1463.759;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;379;363.3502,-1116.523;Inherit;False;Property;_SecondMaskAffectsRamp;Second Mask Affects Ramp;33;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;573.631,-1469.09;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;376;647.1516,-1088.544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;188;723.3102,-1461.968;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;39;562.0975,428.2522;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;838.9972,277.4519;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;380;816.6196,-1286.976;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;432;33.41113,993.6212;Inherit;False;2141.994;326.3536;;13;435;437;436;434;444;443;442;441;440;439;438;448;449;Depth Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;43;995.3334,281.7289;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;158;896.5001,-1462.864;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;307;2179.224,-1889.589;Inherit;False;2161.814;1391.776;;23;360;3;55;2;149;54;56;189;148;1;190;192;41;150;191;362;364;361;363;427;429;430;431;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;427;2244.747,-1433.408;Inherit;False;Property;_RampIgnoreVertexColor;Ramp Ignore Vertex Color;11;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;449;54.73441,1065.975;Inherit;False;Property;_SoftParticlesDistance;Soft Particles Distance;98;0;Create;True;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;159;1065.994,-1459.197;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;41;2349.39,-1673.635;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;161;1224.098,337.9238;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;1034.256,475.9577;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;3;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;191;2310.518,-1163.459;Inherit;False;Property;_RampColorTint;Ramp Color Tint;6;0;Create;True;0;0;False;0;False;1,1,1,1;0.7607843,0.5803921,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;162;1377.158,341.9516;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;150;2226.722,-1352.896;Inherit;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;False;-1;None;410dc3309df20a841a4d814242d4a5a8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;430;2595.47,-1572.687;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;434;304.9957,1047.818;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;444;1656.416,1063.165;Inherit;False;Constant;_Float7;Float 5;46;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;362;3479.528,-661.2461;Inherit;False;Property;_SecondMaskBoostsEmission;Second Mask Boosts Emission;34;0;Create;True;0;0;False;0;False;0;0;0;40;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;450;1445.96,866.7161;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;2321.688,-1839.589;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,0.6530125,0.514151,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;431;2716.47,-1571.687;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;2568.257,-1246.6;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;163;1522.161,341.9516;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;364;3782.266,-599.0708;Inherit;False;Constant;_Float3;Float 3;79;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;361;3784.928,-716.5547;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;448;1860.992,1104.092;Inherit;False;Property;_SoftParticlesEnabled;Soft Particles Enabled;97;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;164;1680.591,335.2386;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;2580.448,-1741.134;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;3368.45,-1513.857;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;2776.012,-1431.815;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;56;3417.124,-1606.999;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;149;3176.073,-1267.246;Inherit;False;Property;_RampEnabled;Ramp Enabled;4;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;363;3949.74,-672.3484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;335;-5969.795,-2538.309;Inherit;False;384;142;Slower Compile;1;228;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;3319.551,-1080.212;Inherit;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;4;20;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;55;3634.902,-1544.434;Inherit;False;Property;_EmissionVertexStreamEnabled;Emission Vertex Stream Enabled;96;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;334;-268.7717,-3915.482;Inherit;False;353;141;Slower Compile;1;324;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;446;1972.24,442.0714;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;418;-5215.936,-3029.868;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;324;-255.358,-3875.876;Inherit;False;Property;_CustomColorMaskEnabled;Custom Color Mask Enabled;12;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;-5543.519,518.1015;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;4141.81,-1250.153;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;290;-3765.577,3693.081;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;382;-5173.774,4064.174;Inherit;False;Property;_DissolveMaskRemapMin;Dissolve Mask Remap Min;69;0;Create;True;0;0;False;0;False;-1;-2;-2;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;228;-5958.643,-2499.262;Inherit;False;Property;_DistortionRadialUseOldMethod;Distortion Radial Use Old Method;90;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;425;-4399,3247.542;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;360;3618.436,-764.9554;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;28;-4714.984,3386.196;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;50;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;416;-5691.196,-3078.735;Inherit;False;Property;_DistortionRadialSwitch;Distortion Radial Switch;87;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;356;-4318.338,692.5092;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;419;-5062.611,-2813.623;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;417;-5427.037,-3075.168;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;189;2770.702,-1084.032;Inherit;False;Property;_RampAffectedByVertexColor;Ramp Affected By Vertex Color;7;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;420;-5114.611,-2618.623;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;447;2216.463,424.9928;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;442;1493.074,1139.418;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;424;-4613,3229.542;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;438;974.342,1165.121;Float;False;Property;_DepthMaskExp1;Depth Mask Exp;58;0;Create;True;0;0;False;0;False;2;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;439;1025.283,1045.87;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;355;-4116.25,-1050.444;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;440;1221.074,1243.418;Inherit;False;Property;_DepthMaskMultiply1;Depth Mask Multiply;60;0;Create;True;0;0;False;0;False;0.25;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;435;452.557,1177.442;Inherit;False;Property;_DepthMaskRemapMax1;Depth Mask Remap Max;56;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;429;2373.47,-1512.687;Inherit;False;Constant;_Float5;Float 5;91;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;441;1202.041,1112.682;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;443;1659.181,1136.119;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;289;-4064.013,3688.843;Inherit;False;Property;_DissolveMaskScale;Dissolve Mask Scale;70;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;436;549.2762,1049.641;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;437;765.8525,1090.937;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;374;-69.98584,320.329;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5106.775,-242.5273;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/DissolveParticleAdvanced;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;412;0;410;0
WireConnection;412;1;411;0
WireConnection;263;0;261;0
WireConnection;263;1;262;0
WireConnection;413;0;265;0
WireConnection;413;1;412;0
WireConnection;260;0;263;0
WireConnection;260;1;258;3
WireConnection;19;0;18;0
WireConnection;195;0;19;0
WireConnection;266;0;260;0
WireConnection;266;1;413;0
WireConnection;264;1;266;0
WireConnection;194;0;195;0
WireConnection;194;1;195;1
WireConnection;196;0;194;0
WireConnection;196;1;197;0
WireConnection;196;2;198;0
WireConnection;58;0;19;0
WireConnection;401;0;402;0
WireConnection;401;1;264;1
WireConnection;215;0;213;0
WireConnection;215;1;196;0
WireConnection;268;0;401;0
WireConnection;268;1;258;4
WireConnection;208;0;206;2
WireConnection;208;1;205;0
WireConnection;207;0;206;2
WireConnection;207;1;204;0
WireConnection;214;0;58;0
WireConnection;214;1;212;0
WireConnection;399;0;398;0
WireConnection;397;0;268;0
WireConnection;277;0;275;0
WireConnection;277;1;276;0
WireConnection;351;0;349;0
WireConnection;351;1;350;0
WireConnection;199;0;214;0
WireConnection;199;1;215;0
WireConnection;209;0;207;0
WireConnection;209;1;208;0
WireConnection;348;0;203;0
WireConnection;348;1;351;0
WireConnection;211;0;209;0
WireConnection;211;1;199;0
WireConnection;278;0;277;0
WireConnection;278;1;279;1
WireConnection;332;0;331;0
WireConnection;396;0;268;0
WireConnection;396;1;397;0
WireConnection;396;2;399;0
WireConnection;57;0;19;0
WireConnection;333;0;211;0
WireConnection;333;1;57;0
WireConnection;333;2;332;0
WireConnection;210;0;348;0
WireConnection;210;1;209;0
WireConnection;210;2;293;3
WireConnection;280;0;274;0
WireConnection;280;1;278;0
WireConnection;98;0;99;0
WireConnection;400;0;272;0
WireConnection;400;1;396;0
WireConnection;269;0;400;0
WireConnection;273;1;280;0
WireConnection;202;1;210;0
WireConnection;202;0;333;0
WireConnection;102;0;98;0
WireConnection;101;0;102;0
WireConnection;101;1;102;1
WireConnection;219;0;58;0
WireConnection;219;1;57;0
WireConnection;201;1;202;0
WireConnection;366;0;269;0
WireConnection;281;0;273;1
WireConnection;281;1;282;0
WireConnection;106;0;98;0
WireConnection;344;0;342;0
WireConnection;344;1;343;0
WireConnection;226;0;201;1
WireConnection;226;1;227;0
WireConnection;103;0;101;0
WireConnection;103;1;104;0
WireConnection;103;2;105;0
WireConnection;243;0;29;3
WireConnection;243;3;244;0
WireConnection;243;4;245;0
WireConnection;354;0;353;0
WireConnection;221;0;220;0
WireConnection;221;1;222;0
WireConnection;383;0;219;0
WireConnection;383;1;57;0
WireConnection;383;2;384;0
WireConnection;365;1;366;0
WireConnection;283;0;281;0
WireConnection;294;0;226;0
WireConnection;294;3;295;0
WireConnection;294;4;296;0
WireConnection;218;1;221;0
WireConnection;218;0;383;0
WireConnection;347;0;181;0
WireConnection;347;1;344;0
WireConnection;347;2;243;0
WireConnection;108;0;106;0
WireConnection;108;1;110;0
WireConnection;109;0;103;0
WireConnection;109;1;111;0
WireConnection;127;0;126;0
WireConnection;255;0;253;2
WireConnection;255;1;252;0
WireConnection;352;0;283;0
WireConnection;352;1;365;1
WireConnection;352;2;354;0
WireConnection;454;0;451;0
WireConnection;454;1;452;0
WireConnection;254;0;253;2
WireConnection;254;1;251;0
WireConnection;256;0;254;0
WireConnection;256;1;255;0
WireConnection;16;0;347;0
WireConnection;16;2;29;3
WireConnection;128;0;127;0
WireConnection;217;0;294;0
WireConnection;217;1;216;0
WireConnection;217;2;218;0
WireConnection;217;3;249;4
WireConnection;217;4;352;0
WireConnection;455;0;454;0
WireConnection;455;1;186;0
WireConnection;107;0;108;0
WireConnection;107;1;109;0
WireConnection;403;1;405;0
WireConnection;403;0;217;0
WireConnection;97;1;16;0
WireConnection;97;0;107;0
WireConnection;250;0;455;0
WireConnection;250;1;256;0
WireConnection;133;0;128;0
WireConnection;133;1;128;1
WireConnection;62;0;403;0
WireConnection;62;1;250;0
WireConnection;340;0;185;0
WireConnection;340;1;339;0
WireConnection;9;1;97;0
WireConnection;246;0;182;3
WireConnection;246;3;247;0
WireConnection;246;4;248;0
WireConnection;139;0;133;0
WireConnection;139;1;130;0
WireConnection;139;2;129;0
WireConnection;134;0;127;0
WireConnection;136;0;132;2
WireConnection;136;1;131;0
WireConnection;422;0;421;0
WireConnection;140;0;139;0
WireConnection;140;1;138;0
WireConnection;341;0;142;0
WireConnection;341;1;340;0
WireConnection;341;2;246;0
WireConnection;137;0;134;0
WireConnection;137;1;135;0
WireConnection;4;1;62;0
WireConnection;179;0;132;2
WireConnection;179;1;177;0
WireConnection;257;0;182;3
WireConnection;27;0;9;1
WireConnection;178;0;136;0
WireConnection;178;1;179;0
WireConnection;298;0;4;1
WireConnection;298;1;4;2
WireConnection;298;2;4;3
WireConnection;298;3;4;4
WireConnection;143;0;137;0
WireConnection;143;1;140;0
WireConnection;423;0;9;1
WireConnection;423;1;27;0
WireConnection;423;2;422;0
WireConnection;183;0;341;0
WireConnection;183;2;257;0
WireConnection;180;0;143;0
WireConnection;180;1;178;0
WireConnection;180;2;182;3
WireConnection;426;0;423;0
WireConnection;118;0;297;0
WireConnection;118;1;298;0
WireConnection;144;0;183;0
WireConnection;144;1;178;0
WireConnection;145;1;144;0
WireConnection;145;0;180;0
WireConnection;389;0;391;0
WireConnection;389;1;390;0
WireConnection;23;0;426;0
WireConnection;23;1;24;0
WireConnection;292;0;118;0
WireConnection;112;1;145;0
WireConnection;357;0;358;0
WireConnection;357;1;365;1
WireConnection;357;2;359;0
WireConnection;394;0;389;0
WireConnection;393;0;390;0
WireConnection;122;0;292;0
WireConnection;95;0;23;0
WireConnection;93;0;92;1
WireConnection;93;4;90;0
WireConnection;36;0;40;1
WireConnection;36;3;389;0
WireConnection;36;4;394;0
WireConnection;300;0;122;0
WireConnection;300;1;122;1
WireConnection;300;2;122;2
WireConnection;300;3;122;3
WireConnection;94;0;95;0
WireConnection;94;1;93;0
WireConnection;392;0;284;1
WireConnection;392;3;393;0
WireConnection;392;4;390;0
WireConnection;113;0;112;1
WireConnection;113;1;114;0
WireConnection;113;2;357;0
WireConnection;115;0;113;0
WireConnection;286;0;36;0
WireConnection;286;1;392;0
WireConnection;96;0;94;0
WireConnection;301;0;300;0
WireConnection;330;0;321;0
WireConnection;330;1;329;0
WireConnection;91;0;96;0
WireConnection;323;0;330;0
WireConnection;173;0;115;0
WireConnection;173;1;174;0
WireConnection;287;0;286;0
WireConnection;146;0;301;0
WireConnection;146;1;147;0
WireConnection;319;0;323;0
WireConnection;319;1;323;1
WireConnection;319;2;323;2
WireConnection;319;3;323;3
WireConnection;13;0;91;0
WireConnection;13;1;287;0
WireConnection;65;0;146;0
WireConnection;65;1;44;0
WireConnection;65;2;173;0
WireConnection;65;3;365;2
WireConnection;66;0;65;0
WireConnection;326;0;319;0
WireConnection;239;0;238;0
WireConnection;239;1;13;0
WireConnection;322;0;326;0
WireConnection;322;1;173;0
WireConnection;322;2;365;2
WireConnection;322;3;44;0
WireConnection;242;0;239;0
WireConnection;242;1;66;0
WireConnection;35;0;13;0
WireConnection;240;0;242;0
WireConnection;327;0;322;0
WireConnection;231;0;240;0
WireConnection;231;1;233;0
WireConnection;316;0;327;0
WireConnection;316;1;35;0
WireConnection;7;0;66;0
WireConnection;7;1;35;0
WireConnection;8;0;7;0
WireConnection;232;0;231;0
WireConnection;232;1;234;0
WireConnection;318;0;316;0
WireConnection;151;0;146;0
WireConnection;151;1;8;0
WireConnection;151;2;152;0
WireConnection;235;0;236;0
WireConnection;235;1;232;0
WireConnection;325;0;326;0
WireConnection;325;1;318;0
WireConnection;325;2;328;0
WireConnection;320;0;151;0
WireConnection;320;1;325;0
WireConnection;320;2;317;0
WireConnection;314;0;312;0
WireConnection;314;1;235;0
WireConnection;314;2;313;0
WireConnection;155;0;154;0
WireConnection;155;1;320;0
WireConnection;155;2;314;0
WireConnection;414;0;155;0
WireConnection;187;0;414;0
WireConnection;157;0;187;0
WireConnection;157;1;156;0
WireConnection;376;0;379;0
WireConnection;376;1;365;1
WireConnection;188;0;157;0
WireConnection;38;0;8;0
WireConnection;38;1;39;4
WireConnection;380;0;188;0
WireConnection;380;1;376;0
WireConnection;43;0;38;0
WireConnection;158;0;380;0
WireConnection;159;0;158;0
WireConnection;161;0;43;0
WireConnection;162;0;161;0
WireConnection;162;1;160;0
WireConnection;150;1;159;0
WireConnection;430;0;41;0
WireConnection;430;1;427;0
WireConnection;434;0;449;0
WireConnection;450;0;434;0
WireConnection;431;0;430;0
WireConnection;192;0;150;0
WireConnection;192;1;191;0
WireConnection;163;0;162;0
WireConnection;361;0;365;1
WireConnection;361;1;362;0
WireConnection;448;1;444;0
WireConnection;448;0;450;0
WireConnection;164;0;163;0
WireConnection;148;0;1;0
WireConnection;148;1;41;0
WireConnection;190;0;431;0
WireConnection;190;1;192;0
WireConnection;149;1;148;0
WireConnection;149;0;190;0
WireConnection;363;0;361;0
WireConnection;363;1;364;0
WireConnection;55;1;56;0
WireConnection;55;0;54;2
WireConnection;446;0;164;0
WireConnection;446;1;448;0
WireConnection;418;0;210;0
WireConnection;418;1;333;0
WireConnection;418;2;417;0
WireConnection;324;1;151;0
WireConnection;324;0;320;0
WireConnection;184;0;185;0
WireConnection;184;1;246;0
WireConnection;3;0;149;0
WireConnection;3;1;2;0
WireConnection;3;2;55;0
WireConnection;3;3;235;0
WireConnection;3;4;363;0
WireConnection;290;0;36;0
WireConnection;290;1;289;0
WireConnection;228;1;211;0
WireConnection;228;0;57;0
WireConnection;425;0;424;0
WireConnection;425;1;28;0
WireConnection;360;0;269;0
WireConnection;28;1;9;1
WireConnection;28;0;27;0
WireConnection;356;0;269;0
WireConnection;419;0;416;0
WireConnection;417;0;416;0
WireConnection;189;1;192;0
WireConnection;189;0;190;0
WireConnection;420;0;202;0
WireConnection;420;1;419;0
WireConnection;447;0;446;0
WireConnection;442;0;441;0
WireConnection;442;1;440;0
WireConnection;424;0;421;0
WireConnection;439;0;437;0
WireConnection;355;0;269;0
WireConnection;441;0;439;0
WireConnection;441;1;438;0
WireConnection;443;0;442;0
WireConnection;436;0;434;0
WireConnection;437;0;436;0
WireConnection;437;2;435;0
WireConnection;374;0;269;0
WireConnection;0;2;3;0
WireConnection;0;9;447;0
ASEEND*/
//CHKSM=0B01F105212A5C1D571B721EC05EE7775D56D307