using System.Collections.Generic;
using UnityEngine;

namespace Ullimgol
{
    /// <summary>
    /// v6: a short bright flash on a creature's body when a hit actually does damage — the clearest "it's getting hurt" cue,
    /// on every creature (rigged or procedural). Tints the base colour through a property block, then fades out (~0.15 s).
    /// </summary>
    public class HitFlash : MonoBehaviour
    {
        readonly List<Renderer> rs = new List<Renderer>(); readonly List<Color> baseCols = new List<Color>();
        MaterialPropertyBlock mpb; float t = -1f, k;
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        public void Bind(Transform model)
        {
            rs.Clear(); baseCols.Clear(); mpb = new MaterialPropertyBlock();
            foreach (var r in model.GetComponentsInChildren<Renderer>(true))
            {
                if (r is ParticleSystemRenderer || r.sharedMaterial == null || !r.sharedMaterial.HasProperty(BaseColor)) continue;
                rs.Add(r); baseCols.Add(r.sharedMaterial.GetColor(BaseColor));
            }
        }

        public void Flash(float strength = 1f) { t = 0f; k = Mathf.Clamp01(strength); }

        void LateUpdate()
        {
            if (t < 0f) return;
            t += Time.deltaTime;
            float u = t < 0.03f ? 1f : Mathf.Clamp01(1f - (t - 0.03f) / 0.14f);
            for (int i = 0; i < rs.Count; i++)
            {
                var r = rs[i]; if (r == null) continue;
                r.GetPropertyBlock(mpb);
                var c = baseCols[i];
                // toward a hot off-white, a touch of red: reads in the dark without looking like a muzzle flash
                var hot = new Color(1.9f, 1.55f, 1.45f, c.a);
                mpb.SetColor(BaseColor, Color.Lerp(c, hot, u * (0.55f + 0.45f * k)));
                r.SetPropertyBlock(mpb);
            }
            if (u <= 0f)
            {
                t = -1f;
                for (int i = 0; i < rs.Count; i++) { var r = rs[i]; if (r == null) continue; r.GetPropertyBlock(mpb); mpb.SetColor(BaseColor, baseCols[i]); r.SetPropertyBlock(mpb); }
            }
        }
    }
}
