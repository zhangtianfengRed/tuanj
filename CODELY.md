

## Codely Structured Memories

### User

### Feedback
- [2026-09-10 01:21:50] 缓存/运行时/机器本地文件一律不上传 git：.gitignore 已排除 .codely-cli/（1.9GB）、.codely.packages/、.codely/、.com-unity-codely.json（Codely 心跳运行时状态，2026-09-10 从提交中移除）；有意保留 .codelyignore、CODELY.md、screenshots 等小文件。**Why:** 用户明确要求"缓存文件一类的不要上传"，缓存类内容无价值且体积巨大。**How to apply:** 以后遇到新生成的工具工作目录、心跳/端口/会话状态文件，默认加 .gitignore 而不是提交。

### Project
- [2026-08-29 00:20:31] Project converted from top-down to first-person camera. RoomTopDownPlayerMovement.cs has firstPersonMode field. Camera1 is child of RoomHansSuite. RoomFirstPersonCameraInteractor handles mouse look. Player model on FPBody layer (excluded from main camera). Mirror uses PlanarMirror.cs with RealtimeMirrorSurface child under ShowerMirror_02.
- [2026-08-30 14:36:41] Blink detection system — WORKING with Dlib FaceLandmark Detector (Enox Software v2.0.2). Replaced MediaPipe (too slow on CPU, 5-8 FPS) with Dlib (30+ FPS on CPU). Uses dlib 68-point landmarks + EAR algorithm + EMA smoothing (alpha=0.4). Current params: threshold=0.13, consecFramesForClose=4, consecFramesForOpen=4, cooldown=0.5s. NOTE: User wears glasses — glasses reflections degrade dlib landmark accuracy, EAR drops from 0.26~0.40 (no glasses) to 0.10~0.25 (with glasses). Works much better without glasses. Model: sp_human_face_68.dat (full, not mobile). Scripts at Assets/Script/BlinkDetection/. To extend: subscribe to BlinkEventSystem.OnBlinkDetected or implement IBlinkEventHandler.








- [2026-08-29 00:20:38] Character model (HansSuite) faces -Z by default; children rotated to (270, 0, 0) to match transform.forward (+Z). Model children: HansSuite, HansSuite_Hairs, HansSuite_Mesh — all share localRotation (270.02, 0, 0).
- [2026-09-05 09:48:58] Unity bridge package cn.tuanjie.codely.bridge is deliberately PINNED in Packages/manifest.json to local mirror "file:../.codely.packages/cn.tuanjie.codely.bridge@1.0.80-exp.1". Why: bridge 1.0.81 (2026-09-04) moved the handshake file from project root .com-unity-codely.json to Temp/, but Cowork CLI 1.0.0-release.57 only reads the root file → endless "Invalid unity_port -1 ... unity_quit" errors and Unity tools unavailable. Fix applied 2026-09-05: pinned bridge to CLI-bundled 1.0.80-exp.1 (writes root heartbeat), installed via bridge manage_package/Client.Add with "file:D:/.../@1.0.80-exp.1" URL (@ in path breaks plain Client.Add). How to apply: do NOT upgrade this package to 1.0.81+ unless the Cowork app/CLI is first updated to a version reading Temp/.com-unity-codely.json; if connection errors reappear after a package update, check this version pairing first.
- [2026-09-10 01:21:48] Git 远程已改为专用新仓库：origin = https://github.com/zhangtianfengRed/tuanj.git（2026-09-10 起全量历史 57 提交分批推送过去），旧仓库 story.git 改名为 backup 远程保留。仓库本体是 blob:none 部分克隆（源自 story.git），个别历史 blob 本地缺失、需用时自动从 backup 惰性拉取。**Why:** 用户要给团结项目单独建一个仓库。**How to apply:** 以后 push/pull 默认走 origin=tuanj.git，story.git 仅作备份；用户网络到 GitHub 不稳定，大体积分批推送（单次<2GB）并配自动重试。
- [2026-09-10 23:01:42] 分批推送 tuanj.git 处于暂停状态（2026-09-10 23:05 用户要求暂停）：远程 main 停在 #43 提交 7d81c9b82（添加城市资源），共 57 提交已传 43，剩 14 个（#44 街道起）。恢复方法：后台独立进程运行 C:\Users\zhang\AppData\Local\Temp\push_history.ps1（自动 fetch 远程 main、只推缺失提交、断网重试；日志在 %TEMP%\push_history.log，完成标志 PUSH_ALL_DONE），传完需校验远程 HEAD==本地 main、停掉 keep_awake.ps1 进程并删除 %TEMP% 下 push_history.ps1/keep_awake.ps1/analyze_chunks.ps1/push_history.log。**How to apply:** 用户说"继续传"即重启该脚本（Start-Process 独立进程方式），完成后做收尾校验与清理。

### Reference

