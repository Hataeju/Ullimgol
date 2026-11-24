# 코드

README에서 이야기한 부분의 코드만 옮겨 왔다. 게임의 다른 코드에 기대고 있어서 이 폴더만으로는 컴파일되지 않고, 전체 소스도 아니다.

**[ScrollKeep.cs](ScrollKeep.cs)**
작업대처럼 다시 그려지는 창에서 목록 위치를 지킨다. 원래 파일 그대로다. 창을 다시 여는 쪽(HubPanels.cs)에서는 이렇게 부른다.

```csharp
static void Reopen(string id) { var ys = ScrollKeep.Capture(modal); UIRoot.I.CloseModal(); Open(modal, id); ScrollKeep.Restore(modal, ys); }
```

**[HitFlash.cs](HitFlash.cs)**
피해를 입은 크리처의 몸을 잠깐 번쩍이게 한다. 원래 파일 그대로다.

**[PhotoStage_excerpt.cs](PhotoStage_excerpt.cs)**
사진 카드를 마우스로 끌어 돌리고, 뒷면을 얼마나 봤는지 재는 부분이다. 사진 앞면 그림과 뒷면 메모를 만드는 부분은 시연을 찍은 뒤에 바뀌어서 뺐다.

**[Locker17_excerpt.cs](Locker17_excerpt.cs)**
첫 사진이 나오는 확률, 그리고 선광장 탈의실에 15번부터 19번 사물함을 세우고 17번에 일지와 테이프를 넣는 부분이다. Story.cs와 StoryProps.cs에서 필요한 곳만 옮겼다.

영어 주석은 원래 코드에 있던 것이고, 맨 위의 한국어 설명과 생략 표시(…)는 옮기면서 붙였다.
