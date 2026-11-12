# Playtest checklist

These are manual acceptance checks for the current prototype, not an automated test report. Leave items unchecked until a tester verifies them.

Record the commit, Unity version, class, level, and any reproduction steps when reporting a problem.

## Menus and progression

- [ ] Title screen opens from `TitleScreen.unity`; Play asks for a class.
- [ ] Controls match the game, and Esc pauses/resumes without advancing combat or reload timers.
- [ ] Victory offers the correct next level, replay, and Main Menu actions.
- [ ] Three deaths produce Game Over; Try Again and R reset the level to three lives.
- [ ] Game Over's Main Menu button works.
- [ ] Levels unlock in order; The Vault stays locked until all three main levels have perfect clears.
- [ ] A perfect clear requires every coin and reaching the finish flag in that attempt.
- [ ] Perfect clears persist after restarting the game and are not lost on a lower-scoring replay.

## Combat and upgrades

- [ ] Ranger pellets deal 20 damage, hit enemies, and stop at solid platforms.
- [ ] Base Ranger fires ten rounds, blocks shots during the five-second reload, then refills.
- [ ] Upgraded Ranger has faster shots, longer range, unlimited ammo, and no reload.
- [ ] Leveling Ranger during a reload cancels that reload immediately.
- [ ] Fighter punches deal 30 damage; upgrading increases speed and doubles knockback.
- [ ] Swordsman deals 30 damage with more reach than Fighter.
- [ ] Upgraded Swordsman attacks faster and sends shockwaves; a close target does not take slash and wave damage from the same swing.
- [ ] Each class upgrades once at half the current level's coins, rounding up.
- [ ] LEVEL UP! appears above the player and follows their movement.
- [ ] Upgrades survive death but reset on restart or a new level; the selected class carries forward.

## Platforming and survival

- [ ] All checkpoints activate and provide clear respawn positions.
- [ ] Earlier checkpoints do not replace later ones.
- [ ] Falls and lethal damage each cost exactly one life.
- [ ] Falling still kills while invincible; damage protection prevents overlapping attacks from draining health instantly.
- [ ] Coins remain collected after death and return on a full restart.
- [ ] Buffered jumps and ledge grace time work without allowing repeated midair jumps.
- [ ] Enemies remain a fair challenge near platform edges and checkpoint approaches.

## Complete playthroughs

For each combination, verify the finish is reachable, all coins are obtainable, and melee classes can handle shooters fairly.

| Course | Ranger | Fighter | Swordsman |
| --- | --- | --- | --- |
| First Steps | Pending | Pending | Pending |
| High Road | Pending | Pending | Pending |
| Sky Siege | Pending | Pending | Pending |
| The Vault | Pending | Pending | Pending |

## Standalone build

- [ ] Build starts at the title screen and includes all four playable scenes.
- [ ] Menus, class selection, pause, restart, and saved unlocks work outside the Editor.
- [ ] Text remains readable at the supported window sizes.
- [ ] Quit closes the standalone application.
- [ ] No missing assets, missing scripts, or unexpected Console/player-log errors appear.
