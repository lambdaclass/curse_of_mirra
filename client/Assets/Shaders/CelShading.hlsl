void CelShading_float(in float3 Normal, in float ToonRampSmoothness, in float3 ClipSpacePos, in float3 WorldPos, in float4 ToonRampTinting,
in float ToonRampOffset, out float3 ToonRampOutput, out float3 Direction)
{

	// set the shader graph node previews
	#ifdef SHADERGRAPH_PREVIEW
		ToonRampOutput = float3(0.5,0.5,0);
		Direction = float3(0.5,0.5,0);
	#else

		// grab the shadow coordinates
		#if SHADOWS_SCREEN
			half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
		#else
			half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		#endif 

		// grab the main light
		#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
			Light light = GetMainLight(shadowCoord);
		#else
			Light light = GetMainLight();
		#endif

		// dot product for toonramp
		half d = dot(Normal, light.direction) * 0.5 + 0.5;
		
		// toonramp in a smoothstep
		half toonRamp = smoothstep(ToonRampOffset, ToonRampOffset+ ToonRampSmoothness, d );
		// multiply with shadows;
		toonRamp *= light.shadowAttenuation;
		// add in lights and extra tinting
		ToonRampOutput = light.color * (toonRamp + ToonRampTinting) ;
		// output direction for rimlight
		Direction = light.direction;
	#endif

}