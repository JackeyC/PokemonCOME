Shader "Spatial Mapping/Spatial Scanning"
{
	Properties
	{
		// Main knobs
		_Center ("Center", Vector) = (0, 0, 0, -1) // world space position
		_Radius ("Radius", Range(0, 10)) = 1 // grows the pulse

		// Pulse knobs
		_PulseColor ("Pulse Color", Color) = (.145, .447, .922)
		_PulseWidth ("Pulse Width", Float) = 1

		// Wireframe knobs
		[MaterialToggle] _UseWireframe ("Use Wireframe", Int) = 1
		_WireframeColor ("Wireframe Color", Color) = (.5, .5, .5)
		_WireframeFill ("Wireframe Fill", Range(0, 1)) = .1

		// Main knobs
		_Center2 ("Previous Center", Vector) = (0, 0, 0, -1) // world space position
		_Radius2 ("Previous Radius", Range(0, 10)) = 1 // grows the pulse

		// Pulse knobs
		_PulseColor2 ("Previous Pulse Color", Color) = (.145, .447, .922)
		_WireframeColor2 ("Previous Wireframe Color", Color) = (.5, .5, .5)
	}
	
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			Offset 50, 100

			CGPROGRAM

			#pragma vertex vert 
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"

			half _Radius;
			half3 _Center;
			half3 _PulseColor;
			half  _PulseWidth;
			half3 _WireframeColor;
			half  _WireframeFill;
			int _UseWireframe;

			half _Radius2;
			half3 _Center2;
			half3 _PulseColor2;
			half3 _WireframeColor2;

		    // http://www.iquilezles.org/www/articles/functions/functions.htm
			half cubicPulse(half c, half w, half x)
			{
				x = abs(x - c);
				if ( x > w )
					return 0;
				x /= w;
				return 1 - x * x * (3 - 2 * x);
			}

			struct v2g
			{
				half4 viewPos : SV_POSITION;
				half  pulse : COLOR;
				half  pulse2 : COLOR1;
			};

			v2g vert(appdata_base v)
			{
				v2g o;

				o.viewPos = UnityObjectToClipPos(v.vertex);

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				half distToCenter = distance(_Center, worldPos.xyz);		
				half pulse = cubicPulse(_Radius, _PulseWidth, distToCenter);

				half distToCenter2 = distance(_Center2, worldPos.xyz);		
				half pulse2 = cubicPulse(_Radius2, _PulseWidth, distToCenter2);

				o.pulse = pulse;
				o.pulse2 = pulse2;

				return o;
			}

			struct g2f
			{
				float4 viewPos : SV_POSITION;
				half3  bary    : COLOR;
				half   pulse   : COLOR1;
				half   pulse2   : COLOR2;
			};

			[maxvertexcount(3)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream)
			{
				// For wireframe
				half3 barys[3] = {
					half3(1, 0, 0),
					half3(0, 1, 0),
					half3(0, 0, 1)
				};

				g2f o;

				[unroll]
				for (uint idx = 0; idx < 3; ++idx)
				{
					o.viewPos = i[idx].viewPos;
					o.bary = barys[idx];
					o.pulse = i[idx].pulse;
					o.pulse2 = i[idx].pulse2;
					triStream.Append(o);
				}
			}

			half4 frag(g2f i) : COLOR
			{
				half3 result = i.pulse * _PulseColor + i.pulse2 * _PulseColor2;

				if (!_UseWireframe)
					return half4(result, 1);

				half triBary = min( min(i.bary.x, i.bary.y), i.bary.z) * 3;
				half fwt = fwidth(triBary);
				half w = smoothstep(fwt, 0, triBary - _WireframeFill);
				
				result += w * (_WireframeColor * i.pulse + _WireframeColor2 * i.pulse2);
				
				return half4(result, 1);
			}

			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
