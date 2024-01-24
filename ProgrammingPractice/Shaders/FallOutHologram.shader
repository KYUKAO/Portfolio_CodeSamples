Shader "Custom/FalloutHologram"
{
    Properties
    {
        _Scale("Scale", Float) = 1.0
        _FresnelPower("Fresnel Power", Float) = 1.0
        _FresnelScale("Fresnel Scale",Float)=1.0
         _FresnelBias("Fresnel Bias",Float)=1.0
        _MainTex("MainTex", 2D) = "white" {}
        [HDR]_EmissionColor("EmissionColor", Color) = (1,0,0,0)
        _Contrast("Contrast", Vector) = (0.5,0.9,0,0)
        
 [Space(20)]
 _OutlineWidth("_OutlineWidth",float) =1
        [HDR]_OutlineColor("_OutlineColor",Color)=(1,0,0,0)   

    }
    SubShader
    {
        Tags {"RenderType"="Opaque"  }

        Pass
        {
//            ///AlphaBlend
//            Blend SrcAlpha OneMinusSrcAlpha
//            ZWrite Off 
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPosition: TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float3 viewDir : TEXCOORD4;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Scale;
            float _FresnelPower;
            float _FresnelScale;
            float  _FresnelBias;
            fixed4 _EmissionColor;
            float2 _Contrast;

            ///Remap
            float remap(float value, float from1, float to1, float from2, float to2)
            {

                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = mul(unity_ObjectToWorld,v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.screenPos = normalize(ComputeScreenPos(o.vertex));
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                /// Remap screen position
                float baseMap = remap(i.screenPos.y, 0, 1, _Scale,  -_Scale);
                baseMap  = sin(baseMap);
                
                baseMap = remap(baseMap, -1, 1, _Contrast.x, _Contrast.y);
                float3 N = normalize(i.worldNormal);
                float3 V = normalize( UnityWorldSpaceViewDir(i.worldPosition.xyz));
                float fresnel =  _FresnelBias+_FresnelScale*pow(1.0-max(dot(N,V),0),_FresnelPower);
                fresnel=pow(fresnel,1);
                float4 mainTex=tex2D(_MainTex, i.uv);
                // float4 blackWhiteTex = (mainTex.r+mainTex.g+mainTex.b);
                
                fresnel = remap(fresnel,0,1,0.5,1);
                float4 col = mainTex * baseMap * fresnel* _EmissionColor;
                // clip(A - _CutoutThreshold);
                return float4(col.rgb, baseMap);
                return  fresnel;
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal:NORMAL;
                float4 tangent:TANGENT;
                float4 color :  COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
                
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xyz += normalize( v.color.rgb*2-1)*_OutlineWidth*0.01;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            float4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
