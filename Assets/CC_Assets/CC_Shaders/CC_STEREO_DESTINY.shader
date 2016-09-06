/*
Stereoscopic Shader

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 5th, 2016.
*/

Shader "CC_Shaders/CC_StereoShaderDestiny" {

	Properties{ }

	SubShader
	{
		//Stereo
		Pass 
		{
			ZTest Always Cull off ZWrite off

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			uniform float InterlaceValue;
			uniform sampler2D leftTopLeft;
			uniform sampler2D leftTopRight;
			uniform sampler2D leftBottomLeft;
			uniform sampler2D leftBottomRight;
			uniform sampler2D rightTopLeft;
			uniform sampler2D rightTopRight;
			uniform sampler2D rightBottomLeft;
			uniform sampler2D rightBottomRight;

			float4 frag(v2f_img IN) : COLOR0
			{

				if (fmod(IN.uv.x * InterlaceValue, 2.0) < 1.0)
				{
					if (IN.uv.x <= 0.5 && IN.uv.y > 0.5) 
					{
						return tex2D(rightTopLeft, IN.uv);
					}
					else if (IN.uv.x <= 0.5 && IN.uv.y <= 0.5)
					{
						return tex2D(rightBottomLeft, IN.uv);
					}
					else if (IN.uv.x > 0.5 && IN.uv.y >= 0.5)
					{
						return tex2D(rightTopRight, IN.uv);
					}
					else 
					{
						return tex2D(rightBottomRight, IN.uv);
					}

				}
				else 
				{
					if (IN.uv.x <= 0.5 && IN.uv.y > 0.5)
					{
						return tex2D(leftTopLeft, IN.uv);
					}
					else if (IN.uv.x <= 0.5 && IN.uv.y <= 0.5)
					{
						return tex2D(leftBottomLeft, IN.uv);
					}
					else if (IN.uv.x > 0.5 && IN.uv.y > 0.5)
					{
						return tex2D(leftTopRight, IN.uv);
					}
					else
					{
						return tex2D(leftBottomRight, IN.uv);
					}
				}
			}

			ENDCG
		}
		
		//Non Stereo
		Pass 
		{
			ZTest Always Cull off ZWrite off

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			uniform sampler2D centerTopLeft;
			uniform sampler2D centerTopRight;
			uniform sampler2D centerBottomLeft;
			uniform sampler2D centerBottomRight;

			float4 frag(v2f_img IN) : COLOR0
			{

				if (IN.uv.x <= 0.5 && IN.uv.y > 0.5) 
				{
					return tex2D(centerTopLeft, IN.uv);
				}
				else if (IN.uv.x <= 0.5 && IN.uv.y <= 0.5)
				{
					return tex2D(centerBottomLeft, IN.uv);
				}
				else if (IN.uv.x > 0.5 && IN.uv.y >= 0.5)
				{
					return tex2D(centerTopRight, IN.uv);
				}
				else 
				{
					return tex2D(centerBottomRight, IN.uv);
				}

			}

			ENDCG
		}
	}

	Fallback off

}
