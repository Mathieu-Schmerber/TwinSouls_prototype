## Overview

Each [Twin](<../Twin.md>) hold a weapon, this is also applicable to other entities.

Weapons define attacks according to different play styles. A weapon playstyle will be designed by the following traits:

* Attack speed
  > How quick a weapon applies damages after the animation started.
* Attack damage
* Combos
  > Some weapons may have multiple chain attacks defining patterns

* Projectile damage
  > Some weapon attacks can cast spells

* Projectile speed

Weapons are imbued with their holder's emitted element, as such any melee attack can apply an element's effect. Melee attacks always apply physical damages, unless a target is emitting the same element as the weapon holder (Cancellation).

Weapons are also a mean to cast spells.

## Weapon types

A weapon playstyle is defined by its type.

Here is the list of weapon types:

**Sword** → quick, weak attacks

* **Attack speed**: **{2}**
* **Combos**:
    * **Attack 1**: **{5}** damage
    * **Attack 2**: **{7}** damage (2 hits), cast 2 spells
* **Projectile damage**: **{5}**
* **Projectile speed**: **{12}**
    
**Axe** → slow, powerfull attacks

- **Attack speed**: **{1}**
- **Combos**:
    * **Attack 1**: **{15}** damage, cast 1 spell
    * **Attack 2**: **{10}** damage (2 hit)
- **Projectile damage**: **{10}**
- **Projectile speed**: **{12}**
    
**Bow** → slow, powerfull attacks

* **Attack speed**: **{1.5}**
* **Combos**:
    * **Attack 1**: **{0}** damage, cast spell
* **Projectile damage**: **{30}**
* **Projectile speed**: **{20}**
* ! Projectile can pierce through **{2 entities}**
    
**Staff** → quick, weak attacks

* **Attack speed**: **{2}**
* **Combos**:
    * **Attack 1**: **{10}** damage, cast spell
* **Projectile damage**: **{15}**
* **Projectile speed**: **{10}**

⚠️ Values are to be set/changed after iterating playtest sessions. <br/> Please refer to the [Play testing section](<../../playtesting/index.md>)