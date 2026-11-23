# 공개 범위

이 저장소는 울림골 작업 폴더를 그대로 올린 것이 아닙니다. 별도 폴더에 검토한 파일만 모아 만든 **포트폴리오 공개본**입니다.
내려받아 실행하거나 빌드할 수 있는 프로젝트가 아닙니다.

## 들어 있는 것

| 파일 | 내용 | 출처 |
|---|---|---|
| `media/ullimgol-v7-demo-photo-clue.mp4` | 사진 단서 본편, 55초, 1280×720 | v7 빌드 게임 시연. 별도 테스트 저장(`locker17` 시나리오) · 무적 옵션으로 녹화하고 편집 |
| `media/ullimgol-v7-demo-bellhead-body-shots.mp4` | 종머리 몸통 연속 사격, 12초, 1280×720 | v7 빌드 게임 시연. `bellhead_hit` 테스트 시나리오(무적, 종머리 체력 3000) |
| `media/photo-turn.gif` · `memo-closeup.jpg` · `locker17-row.jpg` · `locker17-journal-tape.jpg` · `extract-report.jpg` · `bellhead-shot1-replay.jpg` · `bellhead-shot2-replay.jpg` | 위 두 영상에서 뽑은 장면 | 게임 시연 |
| `media/workbench-scroll-before-after.jpg` | 신호탄총 제작 직전 · 직후 두 장면 | 제 직접 플레이 녹화(v7)에서 뽑은 프레임 두 장 |
| `code/` | 선별 코드 4개 | 작업본 소스에서 옮김. 범위는 [code/README.md](code/README.md) |

## 넣지 않은 것

- Unity 프로젝트 전체(`Assets` · `ProjectSettings` · `Packages`), 빌드 · 압축본, 로컬 설정 · 캐시
- 게임 기록(playlog) 원본, 녹화 원본 프레임 · 소리, 테스트 저장 파일. README에는 기록 몇 줄의 핵심 값만 옮겼습니다
- Higgsfield로 만든 이미지 · 3D 모델 · 음성 원본과 생성 작업 기록, 효과음 · 글꼴 파일
- 기획 문서, 이야기 초안, 내부 점검 보고서
- 이전 시연 편집본
- 개인 정보, 계정 · 인증 정보, 로컬 경로

## 영상 속 자료와 권리

- 영상 속 아트(크리처 · 무기 · 소품 · 재질 · 아이콘 · UI 그림)는 Higgsfield로 생성했습니다. 생성물 파일은 이 저장소에 없고, 배포 조건은 Higgsfield 약관을 따릅니다
- 효과음은 OpenGameArt의 CC0 묶음(Kenney, artisticdude, HaelDB, TinyWorlds, Zane Little Music, AntumDeluge, Ove Melaa, kurt)과 게임 안에서 합성한 소리입니다
- 게임 UI와 영상 자막 글꼴: IBM Plex Sans KR · Gowun Batang · Nanum Pen Script · Galmuri(SIL Open Font License 1.1)
- 이 저장소에는 라이선스 파일을 붙이지 않았습니다. 따로 허락하지 않는 한 코드와 영상을 다시 쓸 수 없습니다

## 공개 정리에서 한 확인

- 올린 파일에 API 키 · 토큰 · 이메일 · 로컬 경로 문자열이 없는지 검사했습니다
- 영상 두 개는 처음부터 끝까지 디코딩해 오류가 없음을 확인했습니다
- 게임을 새로 빌드하지 않았고, 게임 코드도 바꾸지 않았습니다. 시연은 v7 빌드를 그대로 썼습니다
