# Crystal World
Platformer2D for Android with pixel art and some feature set.
# Game
https://github.com/xbjane/platformer2D/assets/122320488/47fd5b51-e358-41cc-94f7-8a6754dca9dd
# Hero
![photo_2023-06-12_17-59-11](https://github.com/xbjane/platformer2D/assets/122320488/0fc9b066-7190-43fe-9ac2-02b15749bf70)
The hero's task is to reach the end of the platform without falling off it, collecting crystals and killing enemies along the way. It has the following animations: idle, run, jump, attack, hit, death.
# Enemies
<img width="89" alt="enemy1" src="https://github.com/xbjane/platformer2D/assets/122320488/c5f61e5b-6fa4-4d90-be94-1a10c7d4187a">
<img width="101" alt="enemy2" src="https://github.com/xbjane/platformer2D/assets/122320488/19d9f1fc-38ca-4f0b-a944-c32eef095ec3">
The game provides two types of enemies that can deal damage with an attack and if the hero touches them without attacking. They have the following animations: idle, walk, attack, hit, death. Crystals are given for killing an enemy.

# Reward
![photo_2023-06-12_17-33-04](https://github.com/xbjane/platformer2D/assets/122320488/790592ab-ce99-409a-9fd9-3c2ae53f3d7c)
The game has a system of rewards in the form of crystals.

# Death
![photo_2023-06-12_17-41-22](https://github.com/xbjane/platformer2D/assets/122320488/89ec4698-3f6c-4455-b774-e3edc1c73fd9)
The death of the hero occurs if he fell off the platform, or if all 3 lives have ended.
# Menu

![photo_2023-06-12_17-09-29 (2)](https://github.com/xbjane/platformer2D/assets/122320488/00cdfa78-29d0-4079-a4a9-ed50992179f1)
From the menu, you can go to Settings, view the score, start the game or exit it.
# User Settings
![photo_2023-06-12_17-09-29](https://github.com/xbjane/platformer2D/assets/122320488/47e02fbf-793c-4d16-9f92-9bd81278648a)
Here you can turn on / off effects such as music, SFX, jump vibration. The set values are saved in PlayerPrefs.
# Score Count
![photo_2023-06-12_17-10-00](https://github.com/xbjane/platformer2D/assets/122320488/937b7d1a-9c58-492a-b342-4c542b1f9e44)
The number of crystals earned is written to a file through serialization, after which it is deserialized and displayed on the screen. In this way, you can see the 5 most successful games, excluding games with the same scores. The counter can be cleared to start a new record.
