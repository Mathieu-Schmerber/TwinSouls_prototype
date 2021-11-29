Both player joined the game, using Xbox controllers and the Parsec software in order to play Twin Souls online.

Players in this session have never even opened the game before.

Please note that the players in this session are not the same as the [Discovery session #1](<discovery-session1.md>).

### [Area 1 - Pressure plate](<../prototype/area1.md>)

As in the [Discovery session #1](<discovery-session1.md>), players used this area as their playground to start moving around and testing the voting system. 

[🔍️ Fusion & Repression](<../gameplay/main-mechanic/Fusion-Repression.md>) caused by the 🔥 burning effect **nearly** killed them once, they reacted in time and changed their votes.

As in the previous session, pressure plates were not a struggle in the slightest.

### [Area 2 - Pillars & pressure plates](<../prototype/area2.md>)

Players were confused and took quite some time finding out they could hit the boxes. When they did figure it out, it did not take long to activate the pressure plates.

They quickly realised they could shoot the pillars to change them, but for a few seconds did not understand why the already activated pillar (changed in [Discovery session #1](<discovery-session1.md>) tweaking part) was not changing when hit. When they understood the pillar was actually active by default, they understood that every pillars needed to be lighten up.

Like the previous duo, it took some time to have the idea of using the same element to activated the pillars. For the 💧 ⚡️ pillar, they did also come up with a rather complex set up to solve it, but did not try it because they felt like it was way to complex. They solved the puzzle as intended.

### [Area 3 - Movement ability](<../prototype/area3.md>)

Dashing above the void was instant for them (as the previous duo). However they got stuck a fair minute in front of the wall obstructing the way. Players tried changing elements to detroy it, before trying the ⚡️ moving ability to flash it. One of the players glitched over the wall by timing the 🔥 dash.

After understanding how diffrent movement abilities could be, they quickly understood they had to cast their ❄️ movement ability to get over with this area.

### [Area 4 - Training ground](<../prototype/area4.md>)

Players tried swinging the different weapons for a VERY LONG TIME. Seems like they had way to much fun shooting around while dashing in every corner of the room.

As good as it sounds, I actually had to interfer since they had a hard time understanding how the element mechanic worked when applied to fighting.

### [Area 5 - Monster waves](<../prototype/area5.md>)

Players had a hard time communicating with each other and died 3 times pointlessly, they were not focusing at all since they were laughing the whole tries looking at how unorganized they were.

After getting the hang of dodging and moving around, it was just a matter of time (and some more deaths) to clear all the waves.

Players complained about their own movement speed being to slow to dodge the projectiles without dashing.

Players also complained about the melee attacks, unable to reach the enemies since enemies were faster than the players.

### Conclusion

The previous tweaking from [Discovery session #1](<discovery-session1.md>) are very efficient but not enough for the [Area 2 - Pillars & pressure plates](<../prototype/area2.md>) in terms of feedbacking. The combat need some more balancing as well.

Overall, I am very happy with the movement and fighting mechanics, in both sessions, players had a blast using them.

### Tweaking

* [Area 2 - Pillars & pressure plates](<../prototype/area2.md>):
  * The activated pillar was a great tweak, however I've added a green outline to the pillars when activated to make players understand better what is happening.
  * Increasing the boxes outline width to give a better feel that players can interact with them.
* [Area 3 - Movement ability](<../prototype/area3.md>):
  * Increasing the flashable wall collider in height, so players cannot dash over it
* [Area 4 - Training ground](<../prototype/area4.md>):
  * Adding a bounce feedback on dummies when hit, this way players will better understand if their attack did damage or not.
* [Twin](<../gameplay/Twin.md>):
  * 👟 speed: 5 units → 6 units (move quicker than the enemies)
* [⚔️ Weapons](<../gameplay/combat-mechanic/weapons.md>):
  * Sword (trying to make melee combat more fair):
    * Hitbox increased
    * Attack speed: 1.8 → 2
    * Projectile speed: 11 units → 12 units
  * Axe:
    * Hitbox increased
  * Staff:
    * Attack speed: 1.8 → 2
    * Melee damage: 15 → 10 (don't want it to be stronger than the sword in melee)
    * Hitbox increased