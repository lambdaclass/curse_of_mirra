// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/CenterCurve"
{
	Properties
	{
		_FinalColor("Final Color", Color) = (1,0,0,1)
		_FinalPower("Final Power", Range( 0 , 80)) = 0
		_FinalOpacityPower("Final Opacity Power", Range( 1 , 8)) = 1
		_FinalOpacityExp("Final Opacity Exp", Range( 0.2 , 8)) = 1
		[Toggle(_RAMPENABLED1_ON)] _RampEnabled1("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		[Toggle(_RAMPAFFECTEDBYVERTEXCOLOR1_ON)] _RampAffectedByVertexColor1("Ramp Affected By Vertex Color", Float) = 0
		_RampOffsetMultiply("Ramp Offset Multiply", Range( 1 , 8)) = 1
		_RampOffsetExp1("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_MainMask("Main Mask", 2D) = "white" {}
		[Toggle(_MAINMASKCHANNELSADDORMULTIPLY_ON)] _MainMaskChannelsAddorMultiply("Main Mask Channels Add or Multiply", Float) = 1
		_MainMaskChannelsMultiply("Main Mask Channels Multiply", Vector) = (1,0,0,0)
		_MainMaskChannelsAdd("Main Mask Channels Add", Vector) = (0,0,0,0)
		[Toggle(_FRESNELMASKENABLED_ON)] _FresnelMaskEnabled("Fresnel Mask Enabled", Float) = 1
		_FresnelMaskPower("Fresnel Mask Power", Float) = 1
		_FresnelMaskScale("Fresnel Mask Scale", Float) = 1
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScaleU("Noise 01 Scale U", Float) = 1
		_Noise01ScaleV("Noise 01 Scale V", Float) = 1
		_Noise01Negate("Noise 01 Negate", Range( 0 , 1)) = 0
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = 0
		_Noise01Distortion("Noise 01 Distortion", 2D) = "white" {}
		_Noise01DistortionScaleU("Noise 01 Distortion Scale U", Float) = 1
		_Noise01DistortionScaleV("Noise 01 Distortion Scale V", Float) = 1
		_Noise01DistortionPower("Noise 01 Distortion Power", Range( 0 , 1)) = 0
		_SecondMask("Second Mask", 2D) = "white" {}
		_SecondMaskRotation("Second Mask Rotation", Range( 0 , 1)) = 0
		_SecondMaskLifetimeStretch("Second Mask Lifetime Stretch", Range( 0.2 , 4)) = 1
		_SecondMaskDistortion("Second Mask Distortion", 2D) = "white" {}
		_SecondMaskDistortionScaleU("Second Mask Distortion Scale U", Float) = 1
		_SecondMaskDistortionScaleV("Second Mask Distortion Scale V", Float) = 1
		_SecondMaskDistortionAmount("Second Mask Distortion Amount", Range( 0 , 1)) = 0
		_SecondMaskDistortionRemapMin("Second Mask Distortion Remap Min", Float) = -0.25
		_SecondMaskDistortionRemapMax("Second Mask Distortion Remap Max", Float) = 0.75
		[Toggle(_SECONDMASKFLIPUV_ON)] _SecondMaskFlipUV("Second Mask Flip UV", Float) = 0
		[Toggle(_SECONDMASKFLIPANIMATION_ON)] _SecondMaskFlipAnimation("Second Mask Flip Animation", Float) = 0
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_DissolveTextureScaleU("Dissolve Texture Scale U", Float) = 1
		_DissolveTextureScaleV("Dissolve Texture Scale V", Float) = 1
		_DissolveTextureRemapMax("Dissolve Texture Remap Max", Range( 0.5 , 1)) = 0.75
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
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
		#pragma shader_feature _RAMPENABLED1_ON
		#pragma shader_feature _RAMPAFFECTEDBYVERTEXCOLOR1_ON
		#pragma shader_feature _FRESNELMASKENABLED_ON
		#pragma shader_feature _MAINMASKCHANNELSADDORMULTIPLY_ON
		#pragma shader_feature _SECONDMASKFLIPUV_ON
		#pragma shader_feature _SECONDMASKFLIPANIMATION_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float3 worldPos;
			float3 worldNormal;
			half ASEVFace : VFACE;
			INTERNAL_DATA
			float4 uv2_tex4coord2;
		};

		uniform float _FinalPower;
		uniform float4 _FinalColor;
		uniform float4 _RampColorTint;
		uniform sampler2D _Ramp;
		uniform float _FinalOpacityPower;
		uniform sampler2D _Noise01;
		uniform float4 _Noise01_ST;
		uniform float _Noise01ScaleU;
		uniform float _Noise01ScaleV;
		uniform float _Noise01ScrollSpeed;
		uniform sampler2D _Noise01Distortion;
		uniform float4 _Noise01Distortion_ST;
		uniform float _Noise01DistortionScaleU;
		uniform float _Noise01DistortionScaleV;
		uniform float _Noise01DistortionPower;
		uniform float _Noise01Negate;
		uniform float _FresnelMaskScale;
		uniform float _FresnelMaskPower;
		uniform sampler2D _MainMask;
		uniform float4 _MainMask_ST;
		uniform float4 _MainMaskChannelsMultiply;
		uniform float4 _MainMaskChannelsAdd;
		uniform sampler2D _SecondMask;
		uniform sampler2D _SecondMaskDistortion;
		uniform float4 _SecondMaskDistortion_ST;
		uniform float _SecondMaskDistortionScaleU;
		uniform float _SecondMaskDistortionScaleV;
		uniform float _SecondMaskDistortionRemapMin;
		uniform float _SecondMaskDistortionRemapMax;
		uniform float _SecondMaskDistortionAmount;
		uniform float4 _SecondMask_ST;
		uniform float _SecondMaskLifetimeStretch;
		uniform float _SecondMaskRotation;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _DissolveTextureScaleU;
		uniform float _DissolveTextureScaleV;
		uniform float _DissolveTextureRemapMax;
		uniform float _RampOffsetMultiply;
		uniform float _RampOffsetExp1;
		uniform float _FinalOpacityExp;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult168 = (float2(_Noise01ScaleU , _Noise01ScaleV));
			float2 break170 = ( uv0_Noise01 * appendResult168 );
			float2 appendResult33 = (float2(( break170.x + i.uv_tex4coord.z ) , ( break170.y + ( i.uv_tex4coord.z * 0.73 ) + ( _Time.y * _Noise01ScrollSpeed ) )));
			float2 uv0_Noise01Distortion = i.uv_texcoord * _Noise01Distortion_ST.xy + _Noise01Distortion_ST.zw;
			float2 appendResult165 = (float2(_Noise01DistortionScaleU , _Noise01DistortionScaleV));
			float clampResult136 = clamp( ( tex2D( _Noise01, ( appendResult33 + ( tex2D( _Noise01Distortion, ( uv0_Noise01Distortion * appendResult165 ) ).r * _Noise01DistortionPower ) ) ).r + _Noise01Negate ) , 0.0 , 1.0 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 switchResult12 = (((i.ASEVFace>0)?(ase_worldNormal):(-ase_worldNormal)));
			float fresnelNdotV5 = dot( switchResult12, ase_worldViewDir );
			float fresnelNode5 = ( 0.0 + _FresnelMaskScale * pow( 1.0 - fresnelNdotV5, _FresnelMaskPower ) );
			float clampResult6 = clamp( ( 1.0 - fresnelNode5 ) , 0.0 , 1.0 );
			#ifdef _FRESNELMASKENABLED_ON
				float staticSwitch115 = clampResult6;
			#else
				float staticSwitch115 = 1.0;
			#endif
			float2 uv_MainMask = i.uv_texcoord * _MainMask_ST.xy + _MainMask_ST.zw;
			float4 break95 = ( ( tex2D( _MainMask, uv_MainMask ) * _MainMaskChannelsMultiply ) + _MainMaskChannelsAdd );
			float clampResult100 = clamp( ( break95.r + break95.g + break95.b + break95.a ) , 0.0 , 1.0 );
			float clampResult99 = clamp( ( break95.r * break95.g * break95.b * break95.a ) , 0.0 , 1.0 );
			#ifdef _MAINMASKCHANNELSADDORMULTIPLY_ON
				float staticSwitch98 = clampResult99;
			#else
				float staticSwitch98 = clampResult100;
			#endif
			float2 uv0_SecondMaskDistortion = i.uv_texcoord * _SecondMaskDistortion_ST.xy + _SecondMaskDistortion_ST.zw;
			float2 appendResult173 = (float2(_SecondMaskDistortionScaleU , _SecondMaskDistortionScaleV));
			float2 uv0_SecondMask = i.uv_texcoord * _SecondMask_ST.xy + _SecondMask_ST.zw;
			float2 break124 = uv0_SecondMask;
			#ifdef _SECONDMASKFLIPANIMATION_ON
				float staticSwitch85 = ( 1.0 - i.uv2_tex4coord2.y );
			#else
				float staticSwitch85 = i.uv2_tex4coord2.y;
			#endif
			float temp_output_38_0 = ( ( break124.y * _SecondMaskLifetimeStretch ) + (-_SecondMaskLifetimeStretch + (staticSwitch85 - 0.0) * (1.0 - -_SecondMaskLifetimeStretch) / (1.0 - 0.0)) );
			#ifdef _SECONDMASKFLIPUV_ON
				float staticSwitch84 = ( 1.0 - temp_output_38_0 );
			#else
				float staticSwitch84 = temp_output_38_0;
			#endif
			float2 appendResult39 = (float2(break124.x , staticSwitch84));
			float cos122 = cos( (0.0 + (_SecondMaskRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0)) );
			float sin122 = sin( (0.0 + (_SecondMaskRotation - 0.0) * (( 2.0 * UNITY_PI ) - 0.0) / (1.0 - 0.0)) );
			float2 rotator122 = mul( appendResult39 - float2( 0.5,0.5 ) , float2x2( cos122 , -sin122 , sin122 , cos122 )) + float2( 0.5,0.5 );
			float2 uv0_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			float2 appendResult177 = (float2(_DissolveTextureScaleU , _DissolveTextureScaleV));
			float2 break179 = ( uv0_DissolveTexture * appendResult177 );
			float2 appendResult150 = (float2(( break179.x + i.uv_tex4coord.z ) , ( break179.y + ( i.uv_tex4coord.z * 0.47 ) )));
			float clampResult54 = clamp( ( _FinalOpacityPower * ( clampResult136 * staticSwitch115 * staticSwitch98 * tex2D( _SecondMask, ( ( (_SecondMaskDistortionRemapMin + (tex2D( _SecondMaskDistortion, ( uv0_SecondMaskDistortion * appendResult173 ) ).r - 0.0) * (_SecondMaskDistortionRemapMax - _SecondMaskDistortionRemapMin) / (1.0 - 0.0)) * _SecondMaskDistortionAmount ) + rotator122 ) ).r * ( ( tex2D( _DissolveTexture, appendResult150 ).r + _DissolveTextureRemapMax ) - (0.0 + (i.uv2_tex4coord2.x - 0.0) * (( _DissolveTextureRemapMax + 1.0 ) - 0.0) / (1.0 - 0.0)) ) ) ) , 0.0 , 1.0 );
			float clampResult108 = clamp( ( clampResult54 * _RampOffsetMultiply ) , 0.0 , 1.0 );
			float2 appendResult57 = (float2(( 1.0 - pow( ( 1.0 - clampResult108 ) , _RampOffsetExp1 ) ) , 0.0));
			float4 temp_output_140_0 = ( _RampColorTint * tex2D( _Ramp, appendResult57 ) );
			#ifdef _RAMPAFFECTEDBYVERTEXCOLOR1_ON
				float4 staticSwitch159 = ( temp_output_140_0 * i.vertexColor );
			#else
				float4 staticSwitch159 = temp_output_140_0;
			#endif
			#ifdef _RAMPENABLED1_ON
				float4 staticSwitch137 = staticSwitch159;
			#else
				float4 staticSwitch137 = ( _FinalColor * i.vertexColor );
			#endif
			o.Emission = ( _FinalPower * staticSwitch137 * i.uv_tex4coord.w ).rgb;
			o.Alpha = ( 1.0 - pow( ( 1.0 - clampResult54 ) , _FinalOpacityExp ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;403.0435;369.1768;1.260506;True;False
Node;AmplifyShaderEditor.CommentaryNode;101;-4246.191,2363.423;Inherit;False;3139.612;1662.373;;34;122;123;121;119;120;36;66;73;39;74;75;84;78;72;79;132;86;76;38;41;65;64;85;133;63;131;83;124;40;37;130;171;172;173;Second and Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;40;-4224.314,3260.455;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;118;-3825.159,-1144.606;Inherit;False;2616.433;1076.375;;27;136;134;135;1;68;33;69;31;67;70;34;77;32;46;142;45;29;44;163;164;165;162;166;167;168;169;170;Noise 01;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-3220.154,3158.458;Float;False;Property;_SecondMaskLifetimeStretch;Second Mask Lifetime Stretch;29;0;Create;True;0;0;False;0;False;1;1;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;83;-3452.367,3398.394;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-3442.534,2803.551;Inherit;False;0;36;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;167;-3784.816,-816.4873;Inherit;False;Property;_Noise01ScaleV;Noise 01 Scale V;19;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;-3784.816,-891.4873;Inherit;False;Property;_Noise01ScaleU;Noise 01 Scale U;18;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;157;-3311.272,4190.172;Inherit;False;2192.276;862.8774;;19;177;176;175;152;145;156;155;154;151;153;148;150;149;147;146;158;144;179;178;Simple Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.NegateNode;64;-2904.93,3427.784;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;85;-3255.481,3260.365;Float;False;Property;_SecondMaskFlipAnimation;Second Mask Flip Animation;37;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-3288.127,4528.175;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;40;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-3287.127,4453.175;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;39;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-3615.816,-167.4873;Inherit;False;Property;_Noise01DistortionScaleV;Noise 01 Distortion Scale V;24;0;Create;True;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;168;-3555.816,-861.4873;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-3775.857,-1078.691;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;124;-3191.822,2808.506;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;163;-3618.816,-254.4873;Inherit;False;Property;_Noise01DistortionScaleU;Noise 01 Distortion Scale U;23;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-3005.274,-831.0178;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;117;-2770.192,12.85535;Inherit;False;1582.195;459.0206;;10;17;20;22;12;9;5;11;6;116;115;Fresnel Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;41;-2703.378,3239.917;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-2489.588,3133.939;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;169;-3404.816,-975.4873;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;102;-3039.883,575.4115;Inherit;False;1853.049;505.4031;;11;92;98;100;99;97;96;95;93;23;114;113;Main Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;45;-3252.616,-702.5632;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-3275.431,-544.4666;Float;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;21;0;Create;True;0;0;False;0;False;0;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-3443.911,-427.2559;Inherit;False;0;67;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;177;-3022.127,4485.175;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;171;-3691.251,2575.846;Inherit;False;Property;_SecondMaskDistortionScaleU;Second Mask Distortion Scale U;31;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;172;-3691.251,2652.472;Inherit;False;Property;_SecondMaskDistortionScaleV;Second Mask Distortion Scale V;32;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;145;-3288.518,4252.529;Inherit;False;0;152;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;165;-3317.816,-226.4873;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-2320.377,3248.917;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-3540.161,2429.339;Inherit;False;0;72;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-2768.734,-729.8792;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.73;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;170;-3266.816,-973.4873;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;23;-2988.984,625.4115;Inherit;True;Property;_MainMask;Main Mask;10;0;Create;True;0;0;False;0;False;-1;None;bf35e1c44c7d3714eaa30be6fd03bf1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-3083.816,-330.4873;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;144;-2523.481,4395.887;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;17;-2720.192,88.29879;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;92;-2989.883,817.6852;Inherit;False;Property;_MainMaskChannelsMultiply;Main Mask Channels Multiply;12;0;Create;True;0;0;False;0;False;1,0,0,0;0,4,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;173;-3370.095,2602.891;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;-2882.386,4265.767;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2933.911,-632.7404;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;20;-2499.192,159.2985;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;113;-2642.217,873.6384;Inherit;False;Property;_MainMaskChannelsAdd;Main Mask Channels Add;13;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;86;-2207.652,3476.642;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2668.456,747.9749;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;179;-2755.386,4266.767;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-2291.689,4596.856;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.47;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;67;-2699.366,-572.5432;Inherit;True;Property;_Noise01Distortion;Noise 01 Distortion;22;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;-3121.057,2555.562;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-2606.43,-745.1796;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2674.039,-382.0142;Float;False;Property;_Noise01DistortionPower;Noise 01 Distortion Power;25;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-2612.081,-941.3167;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-2134.667,3752.066;Inherit;False;Property;_SecondMaskRotation;Second Mask Rotation;27;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;114;-2399.313,772.9225;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;119;-2048.823,3847.182;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2374.732,277.395;Float;False;Property;_FresnelMaskScale;Fresnel Mask Scale;16;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;84;-2136.794,3248.919;Float;False;Property;_SecondMaskFlipUV;Second Mask Flip UV;36;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2744.51,2743.484;Float;False;Property;_SecondMaskDistortionRemapMax;Second Mask Distortion Remap Max;35;0;Create;True;0;0;False;0;False;0.75;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-2354.498,-486.4831;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2735.51,2665.484;Float;False;Property;_SecondMaskDistortionRemapMin;Second Mask Distortion Remap Min;34;0;Create;True;0;0;False;0;False;-0.25;-0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-2720.603,2413.423;Inherit;True;Property;_SecondMaskDistortion;Second Mask Distortion;30;0;Create;True;0;0;False;0;False;-1;None;cac42f4ff44b0ae4e9da61044d764346;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;147;-2228.482,4443.887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2424.655,-861.0015;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;146;-2225.482,4325.887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2378.771,356.8759;Float;False;Property;_FresnelMaskPower;Fresnel Mask Power;15;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;12;-2346.193,87.29879;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-1908.977,3057.915;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;5;-2076.194,143.2985;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;-2152.421,-721.9074;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;123;-1810.723,3613.576;Inherit;False;Constant;_Vector0;Vector 0;34;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;150;-2064.481,4380.887;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;121;-1811.777,3754.31;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2339.603,2858.423;Float;False;Property;_SecondMaskDistortionAmount;Second Mask Distortion Amount;33;0;Create;True;0;0;False;0;False;0;0.125;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-2075.852,4858;Float;False;Property;_DissolveTextureRemapMax;Dissolve Texture Remap Max;41;0;Create;True;0;0;False;0;False;0.75;0.75;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;95;-2155.791,746.1505;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;75;-2236.488,2681.976;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.25;False;4;FLOAT;0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-1945.657,4947.903;Float;False;Constant;_Float4;Float 3;25;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;-1790.128,659.3354;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-2031.602,2760.423;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-1896.801,-558.8464;Inherit;False;Property;_Noise01Negate;Noise 01 Negate;20;0;Create;True;0;0;False;0;False;0;0.125;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-1840.194,143.2985;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;-1759.073,4891.515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-1817.128,862.3354;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;122;-1572.865,3595.618;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;152;-1891.025,4353.709;Inherit;True;Property;_DissolveTexture;Dissolve Texture;38;0;Create;True;0;0;False;0;False;-1;None;34a3b7afc8c389e40bda6d478fb654e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1919.676,-755.95;Inherit;True;Property;_Noise01;Noise 01;17;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;151;-1818.766,4662.41;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;155;-1557.766,4696.41;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;100;-1672.128,662.3354;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;154;-1516.129,4474.373;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-1654.51,2976.041;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;99;-1663.128,891.3354;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;6;-1680.194,142.2985;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-1683.844,62.85537;Inherit;False;Constant;_Float2;Float 2;32;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;134;-1613.334,-664.3806;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;98;-1519.128,763.3354;Inherit;False;Property;_MainMaskChannelsAddorMultiply;Main Mask Channels Add or Multiply;11;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;103;99.51563,-43.14231;Inherit;False;1114.804;433.8218;;7;60;54;89;90;91;88;59;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;36;-1450.871,2954.691;Inherit;True;Property;_SecondMask;Second Mask;26;0;Create;True;0;0;False;0;False;-1;None;429909c7a982fd845b290d5eb58d356d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;115;-1484.998,95.73544;Inherit;False;Property;_FresnelMaskEnabled;Fresnel Mask Enabled;14;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;136;-1370.644,-667.1455;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;156;-1281.399,4529.875;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-393.0439,187.9572;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;149.5156,6.857686;Float;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1;1;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;109;-919.8661,-1096.162;Inherit;False;1333.234;328.796;;8;57;61;62;108;105;106;104;107;Ramp UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;412.9257,148.3917;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;54;552.6934,145.7523;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-869.8661,-978.6498;Float;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;8;0;Create;True;0;0;False;0;False;1;4;1;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-547.8652,-1043.65;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;108;-406.1534,-1042.926;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-374.4615,-883.437;Inherit;False;Property;_RampOffsetExp1;Ramp Offset Exp;9;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;104;-250.0234,-1040.267;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;105;-83.45049,-1042.972;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;106;66.22903,-1035.85;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;57;246.3674,-1046.162;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;141;580.258,-1751.798;Inherit;False;1181.638;1050.005;;12;2;4;137;139;140;138;58;47;3;159;161;143;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;3;701.6093,-1279.633;Float;False;Property;_RampColorTint;Ramp Color Tint;6;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;633.0383,-1098.193;Inherit;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;False;-1;None;2c46855af43da484a8c404765da766ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;161;744.9005,-896.9636;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;977.6819,-1161.127;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;1064.7,-971.0641;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;47;658.2788,-1521.636;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;138;630.258,-1701.798;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;88;575.3196,275.6795;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;3;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;159;1242.8,-1007.464;Inherit;False;Property;_RampAffectedByVertexColor1;Ramp Affected By Vertex Color;7;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;89;719.3196,178.6795;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;897.7477,-1602.201;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;143;1482.355,-1685.884;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;137;1202.067,-1356.074;Inherit;False;Property;_RampEnabled1;Ramp Enabled;4;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;90;879.3196,179.6795;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;1161.823,-1581.478;Float;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;0;10;0;80;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2650.205,1378.712;Float;False;Property;_SoftParticlesDistance;Soft Particles Distance;43;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;24;-2405.878,1378.174;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;25;-2007.836,1414.275;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;53;-1961.165,1763.729;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;132;-2707.631,2837.204;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-3298.328,2963.438;Inherit;False;Property;_SecondMaskDirectionSwitch;Second Mask Direction Switch;28;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;133;-2706.886,3040.518;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;42;-1730.644,1431.276;Float;False;Property;_SoftParticlesEnabled;Soft Particles Enabled;42;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;51;-1753.344,1662.678;Float;False;Property;_CameraDepthFadeEnabled;Camera Depth Fade Enabled;45;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2592.018,1856.435;Float;False;Property;_CameraDepthFadeOffset;Camera Depth Fade Offset;47;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1995.047,1542.726;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2408.443,1468.209;Float;False;Property;_SoftParticlesExp;Soft Particles Exp;44;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;28;-2158.936,1415.437;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2600.584,1746.131;Float;False;Property;_CameraDepthFadeLength;Camera Depth Fade Length;46;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;1557.796,-1454.875;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CameraDepthFade;48;-2216.826,1763.743;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1962.469,1662.678;Float;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;91;1027.32,179.6795;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;131;-3005.32,2966.126;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;180;182.8519,429.8115;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2127.816,-234.8059;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/CenterCurve;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;83;0;40;2
WireConnection;64;0;63;0
WireConnection;85;1;40;2
WireConnection;85;0;83;0
WireConnection;168;0;166;0
WireConnection;168;1;167;0
WireConnection;124;0;37;0
WireConnection;41;0;85;0
WireConnection;41;3;64;0
WireConnection;65;0;124;1
WireConnection;65;1;63;0
WireConnection;169;0;32;0
WireConnection;169;1;168;0
WireConnection;177;0;175;0
WireConnection;177;1;176;0
WireConnection;165;0;163;0
WireConnection;165;1;164;0
WireConnection;38;0;65;0
WireConnection;38;1;41;0
WireConnection;142;0;29;3
WireConnection;170;0;169;0
WireConnection;162;0;77;0
WireConnection;162;1;165;0
WireConnection;173;0;171;0
WireConnection;173;1;172;0
WireConnection;178;0;145;0
WireConnection;178;1;177;0
WireConnection;46;0;45;2
WireConnection;46;1;44;0
WireConnection;20;0;17;0
WireConnection;86;0;38;0
WireConnection;93;0;23;0
WireConnection;93;1;92;0
WireConnection;179;0;178;0
WireConnection;158;0;144;3
WireConnection;67;1;162;0
WireConnection;174;0;76;0
WireConnection;174;1;173;0
WireConnection;31;0;170;1
WireConnection;31;1;142;0
WireConnection;31;2;46;0
WireConnection;34;0;170;0
WireConnection;34;1;29;3
WireConnection;114;0;93;0
WireConnection;114;1;113;0
WireConnection;84;1;38;0
WireConnection;84;0;86;0
WireConnection;69;0;67;1
WireConnection;69;1;70;0
WireConnection;72;1;174;0
WireConnection;147;0;179;1
WireConnection;147;1;158;0
WireConnection;33;0;34;0
WireConnection;33;1;31;0
WireConnection;146;0;179;0
WireConnection;146;1;144;3
WireConnection;12;0;17;0
WireConnection;12;1;20;0
WireConnection;39;0;124;0
WireConnection;39;1;84;0
WireConnection;5;0;12;0
WireConnection;5;2;22;0
WireConnection;5;3;9;0
WireConnection;68;0;33;0
WireConnection;68;1;69;0
WireConnection;150;0;146;0
WireConnection;150;1;147;0
WireConnection;121;0;120;0
WireConnection;121;4;119;0
WireConnection;95;0;114;0
WireConnection;75;0;72;1
WireConnection;75;3;78;0
WireConnection;75;4;79;0
WireConnection;96;0;95;0
WireConnection;96;1;95;1
WireConnection;96;2;95;2
WireConnection;96;3;95;3
WireConnection;73;0;75;0
WireConnection;73;1;74;0
WireConnection;11;0;5;0
WireConnection;153;0;149;0
WireConnection;153;1;148;0
WireConnection;97;0;95;0
WireConnection;97;1;95;1
WireConnection;97;2;95;2
WireConnection;97;3;95;3
WireConnection;122;0;39;0
WireConnection;122;1;123;0
WireConnection;122;2;121;0
WireConnection;152;1;150;0
WireConnection;1;1;68;0
WireConnection;155;0;151;1
WireConnection;155;4;153;0
WireConnection;100;0;96;0
WireConnection;154;0;152;1
WireConnection;154;1;149;0
WireConnection;66;0;73;0
WireConnection;66;1;122;0
WireConnection;99;0;97;0
WireConnection;6;0;11;0
WireConnection;134;0;1;1
WireConnection;134;1;135;0
WireConnection;98;1;100;0
WireConnection;98;0;99;0
WireConnection;36;1;66;0
WireConnection;115;1;116;0
WireConnection;115;0;6;0
WireConnection;136;0;134;0
WireConnection;156;0;154;0
WireConnection;156;1;155;0
WireConnection;21;0;136;0
WireConnection;21;1;115;0
WireConnection;21;2;98;0
WireConnection;21;3;36;1
WireConnection;21;4;156;0
WireConnection;60;0;59;0
WireConnection;60;1;21;0
WireConnection;54;0;60;0
WireConnection;61;0;54;0
WireConnection;61;1;62;0
WireConnection;108;0;61;0
WireConnection;104;0;108;0
WireConnection;105;0;104;0
WireConnection;105;1;107;0
WireConnection;106;0;105;0
WireConnection;57;0;106;0
WireConnection;58;1;57;0
WireConnection;140;0;3;0
WireConnection;140;1;58;0
WireConnection;160;0;140;0
WireConnection;160;1;161;0
WireConnection;159;1;140;0
WireConnection;159;0;160;0
WireConnection;89;0;54;0
WireConnection;139;0;138;0
WireConnection;139;1;47;0
WireConnection;137;1;139;0
WireConnection;137;0;159;0
WireConnection;90;0;89;0
WireConnection;90;1;88;0
WireConnection;24;0;26;0
WireConnection;25;0;28;0
WireConnection;53;0;48;0
WireConnection;132;0;124;0
WireConnection;132;1;124;1
WireConnection;132;2;131;0
WireConnection;133;0;124;1
WireConnection;133;1;124;0
WireConnection;133;2;131;0
WireConnection;42;1;43;0
WireConnection;42;0;25;0
WireConnection;51;1;52;0
WireConnection;51;0;53;0
WireConnection;28;0;24;0
WireConnection;28;1;27;0
WireConnection;2;0;4;0
WireConnection;2;1;137;0
WireConnection;2;2;143;4
WireConnection;48;0;49;0
WireConnection;48;1;50;0
WireConnection;91;0;90;0
WireConnection;131;0;130;0
WireConnection;0;2;2;0
WireConnection;0;9;91;0
ASEEND*/
//CHKSM=F1A77B4EF5B0BDB33CFE9B9EAD984CEA5CAFF2CD