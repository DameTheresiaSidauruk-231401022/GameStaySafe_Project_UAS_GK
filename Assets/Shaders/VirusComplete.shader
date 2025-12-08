Shader "VirusComplete"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        // --- Bagian Outline (Diatur di Inspector) ---
        _OutlineColor ("Outline Color", Color) = (0,1,0,1) // Hijau Default
        _OutlineThickness ("Thickness", Range(0,5)) = 1

        // --- Bagian Flash (Diatur lewat Script) ---
        _FlashColor ("Flash Color", Color) = (1,1,1,1) // Putih
        _FlashAmount ("Flash Amount", Range(0,1)) = 0  // 0 = Normal, 1 = Putih
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Biar outline akurat di berbagai ukuran gambar
            
            float4 _OutlineColor;
            float _OutlineThickness;
            float4 _FlashColor;
            float _FlashAmount;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // 1. LOGIKA OUTLINE
                // Cek 8 arah mata angin untuk mencari pinggiran
                float2 offsets[8] = {
                    float2(1,0), float2(-1,0), float2(0,1), float2(0,-1),
                    float2(1,1), float2(1,-1), float2(-1,1), float2(-1,-1)
                };

                float outlineAlpha = 0;
                for(int n=0; n<8; n++)
                {
                    // Gunakan TexelSize agar ketebalan konsisten
                    float2 uv = i.uv + offsets[n] * _MainTex_TexelSize.xy * _OutlineThickness;
                    outlineAlpha = max(outlineAlpha, tex2D(_MainTex, uv).a);
                }

                float4 col = tex2D(_MainTex, i.uv);
                
                // Area outline adalah area yang transparan pada gambar asli, tapi punya alpha di tetangganya
                outlineAlpha = outlineAlpha * (1 - col.a);
                
                // Gabungkan Warna Asli + Warna Outline
                float4 resultColor = col + outlineAlpha * _OutlineColor;

                // 2. LOGIKA HIT FLASH
                // Lerp (Campur) warna hasil tadi dengan warna putih (_FlashColor)
                // Kita kalikan _FlashAmount dengan alpha agar kotak transparan tidak ikut memutih
                float finalAlpha = max(col.a, outlineAlpha);
                float4 finalColor = lerp(resultColor, _FlashColor, _FlashAmount * finalAlpha);
                
                finalColor.a = finalAlpha;

                return finalColor;
            }
            ENDCG
        }
    }
}