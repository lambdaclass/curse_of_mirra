Shader "Unlit/OverlayShader"
{
    HLSLINCLUDE
 
        #include "UnityCG.cginc"
 
        // This provides access to the vertices of the mesh being rendered
        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };
 
        struct v2f
        {
            float4 vertex : SV_POSITION;
        };
 
        // Vertex shader
        v2f Vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            return o;
        }
 
        // Fragment shader (also called Pixel shader)
        float4 Frag(v2f i) : SV_Target
        {
            // For now, just output the color red
            float4 col = float4(1, 0, 0, 1);
            return col;
        }
 
    ENDHLSL
 
    SubShader
    {
        Pass
        {
            ZTest LEqual // Do not render "behind" existing pixels
            ZWrite Off // Do not write to the depth buffer
            Cull Back // Do not render triangles pointing away from the camera
            Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending
 
            // Run a shader program with the specified vertex and fragment shaders
            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
