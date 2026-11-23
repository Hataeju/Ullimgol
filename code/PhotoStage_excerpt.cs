// 발췌: Assets/Scripts/UI/PhotoStage.cs (울림골 작업본)
// v7에서 추가한 "사진을 잡아 돌려 뒷면을 읽는" 부분만 남겼다. 이 파일만으로는 컴파일되지 않는다(K, Story, HUD 등 게임의 다른 코드에 의존).

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ullimgol.UI
{
    /// <summary>
    /// v7 playtest: the old photo as an object you hold, not a picture with a flip button. A small stage far below the world
    /// (its own camera drawing into a render texture) holds a thin photo card: the print on the front, the paper back with the
    /// ballpoint note on the back. Dragging turns it over and around; the wheel brings it closer.
    /// The faces are unlit (always readable) and brighten or dim with how squarely they face you, so turning it reads as 3D.
    /// </summary>
    public class PhotoStage : MonoBehaviour
    {
        const int StageLayer = 30;
        static PhotoStage inst;
        Camera cam; Transform card; RenderTexture rt; Material front, back, edge;
        public float yaw, pitch, tYaw, tPitch, vYaw, dist = 0.27f, tDist = 0.27f; public bool dragging;
        float backT;
        public RenderTexture Texture => rt;

        public static PhotoStage Get() { if (inst == null) inst = Build(); return inst; }

        public void ResetView(bool showBack)
        {
            yaw = tYaw = showBack ? 180f : 0f; pitch = tPitch = 0f; vYaw = 0f; dist = tDist = 0.27f; backT = 0f; dragging = false;
            Apply();
        }

        // … Build(): 사진 카드(앞면 인쇄 · 뒷면 종이)와 뒷면 볼펜 메모를 만드는 부분은 생략.
        //   이후 작업본(v8)에서 앞면 그림과 메모 문구가 바뀌어, v7 시연 화면과 같지 않다.

        void Update()
        {
            float dt = Mathf.Min(Time.unscaledDeltaTime, 0.05f);
            if (!dragging && Mathf.Abs(vYaw) > 0.01f)
            {
                tYaw += vYaw * dt;
                vYaw = Mathf.MoveTowards(vYaw, 0f, (Mathf.Abs(vYaw) * 4f + 30f) * dt);
            }
            float k = 1f - Mathf.Exp(-dt * 14f);
            yaw = Mathf.Lerp(yaw, tYaw, k); pitch = Mathf.Lerp(pitch, tPitch, k); dist = Mathf.Lerp(dist, tDist, 1f - Mathf.Exp(-dt * 10f));
            Apply();
            // the first time the back is held up long enough to read, the note counts as read
            bool showingBack = Vector3.Dot(card.forward, Vector3.forward) < -0.3f;
            backT = showingBack ? backT + dt : 0f;
            if (backT > 0.5f && !Story.Has(Story.PhotoBack))
            {
                Story.Set(Story.PhotoBack); SaveSys.Save();
                HUD.PendingCaption = "선광장 탈의실, 17번 사물함.";
            }
        }

        void Apply()
        {
            if (card == null) return;
            card.localRotation = Quaternion.Euler(pitch, yaw, -1.2f);
            if (cam != null) cam.transform.localPosition = new Vector3(0, 0, -dist);
            // a face turned edge-on gets darker, like paper under a lamp
            float f = Vector3.Dot(card.forward, Vector3.forward);   // +1: front faces you, -1: back faces you
            front.SetColor("_BaseColor", Color.white * Mathf.Lerp(0.55f, 1f, Mathf.Clamp01(f)));
            back.SetColor("_BaseColor", Color.white * Mathf.Lerp(0.55f, 0.97f, Mathf.Clamp01(-f)));
        }
    }

    /// <summary>The picture of the stage in the vault: drag to turn the photo, wheel to bring it closer.</summary>
    public class PhotoTurn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerDownHandler
    {
        public PhotoStage stage;
        public void OnPointerDown(PointerEventData e) { if (stage != null) stage.vYaw = 0f; }
        public void OnBeginDrag(PointerEventData e) { if (stage != null) stage.dragging = true; }
        public void OnDrag(PointerEventData e)
        {
            if (stage == null) return;
            // grab-and-turn: the surface follows the mouse
            float dy = -e.delta.x * 0.4f;
            stage.tYaw += dy;
            stage.tPitch = Mathf.Clamp(stage.tPitch + e.delta.y * 0.3f, -50f, 50f);
            stage.vYaw = Mathf.Lerp(stage.vYaw, dy / Mathf.Max(0.008f, Time.unscaledDeltaTime), 0.5f);
        }
        public void OnEndDrag(PointerEventData e) { if (stage == null) return; stage.dragging = false; stage.vYaw = Mathf.Clamp(stage.vYaw, -540f, 540f); }
        public void OnScroll(PointerEventData e) { if (stage != null) stage.tDist = Mathf.Clamp(stage.tDist - e.scrollDelta.y * 0.025f, 0.12f, 0.34f); }
        void OnDisable() { if (stage != null) stage.gameObject.SetActive(false); }
    }
}
