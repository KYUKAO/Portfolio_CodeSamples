Shader "MyStylizedShader"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {}
        _M_ST ("M_ST", Vector) = (1,1,0,0)
    	[HDR]_BaseColor("Base Color",Color) =(1,1,1,1)
        _AmbientStrength("Ambient Strength",Range(0,1.0)) = 0.1
        _DiffStrength("Diff Strength",Range(0,1.0)) = 0.1
        _SpecStrength("Spec Strength",Range(0,5.0)) = 0.1
        _SpecPow("Specular Pow",Range(0.1,256)) = 1
        _Brightness("Brightness",Range(0,2.0)) = 0.5
        
	    _Noise1 ("Noise 1", 2D) = "black" {}
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _N1_ST ("N1_ST", Vector) = (1,1,0,0)

        _Noise2 ("_Noise 2", 2D) = "black" {}
        _Color2 ("Color 2", Color) = (1,1,1,1)
		_N2_ST ("N2_ST", Vector) = (1,1,0,0)

        _Noise3 ("_Noise 3", 2D) = "black" {}
        _Color3 ("Color 3", Color) = (1,1,1,1)
		_N3_ST ("N3_ST", Vector) = (1,1,0,0)
    	
    	_Noise4 ("_Noise 4", 2D) = "black" {}
        _Color4 ("Color 4", Color) = (1,1,1,1)
		_N4_ST ("N4_ST", Vector) = (1,1,0,0)
    	
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineSize("OutlineSize", Range(1.0,1.5)) = 1.01
    	
    	_ShadowColor("ShadowColor",Color) = (0,0,0,1)
    }
        SubShader
        {
	Tags {
			"LightMode" = "ForwardBase"
		}
		Pass
		{
            
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			
			#include "UnityLightingCommon.cginc" // for _LightColor0
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"//Shadow

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv: TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv: TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				SHADOW_COORDS(2)//Shadow
			};

			
			v2f vert(appdata v)
			{
				v2f o;
				o.uv = v.uv;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				TRANSFER_SHADOW(o)//Shadow
				return o;
			}
			
			sampler2D _MainTex;
			float4  _M_ST;
			float4 _BaseColor;
			float _AmbientStrength;
			float _DiffStrength;
			float _SpecStrength;
			float _SpecPow;
			float _Brightness;

			  sampler2D _Noise1;
            float4 _Color1;
            float4  _N1_ST;

            sampler2D _Noise2;
            float4 _Color2;
            float4  _N2_ST;

            sampler2D _Noise3;
            float4 _Color3;
            float4  _N3_ST;

			sampler2D _Noise4;
            float4 _Color4;
            float4  _N4_ST;
			
			float4 _ShadowColor;
			
			float4 frag(v2f i) : SV_Target
			{
				float4 baseColor = tex2D(_MainTex, i.uv*_M_ST.xy+_M_ST.zw)*_BaseColor;
				float3 ambient = _LightColor0 * _AmbientStrength;
				float3 diff = dot(i.normal,_WorldSpaceLightPos0) * _LightColor0 * _DiffStrength;
				float3 reflectDir = reflect(-_WorldSpaceLightPos0, i.normal);
				float3 spec = pow(max(dot(i.viewDir, reflectDir), 0.0), _SpecPow) * _LightColor0 * _SpecStrength;


				//shadow
				fixed shadow = SHADOW_ATTENUATION(i);

				 // Sample each texture
                fixed alpha1 = tex2D(_Noise1, i.uv*_N1_ST.xy+_N1_ST.zw).r;
                fixed alpha2 = tex2D(_Noise2, i.uv*_N2_ST.xy+_N2_ST.zw).r;
                fixed alpha3 = tex2D(_Noise3, i.uv*_N3_ST.xy+_N3_ST.zw).r;
                fixed alpha4 = tex2D(_Noise4, i.uv*_N4_ST.xy+_N4_ST.zw).r;
     
                // Calculate the final color
                float4 newColor = lerp(baseColor, _Color1, alpha1); 
                newColor = lerp(newColor, _Color2, alpha2);
                newColor = lerp(newColor, _Color3, alpha3);
                newColor = lerp(newColor, _Color4, alpha4);
				
				//final color
				float4 final_color = float4((spec + diff + ambient),1.0) * newColor * _Brightness;
				
				//shadow color
				float4 shadowColor = final_color*(1-shadow)*_ShadowColor;
				final_color=final_color*shadow+shadowColor;
				return final_color;
			}
			ENDCG
            }

            Pass
            {
		Cull Front
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                
                fixed4 _OutlineColor;
                float _OutlineSize;
                
                struct appdata
                {
                    float4 vertex:POSITION;
                };
                struct v2f
                {
                    float4 clipPos:SV_POSITION;
                };
                v2f vert(appdata v)
                {
                    v2f o;
                    o.clipPos = UnityObjectToClipPos(v.vertex * _OutlineSize);
                    return o;
                }
                fixed4 frag(v2f i) : SV_Target
                {
                    return _OutlineColor;
                }
                ENDCG
            }
        	
        	Pass
		{
			Tags {"LightMode" = "ShadowCaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
    }
}
