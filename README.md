# Solograph Racing — Unity 6 Android Project

This repository is prepared for cloud Android builds with GitHub Actions and GameCI. It uses Unity 6.0.0.58f2.

## What is included
- Starter car controller and follow camera
- Mobile touch controls
- Ordered checkpoints and 3-lap race
- HUD, timer, finish and restart flow
- Editor script that generates a playable demo scene automatically
- GitHub Actions workflow that builds an Android APK

## GitHub setup
1. Create a GitHub repository and upload the contents of this folder.
2. In repository Settings → Secrets and variables → Actions, add `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD`. For Unity Personal, `UNITY_LICENSE` is the contents of your activated `.ulf` license file.
3. Run Actions → Build Android APK → Run workflow, or push to `main`.
4. Open the completed workflow run and download the `Solograph-Racing-APK` artifact.

## If you do not yet have a Unity license file
Run the `Request Unity activation file` workflow. Download its artifact and follow Unity/GameCI's current Personal-license activation process.

## Important
The generated demo scene is intentionally simple. It is a playable foundation, not the final art/game. Replace the primitive track and car with your actual models/assets as the project grows.
