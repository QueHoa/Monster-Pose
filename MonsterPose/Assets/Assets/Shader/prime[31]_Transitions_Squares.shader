Shader "prime[31]/Transitions/Squares" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Fade to Color", Vector) = (0,0,0,1)
		_Progress ("Progress", Range(0, 1)) = 0
		_Size ("Size", Vector) = (64,45,0,0)
		_Smoothness ("Smoothness", Float) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}