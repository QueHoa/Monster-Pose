Shader "prime[31]/Transitions/Doorway" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Perspective ("Perspective", Range(0, 2)) = 1.5
		_Depth ("Depth", Range(0, 5)) = 3
		_Direction ("Direction", Float) = 1
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