# COMP 312 - 8-Bit Runner

A Unity 2D platform action game developed for COMP 312: Open Source Computing at Loyola University Chicago.

## Developers

- Giancarlo Herrera
- Shu Perez

## Current status

Playable prototype with placeholder graphics. The campaign, combat, classes, upgrades, and menus are implemented. Final art, sound, full difficulty balancing, and standalone-build testing remain in progress.

## Run the game

1. Clone this repository or download and extract it.
2. In Unity Hub, select **Add project from disk** and choose the repository folder containing `Assets`, `Packages`, and `ProjectSettings`.
3. Open with **Unity 6000.6.0f1** and allow packages and scripts to import.
4. Open `Assets/Scenes/TitleScreen.unity` and press **Play**.
5. Select **Play**, choose a class, and begin Level 1.

The Unity Editor is currently required; a standalone release has not been published.

## Controls

| Action | Control |
| --- | --- |
| Move | A / D or left / right arrows |
| Jump | Space |
| Class attack | J |
| Pause / resume | Esc |
| Restart after Game Over | R or Try Again |

## Classes and upgrades

| Class | Base attack | Half-coin upgrade |
| --- | --- | --- |
| Ranger | Gun pellets deal 20 damage. A 10-round magazine reloads automatically in 5 seconds. | Faster shots, longer range, unlimited ammo, and no reload. An active reload is canceled immediately. |
| Fighter | Short-range punches deal 30 damage. | Faster punches and double knockback. |
| Swordsman | Slashes deal 30 damage with longer reach than punches. | Faster slashes and traveling shockwaves that deal 30 damage. An enemy already hit by a slash is not damaged again by that swing's shockwave. |

Collect half the coins in the current level to upgrade once. Odd totals round up. A cyan **LEVEL UP!** message appears above the player, and the HUD shows class and upgrade status.

Your class carries between levels. Upgrades and collected coins survive a death within the current attempt. Restarting or entering another level resets the upgrade and that level's collected coins. Ranger respawns with a full magazine.

## Campaign

All courses have fixed layouts. Later levels add more demanding platforming, enemies, and hazards.

| Level | Name | Scene | Coins | Checkpoints |
| --- | --- | --- | --- | --- |
| 1 | First Steps | `Assets/MainLevel.unity` | 28 | 3 |
| 2 | High Road | `Assets/Scenes/Level2.unity` | 33 | 2 |
| 3 | Sky Siege | `Assets/Scenes/Level3.unity` | 39 | 2 |
| Secret | The Vault | `Assets/Scenes/SecretLevel.unity` | 45 | 1 |

Complete main levels in order to unlock the next one. To unlock **The Vault**, collect every coin **and finish** each of the three main levels: 100 main-level coins in total. Collecting only part of a level's coins does not count as a perfect clear.

Perfect clears and level unlocks are saved locally with Unity PlayerPrefs. You can replay completed levels to earn missing perfect clears; a later replay with fewer coins does not erase an earned perfect clear.

## Survival and game flow

- Start each level with 100 health and three lives.
- Cyan checkpoint flags turn green when activated and become your respawn location.
- Running out of health or falling below the map costs one life.
- The first two deaths respawn you with full health. Losing the last life ends the attempt.
- Damage briefly flashes the player red and grants short invincibility. Falling still kills during invincibility.
- Orange enemies warn before firing. Solid platforms block projectiles.
- The title, class selection, controls, level select, pause, victory, and Game Over screens provide the full play/replay loop.

## Project structure

- `Assets/Scripts/PlayerMovement.cs`: movement, jump buffering, and ledge grace time.
- `Assets/Scripts/PlayerHealth.cs`: damage, lives, checkpoints, and respawning.
- `Assets/Scripts/PlayerAttack.cs`: class attacks, Ranger ammunition, and upgrades.
- `Assets/Scripts/EnemyPatrol.cs`, `EnemyAttack.cs`, and `EnemyHealth.cs`: enemy behavior and combat.
- `Assets/Scripts/PlayerProjectile.cs` and `EnemyProjectile.cs`: projectile movement and hits.
- `Assets/Scripts/GameFlow.cs`: menus, class selection, and level transitions.
- `Assets/Scripts/CampaignProgress.cs`: saved unlocks and perfect clears.
- `Assets/Scripts/PlayerProgress.cs`, `CoinPickup.cs`, and `Checkpoint.cs`: collectibles and progression.

Unity generates `Library`, `Temp`, `Logs`, and `UserSettings` locally; they are excluded from version control. Keep Unity `.meta` files with their associated assets.

## Validation and next steps

Development checks have covered script compilation, menu navigation, scene loading, unlock logic, checkpoint/life handling, class damage and upgrades, Ranger reloading, and calculated jump reach. These checks do not replace a complete playthrough with every class.

Use the [playtest checklist](docs/PLAYTEST.md) for the next testing pass. Priorities are class balance, enemy edge behavior, visual assets and animations, audio, and standalone-build verification.

## Built with

Unity, C#, TextMeshPro / uGUI, Git, and GitHub. The project includes Unity's Pipeline package for local Editor tooling.

## License

This project is licensed under the [MIT License](LICENSE).
