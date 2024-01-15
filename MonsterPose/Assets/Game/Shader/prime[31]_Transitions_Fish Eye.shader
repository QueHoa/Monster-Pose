Shader "prime[31]/Transitions/Fish Eye" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Size ("Size", Range(0, 0.4)) = 0.2
		_Zoom ("Zoom", Range(0, 150)) = 100
		_ColorSeparation ("Color Separation", Range(0, 5)) = 0.2
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}