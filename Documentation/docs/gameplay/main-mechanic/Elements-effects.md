## Primary effects

Each element has its effect, that can be applied by the means of attacks or spells.

* 🔥 Fire → Burn effect

  > Applies **{x}** damages each **{y}** seconds for **{z}** seconds
* ❄️Ice → Slow effect

  > Slows by **{x}**% for **{y}** seconds
* ⚡️Lightning → Stun effect

  > Restricts the target from moving or using abilities for **{x}** seconds
* 💧 Water → Weak effect

  > Reduces target's resistance and power by **{x}**% for **{y}** seconds

⚠️ Values are to be set/changed after iterating playtest sessions

## Fusion effects

Combining two primary effects on a target results in the effects fusion.

* 💥 Explosion → 🔥 + ⚡️

  > Applies burn effect to the target. Everytime the burn effect applies damages, the target gets the stun effect for **{x}** seconds. 
* ☃️ Freeze → 💧 + ❄️

  > Restricts the target from moving or using abilities for **{x}** seconds. The target's resistance is reduced by **{x}**% in the meantime.

⚠️ Values are to be set/changed after iterating playtest sessions

## Repression effects

According to the [🎡 Elements equality wheel](<Elements-equality-wheel.md>), trying to apply an element's effect on a target already suffering from a non-complimentary or unsimilar element's effect results in the clearance of the target effects.

> 💡 **For example** 
>
> Trying to apply the 💧weak effect on a 🔥 burning target results in the cancelation of the 🔥 burning effect. The 💧 weak effect is not applied.