# GAME360-Final-BulletWitch

[Game Banner](Screenshots/banner.png)

## Game Overview
- Genre: SHMUP
- Platform: Windows
- Development Time: 11 days
- Unity Version: 6000.2.6f2
- Course: GAME 360 - Development with Game Engines, Fall 2025

## How to Play

**Installation**

Download the latest release, and run "BulletWitch.exe".

**Controls**

- Movement: WASD or Arrow Keys / Left Joystick
- Shoot: Left Click or Enter / Button West or Trigger
- Powerup/Bomb: E Key or Right Click / Button North
- Slow Time: Left Shift or Q or Middle Mouse Button / Left Shoulder
- Debug (skip level): Slash / Select

## Features
- Sound effects
- Music
- Multiple weapons/bullet types
- Multiple enemies with different AI
- Boss fight
- Multiple levels (Defeat a given amount of enemies to progress)
- Volume slider
- Score system (Collect coins, defeat enemies for score)
- Collectibles
- Win condition (Defeat final boss)
- Lose condition (Run out of time/lives)

Implemented Mechanics
- Bullet: Fire bullets at enemies, change weapons as you progress.
- Bomb: Kill all enemies (excluding bosses) and destroy all bullets on screen.
- Slow Time: Reduce the speed of time to allow more precise dodging.

## Architecture
- AudioManager: Singleton responsible for playing sound effects and changing music.
- AchievementManager: Observer responsible for storing and unlocking achievements.
- CollectibleSpawner: Singleton responsible for spawning collectibles at random positions.
- EnemySpawner: Singleton responsible for spawning enemies and bosses.
- EventManager: Responsible for observer pattern.
- GameManager: Singleton responsible for storing game stats and useful methods like AddScore(). Uses state machine to switch between states such as PlayingState and MainMenuState.
- UIManager: Responsible for updating and displaying UI elements.
- LevelManager: Uses state machines to switch between levels and their unique behaviors, including enemy spawning and collectible spawning behaviors.
- PlayerController: Responsible for player controls and behaviors, uses state machine to switch between states such as IdleState and DamagedState.


## Screenshots

[Screenshot 1](Screenshots/gameplay1.png)

*Main gameplay showing bullet feature*

[Screenshot 2](Screenshots/gameplay2.png)

*Main gameplay showing bomb feature*

[Screenshot 3](Screenshots/menu.png)

*Main menu interface*

[Screenshot 4](Screenshots/victory.png)

*Victory screen*

[Screenshot 5](Screenshots/gameover.png)

*Game over screen*

## Credits

- Audio (Music & SFX)
Lysander Reese

- Art Assets
Lysander Reese

## Post-Mortem

**What I learned**

I learned more about C# and began understanding state machine patterns more. I learned how to use Unity to create a video game. I also learned terms like "Singleton".

**Future Improvements**

- Better use of classes
- Cleaner code
- Visual improvements
- Less use of switches

## Developer

**Lysander Reese**

Student ID: 01232389

Old Dominion University - GAME 360

Fall 2025

Email: lrees010@odu.edu