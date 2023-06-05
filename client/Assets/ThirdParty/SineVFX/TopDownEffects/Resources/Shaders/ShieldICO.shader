// Upgrade NOTE: upgraded instancing buffer 'SineVFXTopDownEffectsShieldICO' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/ShieldICO"
{
	Properties
	{
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalPower("Final Power", Range( 0 , 100)) = 4
		_FinalOpacityPower("Final Opacity Power", Float) = 1
		[Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampAffectedByDynamics("Ramp Affected By Dynamics", Range( 0 , 1)) = 1
		_RampOffsetMultiply("Ramp Offset Multiply", Float) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_MainTex("MainTex", 2D) = "white" {}
		_MainTexChannels("MainTex Channels", Vector) = (1,0,0,0)
		_MainTexScrollSpeed("MainTex Scroll Speed", Float) = 0
		_OffsetTexture("Offset Texture", 2D) = "white" {}
		_OffsetTextureScaleU("Offset Texture Scale U", Float) = 1
		_OffsetTextureScaleV("Offset Texture Scale V", Float) = 1
		_OffsetTextureScrollSpeedU("Offset Texture Scroll Speed U", Float) = 0
		_OffsetTextureScrollSpeedV("Offset Texture Scroll Speed V", Float) = 0
		_OffsetStyle("Offset Style", Range( 0 , 1)) = 0
		_OffsetPower("Offset Power", Float) = 0
		_OffsetGChannelMasking("Offset G Channel Masking", Range( 0 , 1)) = 0
		_OffsetToOpacityNegate("Offset To Opacity Negate", Range( 0 , 1)) = 1
		_OffsetToOpacityExp("Offset To Opacity Exp", Range( 0.2 , 8)) = 1
		_OffsetToOpacityTopFixExp("Offset To Opacity Top Fix Exp", Range( 0.2 , 8)) = 8
		_OffsetToOpacityTopFixValue("Offset To Opacity Top Fix Value", Range( 0 , 1)) = 0.25
		[Toggle(_TRISGLAREENABLED_ON)] _TrisGlareEnabled("Tris Glare Enabled", Float) = 0
		_TrisGlareTexture("Tris Glare Texture", 2D) = "white" {}
		_TrisGlareTextureScaleU("Tris Glare Texture Scale U", Float) = 1
		_TrisGlareTextureScaleV("Tris Glare Texture Scale V", Float) = 1
		_TrisGlareNegate("Tris Glare Negate", Range( 0 , 1)) = 1
		[Toggle(_VERTICALGRADIENTENABLED_ON)] _VerticalGradientEnabled("Vertical Gradient Enabled", Float) = 1
		_VerticalGradientFlipSwitch("Vertical Gradient Flip Switch", Range( 0 , 1)) = 0
		_VerticalGradientAbsSwitch("Vertical Gradient Abs Switch", Range( 0 , 1)) = 1
		_VerticalGradientClampSwitch("Vertical Gradient Clamp Switch", Range( 0 , 1)) = 0
		_VerticalGradientRemapMax("Vertical Gradient Remap Max", Float) = 1
		_VerticalGradientOffset("Vertical Gradient Offset", Float) = 0
		_VerticalGradientProfile("Vertical Gradient Profile", 2D) = "white" {}
		[Toggle(_INNERRIMENABLED_ON)] _InnerRimEnabled("Inner Rim Enabled", Float) = 0
		_InnerRimExp("Inner Rim Exp", Range( 0.1 , 16)) = 4
		_InnerRimFlipSwitch("Inner Rim Flip Switch", Range( 0 , 1)) = 1
		[Toggle(_WAVESENABLED_ON)] _WavesEnabled("Waves Enabled", Float) = 0
		_WavesSpeed("Waves Speed", Float) = 4
		_WavesScale("Waves Scale", Float) = 8
		[Toggle(_DEPTHMASKENABLED_ON)] _DepthMaskEnabled("Depth Mask Enabled", Float) = 0
		_DepthMaskDistance("Depth Mask Distance", Float) = 1
		_DepthMaskRemapMax("Depth Mask Remap Max", Float) = 1
		_DepthMaskExp("Depth Mask Exp", Float) = 2
		_DepthMaskMultiply("Depth Mask Multiply", Float) = 0.25
		_AppearGradientFlipSwitch("Appear Gradient Flip Switch", Range( 0 , 1)) = 0
		_AppearGradientAbsSwitch("Appear Gradient Abs Switch", Range( 0 , 1)) = 1
		_AppearGradientClampSwitch("Appear Gradient Clamp Switch", Range( 0 , 1)) = 0
		_AppearGradientRemapMax("Appear Gradient Remap Max", Float) = 1
		_AppearGradientOffset("Appear Gradient Offset", Float) = 0
		_AppearGradientDynamicsNegate("Appear Gradient Dynamics Negate", Range( 0 , 1)) = 0
		_AppearGradientDistortionAmount("Appear Gradient Distortion Amount", Range( 0 , 1)) = 0
		_AppearGradientProfile("Appear Gradient Profile", 2D) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
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
		#pragma multi_compile_instancing
		#pragma shader_feature _WAVESENABLED_ON
		#pragma shader_feature _RAMPENABLED_ON
		#pragma shader_feature _DEPTHMASKENABLED_ON
		#pragma shader_feature _VERTICALGRADIENTENABLED_ON
		#pragma shader_feature _INNERRIMENABLED_ON
		#pragma shader_feature _TRISGLAREENABLED_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
			float3 worldNormal;
			half ASEVFace : VFACE;
			float3 viewDir;
			float2 uv_texcoord;
			float2 uv2_texcoord2;
			float2 uv3_texcoord3;
			float4 vertexColor : COLOR;
		};

		uniform float _WavesScale;
		uniform float _WavesSpeed;
		uniform sampler2D _OffsetTexture;
		uniform float _OffsetStyle;
		uniform float _OffsetTextureScaleU;
		uniform float _OffsetTextureScaleV;
		uniform float _OffsetTextureScrollSpeedU;
		uniform float _OffsetTextureScrollSpeedV;
		uniform float _OffsetPower;
		uniform sampler2D _VerticalGradientProfile;
		uniform float _VerticalGradientAbsSwitch;
		uniform float _VerticalGradientClampSwitch;
		uniform float _VerticalGradientOffset;
		uniform float _VerticalGradientRemapMax;
		uniform float _VerticalGradientFlipSwitch;
		uniform float _OffsetGChannelMasking;
		uniform float4 _FinalColor;
		uniform sampler2D _Ramp;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthMaskDistance;
		uniform float _DepthMaskRemapMax;
		uniform float _DepthMaskExp;
		uniform float _DepthMaskMultiply;
		uniform float _InnerRimFlipSwitch;
		uniform float _InnerRimExp;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _MainTexScrollSpeed;
		uniform float4 _MainTexChannels;
		uniform float _OffsetToOpacityTopFixValue;
		uniform float _OffsetToOpacityExp;
		uniform float _OffsetToOpacityNegate;
		uniform float _OffsetToOpacityTopFixExp;
		uniform float _FinalOpacityPower;
		uniform sampler2D _TrisGlareTexture;
		uniform float _TrisGlareTextureScaleU;
		uniform float _TrisGlareTextureScaleV;
		uniform float _TrisGlareNegate;
		uniform sampler2D _AppearGradientProfile;
		uniform float _AppearGradientAbsSwitch;
		uniform float _AppearGradientOffset;
		uniform float _AppearGradientDistortionAmount;
		uniform float _AppearGradientRemapMax;
		uniform float _AppearGradientFlipSwitch;
		uniform float _AppearGradientDynamicsNegate;
		uniform float _RampAffectedByDynamics;
		uniform float _RampOffsetMultiply;
		uniform float _RampOffsetExp;
		uniform float4 _RampColorTint;
		uniform float _FinalPower;

		UNITY_INSTANCING_BUFFER_START(SineVFXTopDownEffectsShieldICO)
			UNITY_DEFINE_INSTANCED_PROP(float, _AppearGradientClampSwitch)
#define _AppearGradientClampSwitch_arr SineVFXTopDownEffectsShieldICO
		UNITY_INSTANCING_BUFFER_END(SineVFXTopDownEffectsShieldICO)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			#ifdef _WAVESENABLED_ON
				float staticSwitch221 = (0.0 + (sin( ( ( _WavesScale * ase_vertex3Pos.y ) + ( _Time.y * _WavesSpeed ) ) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			#else
				float staticSwitch221 = 1.0;
			#endif
			float2 lerpResult265 = lerp( v.texcoord1.xy , v.texcoord2.xy , _OffsetStyle);
			float2 appendResult98 = (float2(_OffsetTextureScaleU , _OffsetTextureScaleV));
			float2 break94 = ( lerpResult265 * appendResult98 );
			float2 appendResult93 = (float2(( break94.x + ( _Time.y * _OffsetTextureScrollSpeedU ) ) , ( break94.y + ( _Time.y * _OffsetTextureScrollSpeedV ) )));
			float temp_output_216_0 = ( staticSwitch221 * tex2Dlod( _OffsetTexture, float4( appendResult93, 0, 0.0) ).r );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float lerpResult102 = lerp( ase_vertex3Pos.y , abs( ase_vertex3Pos.y ) , round( _VerticalGradientAbsSwitch ));
			float clampResult250 = clamp( lerpResult102 , 0.0 , 100.0 );
			float lerpResult249 = lerp( lerpResult102 , clampResult250 , _VerticalGradientClampSwitch);
			float clampResult108 = clamp( (0.0 + (( lerpResult249 + _VerticalGradientOffset ) - 0.0) * (1.0 - 0.0) / (_VerticalGradientRemapMax - 0.0)) , 0.0 , 1.0 );
			float lerpResult112 = lerp( clampResult108 , ( 1.0 - clampResult108 ) , round( _VerticalGradientFlipSwitch ));
			float2 appendResult113 = (float2(lerpResult112 , 0.0));
			float4 tex2DNode114 = tex2Dlod( _VerticalGradientProfile, float4( appendResult113, 0, 0.0) );
			float lerpResult144 = lerp( 1.0 , tex2DNode114.g , _OffsetGChannelMasking);
			v.vertex.xyz += ( temp_output_216_0 * ase_worldNormal * _OffsetPower * lerpResult144 );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float lerpResult102 = lerp( ase_vertex3Pos.y , abs( ase_vertex3Pos.y ) , round( _VerticalGradientAbsSwitch ));
			float clampResult250 = clamp( lerpResult102 , 0.0 , 100.0 );
			float lerpResult249 = lerp( lerpResult102 , clampResult250 , _VerticalGradientClampSwitch);
			float clampResult108 = clamp( (0.0 + (( lerpResult249 + _VerticalGradientOffset ) - 0.0) * (1.0 - 0.0) / (_VerticalGradientRemapMax - 0.0)) , 0.0 , 1.0 );
			float lerpResult112 = lerp( clampResult108 , ( 1.0 - clampResult108 ) , round( _VerticalGradientFlipSwitch ));
			float2 appendResult113 = (float2(lerpResult112 , 0.0));
			float4 tex2DNode114 = tex2D( _VerticalGradientProfile, appendResult113 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth231 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth231 = abs( ( screenDepth231 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthMaskDistance ) );
			float clampResult234 = clamp( (0.0 + (( 1.0 - distanceDepth231 ) - 0.0) * (1.0 - 0.0) / (_DepthMaskRemapMax - 0.0)) , 0.0 , 1.0 );
			float clampResult237 = clamp( ( pow( clampResult234 , _DepthMaskExp ) * _DepthMaskMultiply ) , 0.0 , 1.0 );
			#ifdef _DEPTHMASKENABLED_ON
				float staticSwitch246 = clampResult237;
			#else
				float staticSwitch246 = 0.0;
			#endif
			float temp_output_243_0 = ( tex2DNode114.r + staticSwitch246 );
			float3 ase_worldNormal = i.worldNormal;
			float3 switchResult140 = (((i.ASEVFace>0)?(ase_worldNormal):(-ase_worldNormal)));
			float dotResult125 = dot( switchResult140 , i.viewDir );
			float clampResult127 = clamp( dotResult125 , 0.0 , 1.0 );
			float lerpResult135 = lerp( clampResult127 , ( 1.0 - clampResult127 ) , round( _InnerRimFlipSwitch ));
			float temp_output_128_0 = pow( lerpResult135 , _InnerRimExp );
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 appendResult256 = (float2(( _MainTexScrollSpeed * _Time.y ) , 0.0));
			float4 tex2DNode4 = tex2D( _MainTex, ( uv0_MainTex + appendResult256 ) );
			#ifdef _WAVESENABLED_ON
				float staticSwitch221 = (0.0 + (sin( ( ( _WavesScale * ase_vertex3Pos.y ) + ( _Time.y * _WavesSpeed ) ) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			#else
				float staticSwitch221 = 1.0;
			#endif
			float2 lerpResult265 = lerp( i.uv2_texcoord2 , i.uv3_texcoord3 , _OffsetStyle);
			float2 appendResult98 = (float2(_OffsetTextureScaleU , _OffsetTextureScaleV));
			float2 break94 = ( lerpResult265 * appendResult98 );
			float2 appendResult93 = (float2(( break94.x + ( _Time.y * _OffsetTextureScrollSpeedU ) ) , ( break94.y + ( _Time.y * _OffsetTextureScrollSpeedV ) )));
			float temp_output_216_0 = ( staticSwitch221 * tex2D( _OffsetTexture, appendResult93 ).r );
			float clampResult328 = clamp( abs( ase_vertex3Pos.y ) , 0.0 , 1.0 );
			float lerpResult330 = lerp( _OffsetToOpacityTopFixValue , ( pow( temp_output_216_0 , _OffsetToOpacityExp ) + _OffsetToOpacityNegate ) , ( 1.0 - pow( clampResult328 , _OffsetToOpacityTopFixExp ) ));
			float clampResult155 = clamp( lerpResult330 , 0.0 , 1.0 );
			float4 appendResult149 = (float4(clampResult155 , clampResult155 , 1.0 , 1.0));
			float4 break69 = ( ( tex2DNode4 * _MainTexChannels ) * appendResult149 );
			float clampResult71 = clamp( ( break69.r + break69.g + break69.b + break69.a ) , 0.0 , 1.0 );
			#ifdef _VERTICALGRADIENTENABLED_ON
				float staticSwitch117 = temp_output_243_0;
			#else
				float staticSwitch117 = 1.0;
			#endif
			#ifdef _INNERRIMENABLED_ON
				float staticSwitch137 = temp_output_128_0;
			#else
				float staticSwitch137 = 1.0;
			#endif
			float clampResult340 = clamp( ( staticSwitch137 + staticSwitch246 ) , 0.0 , 1.0 );
			float2 appendResult199 = (float2(_TrisGlareTextureScaleU , _TrisGlareTextureScaleV));
			float2 break202 = ( i.uv3_texcoord3 * appendResult199 );
			float2 appendResult198 = (float2(( break202.x + ( _Time.y * _OffsetTextureScrollSpeedU ) ) , ( break202.y + ( _Time.y * _OffsetTextureScrollSpeedV ) )));
			float clampResult207 = clamp( ( tex2D( _TrisGlareTexture, appendResult198 ).r + _TrisGlareNegate ) , 0.0 , 1.0 );
			#ifdef _TRISGLAREENABLED_ON
				float staticSwitch208 = clampResult207;
			#else
				float staticSwitch208 = 1.0;
			#endif
			float clampResult288 = clamp( ( staticSwitch208 + tex2DNode4.g ) , 0.0 , 1.0 );
			float lerpResult291 = lerp( ase_vertex3Pos.y , abs( ase_vertex3Pos.y ) , round( _AppearGradientAbsSwitch ));
			float clampResult294 = clamp( lerpResult291 , 0.0 , 100.0 );
			float _AppearGradientClampSwitch_Instance = UNITY_ACCESS_INSTANCED_PROP(_AppearGradientClampSwitch_arr, _AppearGradientClampSwitch);
			float lerpResult295 = lerp( lerpResult291 , clampResult294 , _AppearGradientClampSwitch_Instance);
			float clampResult302 = clamp( (0.0 + (( lerpResult295 + _AppearGradientOffset + ( clampResult155 * _AppearGradientDistortionAmount ) ) - 0.0) * (1.0 - 0.0) / (_AppearGradientRemapMax - 0.0)) , 0.0 , 1.0 );
			float lerpResult305 = lerp( clampResult302 , ( 1.0 - clampResult302 ) , round( _AppearGradientFlipSwitch ));
			float2 appendResult307 = (float2(lerpResult305 , 0.0));
			float4 tex2DNode310 = tex2D( _AppearGradientProfile, appendResult307 );
			float clampResult319 = clamp( ( clampResult71 + _AppearGradientDynamicsNegate ) , 0.0 , 1.0 );
			float clampResult8 = clamp( ( ( ( clampResult71 * staticSwitch117 * clampResult340 * _FinalOpacityPower * clampResult288 * tex2DNode310.g ) + ( tex2DNode310.r * tex2DNode310.g * clampResult319 * temp_output_128_0 ) ) - 0.0 ) , 0.0 , 1.0 );
			float temp_output_38_0 = ( clampResult8 * i.vertexColor.a );
			float lerpResult180 = lerp( ( temp_output_243_0 * temp_output_128_0 ) , temp_output_38_0 , _RampAffectedByDynamics);
			float clampResult178 = clamp( ( lerpResult180 * _RampOffsetMultiply ) , 0.0 , 1.0 );
			float2 appendResult167 = (float2(( 1.0 - pow( ( 1.0 - clampResult178 ) , _RampOffsetExp ) ) , 0.0));
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch168 = ( tex2D( _Ramp, appendResult167 ) * _RampColorTint );
			#else
				float4 staticSwitch168 = _FinalColor;
			#endif
			o.Emission = ( staticSwitch168 * _FinalPower ).rgb;
			o.Alpha = temp_output_38_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;-581.7726;828.7631;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;184;-2631.57,2572.18;Inherit;False;3009.277;1053.369;;33;97;96;98;84;95;88;86;87;89;94;90;91;92;93;157;79;156;153;154;155;145;143;81;83;144;82;149;216;259;264;266;265;258;Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;266;-2593.882,2749.815;Inherit;False;Property;_OffsetStyle;Offset Style;24;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;84;-2560.474,3015.149;Inherit;False;1;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;264;-2578.882,2844.815;Inherit;False;2;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;223;-4473.483,2566.064;Inherit;False;1427.537;703.9998;;11;210;219;220;212;215;218;211;213;214;222;221;Waves;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-2581.57,3237.23;Inherit;False;Property;_OffsetTextureScaleV;Offset Texture Scale V;21;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-2577.524,3150.802;Inherit;False;Property;_OffsetTextureScaleU;Offset Texture Scale U;20;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;212;-4348.709,2943.358;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;210;-4423.483,2780.649;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;98;-2313.168,3182.08;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-4417.122,2616.064;Inherit;False;Property;_WavesScale;Waves Scale;51;0;Create;True;0;0;False;0;False;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;265;-2290.882,2916.815;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;219;-4325.122,3155.064;Inherit;False;Property;_WavesSpeed;Waves Speed;50;0;Create;True;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-2020.48,3343.334;Inherit;False;Property;_OffsetTextureScrollSpeedU;Offset Texture Scroll Speed U;22;0;Create;True;0;0;False;0;False;0;0.125;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;88;-1928.804,3205.764;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-4197.265,2744.523;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-2092.365,3062.801;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;218;-4084.122,3060.064;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-2020.517,3420.128;Inherit;False;Property;_OffsetTextureScrollSpeedV;Offset Texture Scroll Speed V;23;0;Create;True;0;0;False;0;False;0;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;94;-1962.366,3066.801;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-1637.572,3286.625;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-1627.572,3426.624;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;211;-3994.947,2886.277;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;213;-3858.045,2892.103;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;92;-1293.454,3273.523;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;-1297.454,3154.523;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;93;-1121.85,3198.405;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;324;-1068.674,2289.606;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;115;-5019.89,104.0824;Inherit;False;3647.354;512.688;;22;117;243;118;114;113;112;110;111;108;109;104;107;105;102;106;101;103;99;100;248;249;250;Vertical Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-3599.541,2795.468;Inherit;False;Constant;_Float4;Float 4;41;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;214;-3713.887,2889.521;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-4969.89,157.0826;Inherit;False;Property;_VerticalGradientAbsSwitch;Vertical Gradient Abs Switch;41;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;221;-3344.945,2833.211;Inherit;False;Property;_WavesEnabled;Waves Enabled;49;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;79;-932.344,3159.94;Inherit;True;Property;_OffsetTexture;Offset Texture;19;0;Create;True;0;0;False;0;False;-1;None;0d57fcffb325773469c807d4f445589f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;334;-880.9507,2334.201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;99;-4741.515,283.8245;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;217;-2503.37,3985.564;Inherit;False;2617.879;634.6445;;18;199;193;201;200;195;202;194;196;197;198;204;205;206;207;209;208;322;323;Tris Glare;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;257;-3525.583,-597.6761;Inherit;False;1045;584;;10;260;252;251;256;254;253;255;261;262;263;MainTex UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;328;-756.9026,2335.706;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-845.9333,2936.106;Inherit;False;Property;_OffsetToOpacityExp;Offset To Opacity Exp;28;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;103;-4494.889,321.0827;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;101;-4665.89,160.0826;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-588.5135,3119.657;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;311;-6908.92,4663.909;Inherit;False;3294.539;686.1128;;21;289;292;293;290;291;294;296;295;299;301;297;300;303;302;306;304;305;309;307;310;308;Appear Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;332;-907.7482,2455.119;Inherit;False;Property;_OffsetToOpacityTopFixExp;Offset To Opacity Top Fix Exp;32;0;Create;True;0;0;False;0;False;8;8;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;156;-545.9098,2976.387;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;253;-3450.583,-164.6761;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;102;-4336.889,154.0826;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;292;-6858.92,4713.909;Inherit;False;Property;_AppearGradientAbsSwitch;Appear Gradient Abs Switch;58;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;289;-6704.684,4835.878;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;229;-3760.992,1494.621;Inherit;False;2132.308;341.7243;;13;237;240;236;239;234;235;241;242;232;231;230;246;247;Depth Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-569.5884,2743.499;Inherit;False;Property;_OffsetToOpacityNegate;Offset To Opacity Negate;27;0;Create;True;0;0;False;0;False;1;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;255;-3479.583,-238.6761;Inherit;False;Property;_MainTexScrollSpeed;MainTex Scroll Speed;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;119;-2961.7,776.0465;Inherit;False;1609.192;599.4163;;14;137;138;128;135;126;136;134;133;127;125;124;122;140;141;Inner Rim;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;323;-2420.524,4464.672;Inherit;False;Property;_TrisGlareTextureScaleV;Tris Glare Texture Scale V;37;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;322;-2420.524,4380.672;Inherit;False;Property;_TrisGlareTextureScaleU;Tris Glare Texture Scale U;36;0;Create;True;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;326;-602.1333,2335.515;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;290;-6478.196,4919.142;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;199;-2385.489,4242.664;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;327;-370.3408,2335.68;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;293;-6571.407,4723.493;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;154;-221.049,2888.275;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;333;-511.9507,2247.201;Inherit;False;Property;_OffsetToOpacityTopFixValue;Offset To Opacity Top Fix Value;33;0;Create;True;0;0;False;0;False;0.25;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-3699.341,1568.415;Float;False;Property;_DepthMaskDistance;Depth Mask Distance;53;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;250;-4161.194,237.5094;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;193;-2453.37,4035.564;Inherit;False;2;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;254;-3212.583,-180.6761;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;122;-2938.109,899.2327;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;248;-4548.194,487.5093;Inherit;False;Property;_VerticalGradientClampSwitch;Vertical Gradient Clamp Switch;42;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;231;-3458.666,1548.818;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;141;-2780.105,1067.919;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-3946.608,413.1454;Inherit;False;Property;_VerticalGradientOffset;Vertical Gradient Offset;44;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;201;-2001.126,4266.348;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;291;-6263.657,4801.229;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;330;-105.21,2299.138;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;256;-3053.583,-182.6761;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-2164.687,4123.385;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;249;-3952.195,195.5094;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;251;-3446.583,-533.6761;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;242;-3471.643,1688.689;Inherit;False;Property;_DepthMaskRemapMax;Depth Mask Remap Max;54;0;Create;True;0;0;False;0;False;1;3.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;294;-6067.725,4878.965;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-3658.544,257.4301;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;124;-2738.646,1172.048;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;252;-2645.583,-385.6761;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;232;-2951.809,1548.748;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;-1699.894,4487.208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;73;-2433.742,-497.2753;Inherit;False;1101.18;480.941;;7;71;67;69;68;66;4;148;Main Texture or Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;202;-2034.688,4127.385;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-1709.894,4347.209;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;140;-2691.647,910.7771;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;296;-6162.497,5053.605;Inherit;False;InstancedProperty;_AppearGradientClampSwitch;Appear Gradient Clamp Switch;59;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;155;-59.49759,2879.275;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-3655.852,404.7993;Inherit;False;Property;_VerticalGradientRemapMax;Vertical Gradient Remap Max;43;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;321;-5682.531,4537.015;Inherit;False;Property;_AppearGradientDistortionAmount;Appear Gradient Distortion Amount;64;0;Create;True;0;0;False;0;False;0;0.125;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;241;-3185.673,1612.431;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;320;-5334.257,4445.815;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;-1369.776,4215.107;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;66;-2309.613,-225.5781;Inherit;False;Property;_MainTexChannels;MainTex Channels;10;0;Create;True;0;0;False;0;False;1,0,0,0;0.25,0,0,0.0125;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;299;-5821.212,5039.867;Inherit;False;Property;_AppearGradientOffset;Appear Gradient Offset;61;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2393.679,-427.389;Inherit;True;Property;_MainTex;MainTex;9;0;Create;True;0;0;False;0;False;-1;None;2af1591491d421149a8a3382d8ce2ae6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;104;-3275.853,246.5982;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;197;-1365.776,4334.107;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;295;-5831.321,4809.748;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;125;-2485.885,1077.569;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-2491.459,861.7017;Inherit;False;Property;_InnerRimFlipSwitch;Inner Rim Flip Switch;48;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-3008.734,436.289;Inherit;False;Property;_VerticalGradientFlipSwitch;Vertical Gradient Flip Switch;40;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;234;-2769.12,1546.87;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;127;-2342.95,1078.204;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;198;-1194.171,4258.989;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;297;-5490.911,4899.284;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-2063.656,-305.3907;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;235;-2820.061,1666.121;Float;False;Property;_DepthMaskExp;Depth Mask Exp;55;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-5555.06,5135.409;Inherit;False;Property;_AppearGradientRemapMax;Appear Gradient Remap Max;60;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;108;-3057.328,256.8189;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;149;210.7075,2870.879;Inherit;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;236;-2592.362,1613.682;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;134;-2179.235,994.6138;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-990.5149,4421.784;Inherit;False;Property;_TrisGlareNegate;Tris Glare Negate;38;0;Create;True;0;0;False;0;False;1;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;300;-5167.433,4923.852;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;136;-2219.236,870.614;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;204;-1004.681,4224.344;Inherit;True;Property;_TrisGlareTexture;Tris Glare Texture;35;0;Create;True;0;0;False;0;False;-1;None;170a0874b6766f2449368f259923c19d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RoundOpNode;111;-2681.539,345.0308;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;-2573.329,1744.418;Inherit;False;Property;_DepthMaskMultiply;Depth Mask Multiply;56;0;Create;True;0;0;False;0;False;0.25;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;110;-2741.753,188.6925;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1968.5,-123.8522;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;112;-2528.936,301.4308;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;-2301.329,1640.418;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;69;-1925.063,-304.1308;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;303;-4958.606,5097.192;Inherit;False;Property;_AppearGradientFlipSwitch;Appear Gradient Flip Switch;57;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;302;-4970.891,4929.312;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-1975.237,940.614;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-2215.33,1273.701;Inherit;False;Property;_InnerRimExp;Inner Rim Exp;47;0;Create;True;0;0;False;0;False;4;2;0.1;16;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;206;-570.5201,4309.205;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-445.3161,4193.494;Inherit;False;Constant;_Float3;Float 3;38;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-2137.987,1564.165;Inherit;False;Constant;_Float5;Float 5;46;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;207;-442.8318,4309.31;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;306;-4703.374,4824.216;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;113;-2313.596,313.3116;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;237;-2135.222,1637.119;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;128;-1817.21,1028.224;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1796.325,945.3464;Inherit;False;Constant;_Float1;Float 1;24;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;304;-4671.98,5104.017;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-1669.298,-307.9105;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;71;-1533.419,-308.262;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;246;-1929.521,1608.723;Inherit;False;Property;_DepthMaskEnabled;Depth Mask Enabled;52;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;137;-1632.193,973.9336;Inherit;False;Property;_InnerRimEnabled;Inner Rim Enabled;46;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;318;-1257.879,-390.9992;Inherit;False;Property;_AppearGradientDynamicsNegate;Appear Gradient Dynamics Negate;63;0;Create;True;0;0;False;0;False;0;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;114;-2172.595,290.3116;Inherit;True;Property;_VerticalGradientProfile;Vertical Gradient Profile;45;0;Create;True;0;0;False;0;False;-1;None;ee72dffe3d130754f8e391edbe398f04;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;208;-190.4908,4257.457;Inherit;False;Property;_TrisGlareEnabled;Tris Glare Enabled;34;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;305;-4467.251,4957.974;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;243;-1824.169,391.7099;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;339;-1136.057,757.5578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;307;-4089.131,5036.239;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;317;-948.1503,-475.1101;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-2025.334,205.6323;Inherit;False;Constant;_Float0;Float 0;22;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;74;-1196.412,106.8685;Inherit;False;1388.549;404.67;;7;38;39;8;7;238;116;165;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;287;199.2944,4207.346;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;288;399.2714,4266.017;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;340;-1007.46,756.9104;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;117;-1747.043,273.998;Inherit;False;Property;_VerticalGradientEnabled;Vertical Gradient Enabled;39;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;310;-3938.385,5012.279;Inherit;True;Property;_AppearGradientProfile;Appear Gradient Profile;65;0;Create;True;0;0;False;0;False;-1;None;5bd9da326da7b6c469a38f9bbc4d9ec2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;319;-802.9938,-468.062;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-1184.252,156.6196;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;316;-632.8336,-474.4279;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-852.6934,253.7163;Inherit;False;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;238;-679.517,246.2686;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-539.3094,191.8288;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;-374.7098,181.5272;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;39;-404.5695,303.7124;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;182;-1643.262,-1155.992;Inherit;False;1675.998;358.3306;;10;179;180;176;177;178;173;172;174;175;181;Ramp UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-1460.648,-930.6616;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-151.6697,280.9121;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-1593.262,-1085.771;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;6;0;Create;True;0;0;False;0;False;1;0.85;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;180;-1228.96,-1098.018;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-1045.611,-1044.527;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;7;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;-767.3133,-1105.992;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;178;-618.2709,-1103.548;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;172;-578.1317,-975.8105;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;8;0;Create;True;0;0;False;0;False;1;8;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;173;-446.3449,-1100.098;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;174;-300.0137,-1102.124;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;72;262.1499,-1421.751;Inherit;False;1518.337;862.0967;;8;168;2;3;1;167;166;171;170;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;175;-154.2641,-1094.648;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;167;304.4994,-1098.298;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;170;552.3237,-926.8035;Inherit;False;Property;_RampColorTint;Ramp Color Tint;5;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;166;468.8099,-1119.744;Inherit;True;Property;_Ramp;Ramp;4;0;Create;True;0;0;False;0;False;-1;None;ed17826310cbfb74384b61ba285b5fad;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;143;-1287.979,2714.18;Inherit;False;Property;_OffsetGChannelMasking;Offset G Channel Masking;26;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-1160.979,2622.18;Inherit;False;Constant;_Float2;Float 2;26;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;879.8612,-1006.969;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;381.1499,-1371.751;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;0.5943396,0.5943396,0.5943396,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;76;-4773.727,-1089.918;Inherit;False;861.1541;384.4372;;5;25;26;20;18;19;Radial Dissolve Option (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;81;-826.8661,3364.749;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;83;-828.8135,3510.549;Inherit;False;Property;_OffsetPower;Offset Power;25;0;Create;True;0;0;False;0;False;0;-0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;1315.757,-662.1313;Inherit;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;4;40;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;144;-937.8562,2654.179;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;75;-7958.767,1225.954;Inherit;False;2907.625;864.141;;21;16;56;57;59;60;15;30;17;29;9;27;28;24;55;58;62;63;64;65;61;54;Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;286;-874.2848,994.2014;Inherit;False;1271.459;629.8335;;8;276;285;284;274;275;283;282;277;Rim Negare Adjustments (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;168;1198.168,-1196.232;Inherit;False;Property;_RampEnabled;Ramp Enabled;3;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;262;-3167.816,-396.9387;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;-6392.571,1275.954;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;277;222.1738,1339.157;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;57;-6142.397,1303.03;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-7607.484,1729.394;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-6604.774,1851.182;Inherit;False;Property;_DissolveExp;Dissolve Exp;17;0;Create;True;0;0;False;0;False;6.47;6.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-4081.573,-967.4813;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;336;-3913.498,2360.543;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;26;-4403.573,-820.4813;Inherit;False;Property;_DissolveRadialAmount;Dissolve Radial Amount;16;0;Create;True;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-6440.896,1469.998;Inherit;False;Property;_DissolveExpReversed;Dissolve Exp Reversed;18;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;60;-5679.064,1557.086;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;284;-703.7433,1157.587;Inherit;False;Property;_OffsetToOpacityDotsNegate;Offset To Opacity Dots Negate;31;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-535.6038,3296.386;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-2912.416,-528.5385;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;61;-5816.037,1844.129;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;282;-115.6628,1287.869;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;258;149.0719,3120.912;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;-358.6777,1044.201;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;65;-5226.141,1636.366;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;309;-4574.191,5235.021;Inherit;False;Property;_AppearGradientProgress;Appear Gradient Progress;62;0;Create;True;0;0;False;0;False;0;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;341;66.30217,3873.518;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;59;-5836.021,1554.404;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;18;-4723.727,-1039.918;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-7908.766,1726.871;Inherit;False;Property;_DissolveTextureScale;Dissolve Texture Scale;15;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-4501.428,-1038.618;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-4990.124,1413.91;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;259;-160.3929,3243.366;Inherit;False;Property;_OffsetToOpacityRimIntensity;Offset To Opacity Rim Intensity;30;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-7862.337,1888.095;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-7197.747,1675.252;Inherit;True;Property;_DissolveTexture;Dissolve Texture;13;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;63;-5589.892,1839.367;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;16;-7428.466,1694.152;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-5358.976,1634.743;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;308;-4272.943,5085.652;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-3418.816,-412.9387;Inherit;False;Property;_MainTexStretch;MainTex Stretch;11;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;62;-5518.591,1558.988;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;275;-316.0364,1417.821;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;274;-515.5506,1378.219;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;285;-507.478,1512.316;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;58;-6108.536,1650.661;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;55;-6391.243,1763.122;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;263;-3352.816,-342.9387;Inherit;False;Constant;_Float6;Float 6;49;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;15;-7710.603,1546.877;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;1611.887,-741.1287;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;27;-6896.834,1851.313;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-824.2848,1509.035;Inherit;False;Property;_OffsetToOpacityRimNegate;Offset To Opacity Rim Negate;29;0;Create;True;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;20;-4314.226,-1037.318;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;28;-6729.39,1697.164;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;14;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1928.813,-36.69239;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/ShieldICO;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;98;0;96;0
WireConnection;98;1;97;0
WireConnection;265;0;84;0
WireConnection;265;1;264;0
WireConnection;265;2;266;0
WireConnection;215;0;220;0
WireConnection;215;1;210;2
WireConnection;95;0;265;0
WireConnection;95;1;98;0
WireConnection;218;0;212;2
WireConnection;218;1;219;0
WireConnection;94;0;95;0
WireConnection;89;0;88;2
WireConnection;89;1;86;0
WireConnection;90;0;88;2
WireConnection;90;1;87;0
WireConnection;211;0;215;0
WireConnection;211;1;218;0
WireConnection;213;0;211;0
WireConnection;92;0;94;1
WireConnection;92;1;90;0
WireConnection;91;0;94;0
WireConnection;91;1;89;0
WireConnection;93;0;91;0
WireConnection;93;1;92;0
WireConnection;214;0;213;0
WireConnection;221;1;222;0
WireConnection;221;0;214;0
WireConnection;79;1;93;0
WireConnection;334;0;324;2
WireConnection;328;0;334;0
WireConnection;103;0;99;2
WireConnection;101;0;100;0
WireConnection;216;0;221;0
WireConnection;216;1;79;1
WireConnection;156;0;216;0
WireConnection;156;1;157;0
WireConnection;102;0;99;2
WireConnection;102;1;103;0
WireConnection;102;2;101;0
WireConnection;326;0;328;0
WireConnection;326;1;332;0
WireConnection;290;0;289;2
WireConnection;199;0;322;0
WireConnection;199;1;323;0
WireConnection;327;0;326;0
WireConnection;293;0;292;0
WireConnection;154;0;156;0
WireConnection;154;1;153;0
WireConnection;250;0;102;0
WireConnection;254;0;255;0
WireConnection;254;1;253;2
WireConnection;231;0;230;0
WireConnection;141;0;122;0
WireConnection;291;0;289;2
WireConnection;291;1;290;0
WireConnection;291;2;293;0
WireConnection;330;0;333;0
WireConnection;330;1;154;0
WireConnection;330;2;327;0
WireConnection;256;0;254;0
WireConnection;200;0;193;0
WireConnection;200;1;199;0
WireConnection;249;0;102;0
WireConnection;249;1;250;0
WireConnection;249;2;248;0
WireConnection;294;0;291;0
WireConnection;105;0;249;0
WireConnection;105;1;106;0
WireConnection;252;0;251;0
WireConnection;252;1;256;0
WireConnection;232;0;231;0
WireConnection;195;0;201;2
WireConnection;195;1;87;0
WireConnection;202;0;200;0
WireConnection;194;0;201;2
WireConnection;194;1;86;0
WireConnection;140;0;122;0
WireConnection;140;1;141;0
WireConnection;155;0;330;0
WireConnection;241;0;232;0
WireConnection;241;2;242;0
WireConnection;320;0;155;0
WireConnection;320;1;321;0
WireConnection;196;0;202;0
WireConnection;196;1;194;0
WireConnection;4;1;252;0
WireConnection;104;0;105;0
WireConnection;104;2;107;0
WireConnection;197;0;202;1
WireConnection;197;1;195;0
WireConnection;295;0;291;0
WireConnection;295;1;294;0
WireConnection;295;2;296;0
WireConnection;125;0;140;0
WireConnection;125;1;124;0
WireConnection;234;0;241;0
WireConnection;127;0;125;0
WireConnection;198;0;196;0
WireConnection;198;1;197;0
WireConnection;297;0;295;0
WireConnection;297;1;299;0
WireConnection;297;2;320;0
WireConnection;68;0;4;0
WireConnection;68;1;66;0
WireConnection;108;0;104;0
WireConnection;149;0;155;0
WireConnection;149;1;155;0
WireConnection;236;0;234;0
WireConnection;236;1;235;0
WireConnection;134;0;127;0
WireConnection;300;0;297;0
WireConnection;300;2;301;0
WireConnection;136;0;133;0
WireConnection;204;1;198;0
WireConnection;111;0;109;0
WireConnection;110;0;108;0
WireConnection;148;0;68;0
WireConnection;148;1;149;0
WireConnection;112;0;108;0
WireConnection;112;1;110;0
WireConnection;112;2;111;0
WireConnection;240;0;236;0
WireConnection;240;1;239;0
WireConnection;69;0;148;0
WireConnection;302;0;300;0
WireConnection;135;0;127;0
WireConnection;135;1;134;0
WireConnection;135;2;136;0
WireConnection;206;0;204;1
WireConnection;206;1;205;0
WireConnection;207;0;206;0
WireConnection;306;0;302;0
WireConnection;113;0;112;0
WireConnection;237;0;240;0
WireConnection;128;0;135;0
WireConnection;128;1;126;0
WireConnection;304;0;303;0
WireConnection;67;0;69;0
WireConnection;67;1;69;1
WireConnection;67;2;69;2
WireConnection;67;3;69;3
WireConnection;71;0;67;0
WireConnection;246;1;247;0
WireConnection;246;0;237;0
WireConnection;137;1;138;0
WireConnection;137;0;128;0
WireConnection;114;1;113;0
WireConnection;208;1;209;0
WireConnection;208;0;207;0
WireConnection;305;0;302;0
WireConnection;305;1;306;0
WireConnection;305;2;304;0
WireConnection;243;0;114;1
WireConnection;243;1;246;0
WireConnection;339;0;137;0
WireConnection;339;1;246;0
WireConnection;307;0;305;0
WireConnection;317;0;71;0
WireConnection;317;1;318;0
WireConnection;287;0;208;0
WireConnection;287;1;4;2
WireConnection;288;0;287;0
WireConnection;340;0;339;0
WireConnection;117;1;118;0
WireConnection;117;0;243;0
WireConnection;310;1;307;0
WireConnection;319;0;317;0
WireConnection;316;0;310;1
WireConnection;316;1;310;2
WireConnection;316;2;319;0
WireConnection;316;3;128;0
WireConnection;116;0;71;0
WireConnection;116;1;117;0
WireConnection;116;2;340;0
WireConnection;116;3;165;0
WireConnection;116;4;288;0
WireConnection;116;5;310;2
WireConnection;238;0;116;0
WireConnection;238;1;316;0
WireConnection;7;0;238;0
WireConnection;8;0;7;0
WireConnection;181;0;243;0
WireConnection;181;1;128;0
WireConnection;38;0;8;0
WireConnection;38;1;39;4
WireConnection;180;0;181;0
WireConnection;180;1;38;0
WireConnection;180;2;179;0
WireConnection;177;0;180;0
WireConnection;177;1;176;0
WireConnection;178;0;177;0
WireConnection;173;0;178;0
WireConnection;174;0;173;0
WireConnection;174;1;172;0
WireConnection;175;0;174;0
WireConnection;167;0;175;0
WireConnection;166;1;167;0
WireConnection;171;0;166;0
WireConnection;171;1;170;0
WireConnection;144;0;145;0
WireConnection;144;1;114;2
WireConnection;144;2;143;0
WireConnection;168;1;1;0
WireConnection;168;0;171;0
WireConnection;262;0;261;0
WireConnection;262;1;263;0
WireConnection;277;0;282;0
WireConnection;57;0;54;1
WireConnection;57;4;56;0
WireConnection;30;0;17;0
WireConnection;30;1;29;4
WireConnection;25;0;20;0
WireConnection;25;1;26;0
WireConnection;336;0;265;0
WireConnection;60;0;59;0
WireConnection;82;0;216;0
WireConnection;82;1;81;0
WireConnection;82;2;83;0
WireConnection;82;3;144;0
WireConnection;260;0;251;0
WireConnection;260;1;262;0
WireConnection;282;0;275;0
WireConnection;282;1;283;0
WireConnection;258;0;155;0
WireConnection;258;1;277;0
WireConnection;283;0;4;3
WireConnection;283;1;128;0
WireConnection;65;0;64;0
WireConnection;59;0;58;0
WireConnection;59;1;57;0
WireConnection;19;0;18;0
WireConnection;77;0;65;0
WireConnection;9;1;16;0
WireConnection;63;0;61;1
WireConnection;16;0;15;0
WireConnection;16;1;30;0
WireConnection;16;2;29;3
WireConnection;64;0;62;0
WireConnection;64;1;63;0
WireConnection;308;0;305;0
WireConnection;308;1;309;0
WireConnection;62;0;60;0
WireConnection;275;0;274;0
WireConnection;275;1;285;0
WireConnection;274;0;137;0
WireConnection;285;0;276;0
WireConnection;58;0;55;0
WireConnection;55;0;28;0
WireConnection;55;1;24;0
WireConnection;3;0;168;0
WireConnection;3;1;2;0
WireConnection;27;0;9;1
WireConnection;20;0;19;0
WireConnection;28;1;9;1
WireConnection;28;0;27;0
WireConnection;0;2;3;0
WireConnection;0;9;38;0
WireConnection;0;11;82;0
ASEEND*/
//CHKSM=E0078CC88C8C99A7C528CEC771C1E1B90DAE8DCA