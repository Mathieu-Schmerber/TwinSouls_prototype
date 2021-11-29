## Overview

Each [Twin](<../Twin.md>) hold a weapon, this is also applicable to other entities.

Weapons define attacks according to different play styles. A weapon playstyle will be designed by the following traits:

* Attack speed

  > How quick a weapon applies damages after the animation started.
* Attack damage
* Combos

  > Some weapons may have multiple chain attacks defining patterns
* Range
* Spell types

  > Some weapon attacks can cast spells

Weapons are imbued with their holder's emitted element, as such any melee attack can apply an element's effect. Melee attacks always apply physical damages, unless a target is emitting the same element as the weapon holder (Cancellation).

Weapons are also a mean to cast spells.

## Weapon types

A weapon playstyle is essentially defined by its type, however this may differ with some very special weapons.

Each caracteristic value of  a weapon are randomly generated along a defined range

Here is the list of weapon types:

* **Sword** → quick, short ranged, weak attacks, single target spells.
  * **Attack speed**: **{min} **to **{max}**
  * **Attack damage**: **{min}** to **{max}**
  * **Combos**: **{min}** to **{max}** attacks
  * **Spell type**: projectile
* **Axe** → slow, short ranged, powerfull attacks, AOE spells.
  * **Attack speed**: **{min} **to **{max}**
  * **Attack damage**: **{min}** to **{max}**
  * **Combos**: **{min}** to **{max}** attacks
  * **Spell type**: projectile
* **Bow** → slow, long ranges, powerfull attacks, single target spells.
  * **Attack speed**: **{min} **to **{max}**
  * **Attack damage**: **{min}** to **{max}**
  * **Combos**: **{min}** to **{max}** attacks
  * **Spell type**: projectile
* **Staff** → quick, long ranged, weak attacks, AOE spells.
  * **Attack speed**: **{min} **to **{max}**
  * **Attack damage**: **{min}** to **{max}**
  * **Combos**: **{min}** to **{max}** attacks
  * **Spell type**: projectile

⚠️ Values are to be set/changed after iterating playtest sessions