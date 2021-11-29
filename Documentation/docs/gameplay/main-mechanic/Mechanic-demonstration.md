## 💡 General knowledge

‼️ **Be aware** 

> There's a difference to be made between being **affected by an effect **and **emitting an element.**

**📚️ Link logical rule**

> The reason both [Twin](<../Twin.md>)s can enhance each other through the link, can be explained thanks to the elements principle.
>
> You have to see the link as a mean for a [Twin](<../Twin.md>) to constantly try affecting the other. 
>
> This way if a [Twin](<../Twin.md>) getting affected by a complementary element, result in a boost. 
>
> On the other hand if he gets affected by a stronger element, the [Twin](<../Twin.md>) will suffer from the strong element effects.
>
> Essentially, a [Twin](<../Twin.md>) emitting 🔥 receiving ⚡️ through the link, thus enhancing him is the very same as a [Twin](<../Twin.md>) emitting 🔥 getting hit by a ⚡️ attack.
>
> > ❕ The element principle can be applied to any type of entity (enemies included)

## Element expression rules

The **📚️ Link logical rule** in mind, we will consider the following expressions where an attack element can either be a element from an enemy attack, or an element going through the link, reaching the [Twin](<../Twin.md>).

### Expression format

⚫️, 🔵 and 🔴 are elements.

an expression like:

* ⚫️&🔵 → \[ 🔴 \] = ( ⚫️ + 🔵 + 🔴)

Is explained this way:

* > attack element(s) → \[emmited element\] = (element equation)  = simplification

### Rules

1. An equation should not contain a same element multiple times, remove the duplicates
2. The emitted element should always be prior

**Processing an element equation in steps: **

1. Removing the duplicates
2. Removing the elements weaker than the emitted element
3. Removing the emitted element if a stronger element is present in the equation

   → we'll be calling **{t}** the time of the effect related to the stronger element
4. Conclude with the result:
   1. If the result is empty, the entity stops emitting for **{t}**.
   2. If the result does not contain the emitted element, apply the result's effect to the entity. The entity stops emitting for **{t}**.
   3. If the result contains only the emitted element, no change to the entity.
   4. If the result contains the emitted element and other elements, continue emitting and add the other elements as boost effects.

### Examples

**→ Fusion logical expression (Duplicates rule)**

**Example context: **

An enemy emitting ❄️, getting hit by a ☃️(❄️ & 💧) spell.

**Expression and solution**

```
❄️ & 💧 → [❄️] = (❄️ + 💧 + ❄️) = (❄️ + 💧)
# Step 1: removing ❄️ duplicates
# Step 2: nothing to remove
# Step 3: nothing to remove
# Step 4: The emitted element is present in the result with another one
			-> Emitting ❄️
            -> Boosting with 💧 for 💧 effect duration
```

**→ Fusion logical expression (Emitted prior rule)**

**Example context: **

A [Twin](<../Twin.md>) emitting ⚡️ receiving 🔥 from its other [Twin](<../Twin.md>) and getting hit by a 💧 spell.

**Expression and solution**

```
🔥 & 💧 → [⚡️] = (🔥 + 💧 + ⚡️) = (🔥 + ⚡️) 
# Step 1: nothing to remove
# Step 2: ⚡️ > 💧, removing 💧
# Step 3: nothing to remove
# Step 4: The emitted element is present in the result with another one
			-> Emitting ⚡️
            -> Boosting with 🔥 for 🔥 effect duration
```

**→ Repression logical expression (Emitted prior rule)**

**Example context: **

A [Twin](<../Twin.md>) emitting 🔥 receiving ⚡️ from its other [Twin](<../Twin.md>) and getting hit by a 💧 spell.

**Expression and solution**

```
⚡️ & 💧 → [🔥] = (⚡️ + 💧 + 🔥) = (⚡️ + 💧) 
# Step 1: nothing to remove
# Step 2: nothing to remove
# Step 3: 🔥 < 💧, {t} = 💧 effect duration
# Step 4: The emitted element is not in the result
			-> Emitting nothing for {t} duration
            -> Affect entity no effect, since ⚡️ > 💧
# Since the other Twin is constantly applying ⚡️ and this Twin no longer emitting,
# here is the nexts expressions while {t} duration:
⚡️ → [🔥] = (⚡️)
```

> ⁉️ Notice how the two lasts examples are processing the very same elements but result in two very different outcomes thanks to the priority of the emitted element.