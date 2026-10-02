# RacingGame — Godot 4 mobile racing prototype

First playable loop: car + on-screen mobile controls + follow camera +
an oval track with checkpoints + lap counter + finish screen.

## How to run
1. Open Godot 4.x, choose "Import", select this folder's `project.godot`.
2. Press Play (F5). `scenes/Main.tscn` is the main scene.
3. On desktop, use W/S (or ↑/↓) to accelerate/brake and A/D (or ←/→) to
   steer — the on-screen buttons are set to `visibility_mode = Always`
   so you'll see and can click them too, which is a handy way to check
   they're wired up correctly before exporting to a phone.

## How the mobile controls work
`scenes/TouchControls.tscn` has four `TouchScreenButton` nodes, each with
its `action` property set to `"accelerate"`, `"brake"`, `"steer_left"`, or
`"steer_right"`. Those are the exact same input actions `car.gd` reads via
`Input.get_axis(...)`, and they're registered for keyboard too in
`scripts/input_map_setup.gd`. So the car script itself never needed to
change for touch — the buttons just drive the same actions a keyboard
would.

## Track / laps
- `Main.tscn` has 6 `Checkpoint` areas (`Checkpoint0`…`Checkpoint5`)
  arranged in a loop. `Checkpoint0` is the start/finish line.
- `scripts/lap_manager.gd` (an autoload) only accepts checkpoints in
  order, so you can't shortcut the course. Crossing checkpoint 0 again
  after hitting 1–5 counts as a completed lap.
- After 3 laps (`laps_to_win` on `LapManager`) the race ends,
  `FinishScreen.tscn` pops up with your time, and its Restart button
  reloads the scene.

## Offline play
- **Android APK**: offline by default once installed — the game makes no
  network calls.
- **Web (GitHub Pages) build**: exported as a Progressive Web App
  (`progressive_web_app/enabled=true` in `export_presets.cfg`). Visit the
  page once while online, then it's cached by a service worker and will
  keep working with no connection after that. On a phone, use the
  browser's "Add to Home Screen" option to launch it like an installed app.

## Exporting to Android
Use Godot's standard Android export (Project > Export > Add Android),
with a configured Android SDK/debug keystore. Nothing in this project
needs anything beyond Godot's default mobile renderer, which is already
set in `project.godot`.

## Where to go next
- Swap the placeholder box car / flat ground for real meshes and a
  textured road in `assets/`.
- Add wall colliders along the track edges so you can't drive off it.
- Tune `acceleration`, `max_speed`, `steering_speed` on the `Car` node
  in the inspector to change handling feel.
