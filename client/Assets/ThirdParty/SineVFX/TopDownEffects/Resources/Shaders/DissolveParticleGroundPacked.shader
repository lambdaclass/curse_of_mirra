// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/DissolveParticleGroundPacked"
{
	Properties
	{
		_FinalPower("Final Power", Float) = 4
		_FinalOpacityPower("Final Opacity Power", Float) = 1
		_FinalOpacityExp("Final Opacity Exp", Range( 0.2 , 20)) = 1
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampOffsetMultiply("Ramp Offset Multiply", Range( 0 , 4)) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_RampSmoothstepMin("Ramp Smoothstep Min", Float) = 0.25
		_RampSmoothstepMax("Ramp Smoothstep Max", Float) = 0.75
		_PackedTex("PackedTex", 2D) = "white" {}
		_HeightSlopeControl("Height Slope Control", Range( 0 , 1)) = 0
		_HeightBoost("Height Boost", Float) = 1
		_HeightMapNegate("Height Map Negate", Range( 0 , 1)) = 0
		_AlbedoColor("Albedo Color", Color) = (0.6226415,0.4755985,0.4258633,1)
		_AlbedoColor2("Albedo Color 2", Color) = (0,0,0,1)
		_AlbedoMapExp("Albedo Map Exp", Range( 0.5 , 2)) = 1
		_OpacityMask("Opacity Mask", 2D) = "white" {}
		_OpacityMaskScaler("Opacity Mask Scaler", Range( 0.5 , 2)) = 1
		_OpacityMaskPower("Opacity Mask Power", Float) = 1
		_OpacityMaskExp("Opacity Mask Exp", Range( 0.2 , 8)) = 0.2
		_SecondMask("Second Mask", 2D) = "white" {}
		_SecondMaskScaleU("Second Mask Scale U", Float) = 1
		_SecondMaskScaleV("Second Mask Scale V", Float) = 1
		_SecondMaskExp("Second Mask Exp", Range( 0.2 , 4)) = 1
		_SecondMaskEdgeGlow("Second Mask Edge Glow", Float) = 0
		_SecondMaskProfile("Second Mask Profile", 2D) = "white" {}
		_LavaAppearMask("Lava Appear Mask", 2D) = "white" {}
		_LavaAppearMaskScaleU("Lava Appear Mask Scale U", Float) = 1
		_LavaAppearMaskScaleV("Lava Appear Mask Scale V", Float) = 1
		_LavaAppearMaskExp("Lava Appear Mask Exp", Range( 0.2 , 8)) = 1
		[Toggle(_LAVANOISEENABLED_ON)] _LavaNoiseEnabled("Lava Noise Enabled", Float) = 1
		_LavaNoise("Lava Noise", 2D) = "white" {}
		_LavaNoiseScaleU("Lava Noise Scale U", Float) = 1
		_LavaNoiseScaleV("Lava Noise Scale V", Float) = 1
		_LavaNoiseNegate("Lava Noise Negate", Range( 0 , 1)) = 0
		_LavaNoiseScrollSpeed("Lava Noise Scroll Speed", Float) = 0
		_LavaNoiseDistortion("Lava Noise Distortion", 2D) = "white" {}
		_LavaNoiseDistortionScaleU("Lava Noise Distortion Scale U", Float) = 1
		_LavaNoiseDistortionScaleV("Lava Noise Distortion Scale V", Float) = 1
		_LavaNoiseDistortionAmount("Lava Noise Distortion Amount", Float) = 0
		_LavaNoiseDistortionScrollSpeed("Lava Noise Distortion Scroll Speed", Float) = 0.075
		_LavaDistortionSlopeNegate("Lava Distortion Slope Negate", Range( 0 , 1)) = 1
		_LavaDistortionSlopeStyle("Lava Distortion Slope Style", Range( 0 , 1)) = 0
		_LavaOnlyInValleysNegate("Lava Only In Valleys Negate", Range( 0 , 1)) = 1
		_LavaOnlyInValleysExp("Lava Only In Valleys Exp", Range( 0.2 , 8)) = 1
		[Toggle(_SURFACENOISEENABLED_ON)] _SurfaceNoiseEnabled("Surface Noise Enabled", Float) = 0
		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		_SurfaceNoiseScaleU("Surface Noise Scale U", Float) = 1
		_SurfaceNoiseScaleV("Surface Noise Scale V", Float) = 1
		_SurfaceNoiseAdd("Surface Noise Add", Range( 0 , 4)) = 0
		_SurfaceNoiseExp("Surface Noise Exp", Range( 0.2 , 4)) = 1
		[Toggle(_VALLEYSEMISSIONBOOSTENABLED_ON)] _ValleysEmissionBoostEnabled("Valleys Emission Boost Enabled", Float) = 1
		_ValleysEmissionBoostAmount("Valleys Emission Boost Amount", Range( 0 , 100)) = 80
		_ValleysEmissionBoostBloom("Valleys Emission Boost Bloom", Range( 0 , 1)) = 1
		[Toggle(_CRACKSENABLED_ON)] _CracksEnabled("Cracks Enabled", Float) = 1
		_Cracks("Cracks", 2D) = "white" {}
		_CracksScaleU("Cracks Scale U", Float) = 1
		_CracksScaleV("Cracks Scale V", Float) = 1
		_CracksNegate("Cracks Negate", Range( 0 , 1)) = 0
		_CracksNegateSlope("Cracks Negate Slope", Range( 0 , 1)) = 0
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		[Toggle(_DISSOLVETEXTUREFLIP_ON)] _DissolveTextureFlip("Dissolve Texture Flip", Float) = 1
		_DissolveTextureScaleU("Dissolve Texture Scale U", Float) = 1
		_DissolveTextureScaleV("Dissolve Texture Scale V", Float) = 1
		_DissolveTextureRandomMin("Dissolve Texture Random Min", Range( 0.5 , 1)) = 0.9
		_DissolveTextureRandomMax("Dissolve Texture Random Max", Range( 1 , 1.5)) = 1.1
		_DissolveExp("Dissolve Exp", Float) = 6.47
		_DissolveExpReversed("Dissolve Exp Reversed", Float) = 2
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _CRACKSENABLED_ON
		#pragma shader_feature _LAVANOISEENABLED_ON
		#pragma shader_feature _SURFACENOISEENABLED_ON
		#pragma shader_feature _VALLEYSEMISSIONBOOSTENABLED_ON
		#pragma shader_feature _DISSOLVETEXTUREFLIP_ON
		#pragma surface surf Unlit alpha:fade keepalpha noambient novertexlights nolightmap  nodynlightmap nodirlightmap 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _AlbedoColor2;
		uniform float4 _AlbedoColor;
		uniform sampler2D _PackedTex;
		uniform float4 _PackedTex_ST;
		uniform sampler2D _Cracks;
		uniform float4 _Cracks_ST;
		uniform float _CracksScaleU;
		uniform float _CracksScaleV;
		uniform float _CracksNegate;
		uniform float _CracksNegateSlope;
		uniform float _HeightMapNegate;
		uniform float _AlbedoMapExp;
		uniform float _HeightSlopeControl;
		uniform sampler2D _Ramp;
		uniform float _RampOffsetMultiply;
		uniform sampler2D _LavaNoise;
		uniform sampler2D _LavaNoiseDistortion;
		uniform float4 _LavaNoiseDistortion_ST;
		uniform float _LavaNoiseDistortionScaleU;
		uniform float _LavaNoiseDistortionScaleV;
		uniform float _LavaNoiseDistortionScrollSpeed;
		uniform float _LavaNoiseDistortionAmount;
		uniform float _LavaDistortionSlopeStyle;
		uniform float _LavaDistortionSlopeNegate;
		uniform float4 _LavaNoise_ST;
		uniform float _LavaNoiseScaleU;
		uniform float _LavaNoiseScaleV;
		uniform float _LavaNoiseScrollSpeed;
		uniform float _LavaNoiseNegate;
		uniform float _RampSmoothstepMin;
		uniform float _RampSmoothstepMax;
		uniform sampler2D _LavaAppearMask;
		uniform float4 _LavaAppearMask_ST;
		uniform float _LavaAppearMaskScaleU;
		uniform float _LavaAppearMaskScaleV;
		uniform float _LavaAppearMaskExp;
		uniform sampler2D _SurfaceNoise;
		uniform float4 _SurfaceNoise_ST;
		uniform float _SurfaceNoiseScaleU;
		uniform float _SurfaceNoiseScaleV;
		uniform float _SurfaceNoiseAdd;
		uniform float _SurfaceNoiseExp;
		uniform float _HeightBoost;
		uniform sampler2D _SecondMaskProfile;
		uniform sampler2D _SecondMask;
		uniform float4 _SecondMask_ST;
		uniform float _SecondMaskScaleU;
		uniform float _SecondMaskScaleV;
		uniform float _SecondMaskExp;
		uniform float _SecondMaskEdgeGlow;
		uniform float _LavaOnlyInValleysExp;
		uniform float _LavaOnlyInValleysNegate;
		uniform float _RampOffsetExp;
		uniform float _FinalPower;
		uniform float4 _RampColorTint;
		uniform float _ValleysEmissionBoostAmount;
		uniform float _ValleysEmissionBoostBloom;
		uniform float _FinalOpacityPower;
		uniform sampler2D _OpacityMask;
		uniform float4 _OpacityMask_ST;
		uniform float _OpacityMaskScaler;
		uniform float _OpacityMaskPower;
		uniform float _OpacityMaskExp;
		uniform sampler2D _DissolveTexture;
		uniform float _DissolveTextureScaleU;
		uniform float _DissolveTextureScaleV;
		uniform float _DissolveTextureRandomMin;
		uniform float _DissolveTextureRandomMax;
		uniform float _DissolveExp;
		uniform float _DissolveExpReversed;
		uniform float _FinalOpacityExp;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_PackedTex = i.uv_texcoord * _PackedTex_ST.xy + _PackedTex_ST.zw;
			float4 tex2DNode69 = tex2D( _PackedTex, uv0_PackedTex );
			float temp_output_228_0 = ( tex2DNode69.r + 0.0 );
			float2 uv0_Cracks = i.uv_texcoord * _Cracks_ST.xy + _Cracks_ST.zw;
			float2 appendResult485 = (float2(_CracksScaleU , _CracksScaleV));
			float temp_output_229_0 = ( tex2DNode69.g + 0.0 );
			float clampResult140 = clamp( ( ( tex2D( _Cracks, ( uv0_Cracks * appendResult485 ) ).r * ( 1.0 - _CracksNegate ) ) + _CracksNegate + ( temp_output_229_0 * _CracksNegateSlope ) ) , 0.0 , 1.0 );
			#ifdef _CRACKSENABLED_ON
				float staticSwitch510 = clampResult140;
			#else
				float staticSwitch510 = 1.0;
			#endif
			float clampResult109 = clamp( ( ( tex2DNode69.b + 0.0 ) + _HeightMapNegate ) , 0.0 , 1.0 );
			float temp_output_231_0 = ( tex2DNode69.a + 0.0 );
			float temp_output_78_0 = ( temp_output_231_0 * _HeightSlopeControl );
			float clampResult163 = clamp( ( pow( ( temp_output_228_0 * staticSwitch510 * clampResult109 ) , _AlbedoMapExp ) - temp_output_78_0 ) , 0.0 , 1.0 );
			float4 lerpResult99 = lerp( _AlbedoColor2 , _AlbedoColor , clampResult163);
			float2 uv0_LavaNoiseDistortion = i.uv_texcoord * _LavaNoiseDistortion_ST.xy + _LavaNoiseDistortion_ST.zw;
			float2 appendResult473 = (float2(_LavaNoiseDistortionScaleU , _LavaNoiseDistortionScaleV));
			float lerpResult394 = lerp( pow( ( 1.0 - temp_output_229_0 ) , 8.0 ) , temp_output_231_0 , _LavaDistortionSlopeStyle);
			float clampResult397 = clamp( ( lerpResult394 + _LavaDistortionSlopeNegate ) , 0.0 , 1.0 );
			float2 uv0_LavaNoise = i.uv_texcoord * _LavaNoise_ST.xy + _LavaNoise_ST.zw;
			float2 appendResult472 = (float2(_LavaNoiseScaleU , _LavaNoiseScaleV));
			float clampResult374 = clamp( ( tex2D( _LavaNoise, ( ( tex2D( _LavaNoiseDistortion, ( ( uv0_LavaNoiseDistortion * appendResult473 ) + ( _Time.y * _LavaNoiseDistortionScrollSpeed ) ) ).r * _LavaNoiseDistortionAmount * clampResult397 ) + ( ( uv0_LavaNoise * appendResult472 ) + ( _Time.y * _LavaNoiseScrollSpeed ) ) ) ).r + _LavaNoiseNegate ) , 0.0 , 1.0 );
			#ifdef _LAVANOISEENABLED_ON
				float staticSwitch504 = clampResult374;
			#else
				float staticSwitch504 = 1.0;
			#endif
			float2 uv0_LavaAppearMask = i.uv_texcoord * _LavaAppearMask_ST.xy + _LavaAppearMask_ST.zw;
			float2 appendResult494 = (float2(_LavaAppearMaskScaleU , _LavaAppearMaskScaleV));
			float2 uv0_SurfaceNoise = i.uv_texcoord * _SurfaceNoise_ST.xy + _SurfaceNoise_ST.zw;
			float2 appendResult489 = (float2(_SurfaceNoiseScaleU , _SurfaceNoiseScaleV));
			float clampResult82 = clamp( ( temp_output_229_0 - temp_output_78_0 ) , 0.0 , 1.0 );
			#ifdef _SURFACENOISEENABLED_ON
				float staticSwitch501 = pow( ( tex2D( _SurfaceNoise, ( uv0_SurfaceNoise * appendResult489 ) ).r * _SurfaceNoiseAdd * clampResult82 ) , _SurfaceNoiseExp );
			#else
				float staticSwitch501 = 0.0;
			#endif
			float clampResult80 = clamp( ( ( temp_output_229_0 * clampResult109 * _HeightBoost * staticSwitch510 ) - temp_output_78_0 ) , 0.0 , 1.0 );
			float2 uv0_SecondMask = i.uv_texcoord * _SecondMask_ST.xy + _SecondMask_ST.zw;
			float2 appendResult478 = (float2(_SecondMaskScaleU , _SecondMaskScaleV));
			float clampResult253 = clamp( ( tex2D( _SecondMask, ( uv0_SecondMask * appendResult478 ) ).r + i.uv_tex4coord.w ) , 0.0 , 1.0 );
			float temp_output_291_0 = ( 1.0 - pow( ( 1.0 - clampResult253 ) , _SecondMaskExp ) );
			float2 appendResult254 = (float2(temp_output_291_0 , 0.0));
			float4 tex2DNode255 = tex2D( _SecondMaskProfile, appendResult254 );
			float clampResult481 = clamp( ( ( ( 1.0 - pow( ( 1.0 - tex2D( _LavaAppearMask, ( uv0_LavaAppearMask * appendResult494 ) ).r ) , _LavaAppearMaskExp ) ) + i.uv2_tex4coord2.z ) + ( ( 1.0 - ( staticSwitch501 + clampResult80 ) ) - 1.0 ) + ( temp_output_231_0 * tex2DNode255.r * _SecondMaskEdgeGlow ) ) , 0.0 , 1.0 );
			float smoothstepResult19 = smoothstep( _RampSmoothstepMin , _RampSmoothstepMax , clampResult481);
			float clampResult403 = clamp( ( pow( ( 1.0 - temp_output_229_0 ) , _LavaOnlyInValleysExp ) + _LavaOnlyInValleysNegate ) , 0.0 , 1.0 );
			float2 appendResult21 = (float2(( 1.0 - pow( ( 1.0 - ( _RampOffsetMultiply * staticSwitch504 * smoothstepResult19 * clampResult403 ) ) , _RampOffsetExp ) ) , 0.0));
			float clampResult179 = clamp( ( ( ( 1.0 - temp_output_229_0 ) * ( _ValleysEmissionBoostAmount + 1.0 ) ) - _ValleysEmissionBoostAmount ) , 0.0 , 1.0 );
			#ifdef _VALLEYSEMISSIONBOOSTENABLED_ON
				float staticSwitch449 = clampResult179;
			#else
				float staticSwitch449 = temp_output_228_0;
			#endif
			o.Emission = ( lerpResult99 + ( tex2D( _Ramp, appendResult21 ) * _FinalPower * _RampColorTint * ( ( ( 1.0 - staticSwitch449 ) * temp_output_231_0 * _ValleysEmissionBoostBloom ) + staticSwitch449 + 1.0 ) * ( clampResult481 * smoothstepResult19 ) * i.uv2_tex4coord2.y ) ).rgb;
			float2 uv0_OpacityMask = i.uv_texcoord * _OpacityMask_ST.xy + _OpacityMask_ST.zw;
			float clampResult300 = clamp( ( tex2D( _OpacityMask, (float2( 0,0 ) + (( (float2( -1,-1 ) + (uv0_OpacityMask - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) * _OpacityMaskScaler ) - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 ))) ).r * _OpacityMaskPower ) , 0.0 , 1.0 );
			float clampResult531 = clamp( ( 1.0 - pow( ( 1.0 - clampResult300 ) , _OpacityMaskExp ) ) , 0.001 , 0.999 );
			float clampResult240 = clamp( ( _FinalOpacityPower * clampResult531 * tex2DNode255.g ) , 0.0 , 1.0 );
			float2 appendResult318 = (float2(_DissolveTextureScaleU , _DissolveTextureScaleV));
			float2 temp_output_326_0 = (( i.uv_texcoord * appendResult318 * (_DissolveTextureRandomMin + (i.uv_tex4coord.z - 0.0) * (_DissolveTextureRandomMax - _DissolveTextureRandomMin) / (120.0 - 0.0)) )*1.0 + i.uv_tex4coord.z);
			float4 tex2DNode329 = tex2D( _DissolveTexture, temp_output_326_0 );
			#ifdef _DISSOLVETEXTUREFLIP_ON
				float staticSwitch331 = ( 1.0 - tex2DNode329.r );
			#else
				float staticSwitch331 = tex2DNode329.r;
			#endif
			float clampResult347 = clamp( ( 1.0 - pow( ( 1.0 - pow( staticSwitch331 , _DissolveExp ) ) , (1.0 + (i.uv2_tex4coord2.x - 0.0) * (_DissolveExpReversed - 1.0) / (1.0 - 0.0)) ) ) , 0.0 , 1.0 );
			float temp_output_338_0 = (-1.0 + (i.uv2_tex4coord2.x - 0.0) * (1.0 - -1.0) / (1.0 - 0.0));
			float clampResult350 = clamp( ( clampResult347 + temp_output_338_0 ) , 0.0 , 1.0 );
			float clampResult277 = clamp( ( clampResult240 - clampResult350 ) , 0.0 , 1.0 );
			float clampResult237 = clamp( ( 1.0 - pow( ( 1.0 - ( clampResult277 * i.vertexColor.a ) ) , _FinalOpacityExp ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult237;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;88.27051;835.7499;1.274537;True;False
Node;AmplifyShaderEditor.CommentaryNode;441;-4148.098,-448.3985;Inherit;False;1414.542;591.9119;;13;140;139;153;137;152;138;135;141;136;482;483;485;486;Cracks;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;483;-4102.399,-156.5974;Inherit;False;Property;_CracksScaleV;Cracks Scale V;61;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;482;-4104.399,-227.5974;Inherit;False;Property;_CracksScaleU;Cracks Scale U;60;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;134;-5712.017,143.3597;Inherit;False;0;69;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;302;-2743.093,3505.969;Inherit;False;2331.969;568.1284;;17;274;232;281;283;253;290;289;288;291;254;255;478;477;479;480;499;513;Second Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;141;-4107.474,-365.1471;Inherit;False;0;135;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;485;-3910.398,-204.5974;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;480;-2714.989,3845.136;Inherit;False;Property;_SecondMaskScaleV;Second Mask Scale V;24;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;479;-2711.989,3766.136;Inherit;False;Property;_SecondMaskScaleU;Second Mask Scale U;23;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-3712.553,-97.27182;Inherit;False;Property;_CracksNegate;Cracks Negate;62;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;69;-5420.692,110.1367;Inherit;True;Property;_PackedTex;PackedTex;9;0;Create;True;0;0;False;0;False;-1;None;10bd4fb151e61ba45ae2536661051240;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;486;-3459.138,-284.2304;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;135;-3527.553,-385.2718;Inherit;True;Property;_Cracks;Cracks;59;0;Create;True;0;0;False;0;False;-1;None;7c0bc042e09e8dd46bf9cc23c51c3f8c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;229;-4926.354,86.12152;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;274;-2725.842,3578.149;Inherit;False;0;232;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;443;-3155.449,321.0294;Inherit;False;506.2568;183;;2;78;79;Slope Control;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-3526.421,17.26191;Inherit;False;Property;_CracksNegateSlope;Cracks Negate Slope;63;0;Create;True;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;138;-3403.553,-171.2717;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;303;-4951.468,4218.841;Inherit;False;4580.409;1828.732;;29;350;349;347;345;343;341;339;338;336;335;334;333;332;331;330;329;328;326;323;320;319;318;315;312;311;309;308;351;521;Dissolve For Subtraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;444;-2359.652,-40.72307;Inherit;False;1457.353;531.9969;;12;55;52;56;54;24;82;81;32;487;488;489;490;Surface Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;478;-2456.309,3797.266;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;445;-3174.075,775.7416;Inherit;False;1548.444;528.5823;;7;108;107;89;71;77;80;109;Height;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;477;-2374.623,3653.132;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;231;-4927.241,517.7049;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-3184.553,-253.272;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-3216.924,-42.63816;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;487;-2324.546,180.8579;Inherit;False;Property;_SurfaceNoiseScaleU;Surface Noise Scale U;51;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-3137.449,413.4771;Inherit;False;Property;_HeightSlopeControl;Height Slope Control;10;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;488;-2324.546,257.8579;Inherit;False;Property;_SurfaceNoiseScaleV;Surface Noise Scale V;52;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;315;-4444.98,5622.945;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;312;-4088.104,5891.527;Inherit;False;Property;_DissolveTextureRandomMax;Dissolve Texture Random Max;70;0;Create;True;0;0;False;0;False;1.1;1.1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;309;-4116.888,5492.407;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;68;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;308;-4115.888,5413.407;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;67;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;311;-4092.104,5811.527;Inherit;False;Property;_DissolveTextureRandomMin;Dissolve Texture Random Min;69;0;Create;True;0;0;False;0;False;0.9;0.9;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;489;-2058.546,207.8579;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;318;-3800.888,5445.407;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;319;-3754.907,5734.132;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;523;-2695.692,3014.63;Inherit;False;0;293;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-2818.192,371.0294;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;230;-4927.097,303.6108;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;398;-3598.688,-1373.86;Inherit;False;1135.386;387.9904;;7;391;392;394;395;396;393;397;Lava Noise Distortion Slope;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-3022.553,-128.2717;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;358;-1657.339,-931.3571;Inherit;False;1521.886;452.7298;;13;495;494;493;492;27;357;16;356;355;354;352;491;515;Lava Appear Control;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-3124.075,1078.899;Inherit;False;Property;_HeightMapNegate;Height Map Negate;12;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;232;-2181.13,3575.321;Inherit;True;Property;_SecondMask;Second Mask;22;0;Create;True;0;0;False;0;False;-1;None;fc46fa9da10c1bc47a7e677aa5d431c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-2311.747,40.93868;Inherit;False;0;24;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;513;-2189.248,3874.071;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;512;-2698.064,-356.2214;Inherit;False;404.4404;280.2144;;2;511;510;Cracks Switch;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;380;-2265.64,-2198.597;Inherit;False;2159.703;906.9894;;26;362;374;373;368;370;377;363;378;365;379;376;364;385;366;383;384;382;381;469;470;471;472;473;474;475;476;Lava Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;320;-3898.255,5228.752;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;492;-1642.909,-741.3259;Inherit;False;Property;_LavaAppearMaskScaleU;Lava Appear Mask Scale U;31;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;493;-1640.909,-660.3259;Inherit;False;Property;_LavaAppearMaskScaleV;Lava Appear Mask Scale V;32;0;Create;True;0;0;False;0;False;1;1.54;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;490;-1866.546,122.8579;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;323;-3524.628,5268.646;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;528;-2487.814,3183.285;Inherit;False;Property;_OpacityMaskScaler;Opacity Mask Scaler;19;0;Create;True;0;0;False;0;False;1;1;0.5;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;526;-2391.067,3015.937;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;476;-2246.161,-1929.136;Inherit;False;Property;_LavaNoiseDistortionScaleV;Lava Noise Distortion Scale V;42;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;283;-1882.553,3681.623;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;511;-2648.064,-306.2214;Inherit;False;Constant;_Float4;Float 4;78;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;81;-1709.107,326.8181;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;475;-2247.161,-2012.136;Inherit;False;Property;_LavaNoiseDistortionScaleU;Lava Noise Distortion Scale U;41;0;Create;True;0;0;False;0;False;1;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;107;-2848.716,1061.327;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;391;-3548.688,-1323.86;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;140;-2893.553,-127.2717;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;494;-1367.909,-710.3259;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;491;-1641.556,-869.0692;Inherit;False;0;352;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;527;-2089.056,3068.233;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;326;-3320.413,5459.953;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;24;-1703.965,23.95278;Inherit;True;Property;_SurfaceNoise;Surface Noise;50;0;Create;True;0;0;False;0;False;-1;None;165e54ca813c80b429e4e3e6c6f04a59;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;384;-2228.906,-2153.319;Inherit;False;0;376;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;382;-2136.601,-1777.725;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;510;-2580.624,-214.007;Inherit;False;Property;_CracksEnabled;Cracks Enabled;58;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1692.296,225.6683;Inherit;False;Property;_SurfaceNoiseAdd;Surface Noise Add;53;0;Create;True;0;0;False;0;False;0;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-2434.527,866.8873;Inherit;False;Property;_HeightBoost;Height Boost;11;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;381;-2206.813,-1618.411;Inherit;False;Property;_LavaNoiseDistortionScrollSpeed;Lava Noise Distortion Scroll Speed;44;0;Create;True;0;0;False;0;False;0.075;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;473;-1947.222,-1984.127;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;109;-2697.693,1056.324;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;253;-1756.474,3678.839;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;82;-1562.036,326.8878;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;392;-3394.688,-1320.86;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;393;-3439.24,-1106.737;Inherit;False;Property;_LavaDistortionSlopeStyle;Lava Distortion Slope Style;46;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;329;-2575.429,4797.69;Inherit;True;Property;_DissolveTexture;Dissolve Texture;65;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;529;-1934.782,3053.852;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;-1,-1;False;2;FLOAT2;1,1;False;3;FLOAT2;0,0;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;396;-3157.302,-1087.87;Inherit;False;Property;_LavaDistortionSlopeNegate;Lava Distortion Slope Negate;45;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;289;-1791.929,3837.746;Inherit;False;Property;_SecondMaskExp;Second Mask Exp;25;0;Create;True;0;0;False;0;False;1;1;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1369.602,282.4295;Inherit;False;Property;_SurfaceNoiseExp;Surface Noise Exp;54;0;Create;True;0;0;False;0;False;1;1;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;503;-856.8604,86.28298;Inherit;False;531.1068;288.001;;2;501;502;Surface Noise Switch;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;470;-1644.429,-1721.205;Inherit;False;Property;_LavaNoiseScaleU;Lava Noise Scale U;36;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-2184.428,827.6906;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;471;-1649.429,-1646.205;Inherit;False;Property;_LavaNoiseScaleV;Lava Noise Scale V;37;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;474;-1867.161,-2100.136;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;301;-1606.022,2972.311;Inherit;False;1194.1;420.5183;Comment;8;293;299;298;300;297;294;296;295;Opacity Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;495;-1232.909,-827.3259;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-1346.86,96.2848;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;290;-1614.107,3675.883;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;394;-3031.857,-1229.614;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;383;-1887.034,-1698.883;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;293;-1556.022,3022.311;Inherit;True;Property;_OpacityMask;Opacity Mask;18;0;Create;True;0;0;False;0;False;-1;None;2bc344f77a919034c93cdae977e870d6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;330;-2277.73,4973.874;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;299;-1497.504,3217.152;Inherit;False;Property;_OpacityMaskPower;Opacity Mask Power;20;0;Create;True;0;0;False;0;False;1;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;395;-2781.396,-1178.494;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;352;-1102.459,-871.0929;Inherit;True;Property;_LavaAppearMask;Lava Appear Mask;30;0;Create;True;0;0;False;0;False;-1;None;6acb2dfea1fd7c24d98057f410aaa37f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;366;-1710.215,-1390.132;Inherit;False;Property;_LavaNoiseScrollSpeed;Lava Noise Scroll Speed;39;0;Create;True;0;0;False;0;False;0;0.125;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;385;-1731.657,-2003.216;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;288;-1445.404,3671.323;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;502;-806.8604,136.283;Inherit;False;Constant;_Float1;Float 1;75;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;364;-1628.783,-1539.974;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;55;-1052.948,148.7995;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;362;-1657.57,-1841.224;Inherit;False;0;368;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;77;-1976.075,831.4302;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;472;-1422.429,-1690.205;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;298;-1213.734,3084.617;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;331;-2158.73,4822.873;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;66;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;355;-806.8729,-840.5468;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;397;-2638.302,-1177.87;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;332;-2019.981,4940.981;Inherit;False;Property;_DissolveExp;Dissolve Exp;74;0;Create;True;0;0;False;0;False;6.47;6.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;354;-1077.874,-672.5469;Inherit;False;Property;_LavaAppearMaskExp;Lava Appear Mask Exp;33;0;Create;True;0;0;False;0;False;1;8;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;80;-1800.631,825.7416;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;469;-1253.552,-1824.331;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;365;-1379.214,-1461.132;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;376;-1532.977,-2129.592;Inherit;True;Property;_LavaNoiseDistortion;Lava Noise Distortion;40;0;Create;True;0;0;False;0;False;-1;None;fc46fa9da10c1bc47a7e677aa5d431c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;501;-623.7535,236.284;Inherit;False;Property;_SurfaceNoiseEnabled;Surface Noise Enabled;49;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;447;-817.9451,760.4119;Inherit;False;651.9713;386.252;;4;4;6;53;5;Height with Surface Noise added;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;379;-1538.834,-1931.868;Inherit;False;Property;_LavaNoiseDistortionAmount;Lava Noise Distortion Amount;43;0;Create;True;0;0;False;0;False;0;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;291;-1286.96,3672.463;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;335;-2094.974,4290.855;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;333;-1792.945,4849.925;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;336;-1919.301,4552.9;Inherit;False;Property;_DissolveExpReversed;Dissolve Exp Reversed;75;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-767.9454,810.4119;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;363;-1152.777,-1657.473;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;442;-524.5406,1846.488;Inherit;False;561.8946;235.392;;2;418;409;Second Mask Edge Glow;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;254;-1202.739,3874.535;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;378;-1188.327,-2013.669;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;356;-652.8734,-841.5468;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;300;-1055.778,3084.635;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;294;-907.1362,3088.68;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;297;-1131.397,3277.829;Inherit;False;Property;_OpacityMaskExp;Opacity Mask Exp;21;0;Create;True;0;0;False;0;False;0.2;6;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;341;-1486.14,4635.963;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;339;-1620.801,4385.932;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;408;-3601.018,-840.3943;Inherit;False;895.5864;327.6767;;6;399;406;405;401;403;407;Lava Only In Valleys;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;515;-528.2813,-660.0326;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;418;-474.5406,1966.88;Inherit;False;Property;_SecondMaskEdgeGlow;Second Mask Edge Glow;26;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;255;-1069.739,3846.535;Inherit;True;Property;_SecondMaskProfile;Second Mask Profile;28;0;Create;True;0;0;False;0;False;-1;None;5bd9da326da7b6c469a38f9bbc4d9ec2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;377;-928.4485,-1920.565;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;357;-508.8733,-841.5468;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-557.974,813.6641;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-539.9739,1031.664;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;296;-754.2307,3092.079;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;343;-1314.425,4637.306;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;227;-3482.802,1861.518;Inherit;False;1838.138;528.1398;;12;181;177;178;179;180;166;85;84;86;88;449;509;Valleys Emission Boost;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-266.2545,-816.5977;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;368;-755.3597,-1956.913;Inherit;True;Property;_LavaNoise;Lava Noise;35;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;386;-18.35864,-622.0073;Inherit;False;2225.507;802.2448;;18;387;48;49;65;50;22;21;361;61;62;19;14;216;215;388;389;390;481;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;-131.646,1896.488;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;370;-725.9297,-1761.333;Inherit;False;Property;_LavaNoiseNegate;Lava Noise Negate;38;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;399;-3442.983,-790.3943;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;406;-3551.018,-719.4449;Inherit;False;Property;_LavaOnlyInValleysExp;Lava Only In Valleys Exp;48;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-339.9739,819.6641;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;506;-36.01318,-1842.192;Inherit;False;476.4563;323.8367;;2;504;505;Lava Noise Switch;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;280;-22.74086,2944.965;Inherit;False;1779.033;436.4939;;12;239;238;240;276;233;234;235;237;278;279;236;277;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;295;-609.9211,3091.846;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;334;-1965.663,5334.045;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;345;-1157.467,4639.988;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-3432.801,2274.661;Inherit;False;Property;_ValleysEmissionBoostAmount;Valleys Emission Boost Amount;56;0;Create;True;0;0;False;0;False;80;60;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;405;-3263.9,-772.647;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;532;-334.551,3090.455;Inherit;False;225;206;;1;531;Fix Artifacts;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;407;-3413.349,-627.7176;Inherit;False;Property;_LavaOnlyInValleysNegate;Lava Only In Valleys Negate;47;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;31.5076,-384.5198;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;373;-418.1404,-1859.98;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;531;-284.551,3140.455;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.001;False;2;FLOAT;0.999;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;338;-1588.565,5333.391;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;347;-994.4059,4643.103;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;27.25913,2994.963;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;1;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;215;38.79141,-74.83443;Inherit;False;Property;_RampSmoothstepMin;Ramp Smoothstep Min;7;0;Create;True;0;0;False;0;False;0.25;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;39.93151,12.3669;Inherit;False;Property;_RampSmoothstepMax;Ramp Smoothstep Max;8;0;Create;True;0;0;False;0;False;0.75;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;505;13.98682,-1792.192;Inherit;False;Constant;_Float2;Float 2;76;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;481;314.5646,-260.5109;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;374;-270.0673,-1863.98;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-3197.849,2134.805;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;401;-3014.414,-765.0677;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;166;-3234.321,2014.164;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;-3002.84,2084.519;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;21;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;349;-718.368,5147.058;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;238;276.5055,2997.658;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;403;-2880.431,-765.7408;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;19;320.1804,-71.40054;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;504;132.4431,-1656.356;Inherit;False;Property;_LavaNoiseEnabled;Lava Noise Enabled;34;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;215.2098,-559.5596;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;5;0;Create;True;0;0;False;0;False;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;240;414.2169,2998.54;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;350;-546.0588,5150.762;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;178;-2844.625,2084.519;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;515.6601,-514.9996;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;387;502.1882,-358.1519;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;6;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;388;655.787,-515.8093;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;276;575.1777,3128.063;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;179;-2685.441,2090.039;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;446;-503.1881,-2951.598;Inherit;False;914.3829;391.064;;5;224;162;163;156;225;Actual Albedo Map;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;228;-4928.982,-127.2352;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;225;-453.188,-2675.534;Inherit;False;Property;_AlbedoMapExp;Albedo Map Exp;16;0;Create;True;0;0;False;0;False;1;1;0.5;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;279;769.2373,3179.458;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;-387.4359,-2901.598;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;449;-2536.579,2040.003;Inherit;False;Property;_ValleysEmissionBoostEnabled;Valleys Emission Boost Enabled;55;0;Create;True;0;0;False;0;False;0;1;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;389;815.787,-515.8093;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;277;779.116,3012.611;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;390;958.787,-515.8093;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;509;-2339.615,2251.123;Inherit;False;Property;_ValleysEmissionBoostBloom;Valleys Emission Boost Bloom;57;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;952.9886,3011.684;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;85;-2164.032,1981.944;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;224;-127.7605,-2773.68;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1964.824,1920.968;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;233;1126.148,3012.456;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-1960.256,2114.745;Inherit;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;361;1018.531,-39.66951;Inherit;False;219;183;;1;360;Double Multiply;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;236;960.5719,3186.38;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;2;0;Create;True;0;0;False;0;False;1;2;0.2;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;162;64.9781,-2726.285;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;448;724.7354,-1754.896;Inherit;False;787.2184;771.4972;;4;99;101;46;500;Non-emissive Surface Coloring;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;1115.621,-517.9387;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;360;1068.532,10.3304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;234;1277.076,3013.804;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;163;236.1949,-2718.937;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;65;1331.813,-235.4709;Inherit;False;Property;_RampColorTint;Ramp Color Tint;4;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;46;798.4515,-1512.63;Inherit;False;Property;_AlbedoColor;Albedo Color;13;0;Create;True;0;0;False;0;False;0.6226415,0.4755985,0.4258633,1;0.4716981,0.4716981,0.4716981,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;498;-1754.573,6198.441;Inherit;False;922.5973;677.4697;;6;348;346;344;342;340;304;Dissolve Mask (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;50;1395.21,-320.6153;Inherit;False;Property;_FinalPower;Final Power;0;0;Create;True;0;0;False;0;False;4;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-1779.37,1996.919;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;514;1233.053,201.557;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;1275.439,-532.3563;Inherit;True;Property;_Ramp;Ramp;3;0;Create;True;0;0;False;0;False;-1;None;59b03cb4cf1aac7499db4faaf3e7616c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;101;796.3287,-1704.896;Inherit;False;Property;_AlbedoColor2;Albedo Color 2;14;0;Create;True;0;0;False;0;False;0,0,0,1;0.1132057,0.1132057,0.1132057,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;304;-1704.573,6653.234;Inherit;False;307;165;;1;337;If mask is empty, keep it at 1f;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;497;-7094.659,4530.776;Inherit;False;1938.881;742.1992;;13;305;306;307;313;310;314;322;316;317;321;324;325;327;Radial Dissolve (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;1736.112,-179.0753;Inherit;True;6;6;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;99;1186.042,-1419.359;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;500;750.4535,-1313.056;Inherit;False;363;308;;1;155;Switch Disabled;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;499;-792.588,3623.949;Inherit;False;363.2511;158.936;;1;275;Switch Disabled;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;235;1425.414,3013.302;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;468;-2569.12,-2964.32;Inherit;False;921.859;304.5781;;4;465;464;466;467;Height Affects Albedo (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;517;-1472.766,-297.6934;Inherit;False;Property;_VSvsGLOBALControlSwitch;VS vs GLOBAL Control Switch;78;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;281;-2268.696,3782.901;Inherit;False;Property;_SecondMaskVSEmilationTEST;Second Mask VS Emilation TEST;29;0;Create;True;0;0;False;0;False;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;314;-6282.169,4714.977;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;351;-2407.732,4614.769;Inherit;False;Property;_DissolveVSEmilationTEST;Dissolve  VS Emilation TEST;64;0;Create;True;0;0;False;0;False;0;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;328;-2948.679,4824.496;Inherit;False;Property;_DissolveTextureRadial;Dissolve Texture Radial;71;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;316;-6367.958,5072.176;Inherit;False;Property;_DissolveTextureRadialScaleU;Dissolve Texture Radial Scale U;72;0;Create;True;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;464;-2519.12,-2902.855;Inherit;False;Property;_AlbedoMapAffectedByHeight;Albedo Map Affected By Height;17;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-594.2173,-738.9269;Inherit;False;Property;_LavaAppearProgress;Lava Appear Progress;79;0;Create;True;0;0;False;0;False;0.31;0.15;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;275;-766.6006,3667.51;Inherit;False;Property;_SecondMaskProfileEnabled;Second Mask Profile Enabled;27;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;521;-4393.887,5529.623;Inherit;False;Constant;_Float5;Float 5;79;0;Create;True;0;0;False;0;False;60;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;466;-2035.893,-2914.32;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;306;-6822.356,4585.459;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;342;-1329.135,6248.441;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;305;-7044.659,4584.159;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;337;-1654.573,6703.234;Inherit;False;Property;_DissolveMaskScale;Dissolve Mask Scale;77;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;321;-6028.683,4824.175;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;155;800.4535,-1248.166;Inherit;True;Property;_AlbedoMapEnabled;Albedo Map Enabled;15;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;2023.969,-196.8505;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;346;-1157.874,6347.836;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;327;-5322.778,4726.377;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;237;1581.292,3014.574;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;325;-5771.267,4597.676;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;317;-6366.665,5157.975;Inherit;False;Property;_DissolveTextureRadialScaleV;Dissolve Texture Radial Scale V;73;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;310;-6254.894,4922.976;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;340;-1671.469,6446.022;Inherit;True;Property;_DissolveMask;Dissolve Mask;76;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;307;-6573.373,4785.175;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OneMinusNode;465;-2208.761,-2896.319;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;322;-6014.377,4580.776;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;467;-1903.26,-2912.742;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;313;-6263.979,4987.977;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;348;-1018.774,6347.836;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;-5766.063,4899.275;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;344;-1338.73,6565.055;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2723.738,-170.5869;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/DissolveParticleGroundPacked;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;485;0;482;0
WireConnection;485;1;483;0
WireConnection;69;1;134;0
WireConnection;486;0;141;0
WireConnection;486;1;485;0
WireConnection;135;1;486;0
WireConnection;229;0;69;2
WireConnection;138;0;136;0
WireConnection;478;0;479;0
WireConnection;478;1;480;0
WireConnection;477;0;274;0
WireConnection;477;1;478;0
WireConnection;231;0;69;4
WireConnection;137;0;135;1
WireConnection;137;1;138;0
WireConnection;153;0;229;0
WireConnection;153;1;152;0
WireConnection;489;0;487;0
WireConnection;489;1;488;0
WireConnection;318;0;308;0
WireConnection;318;1;309;0
WireConnection;319;0;315;3
WireConnection;319;3;311;0
WireConnection;319;4;312;0
WireConnection;78;0;231;0
WireConnection;78;1;79;0
WireConnection;230;0;69;3
WireConnection;139;0;137;0
WireConnection;139;1;136;0
WireConnection;139;2;153;0
WireConnection;232;1;477;0
WireConnection;490;0;32;0
WireConnection;490;1;489;0
WireConnection;323;0;320;0
WireConnection;323;1;318;0
WireConnection;323;2;319;0
WireConnection;526;0;523;0
WireConnection;283;0;232;1
WireConnection;283;1;513;4
WireConnection;81;0;229;0
WireConnection;81;1;78;0
WireConnection;107;0;230;0
WireConnection;107;1;108;0
WireConnection;391;0;229;0
WireConnection;140;0;139;0
WireConnection;494;0;492;0
WireConnection;494;1;493;0
WireConnection;527;0;526;0
WireConnection;527;1;528;0
WireConnection;326;0;323;0
WireConnection;326;2;315;3
WireConnection;24;1;490;0
WireConnection;510;1;511;0
WireConnection;510;0;140;0
WireConnection;473;0;475;0
WireConnection;473;1;476;0
WireConnection;109;0;107;0
WireConnection;253;0;283;0
WireConnection;82;0;81;0
WireConnection;392;0;391;0
WireConnection;329;1;326;0
WireConnection;529;0;527;0
WireConnection;71;0;229;0
WireConnection;71;1;109;0
WireConnection;71;2;89;0
WireConnection;71;3;510;0
WireConnection;474;0;384;0
WireConnection;474;1;473;0
WireConnection;495;0;491;0
WireConnection;495;1;494;0
WireConnection;52;0;24;1
WireConnection;52;1;54;0
WireConnection;52;2;82;0
WireConnection;290;0;253;0
WireConnection;394;0;392;0
WireConnection;394;1;231;0
WireConnection;394;2;393;0
WireConnection;383;0;382;2
WireConnection;383;1;381;0
WireConnection;293;1;529;0
WireConnection;330;0;329;1
WireConnection;395;0;394;0
WireConnection;395;1;396;0
WireConnection;352;1;495;0
WireConnection;385;0;474;0
WireConnection;385;1;383;0
WireConnection;288;0;290;0
WireConnection;288;1;289;0
WireConnection;55;0;52;0
WireConnection;55;1;56;0
WireConnection;77;0;71;0
WireConnection;77;1;78;0
WireConnection;472;0;470;0
WireConnection;472;1;471;0
WireConnection;298;0;293;1
WireConnection;298;1;299;0
WireConnection;331;1;329;1
WireConnection;331;0;330;0
WireConnection;355;0;352;1
WireConnection;397;0;395;0
WireConnection;80;0;77;0
WireConnection;469;0;362;0
WireConnection;469;1;472;0
WireConnection;365;0;364;2
WireConnection;365;1;366;0
WireConnection;376;1;385;0
WireConnection;501;1;502;0
WireConnection;501;0;55;0
WireConnection;291;0;288;0
WireConnection;333;0;331;0
WireConnection;333;1;332;0
WireConnection;53;0;501;0
WireConnection;53;1;80;0
WireConnection;363;0;469;0
WireConnection;363;1;365;0
WireConnection;254;0;291;0
WireConnection;378;0;376;1
WireConnection;378;1;379;0
WireConnection;378;2;397;0
WireConnection;356;0;355;0
WireConnection;356;1;354;0
WireConnection;300;0;298;0
WireConnection;294;0;300;0
WireConnection;341;0;333;0
WireConnection;339;0;335;1
WireConnection;339;4;336;0
WireConnection;255;1;254;0
WireConnection;377;0;378;0
WireConnection;377;1;363;0
WireConnection;357;0;356;0
WireConnection;4;0;53;0
WireConnection;296;0;294;0
WireConnection;296;1;297;0
WireConnection;343;0;341;0
WireConnection;343;1;339;0
WireConnection;27;0;357;0
WireConnection;27;1;515;3
WireConnection;368;1;377;0
WireConnection;409;0;231;0
WireConnection;409;1;255;1
WireConnection;409;2;418;0
WireConnection;399;0;229;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;295;0;296;0
WireConnection;345;0;343;0
WireConnection;405;0;399;0
WireConnection;405;1;406;0
WireConnection;14;0;27;0
WireConnection;14;1;5;0
WireConnection;14;2;409;0
WireConnection;373;0;368;1
WireConnection;373;1;370;0
WireConnection;531;0;295;0
WireConnection;338;0;334;1
WireConnection;347;0;345;0
WireConnection;481;0;14;0
WireConnection;374;0;373;0
WireConnection;181;0;180;0
WireConnection;401;0;405;0
WireConnection;401;1;407;0
WireConnection;166;0;229;0
WireConnection;177;0;166;0
WireConnection;177;1;181;0
WireConnection;349;0;347;0
WireConnection;349;1;338;0
WireConnection;238;0;239;0
WireConnection;238;1;531;0
WireConnection;238;2;255;2
WireConnection;403;0;401;0
WireConnection;19;0;481;0
WireConnection;19;1;215;0
WireConnection;19;2;216;0
WireConnection;504;1;505;0
WireConnection;504;0;374;0
WireConnection;240;0;238;0
WireConnection;350;0;349;0
WireConnection;178;0;177;0
WireConnection;178;1;180;0
WireConnection;61;0;62;0
WireConnection;61;1;504;0
WireConnection;61;2;19;0
WireConnection;61;3;403;0
WireConnection;388;0;61;0
WireConnection;276;0;240;0
WireConnection;276;1;350;0
WireConnection;179;0;178;0
WireConnection;228;0;69;1
WireConnection;156;0;228;0
WireConnection;156;1;510;0
WireConnection;156;2;109;0
WireConnection;449;1;228;0
WireConnection;449;0;179;0
WireConnection;389;0;388;0
WireConnection;389;1;387;0
WireConnection;277;0;276;0
WireConnection;390;0;389;0
WireConnection;278;0;277;0
WireConnection;278;1;279;4
WireConnection;85;0;449;0
WireConnection;224;0;156;0
WireConnection;224;1;225;0
WireConnection;84;0;85;0
WireConnection;84;1;231;0
WireConnection;84;2;509;0
WireConnection;233;0;278;0
WireConnection;162;0;224;0
WireConnection;162;1;78;0
WireConnection;21;0;390;0
WireConnection;360;0;481;0
WireConnection;360;1;19;0
WireConnection;234;0;233;0
WireConnection;234;1;236;0
WireConnection;163;0;162;0
WireConnection;86;0;84;0
WireConnection;86;1;449;0
WireConnection;86;2;88;0
WireConnection;22;1;21;0
WireConnection;49;0;22;0
WireConnection;49;1;50;0
WireConnection;49;2;65;0
WireConnection;49;3;86;0
WireConnection;49;4;360;0
WireConnection;49;5;514;2
WireConnection;99;0;101;0
WireConnection;99;1;46;0
WireConnection;99;2;163;0
WireConnection;235;0;234;0
WireConnection;314;0;307;0
WireConnection;314;1;307;1
WireConnection;328;1;326;0
WireConnection;328;0;327;0
WireConnection;275;1;291;0
WireConnection;275;0;255;2
WireConnection;466;0;109;0
WireConnection;466;1;465;0
WireConnection;306;0;305;0
WireConnection;342;0;338;0
WireConnection;342;1;337;0
WireConnection;321;0;314;0
WireConnection;321;1;310;0
WireConnection;321;2;313;0
WireConnection;155;1;80;0
WireConnection;155;0;163;0
WireConnection;48;0;99;0
WireConnection;48;1;49;0
WireConnection;346;0;342;0
WireConnection;346;1;344;0
WireConnection;327;0;325;0
WireConnection;327;1;324;0
WireConnection;237;0;235;0
WireConnection;325;0;322;0
WireConnection;325;1;316;0
WireConnection;307;0;306;0
WireConnection;465;0;464;0
WireConnection;322;0;306;0
WireConnection;467;0;466;0
WireConnection;348;0;346;0
WireConnection;324;0;321;0
WireConnection;324;1;317;0
WireConnection;344;0;340;1
WireConnection;344;1;337;0
WireConnection;0;2;48;0
WireConnection;0;9;237;0
ASEEND*/
//CHKSM=24D4852D996E4A572FA919E47C5462FCC339E0EF