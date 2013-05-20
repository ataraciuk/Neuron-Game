Shader "Custom/Glow" {
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_Color ("Text Color", Color) = (1,0,1,1)
		_SquareSize("Scan Size", Float) = 1.0
	}

	SubShader {

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {	
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#define SMP1(x,y) UNITY_SAMPLE_1CHANNEL(x,y)
			
			#include "UnityCG.cginc"
			
			#define PI 3.14159265359
			#define INC 0.3927

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR	;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			//this binds uniforms to properties
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;
//			is really square? 
			uniform int _SquareSize;
			
			//vert shader
			v2f vert (appdata_t v)
			{
				v2f o;
				//multiply matrix by vertex vec4f = vec4f
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//color
				o.color = v.color * _Color;
				//
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			float sum_kernel(sampler2D tex, v2f i, float delta) 
			{	
				float a = 0.0;
				float angle = -PI/2.0+0.05;
				float total = PI/INC;
				while (angle < PI/2.0)
				{
					a+= SMP1(tex, float2(i.texcoord.x + delta*cos(angle), i.texcoord.y + delta*sin(angle)));
//					j += 1 ;
					angle+=PI*INC;
				}
			return a;
//			return pow(a, 0.5);
			}
//			
			//frag shader
			fixed4 frag (v2f i) : COLOR
			{	
//				fixed4 col = i.color;
//				fixed4 col = _Color;//i.color;
				fixed4 col = fixed4(0.15,0.45,1.,1.);
				float density =   0.15 * sum_kernel(_MainTex, i, 0.0075) 
								+ 0.33 * sum_kernel(_MainTex, i, 0.00025)
								+ 0.8 * sum_kernel(_MainTex, i, 0.0010)
								+ 1.0 * SMP1(_MainTex, i.texcoord);
				col.a *= density*0.2;
				col.r += density*0.15;
				col.g += density*0.15;
				col.b += density*0.15;
				return col;
			}
			ENDCG 
		}
	} 	
}
