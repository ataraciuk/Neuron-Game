Shader "Custom/Skybox Shader" {
	Properties {
//		_MainTex ("Base (RGB)", 2D) = "white" {}
		resolution("Resolution", Vector) = (1024,1024,0,0)
		Rotation("Rotation", Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass {
			GLSLPROGRAM
		
			#ifdef GL_ES
			precision mediump float;
			#endif
			
			// burl figure
			varying vec4 position;
			uniform vec4 Rotation;
			uniform vec2 resolution;
			uniform vec4 _Time;
			
			const float Pi = 3.14159;
			
			
			
			float blend(vec3 v, float t){
			 return (v.r*(1.-(t*2.))+ v.b*t + v.g*t);
			}
			
			vec3 blend3(vec3 v, float t){
				return vec3(blend(v.rgb,t),blend(v.brg,t),blend(v.grb,t));
			}
			#ifdef VERTEX 
		    void main()
			         {
			            position = gl_Vertex + vec4(0.5, 0.5, 0.5, 0.0);
			            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			         }
			#endif
			#ifdef FRAGMENT
			void main()
			{		
				float time = _Time.y/10.0;
				vec4 rads = vec4(radians(Rotation.xyzw));
				vec2 angleoffset = vec2(cos(Pi/4.0-rads.y),sin(rads.x));
				vec2 p=vec2(2.0*gl_FragCoord.xy-resolution)/max(resolution.x,resolution.y) + angleoffset;
				
				for(int i=1;i<40;i++)
				{
					vec2 newp=p;
					newp.x+=(0.6/float(i)) * sin(float(i) * p.y + time +0.8*float(i)   ) + 20.;		
					newp.y+=(0.6/float(i)) * sin(float(i) * p.x + time +0.8*float(i+10)) - 5. ;
					p=newp;
				}
//				p += vec2(cos(rads.x) , sin(rads.y));
				vec3 col = vec3(0.5*sin(3.0*p.x)+0.5,0.5*sin(3.0*p.y)+0.5,sin(p.x+p.y));
				col=sqrt(blend3(col,0.25));
				gl_FragColor=vec4(col, 1.0);
			}
		  #endif
			
		ENDGLSL
		}
	}
//	FallBack "Diffuse"
}