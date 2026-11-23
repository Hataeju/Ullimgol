# 선별 코드

울림골 Unity 작업본에서 README의 근거와 직접 이어지는 부분만 옮겼습니다.
게임의 다른 코드에 의존하므로 **이 폴더만으로는 컴파일되지 않고, 전체 소스도 아닙니다.**

| 파일 | 원래 위치 | 옮긴 범위 | 관련 근거 |
|---|---|---|---|
| [ScrollKeep.cs](ScrollKeep.cs) | `Assets/Scripts/UI/ScrollKeep.cs` | 파일 전체 | 작업대 목록 위치 유지(v7) |
| [HitFlash.cs](HitFlash.cs) | `Assets/Scripts/Creatures/HitFlash.cs` | 파일 전체 | 피해를 입은 몸이 잠깐 번쩍임(v6) |
| [PhotoStage_excerpt.cs](PhotoStage_excerpt.cs) | `Assets/Scripts/UI/PhotoStage.cs` | 발췌 | 사진을 끌어 돌리기 · 뒷면을 읽었는지 판정(v7) |
| [Locker17_excerpt.cs](Locker17_excerpt.cs) | `Assets/Scripts/Core/Story.cs` · `Assets/Scripts/World/StoryProps.cs` | 발췌 | 첫 사진은 확률로, 17번 사물함은 고정 장소에 |

- 영어 주석은 원문 그대로이고, 맨 위 한국어 주석과 `…` 표시는 발췌하면서 붙였습니다
- `PhotoStage.cs`의 `Build()`(사진 카드와 뒷면 메모를 만드는 부분)는 v7 이후 작업본(v8)에서 앞면 그림과 메모 문구가 바뀌어 v7 시연 화면과 같지 않으므로 뺐습니다
- 나머지 세 부분은 v7 시연에서 보이는 동작(끌어 돌리기, 15–19번 사물함 중 17번 표시, 3초 비틀어 열기, 출정 지도 단서, 목록 위치 유지, 피해 번쩍임)과 같은 코드입니다
- `ScrollKeep`은 창을 다시 그리는 곳에서 이렇게 부릅니다(`Assets/Scripts/UI/HubPanels.cs`):

```csharp
static void Reopen(string id) { var ys = ScrollKeep.Capture(modal); UIRoot.I.CloseModal(); Open(modal, id); ScrollKeep.Restore(modal, ys); }
```
