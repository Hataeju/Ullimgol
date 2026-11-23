using UnityEngine;
using UnityEngine.UI;

namespace Ullimgol.UI
{
    /// <summary>
    /// v7 playtest: a panel that rebuilds itself after a click (craft, buy, equip) used to come back scrolled to the top.
    /// Capture() remembers how far each list in the panel was scrolled; Restore() puts the rebuilt lists back there
    /// once their layout exists (a couple of frames).
    /// </summary>
    public class ScrollKeep : MonoBehaviour
    {
        float[] ys; int frames;

        public static float[] Capture(RectTransform root)
        {
            if (root == null) return null;
            var srs = root.GetComponentsInChildren<ScrollRect>();
            var a = new float[srs.Length];
            for (int i = 0; i < srs.Length; i++) a[i] = srs[i].content != null ? srs[i].content.anchoredPosition.y : 0f;
            return a;
        }

        public static void Restore(RectTransform root, float[] ys)
        {
            if (root == null || ys == null || ys.Length == 0) return;
            bool any = false; foreach (var y in ys) if (y > 0.5f) any = true;
            if (!any) return;
            var k = root.GetComponent<ScrollKeep>() ?? root.gameObject.AddComponent<ScrollKeep>();
            k.ys = ys; k.frames = 0; k.enabled = true;
        }

        void LateUpdate()
        {
            Canvas.ForceUpdateCanvases();
            var srs = GetComponentsInChildren<ScrollRect>();
            for (int i = 0; i < srs.Length && i < ys.Length; i++)
            {
                var c = srs[i].content; var vp = srs[i].viewport != null ? srs[i].viewport : (RectTransform)srs[i].transform;
                if (c == null) continue;
                float max = Mathf.Max(0f, c.rect.height - vp.rect.height);
                var p = c.anchoredPosition; p.y = Mathf.Clamp(ys[i], 0f, max); c.anchoredPosition = p;
                srs[i].velocity = Vector2.zero;
            }
            if (++frames >= 3) enabled = false;
        }
    }
}
