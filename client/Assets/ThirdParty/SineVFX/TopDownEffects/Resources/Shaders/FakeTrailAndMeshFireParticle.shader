// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/FakeTrailAndMeshFireParticle"
{
	Properties
	{
		_FinalColor("Final Color", Color) = (1,1,1,1)
		_FinalPower("Final Power", Range( 0 , 60)) = 4
		_FinalOpacityPower("Final Opacity Power", Range( 0 , 40)) = 1
		_FinalOpacityExp("Final Opacity Exp", Float) = 4
		[Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampAffectedByDynamics("Ramp Affected By Dynamics", Range( 0 , 1)) = 1
		_RampOffsetMultiply("Ramp Offset Multiply", Float) = 1
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		_MainMask("Main Mask", 2D) = "white" {}
		_MainMaskScaleU("Main Mask Scale U", Float) = 1
		_MainMaskScaleV("Main Mask Scale V", Float) = 1
		_MainMaskScrollSpeed("Main Mask Scroll Speed", Float) = 0
		_MainMaskExp("Main Mask Exp", Range( 0.2 , 4)) = 1
		_Distortion("Distortion", 2D) = "white" {}
		_DistortionScaleU("Distortion Scale U", Float) = 1
		_DistortionScaleV("Distortion Scale V", Float) = 1
		_DistortionMask("Distortion Mask", 2D) = "white" {}
		_DistortionPowerU("Distortion Power U", Range( -1 , 1)) = 0
		_DistortionPowerV("Distortion Power V", Range( -1 , 1)) = 0
		_DistortionScrollSpeedU("Distortion Scroll Speed U", Float) = 0
		_DistortionScrollSpeedV("Distortion Scroll Speed V", Float) = 0
		_Noise01("Noise 01", 2D) = "white" {}
		_Noise01ScaleU("Noise 01 Scale U", Float) = 1
		_Noise01ScaleV("Noise 01 Scale V", Float) = 1
		_Noise01Negate("Noise 01 Negate", Range( 0 , 1)) = 0
		_Noise01Exp("Noise 01 Exp", Range( 0.2 , 8)) = 1
		_Noise01ScrollSpeed("Noise 01 Scroll Speed", Float) = 0
		_SqueezeUVAmount("Squeeze UV Amount", Float) = 0
		_SqueezeUVExp("Squeeze UV Exp", Range( 0.2 , 8)) = 1
		_EndPointMaskExp("End Point Mask Exp", Range( 0.2 , 8)) = 2
		_StartPointMaskAdd("Start Point Mask Add", Range( 0 , 1)) = 0
		_StartPointMaskExp("Start Point Mask Exp", Range( 0.2 , 8)) = 1
		[Toggle(_RIMMASKENABLED_ON)] _RimMaskEnabled("Rim Mask Enabled", Float) = 0
		_RimMaskExp("Rim Mask Exp", Range( 0.2 , 8)) = 1
		_RimStartPointBoostPower("Rim Start Point Boost Power", Range( 0 , 20)) = 0
		_RimStartPointBoostExp("Rim Start Point Boost Exp", Range( 0.2 , 8)) = 1
		_FakeTrailScrollMultiplier("Fake Trail Scroll Multiplier", Float) = 0.15
		_FakeTrailScrollRandomMin("Fake Trail Scroll Random Min", Range( 0.5 , 1)) = 0.8
		_FakeTrailScrollRandomMax("Fake Trail Scroll Random Max", Range( 1 , 1.5)) = 1.1
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
		#pragma shader_feature _RIMMASKENABLED_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 uv2_tex4coord2;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			half ASEVFace : VFACE;
		};

		uniform float4 _FinalColor;
		uniform sampler2D _Ramp;
		uniform sampler2D _MainMask;
		uniform float4 _MainMask_ST;
		uniform float _MainMaskScaleU;
		uniform float _MainMaskScaleV;
		uniform float _MainMaskScrollSpeed;
		uniform float _FakeTrailScrollMultiplier;
		uniform float _FakeTrailScrollRandomMin;
		uniform float _FakeTrailScrollRandomMax;
		uniform float _SqueezeUVAmount;
		uniform float _SqueezeUVExp;
		uniform sampler2D _Distortion;
		uniform float4 _Distortion_ST;
		uniform float _DistortionScaleU;
		uniform float _DistortionScaleV;
		uniform float _DistortionScrollSpeedU;
		uniform float _DistortionScrollSpeedV;
		uniform float _DistortionPowerU;
		uniform sampler2D _DistortionMask;
		uniform float4 _DistortionMask_ST;
		uniform float _DistortionPowerV;
		uniform float _MainMaskExp;
		uniform float _StartPointMaskExp;
		uniform float _StartPointMaskAdd;
		uniform float _FinalOpacityPower;
		uniform sampler2D _Noise01;
		uniform float4 _Noise01_ST;
		uniform float _Noise01ScaleU;
		uniform float _Noise01ScaleV;
		uniform float _Noise01ScrollSpeed;
		uniform float _Noise01Negate;
		uniform float _Noise01Exp;
		uniform float _EndPointMaskExp;
		uniform float _RampAffectedByDynamics;
		uniform float _RampOffsetMultiply;
		uniform float _RampOffsetExp;
		uniform float _FinalPower;
		uniform float _RimMaskExp;
		uniform float _RimStartPointBoostExp;
		uniform float _RimStartPointBoostPower;
		uniform float _FinalOpacityExp;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float2 uv0_MainMask = i.uv_texcoord * _MainMask_ST.xy + _MainMask_ST.zw;
			float2 appendResult321 = (float2(_MainMaskScaleU , _MainMaskScaleV));
			float2 break323 = ( uv0_MainMask * appendResult321 );
			float2 appendResult150 = (float2(( break323.x + ( _Time.y * _MainMaskScrollSpeed ) + ( ( i.uv_tex4coord.w + i.uv2_tex4coord2.x ) * _FakeTrailScrollMultiplier * (_FakeTrailScrollRandomMin + (i.uv2_tex4coord2.x - 0.0) * (_FakeTrailScrollRandomMax - _FakeTrailScrollRandomMin) / (120.0 - 0.0)) ) + ( i.uv2_tex4coord2.x * 1.4 ) ) , ( break323.y + ( (-1.0 + (break323.y - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _SqueezeUVAmount * pow( ( 1.0 - uv0_MainMask.x ) , _SqueezeUVExp ) ) )));
			float2 uv0_Distortion = i.uv_texcoord * _Distortion_ST.xy + _Distortion_ST.zw;
			float2 appendResult195 = (float2(_DistortionScaleU , _DistortionScaleV));
			float2 appendResult295 = (float2(( _Time.y * _DistortionScrollSpeedU ) , ( _Time.y * _DistortionScrollSpeedV )));
			float2 appendResult298 = (float2(( i.uv2_tex4coord2.x * 1.2 ) , ( i.uv2_tex4coord2.x * 1.8 )));
			float4 tex2DNode156 = tex2D( _Distortion, ( ( uv0_Distortion * appendResult195 ) + appendResult295 + appendResult298 ) );
			float2 uv_DistortionMask = i.uv_texcoord * _DistortionMask_ST.xy + _DistortionMask_ST.zw;
			float4 tex2DNode304 = tex2D( _DistortionMask, uv_DistortionMask );
			float2 appendResult299 = (float2(( tex2DNode156.r * _DistortionPowerU * tex2DNode304.r ) , ( tex2DNode156.r * _DistortionPowerV * tex2DNode304.r )));
			float2 break335 = ( appendResult150 + appendResult299 );
			float clampResult334 = clamp( break335.y , 0.0 , 1.0 );
			float2 appendResult336 = (float2(break335.x , clampResult334));
			float clampResult203 = clamp( ( pow( tex2D( _MainMask, appendResult336 ).r , _MainMaskExp ) + ( pow( ( 1.0 - ( i.uv_texcoord.x + 0.0 ) ) , _StartPointMaskExp ) * _StartPointMaskAdd ) ) , 0.0 , 1.0 );
			float2 uv0_Noise01 = i.uv_texcoord * _Noise01_ST.xy + _Noise01_ST.zw;
			float2 appendResult331 = (float2(_Noise01ScaleU , _Noise01ScaleV));
			float2 break332 = ( uv0_Noise01 * appendResult331 );
			float2 appendResult180 = (float2(( break332.x + ( _Time.y * _Noise01ScrollSpeed ) + ( i.uv2_tex4coord2.x * 1.6 ) ) , break332.y));
			float clampResult115 = clamp( ( tex2D( _Noise01, ( appendResult299 + appendResult180 ) ).r + _Noise01Negate ) , 0.0 , 1.0 );
			float clampResult333 = clamp( ( i.uv_texcoord.x + 0.0 + i.uv_tex4coord.z ) , 0.0 , 1.0 );
			float clampResult66 = clamp( ( clampResult203 * _FinalOpacityPower * pow( clampResult115 , _Noise01Exp ) * pow( ( 1.0 - clampResult333 ) , _EndPointMaskExp ) ) , 0.0 , 1.0 );
			float lerpResult176 = lerp( ( 1.0 - i.uv_texcoord.x ) , clampResult66 , _RampAffectedByDynamics);
			float clampResult275 = clamp( ( lerpResult176 * _RampOffsetMultiply ) , 0.0 , 1.0 );
			float clampResult276 = clamp( pow( clampResult275 , _RampOffsetExp ) , 0.0 , 1.0 );
			float2 appendResult173 = (float2(clampResult276 , 0.0));
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch175 = tex2D( _Ramp, appendResult173 );
			#else
				float4 staticSwitch175 = ( _FinalColor * i.vertexColor );
			#endif
			o.Emission = ( staticSwitch175 * _FinalPower ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 switchResult222 = (((i.ASEVFace>0)?(ase_worldNormal):((WorldNormalVector( i , float3(0,0,-1) )))));
			float fresnelNdotV204 = dot( switchResult222, ase_worldViewDir );
			float fresnelNode204 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV204, 1.0 ) );
			float clampResult228 = clamp( ( 1.0 - fresnelNode204 ) , 0.0 , 1.0 );
			float clampResult210 = clamp( pow( clampResult228 , _RimMaskExp ) , 0.0 , 1.0 );
			float switchResult236 = (((i.ASEVFace>0)?(( pow( ( 1.0 - i.uv_texcoord.x ) , _RimStartPointBoostExp ) * _RimStartPointBoostPower )):(0.0)));
			float clampResult233 = clamp( ( clampResult210 + ( clampResult210 * switchResult236 ) ) , 0.0 , 1.0 );
			#ifdef _RIMMASKENABLED_ON
				float staticSwitch207 = ( clampResult66 * clampResult233 );
			#else
				float staticSwitch207 = clampResult66;
			#endif
			o.Alpha = ( 1.0 - pow( ( 1.0 - ( staticSwitch207 * i.vertexColor.a ) ) , _FinalOpacityExp ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;6719.181;2392.428;1.822433;True;False
Node;AmplifyShaderEditor.CommentaryNode;313;-6268.301,-2143.96;Inherit;False;2668.324;1022.155;;18;323;322;321;320;319;318;150;149;168;167;153;164;152;170;151;165;290;169;Main Mask UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;320;-6045.806,-1537.136;Inherit;False;Property;_MainMaskScaleV;Main Mask Scale V;11;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;319;-6046.806,-1620.136;Inherit;False;Property;_MainMaskScaleU;Main Mask Scale U;10;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;315;-7373.872,-931.7164;Inherit;False;1031.423;1061.098;;13;270;279;297;298;292;291;243;289;307;246;245;277;278;Random Control;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;314;-5450.693,-627.5999;Inherit;False;1839.139;672.2311;;19;193;293;192;294;160;194;195;295;296;302;301;156;304;300;157;299;324;325;326;Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;326;-5342.973,-365.4583;Inherit;False;Property;_DistortionScaleV;Distortion Scale V;16;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;293;-5421.693,-41.60982;Inherit;False;Property;_DistortionScrollSpeedV;Distortion Scroll Speed V;21;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-5346.973,-450.4583;Inherit;False;Property;_DistortionScaleU;Distortion Scale U;15;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;270;-7255.472,-521.4105;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;193;-5421.102,-129.5522;Inherit;False;Property;_DistortionScrollSpeedU;Distortion Scroll Speed U;20;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;321;-5806.806,-1584.136;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;318;-6060.339,-1741.822;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;192;-5377.244,-274.6868;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-5149.626,-193.2445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;297;-6716.317,-470.9846;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;195;-5087.271,-414.8069;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;279;-6716.141,-576.6956;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;160;-5367.178,-576.5999;Inherit;False;0;156;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;294;-5147.693,-64.6098;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;322;-5591.806,-1649.136;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;323;-5436.557,-1648.681;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TexCoordVertexDataNode;243;-7256.801,-741.9738;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;290;-5067.432,-1750.519;Inherit;False;Property;_SqueezeUVExp;Squeeze UV Exp;29;0;Create;True;0;0;False;0;False;1;4;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;292;-7171.141,14.38137;Inherit;False;Property;_FakeTrailScrollRandomMax;Fake Trail Scroll Random Max;39;0;Create;True;0;0;False;0;False;1.1;1.1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;291;-7178.141,-65.61855;Inherit;False;Property;_FakeTrailScrollRandomMin;Fake Trail Scroll Random Min;38;0;Create;True;0;0;False;0;False;0.8;0.8;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;-4892.973,-523.4583;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;295;-4965.693,-120.6098;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;169;-4938.701,-1842.429;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;298;-6509.449,-524.0059;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;246;-7323.872,-834.8694;Inherit;False;Property;_FakeTrailScrollMultiplier;Fake Trail Scroll Multiplier;37;0;Create;True;0;0;False;0;False;0.15;0.175;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-4457.129,-1827.927;Inherit;False;Property;_MainMaskScrollSpeed;Main Mask Scroll Speed;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;289;-6722.427,-92.63564;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;120;False;3;FLOAT;0.8;False;4;FLOAT;1.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;170;-4764.784,-1837.911;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;317;-3475.92,438.877;Inherit;False;2200.742;748.0479;;18;188;115;189;113;114;112;190;180;128;130;131;129;327;329;330;328;331;332;Noise 01;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;296;-4446.644,-344.5275;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-4842.767,-1228.137;Inherit;False;Property;_SqueezeUVAmount;Squeeze UV Amount;28;0;Create;True;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;152;-4408.868,-2029.632;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;307;-7034.156,-613.8041;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;165;-4794.366,-1405.476;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-4301.576,-342.2315;Inherit;False;Property;_DistortionPowerU;Distortion Power U;18;0;Create;True;0;0;False;0;False;0;0.125;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-4144.051,-1902.174;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;330;-3404.287,690.8494;Inherit;False;Property;_Noise01ScaleV;Noise 01 Scale V;24;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;156;-4320.943,-543.1599;Inherit;True;Property;_Distortion;Distortion;14;0;Create;True;0;0;False;0;False;-1;None;de3dab975e978d64696053ab162b6505;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;329;-3406.287,614.8494;Inherit;False;Property;_Noise01ScaleU;Noise 01 Scale U;23;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;245;-6877.379,-881.7164;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;302;-4295.477,-258.7317;Inherit;False;Property;_DistortionPowerV;Distortion Power V;19;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;-6703.683,-355.0416;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-4547.624,-1350.496;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;304;-4311.775,-185.3688;Inherit;True;Property;_DistortionMask;Distortion Mask;17;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;331;-3181.287,642.8494;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;327;-3435.334,490.4905;Inherit;False;0;112;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;168;-3980.642,-1295.37;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;149;-3991.146,-1604.378;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-3992.335,-462.2169;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;300;-3983.377,-313.6315;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;299;-3778.555,-411.162;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-3169.498,1085.008;Inherit;False;Property;_Noise01ScrollSpeed;Noise 01 Scroll Speed;27;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;129;-3135.245,937.8472;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;150;-3789.518,-1444.916;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;328;-3014.637,547.4161;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-2901.818,1043.143;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;332;-2887.287,549.8494;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;277;-6702.468,-262.429;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;316;-2904.396,-1149.52;Inherit;False;1907.985;681.6678;;12;196;258;201;198;4;200;197;148;199;147;202;203;Main Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;303;-3216.339,-1009.053;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;310;-2550.932,1423.672;Inherit;False;2333.505;966.3761;;20;225;223;226;222;204;230;231;239;205;206;228;235;238;234;209;236;210;229;232;233;Rim Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;335;-3245.082,-1653.925;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;128;-2677.27,883.2963;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;196;-2522.99,-912.5948;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;334;-2977.184,-1552.978;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;312;-2229.453,-324.6267;Inherit;False;985.696;416.0977;;7;185;254;257;184;186;183;333;End Point U Linear Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;225;-2500.932,1816.38;Inherit;False;Constant;_Vector0;Vector 0;24;0;Create;True;0;0;False;0;False;0,0,-1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;258;-2276.493,-806.5369;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;180;-2502.017,824.3018;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;190;-2348.624,807.6475;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;254;-2179.453,-147.534;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;198;-2142.417,-792.0718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;223;-2307.01,1607.878;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;336;-2799.879,-1650.042;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;226;-2328.879,1820.754;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexCoordVertexDataNode;185;-2178.671,-274.6267;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;201;-2265.936,-679.9001;Inherit;False;Property;_StartPointMaskExp;Start Point Mask Exp;32;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-2178.535,984.1835;Inherit;False;Property;_Noise01Negate;Noise 01 Negate;25;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;112;-2192.975,785.0463;Inherit;True;Property;_Noise01;Noise 01;22;0;Create;True;0;0;False;0;False;-1;None;34a3b7afc8c389e40bda6d478fb654e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;148;-2075.764,-976.0171;Inherit;False;Property;_MainMaskExp;Main Mask Exp;13;0;Create;True;0;0;False;0;False;1;2;0.2;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2854.396,-1099.52;Inherit;True;Property;_MainMask;Main Mask;9;0;Create;True;0;0;False;0;False;-1;None;ae48ef886b6b97e478595ea91d624bf5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;257;-1846.938,-157.6989;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;222;-2082.041,1712.929;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;200;-1870.183,-787.0306;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-2127.296,-582.8526;Inherit;False;Property;_StartPointMaskAdd;Start Point Mask Add;31;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;333;-1725.434,-262.9999;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;147;-1700.965,-1050.511;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-1842.605,875.4628;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-1621.892,-755.5217;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;204;-1877.327,1713.725;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;230;-1932.935,2046.436;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;239;-1809.744,2185.743;Inherit;False;Property;_RimStartPointBoostExp;Rim Start Point Boost Exp;36;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;186;-1694.79,-23.52901;Inherit;False;Property;_EndPointMaskExp;End Point Mask Exp;30;0;Create;True;0;0;False;0;False;2;2;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;202;-1355.976,-860.7463;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;231;-1718.953,2066.037;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;184;-1621.755,-140.7107;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;115;-1717.552,873.5117;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;205;-1650.704,1705.656;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;311;-1068.572,-157.945;Inherit;False;1986.442;429.2622;;11;65;66;44;208;268;266;267;240;241;207;269;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;189;-1740.426,1004.929;Inherit;False;Property;_Noise01Exp;Noise 01 Exp;26;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;183;-1423.756,-104.7108;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1018.572,-107.945;Inherit;False;Property;_FinalOpacityPower;Final Opacity Power;2;0;Create;True;0;0;False;0;False;1;1;0;40;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-1920.205,1906.944;Inherit;False;Property;_RimMaskExp;Rim Mask Exp;34;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;228;-1487.924,1704.965;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;188;-1530.945,925.3018;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;309;-861.5314,-1067.543;Inherit;False;1416.857;486.5184;;11;178;179;177;176;272;271;275;273;274;276;173;Ramp UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;235;-1602.098,2275.048;Inherit;False;Property;_RimStartPointBoostPower;Rim Start Point Boost Power;35;0;Create;True;0;0;False;0;False;0;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;238;-1501.35,2100.067;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;203;-1171.411,-810.7281;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;209;-1326.374,1786.067;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;234;-1258.912,2130.009;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-676.3367,-72.58061;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;178;-811.5314,-1017.543;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;177;-762.2151,-783.0337;Inherit;False;Property;_RampAffectedByDynamics;Ramp Affected By Dynamics;6;0;Create;True;0;0;False;0;False;1;0.375;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;66;-515.9995,-68.04081;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;236;-1082.645,2133.405;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;210;-1051.351,1787.176;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;179;-611.0917,-992.9049;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;-789.3568,1902.632;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;176;-404.6273,-931.686;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;272;-496.838,-696.0246;Inherit;False;Property;_RampOffsetMultiply;Ramp Offset Multiply;7;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;271;-217.6377,-897.3244;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;232;-531.2703,1760.521;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;233;-392.427,1755.621;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;275;-71.90311,-904.1685;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;274;-205.0825,-744.2188;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;8;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-301.9444,138.3171;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;273;83.82484,-897.0977;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;207;-130.3708,-73.81642;Inherit;False;Property;_RimMaskEnabled;Rim Mask Enabled;33;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;308;553.1854,-1974.136;Inherit;False;1190.274;654.2145;;7;3;2;175;174;265;41;1;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;276;233.3179,-902.793;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;241;-39.63018,60.23643;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;204.0629,8.972153;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;603.1853,-1924.136;Inherit;False;Property;_FinalColor;Final Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;0.5563373,0.4386791,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;41;629.7703,-1730.06;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;173;388.3256,-910.2787;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;267;349.9799,99.59735;Inherit;False;Property;_FinalOpacityExp;Final Opacity Exp;3;0;Create;True;0;0;False;0;False;4;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;906.4572,-1804.796;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;174;799.295,-1547.931;Inherit;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;False;-1;None;244ec440ba2f7824dad8ce1e800567c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;268;404.0742,8.434128;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;1193.186,-1469.832;Inherit;False;Property;_FinalPower;Final Power;1;0;Create;True;0;0;False;0;False;4;10;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;266;568.0866,35.85223;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;175;1133.264,-1648.13;Inherit;False;Property;_RampEnabled;Ramp Enabled;4;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;269;730.8698,35.54044;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;1575.721,-1577.725;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2111.907,-195.2033;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SineVFX/TopDownEffects/FakeTrailAndMeshFireParticle;False;False;False;False;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;321;0;319;0
WireConnection;321;1;320;0
WireConnection;194;0;192;2
WireConnection;194;1;193;0
WireConnection;297;0;270;1
WireConnection;195;0;325;0
WireConnection;195;1;326;0
WireConnection;279;0;270;1
WireConnection;294;0;192;2
WireConnection;294;1;293;0
WireConnection;322;0;318;0
WireConnection;322;1;321;0
WireConnection;323;0;322;0
WireConnection;324;0;160;0
WireConnection;324;1;195;0
WireConnection;295;0;194;0
WireConnection;295;1;294;0
WireConnection;169;0;318;1
WireConnection;298;0;279;0
WireConnection;298;1;297;0
WireConnection;289;0;270;1
WireConnection;289;3;291;0
WireConnection;289;4;292;0
WireConnection;170;0;169;0
WireConnection;170;1;290;0
WireConnection;296;0;324;0
WireConnection;296;1;295;0
WireConnection;296;2;298;0
WireConnection;307;0;243;4
WireConnection;307;1;270;1
WireConnection;165;0;323;1
WireConnection;153;0;152;2
WireConnection;153;1;151;0
WireConnection;156;1;296;0
WireConnection;245;0;307;0
WireConnection;245;1;246;0
WireConnection;245;2;289;0
WireConnection;278;0;270;1
WireConnection;167;0;165;0
WireConnection;167;1;164;0
WireConnection;167;2;170;0
WireConnection;331;0;329;0
WireConnection;331;1;330;0
WireConnection;168;0;323;1
WireConnection;168;1;167;0
WireConnection;149;0;323;0
WireConnection;149;1;153;0
WireConnection;149;2;245;0
WireConnection;149;3;278;0
WireConnection;157;0;156;1
WireConnection;157;1;301;0
WireConnection;157;2;304;1
WireConnection;300;0;156;1
WireConnection;300;1;302;0
WireConnection;300;2;304;1
WireConnection;299;0;157;0
WireConnection;299;1;300;0
WireConnection;150;0;149;0
WireConnection;150;1;168;0
WireConnection;328;0;327;0
WireConnection;328;1;331;0
WireConnection;130;0;129;2
WireConnection;130;1;131;0
WireConnection;332;0;328;0
WireConnection;277;0;270;1
WireConnection;303;0;150;0
WireConnection;303;1;299;0
WireConnection;335;0;303;0
WireConnection;128;0;332;0
WireConnection;128;1;130;0
WireConnection;128;2;277;0
WireConnection;334;0;335;1
WireConnection;258;0;196;1
WireConnection;180;0;128;0
WireConnection;180;1;332;1
WireConnection;190;0;299;0
WireConnection;190;1;180;0
WireConnection;198;0;258;0
WireConnection;336;0;335;0
WireConnection;336;1;334;0
WireConnection;226;0;225;0
WireConnection;112;1;190;0
WireConnection;4;1;336;0
WireConnection;257;0;185;1
WireConnection;257;2;254;3
WireConnection;222;0;223;0
WireConnection;222;1;226;0
WireConnection;200;0;198;0
WireConnection;200;1;201;0
WireConnection;333;0;257;0
WireConnection;147;0;4;1
WireConnection;147;1;148;0
WireConnection;113;0;112;1
WireConnection;113;1;114;0
WireConnection;199;0;200;0
WireConnection;199;1;197;0
WireConnection;204;0;222;0
WireConnection;202;0;147;0
WireConnection;202;1;199;0
WireConnection;231;0;230;1
WireConnection;184;0;333;0
WireConnection;115;0;113;0
WireConnection;205;0;204;0
WireConnection;183;0;184;0
WireConnection;183;1;186;0
WireConnection;228;0;205;0
WireConnection;188;0;115;0
WireConnection;188;1;189;0
WireConnection;238;0;231;0
WireConnection;238;1;239;0
WireConnection;203;0;202;0
WireConnection;209;0;228;0
WireConnection;209;1;206;0
WireConnection;234;0;238;0
WireConnection;234;1;235;0
WireConnection;65;0;203;0
WireConnection;65;1;44;0
WireConnection;65;2;188;0
WireConnection;65;3;183;0
WireConnection;66;0;65;0
WireConnection;236;0;234;0
WireConnection;210;0;209;0
WireConnection;179;0;178;1
WireConnection;229;0;210;0
WireConnection;229;1;236;0
WireConnection;176;0;179;0
WireConnection;176;1;66;0
WireConnection;176;2;177;0
WireConnection;271;0;176;0
WireConnection;271;1;272;0
WireConnection;232;0;210;0
WireConnection;232;1;229;0
WireConnection;233;0;232;0
WireConnection;275;0;271;0
WireConnection;208;0;66;0
WireConnection;208;1;233;0
WireConnection;273;0;275;0
WireConnection;273;1;274;0
WireConnection;207;1;66;0
WireConnection;207;0;208;0
WireConnection;276;0;273;0
WireConnection;240;0;207;0
WireConnection;240;1;241;4
WireConnection;173;0;276;0
WireConnection;265;0;1;0
WireConnection;265;1;41;0
WireConnection;174;1;173;0
WireConnection;268;0;240;0
WireConnection;266;0;268;0
WireConnection;266;1;267;0
WireConnection;175;1;265;0
WireConnection;175;0;174;0
WireConnection;269;0;266;0
WireConnection;3;0;175;0
WireConnection;3;1;2;0
WireConnection;0;2;3;0
WireConnection;0;9;269;0
ASEEND*/
//CHKSM=3F115A7D4C6631AE739DC068F23B9C5C2978BA28