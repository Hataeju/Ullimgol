using System.Collections.Generic;
using UnityEngine;

namespace Ullimgol
{
    /// <summary>
    /// v6: a short bright flash on a creature's body when a hit actually does damage — the clearest "it's getting hurt" cue,
    /// on every creature (rigged or procedural).
    /// v9: the v6 flash only tinted the base colour for ~0.15 s, so in the dark (and under the muzzle flash) it barely showed.
    /// Now the body glows: each renderer gets its own copy of its material with emission switched on (black until a hit),
    /// the hit drives the emission colour (red, the body texture lit up), and the glow holds a moment before fading (~0.5 s in all). The base tint stays too.
    /// The emissive variants of the lit templates (M_LitEmissive, M_LitNormalEmissive, M_LitPBREmissive) are in the build,
    /// so the shader keyword is safe to turn on at runtime.
    /// </summary>
    public class HitFlash : MonoBehaviour
    {
        readonly List<Renderer> rs = new List<Renderer>();
        readonly List<Color> baseCols = new List<Color>(), glowCols = new List<Color>();
        readonly List<bool> glows = new List<bool>();
        MaterialPropertyBlock mpb; float t = -1f, k;
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        const float Hold = 0.08f, Fade = 0.42f, GlowPower = 3.0f;
        static readonly int BaseMap = Shader.PropertyToID("_BaseMap"), EmissionMap = Shader.PropertyToID("_EmissionMap");
        // the tint goes toward a hot off-white; the glow is red, so it stays apart from the orange muzzle flash and outlasts it
        static readonly Color Hot = new Color(1.9f, 1.55f, 1.45f), Glow = new Color(1f, 0.36f, 0.3f);

        public void Bind(Transform model)
        {
            rs.Clear(); baseCols.Clear(); glowCols.Clear(); glows.Clear(); mpb = new MaterialPropertyBlock();
            foreach (var r in model.GetComponentsInChildren<Renderer>(true))
            {
                var m = r.sharedMaterial;
                if (r is ParticleSystemRenderer || m == null || !m.HasProperty(BaseColor)) continue;
                bool glow = m.HasProperty(EmissionColor) && !m.IsKeywordEnabled("_EMISSION");
                var bc = m.GetColor(BaseColor);
                Color glowCol = Glow;
                if (glow)
                {
                    // the glow is the body's own colours lit up (emission map = albedo), so the texture still reads instead of a flat white shape
                    m = new Material(m) { name = m.name + " (hit glow)" };
                    m.EnableKeyword("_EMISSION");
                    m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
                    m.SetColor(EmissionColor, Color.black);
                    var albedo = m.HasProperty(BaseMap) ? m.GetTexture(BaseMap) : null;
                    if (albedo != null && m.HasProperty(EmissionMap)) m.SetTexture(EmissionMap, albedo);
                    else glowCol = new Color(Glow.r * bc.r, Glow.g * bc.g, Glow.b * bc.b);
                    r.sharedMaterial = m;
                }
                rs.Add(r); baseCols.Add(bc); glows.Add(glow); glowCols.Add(glowCol);
            }
        }

        public void Flash(float strength = 1f) { t = 0f; k = Mathf.Clamp01(strength); }

        void LateUpdate()
        {
            if (t < 0f) return;
            t += Time.deltaTime;
            float u = t < Hold ? 1f : Mathf.Clamp01(1f - (t - Hold) / Fade);
            float s = u * u * (3f - 2f * u) * (0.65f + 0.35f * k);   // smooth tail
            for (int i = 0; i < rs.Count; i++)
            {
                var r = rs[i]; if (r == null) continue;
                r.GetPropertyBlock(mpb);
                var c = baseCols[i];
                mpb.SetColor(BaseColor, Color.Lerp(c, new Color(Hot.r, Hot.g, Hot.b, c.a), s * 0.7f));
                if (glows[i]) mpb.SetColor(EmissionColor, glowCols[i] * (GlowPower * s));
                r.SetPropertyBlock(mpb);
            }
            if (u <= 0f)
            {
                t = -1f;
                for (int i = 0; i < rs.Count; i++)
                {
                    var r = rs[i]; if (r == null) continue;
                    r.GetPropertyBlock(mpb);
                    mpb.SetColor(BaseColor, baseCols[i]);
                    if (glows[i]) mpb.SetColor(EmissionColor, Color.black);
                    r.SetPropertyBlock(mpb);
                }
            }
        }
    }
}
