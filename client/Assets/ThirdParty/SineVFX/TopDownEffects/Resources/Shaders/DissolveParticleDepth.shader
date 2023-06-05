// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/DissolveParticleDepth"
{
	Properties
	{
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalPower("Final Power", Range( 0 , 60)) = 0
		_FinalOpacityPower("Final Opacity Power", Range( 1 , 8)) = 1
		_FinalOpacityExp("Final Opacity Exp", Range( 0.2 , 8)) = 1
		[Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		[Toggle(_RAMPAFFECTEDBYVERTEXCOLOR_ON)] _RampAffectedByVertexColor("Ramp Affected By Vertex Color", Float) = 0
		_RampAffectedByDynamics("Ramp Affected By Dynamics", Range( 0 , 1)) = 1
		_RampOffsetMultiply("Ramp Offset Multiply", Float) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		[Toggle(_RIMMASKENABLED_ON)] _RimMaskEnabled("Rim Mask Enabled", Float) = 1
		[Toggle(_RIMMASKFLIP_ON)] _RimMaskFlip("Rim Mask Flip", Float) = 0
		_RimMaskExp("Rim Mask Exp", Range( 0.2 , 8)) = 1
		_OffsetNoise("Offset Noise", 2D) = "white" {}
		_OffsetPower("Offset Power", Float) = 0.5
		_OffsetScaleWithSizeFixSwitch("Offset Scale With Size Fix Switch", Range( 0 , 1)) = 1
		_DissolveNoise("Dissolve Noise", 2D) = "white" {}
		[Toggle(_DISSOLVENOISEFLIP_ON)] _DissolveNoiseFlip("Dissolve Noise Flip", Float) = 0
		_DissolveNoiseExp("Dissolve Noise Exp", Float) = 6.47
		_DissolveNoiseExpReversed("Dissolve Noise Exp Reversed", Float) = 2
		[Toggle(_DEPTHFADEMAXMODE_ON)] _DepthFadeMaxMode("Depth Fade Max Mode", Float) = 1
		[Toggle(_DEPTHFADEFLIP_ON)] _DepthFadeFlip("Depth Fade Flip", Float) = 1
		_DepthFadeDistance("Depth Fade Distance", Float) = 1
		_DepthFadeExp("Depth Fade Exp", Float) = 2
		[HideInInspector]_LossyScaleGlobal("_LossyScaleGlobal", Float) = 1
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _RAMPENABLED_ON
		#pragma shader_feature _RAMPAFFECTEDBYVERTEXCOLOR_ON
		#pragma shader_feature _DEPTHFADEMAXMODE_ON
		#pragma shader_feature _RIMMASKENABLED_ON
		#pragma shader_feature _RIMMASKFLIP_ON
		#pragma shader_feature _DEPTHFADEFLIP_ON
		#pragma shader_feature _DISSOLVENOISEFLIP_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
			float3 worldNormal;
			half ASEVFace : VFACE;
			INTERNAL_DATA
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _OffsetNoise;
		uniform float4 _OffsetNoise_ST;
		uniform float _LossyScaleGlobal;
		uniform float _OffsetPower;
		uniform float _OffsetScaleWithSizeFixSwitch;
		uniform float _FinalPower;
		uniform float4 _FinalColor;
		uniform sampler2D _Ramp;
		uniform float _RampOffsetMultiply;
		uniform float _RimMaskExp;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeDistance;
		uniform float _DepthFadeExp;
		uniform sampler2D _DissolveNoise;
		uniform float4 _DissolveNoise_ST;
		uniform float _DissolveNoiseExp;
		uniform float _DissolveNoiseExpReversed;
		uniform float _RampAffectedByDynamics;
		uniform float _RampOffsetExp;
		uniform float4 _RampColorTint;
		uniform float _FinalOpacityPower;
		uniform float _FinalOpacityExp;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_OffsetNoise = v.texcoord * _OffsetNoise_ST.xy + _OffsetNoise_ST.zw;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 appendResult214 = (float3(v.texcoord1.y , v.texcoord1.z , v.texcoord1.w));
			float lerpResult215 = lerp( 1.0 , distance( ase_worldPos , appendResult214 ) , round( _OffsetScaleWithSizeFixSwitch ));
			v.vertex.xyz += ( tex2Dlod( _OffsetNoise, float4( uv_OffsetNoise, 0, 0.0) ).r * ase_worldNormal * _LossyScaleGlobal * _OffsetPower * v.texcoord1.x * v.texcoord.w * lerpResult215 );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float clampResult150 = clamp( i.uv_tex4coord.z , 0.0 , 1.0 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 switchResult12 = (((i.ASEVFace>0)?(ase_worldNormal):(-ase_worldNormal)));
			float fresnelNdotV5 = dot( switchResult12, ase_worldViewDir );
			float fresnelNode5 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV5, 1.0 ) );
			#ifdef _RIMMASKFLIP_ON
				float staticSwitch173 = ( 1.0 - fresnelNode5 );
			#else
				float staticSwitch173 = fresnelNode5;
			#endif
			float clampResult6 = clamp( staticSwitch173 , 0.0 , 1.0 );
			#ifdef _RIMMASKENABLED_ON
				float staticSwitch169 = pow( clampResult6 , _RimMaskExp );
			#else
				float staticSwitch169 = 1.0;
			#endif
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth24 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth24 = abs( ( screenDepth24 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeDistance ) );
			#ifdef _DEPTHFADEFLIP_ON
				float staticSwitch179 = ( 1.0 - distanceDepth24 );
			#else
				float staticSwitch179 = distanceDepth24;
			#endif
			float clampResult134 = clamp( staticSwitch179 , 0.0 , 1.0 );
			float clampResult25 = clamp( pow( clampResult134 , _DepthFadeExp ) , 0.0 , 1.0 );
			#ifdef _DEPTHFADEMAXMODE_ON
				float staticSwitch180 = max( staticSwitch169 , clampResult25 );
			#else
				float staticSwitch180 = ( staticSwitch169 * clampResult25 );
			#endif
			float clampResult132 = clamp( staticSwitch180 , 0.0 , 1.0 );
			float temp_output_21_0 = ( clampResult132 * 1.0 );
			float2 uv_DissolveNoise = i.uv_texcoord * _DissolveNoise_ST.xy + _DissolveNoise_ST.zw;
			float4 tex2DNode143 = tex2D( _DissolveNoise, uv_DissolveNoise );
			#ifdef _DISSOLVENOISEFLIP_ON
				float staticSwitch110 = ( 1.0 - tex2DNode143.r );
			#else
				float staticSwitch110 = tex2DNode143.r;
			#endif
			float clampResult164 = clamp( ( 1.0 - pow( ( 1.0 - pow( staticSwitch110 , _DissolveNoiseExp ) ) , (1.0 + (i.uv_tex4coord.w - 0.0) * (_DissolveNoiseExpReversed - 1.0) / (1.0 - 0.0)) ) ) , 0.0 , 1.0 );
			float clampResult167 = clamp( ( clampResult164 + (-1.0 + (i.uv_tex4coord.w - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float lerpResult192 = lerp( temp_output_21_0 , ( 1.0 - clampResult167 ) , _RampAffectedByDynamics);
			float clampResult191 = clamp( pow( ( _RampOffsetMultiply * lerpResult192 ) , _RampOffsetExp ) , 0.0 , 1.0 );
			float2 appendResult188 = (float2(clampResult191 , 0.0));
			float4 temp_output_199_0 = ( tex2D( _Ramp, appendResult188 ) * _RampColorTint );
			#ifdef _RAMPAFFECTEDBYVERTEXCOLOR_ON
				float4 staticSwitch195 = ( temp_output_199_0 * i.vertexColor );
			#else
				float4 staticSwitch195 = temp_output_199_0;
			#endif
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch184 = staticSwitch195;
			#else
				float4 staticSwitch184 = ( _FinalColor * i.vertexColor );
			#endif
			o.Emission = ( _FinalPower * clampResult150 * staticSwitch184 ).rgb;
			float clampResult54 = clamp( ( _FinalOpacityPower * temp_output_21_0 ) , 0.0 , 1.0 );
			float clampResult178 = clamp( ( 1.0 - pow( ( 1.0 - ( clampResult54 - clampResult167 ) ) , _FinalOpacityExp ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult178;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;622.61;-2572.969;1.118043;True;False
Node;AmplifyShaderEditor.CommentaryNode;206;-3045.623,68.99287;Inherit;False;2030.041;470.5715;;11;17;20;12;170;5;171;173;6;172;9;169;Rim Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;17;-2995.623,208.2984;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;202;-3005.396,1789.111;Inherit;False;2309.093;831.0731;;16;154;110;157;156;158;160;159;161;163;111;143;164;166;167;165;162;Dissolve For Subtraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.NegateNode;20;-2774.622,279.2982;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;205;-2607.261,619.1226;Inherit;False;1577.771;294.5004;;8;27;134;28;25;26;24;133;179;Depth Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2557.261,691.8569;Float;False;Property;_DepthFadeDistance;Depth Fade Distance;23;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;12;-2621.622,207.2984;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;143;-2955.396,2272.827;Inherit;True;Property;_DissolveNoise;Dissolve Noise;17;0;Create;True;0;0;False;0;False;-1;None;7c3aaf8e11390e140b7689f08a029424;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;5;-2395.063,209.0358;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;111;-2632.49,2392.938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;24;-2304.935,673.3199;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-2349.665,2414.027;Inherit;False;Property;_DissolveNoiseExp;Dissolve Noise Exp;19;0;Create;True;0;0;False;0;False;6.47;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;110;-2424.293,2294.573;Float;False;Property;_DissolveNoiseFlip;Dissolve Noise Flip;18;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;133;-2009.037,750.4858;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;171;-2080.316,312.5937;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-2220.149,2031.953;Inherit;False;Property;_DissolveNoiseExpReversed;Dissolve Noise Exp Reversed;20;0;Create;True;0;0;False;0;False;2;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;179;-1827.928,669.1226;Inherit;False;Property;_DepthFadeFlip;Depth Fade Flip;22;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;156;-2093.794,2330.18;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;173;-1911.786,207.3437;Inherit;False;Property;_RimMaskFlip;Rim Mask Flip;12;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;157;-2171.823,1839.111;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;134;-1541.249,671.372;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;160;-1921.649,1866.187;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1813.709,424.5644;Float;False;Property;_RimMaskExp;Rim Mask Exp;13;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;6;-1659.063,213.9;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;159;-1786.987,2116.219;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1583.189,798.623;Float;False;Property;_DepthFadeExp;Depth Fade Exp;24;0;Create;True;0;0;False;0;False;2;16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-1527.782,118.9929;Inherit;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;172;-1503.287,303.4129;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;161;-1615.273,2117.562;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;28;-1364.491,738.184;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;169;-1314.582,163.1927;Inherit;False;Property;_RimMaskEnabled;Rim Mask Enabled;11;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;25;-1204.49,729.6209;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;207;-862.5558,340.1818;Inherit;False;746.8475;308.3224;;4;136;181;180;132;Mix or Multiply Mode;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;162;-1571.799,2418.184;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;163;-1458.315,2120.244;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-812.5558,515.5042;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;136;-801.9012,390.1818;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;164;-1251.724,2117.537;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;165;-1275.712,2415.347;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;166;-1017.405,2240.605;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;180;-599.3788,435.0244;Inherit;False;Property;_DepthFadeMaxMode;Depth Fade Max Mode;21;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;132;-290.7083,439.7826;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;210;-2126.302,-1152.294;Inherit;False;1438.614;352.7349;;8;186;192;190;187;189;191;188;193;Ramp UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;204;-66.29848,340.5891;Inherit;False;219;183;;1;21;Lerp 0;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;203;-941.8665,1583.257;Inherit;False;237;160;;1;194;Lerp 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;167;-871.3035,2246.325;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;194;-891.8665,1633.257;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-16.29842,390.5891;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-2076.302,-978.3369;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;192;-1758.777,-1016.958;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;186;-1613.559,-1102.294;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;9;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-1375.828,-1029.474;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;190;-1499.109,-914.5591;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;10;0;Create;True;0;0;False;0;False;1;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;189;-1157.073,-980.9053;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;208;212.1913,204.2322;Inherit;False;1346.157;343.5992;;9;60;54;59;168;175;176;177;178;174;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;191;-1008.823,-982.3373;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;226.0275,249.9337;Float;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1;1;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;209;-553.8065,-1472.975;Inherit;False;1737.235;839.1213;;13;198;183;199;47;3;196;195;185;149;150;184;4;2;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;188;-854.6877,-986.3902;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;504.7548,306.3276;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;54;644.5223,304.6883;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;198;-418.2732,-840.8541;Inherit;False;Property;_RampColorTint;Ramp Color Tint;6;0;Create;True;0;0;False;0;False;1,1,1,1;1,0.7098039,0.3960778,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;219;-37.73092,3086.872;Inherit;False;1018.257;564.4822;;8;213;214;212;211;216;217;218;215;Offset scale with Size fix;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;183;-503.8065,-1028.643;Inherit;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;False;-1;None;244ec440ba2f7824dad8ce1e800567c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;47;-205.7128,-1250.938;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-177.5313,-929.8832;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;168;784.2882,300.9432;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;213;12.26908,3317.667;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-253.6984,-1422.975;Float;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;71.09195,-1094.839;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;174;787.0272,432.8314;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;3;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;175;920.1895,303.6671;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;214;259.8391,3362.684;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;212;211.9353,3163.992;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;217;314.0683,3536.354;Inherit;False;Property;_OffsetScaleWithSizeFixSwitch;Offset Scale With Size Fix Switch;16;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;211;477.196,3235.514;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;200;358.4054,1975.248;Inherit;False;599.8839;949.0031;;7;139;144;151;182;99;101;98;Vertex Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.RoundOpNode;218;591.014,3538.965;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;149;570.2333,-1280.655;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;195;260.5854,-946.18;Inherit;False;Property;_RampAffectedByVertexColor;Ramp Affected By Vertex Color;7;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;216;497.0188,3136.872;Inherit;False;Constant;_Float2;Float 2;28;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;36.08701,-1339.943;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;176;1084.048,299.9714;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;201;-2293.036,1162.436;Inherit;False;1252.075;343.949;;6;48;53;52;49;50;51;Camera Depth Fade (Disabled);1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;182;493.7073,2722.25;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;101;526.7161,2477.141;Float;False;Property;_OffsetPower;Offset Power;15;0;Create;True;0;0;False;0;False;0.5;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;479.1163,2397.785;Float;False;Property;_LossyScaleGlobal;_LossyScaleGlobal;28;1;[HideInInspector];Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;151;490.6572,2553.973;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;150;783.4881,-1211.476;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;184;698.4672,-1035.392;Inherit;False;Property;_RampEnabled;Ramp Enabled;4;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;99;507.5471,2252.505;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;4;660.5392,-1378.266;Float;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;0;40;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;177;1221.048,298.9714;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;215;796.5264,3204.937;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;144;408.4054,2025.248;Inherit;True;Property;_OffsetNoise;Offset Noise;14;0;Create;True;0;0;False;0;False;-1;None;192a67dc560de264b94c611d8d4b86f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;53;-1603.617,1313.487;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2243.036,1295.889;Float;False;Property;_CameraDepthFadeLength;Camera Depth Fade Length;26;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;1014.429,-1150.233;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2225.78,1391.385;Float;False;Property;_CameraDepthFadeOffset;Camera Depth Fade Offset;27;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1604.921,1212.436;Float;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;178;1383.349,300.2719;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;789.2889,2253.958;Inherit;False;7;7;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CameraDepthFade;48;-1859.277,1313.501;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;51;-1401.959,1252.496;Float;False;Property;_CameraDepthFadeEnabled;Camera Depth Fade Enabled;25;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1816.01,87.75301;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/DissolveParticleDepth;False;False;False;False;True;True;True;True;True;False;True;True;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;17;0
WireConnection;12;0;17;0
WireConnection;12;1;20;0
WireConnection;5;0;12;0
WireConnection;111;0;143;1
WireConnection;24;0;26;0
WireConnection;110;1;143;1
WireConnection;110;0;111;0
WireConnection;133;0;24;0
WireConnection;171;0;5;0
WireConnection;179;1;24;0
WireConnection;179;0;133;0
WireConnection;156;0;110;0
WireConnection;156;1;154;0
WireConnection;173;1;5;0
WireConnection;173;0;171;0
WireConnection;134;0;179;0
WireConnection;160;0;157;4
WireConnection;160;4;158;0
WireConnection;6;0;173;0
WireConnection;159;0;156;0
WireConnection;172;0;6;0
WireConnection;172;1;9;0
WireConnection;161;0;159;0
WireConnection;161;1;160;0
WireConnection;28;0;134;0
WireConnection;28;1;27;0
WireConnection;169;1;170;0
WireConnection;169;0;172;0
WireConnection;25;0;28;0
WireConnection;163;0;161;0
WireConnection;181;0;169;0
WireConnection;181;1;25;0
WireConnection;136;0;169;0
WireConnection;136;1;25;0
WireConnection;164;0;163;0
WireConnection;165;0;162;4
WireConnection;166;0;164;0
WireConnection;166;1;165;0
WireConnection;180;1;181;0
WireConnection;180;0;136;0
WireConnection;132;0;180;0
WireConnection;167;0;166;0
WireConnection;194;0;167;0
WireConnection;21;0;132;0
WireConnection;192;0;21;0
WireConnection;192;1;194;0
WireConnection;192;2;193;0
WireConnection;187;0;186;0
WireConnection;187;1;192;0
WireConnection;189;0;187;0
WireConnection;189;1;190;0
WireConnection;191;0;189;0
WireConnection;188;0;191;0
WireConnection;60;0;59;0
WireConnection;60;1;21;0
WireConnection;54;0;60;0
WireConnection;183;1;188;0
WireConnection;199;0;183;0
WireConnection;199;1;198;0
WireConnection;168;0;54;0
WireConnection;168;1;167;0
WireConnection;196;0;199;0
WireConnection;196;1;47;0
WireConnection;175;0;168;0
WireConnection;214;0;213;2
WireConnection;214;1;213;3
WireConnection;214;2;213;4
WireConnection;211;0;212;0
WireConnection;211;1;214;0
WireConnection;218;0;217;0
WireConnection;195;1;199;0
WireConnection;195;0;196;0
WireConnection;185;0;3;0
WireConnection;185;1;47;0
WireConnection;176;0;175;0
WireConnection;176;1;174;0
WireConnection;150;0;149;3
WireConnection;184;1;185;0
WireConnection;184;0;195;0
WireConnection;177;0;176;0
WireConnection;215;0;216;0
WireConnection;215;1;211;0
WireConnection;215;2;218;0
WireConnection;53;0;48;0
WireConnection;2;0;4;0
WireConnection;2;1;150;0
WireConnection;2;2;184;0
WireConnection;178;0;177;0
WireConnection;98;0;144;1
WireConnection;98;1;99;0
WireConnection;98;2;139;0
WireConnection;98;3;101;0
WireConnection;98;4;151;1
WireConnection;98;5;182;4
WireConnection;98;6;215;0
WireConnection;48;0;49;0
WireConnection;48;1;50;0
WireConnection;51;1;52;0
WireConnection;51;0;53;0
WireConnection;0;2;2;0
WireConnection;0;9;178;0
WireConnection;0;11;98;0
ASEEND*/
//CHKSM=0038C7225BD1D17D8E27711B2911F15A222496CD