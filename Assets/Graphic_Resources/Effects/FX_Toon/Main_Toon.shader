Shader "Unlit/MainToon"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BumpMap("NormalMap", 2D) = "bump" {}
        _OutlineCol("Outline Color", Color) = (1,1,1,1)
        _OutlineRange("Outline Range",Range(0,5)) = 1
        _AmbientColor("Ambient Color", Color) = (1,1,1,1)
        _Ins("Intensity", Range(0, 5)) = 1
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }



        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
                cull front
                CGPROGRAM
                #pragma surface surf Nolight vertex:vert noshadow noambient
                float4 _OutlineCol;
                float _OutlineRange;
            void vert(inout appdata_full v) {
            v.vertex.xyz = v.vertex.xyz + v.normal.xyz * 0.005/_OutlineRange;
            }
            struct Input
            {
                float4 color:COLOR;
            };
            void surf(Input IN,inout SurfaceOutput o)
            {
            }
            float4 LightingNolight(SurfaceOutput s, float3 lightDir, float atten) {
                return float4(1, 1, 1, 0) * _OutlineCol;
            }
                ENDCG
                cull back
                    //2nd Pass
                    CGPROGRAM
                    #pragma surface surf Toon
                    sampler2D _MainTex;
                    sampler2D _BumpMap;
                    float  _Ins;
                    float4 _AmbientColor;
                    float4 _RimColor;
                    float _RimAmount;
                    float _RimThreshold;

                    struct Input {
                        float2 uv_MainTex;
                        float2 uv_BumpMap;
                        float3 worldPos;
                        float3 viewDir;
                    };
                    void surf(Input IN, inout SurfaceOutput o) {

                        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                        float4 rimDot = 1 - dot(IN.viewDir, o.Normal);
                        float3 normal = normalize(IN.worldPos);
                        float NdotL = dot(_WorldSpaceLightPos0, o.Normal);
                        float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
                        rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                        float4 rim = rimIntensity * _RimColor;

                        o.Albedo = c.rgb + rim;
                        o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) + float3(0,0,1);
                        o.Alpha = c.a;
                        
                    }


                    float4 LightingToon(SurfaceOutput s, float3 lightDir, float viewDir, float atten) {
                        float ndotl = (dot(s.Normal, lightDir) * 0.5 + 0.5);
                        ndotl = ndotl * 2;
                        ndotl = ceil(ndotl) / 3;
                        float4 final;
                        final.rgb = (s.Albedo * (ndotl * (_AmbientColor.rgb + _LightColor0.rgb)))*_Ins;
                        final.a = 1;
                        return final;
                    }
                    ENDCG
        }
            FallBack "Diffuse"
}

