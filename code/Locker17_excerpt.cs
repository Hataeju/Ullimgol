// 울림골 코드 일부. 첫 사진은 우연히 나오고, 다음 단서는 메모에 적힌 자리에 둔다.
// 게임의 다른 코드(Container, RoomInfo, Stack 등)에 기대고 있어서 이 파일만으로는 컴파일되지 않는다.

// ── Assets/Scripts/Core/Story.cs ─────────────────────────────
        // ── where the next story object must appear (13.7-5: a guaranteed, marked place)
        public static bool WantPhoto(int villageRuns, System.Random r)
        {
            if (S == null || Has(PhotoFound)) return false;
            if (villageRuns < 2) return false;                       // from the 3rd village sortie
            double p = villageRuns == 2 ? 0.4 : villageRuns == 3 ? 0.6 : villageRuns == 4 ? 0.8 : 1.0;   // certain by the 6th
            return r.NextDouble() < p;
        }
        public static bool WantLocker17 => Has(PhotoBack) && !Owns("k_journal") && !Stopped;
        // …
        /// small marker shown on the board's zone card
        public static string BoardHint(int zone)
        {
            if (S == null) return "";
            if (zone == 1 && WantLocker17) return "탈의실 17번 사물함 — 사진 뒷면";
            // … (다음 단계 표시 생략)
        }

// ── Assets/Scripts/World/StoryProps.cs ───────────────────────
            // 1. 아랫마을: 세 번째 출격부터 서랍·궤짝 하나에 사진을 넣는다
            if (zone.index == 0 && Story.WantPhoto(s.zoneRuns0, rng))
            {
                var cand = new List<Container>();
                foreach (var c in containers)
                {
                    if (c == null || (c.type != "drawer" && c.type != "chest") || c.story) continue;
                    var cell = CellAt(c.transform.position);
                    if (cell == null || cell.kind != CellKind.Room || d[cell.x, cell.y] < 2) continue;
                    cand.Add(c);
                }
                if (cand.Count > 0)
                {
                    var c = cand[rng.Next(cand.Count)];
                    c.story = true;
                    c.slots.Insert(rng.Next(c.slots.Count + 1), new Slot { item = new Stack("st_photo"), time = 2.6f });
                    Game.I?.Log("story_place photo " + c.type);
                }
            }

            // 2. locker 17 in the works changing room (journal + tape)
            if (zone.index == 1 && Story.WantLocker17)
            {
                RoomInfo r = null;
                foreach (var x in rooms) if (x.type == "lockers" && (r == null || Dist(x) > Dist(r))) r = x;
                if (r == null) foreach (var x in rooms) if (!x.cells[0].tunnel && x.type != "bellchamber" && (r == null || (x.hall ? 0 : 1) > (r.hall ? 0 : 1))) r = x;
                if (r != null) LockerRow(r);
            }
        // …

        void LockerRow(RoomInfo r)
        {
            var walls = RoomWalls(r);
            int n = 5; float w = 0.74f;
            Vector3 p = default; Quaternion q = default; bool ok = false;
            for (; n >= 2 && !ok; n--) ok = AgainstWall(walls, n * w + 0.1f, 0.62f, out p, out q, 40);
            n++;
            if (!ok) { Game.I?.Log("story_place locker17 failed"); return; }
            var right = q * Vector3.right;
            int i17 = n / 2;
            for (int i = 0; i < n; i++)
            {
                int num = 17 - i17 + i;
                var pos = p + right * ((i - (n - 1) / 2f) * w);
                var c = Container.Create(this, "locker", pos, q, rng, zone.danger, 0.1f); FitToWall(c);
                containers.Add(c);
                StoryArt.Plate(c.transform, num, num == 17);
                if (num == 17)
                {
                    c.story = true; c.label = "17번 사물함"; c.pryTime = 3f;
                    c.slots.Clear();
                    c.slots.Add(new Slot { item = new Stack("k_journal"), time = 1.6f });
                    c.slots.Add(new Slot { item = new Stack("st_tape"), time = 1.2f });
                    if (rng.NextDouble() < 0.6) c.slots.Add(new Slot { item = LootGen.Roll(rng, "locker", zone.danger, 0.3f), time = 0.8f });
                }
            }
            keyHint += (keyHint.Length > 0 ? ", " : "") + $"17번 사물함: {Dir8(entryPos, r.Center(S))}쪽 탈의실";
            Game.I?.Log("story_place locker17");
        }
