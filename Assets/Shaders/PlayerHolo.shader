Shader "PlayerHolo"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HDR] _HoloColor ("Holo Color (HDR)", Color) = (0,1,1,1) // Warna Hologram (Cyan/Biru)
        _ScanSpeed ("Scan Speed", Float) = 2.0      // Kecepatan gerak garis
        _ScanDensity ("Scan Density", Float) = 50.0 // Kerapatan garis (makin besar makin tipis)
        _Transparency ("Transparency", Range(0,1)) = 0.8 // Transparansi keseluruhan
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color; // Ambil warna dari SpriteRenderer (jika ada tint)
                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            
            // Variabel Custom Kita
            float4 _HoloColor;
            float _ScanSpeed;
            float _ScanDensity;
            float _Transparency;

            fixed4 frag(v2f IN) : SV_Target
            {
                // 1. Ambil Gambar Asli Pesawat
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                
                // Jika pixel transparan, jangan digambar (biar performa hemat)
                if (c.a < 0.01) discard;

                // 2. Logika Garis Scanline (Sinus Wave)
                // Menggunakan koordinat Y (IN.texcoord.y) dan Waktu (_Time.y)
                // Ditambah 0.5 * 0.5 agar hasilnya range [0.5 - 1.0] (tidak pernah hitam total)
                float scanline = 0.7 + 0.3 * sin((IN.texcoord.y - _Time.y * _ScanSpeed) * _ScanDensity);

                // 3. Gabungkan Warna
                // Warna Asli Texture * Warna Hologram * Efek Scanline
                fixed4 finalColor = c * _HoloColor * scanline;

                // 4. Atur Alpha (Transparansi)
                // Alpha diambil dari texture asli * Slider Transparansi
                finalColor.a = c.a * _Transparency;

                // Kembalikan warna yang sudah dicampur tint sprite asli (opsional)
                finalColor.rgb *= finalColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
}