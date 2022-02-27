You can download the game at: https://thelegendofpit.itch.io/fps-demo

The purpose of this project is to develop a small first-person shooter demo as a result of my first experiences and experiments with the Unity game engine.
The result is not supposed to be a fully fledged, balanced or well designed game, because game design is not a field I have experience of, but to merely be a demo of gameplay features that is hopefully somewhat playable as well.
Since it's my first time with Unity and making a game in general, I had to follow some tutorials at the start to understand how things work so obviously not everything here was 100% my creation (e.g.: aiming and walking, weaponshooting script, weapon pickup system), but pretty much every script was changed with added features (double jump, crouch, wallcheck on weapon pickup and weapon throwing, etc).

In this game, the player is able to perform the following actions:

- walk and look around

- jump 

- crouch

- throw granades

- pick up weapons

- shoot/reload weapons

- drop/throw weapons

- loose health/die when damaged

Advanced movement features:

- sprint

- double-jump

- slide (crouch while sprinting)

- wepon recoil moves the player character

  - shooting the ground after a jump can make the player jump higher (only certain weapons have a strong enough recoil to make this effect noticeable)

- (terrible) grappling hook

Advanced combat features:

- every weapon has infinite reserve ammo, but can't be used indefinitely. Shooting weapons and hitting enemies when throwing them will decrease their durability. The first signs of damage will appear when the durability decreases under 25%. After enough use, a weapon will break completely (until a weapon's destruction, its durability  won't change its performance/behaviour in any way).

- destroying enemies with weapon shots will trigger a damage buff for a few seconds. The effect can be stacked.

- enemies will be stunned for a few seconds (depending on the amount of damage received) when hit by a thrown weapon or grenade. Stunned enemies will not follow the player and won't explode if near the player.

BACKLOG:

----- general -----


----- gameplay -----

----- UI -----
- damage direction on hit

----- art -----


----- audio -----
- music
- AudioManager


----- fixes -----
- residual movement when landing
- crouch
- weapons damaging enemies when lying on the ground


----- refactors/reworks -----
- !!weapon pickup system is terrible!!
- tidy up playermovement script
- spawn/round system
- grappling hook
- granade/weapon ammo UI
- make gamemanager and other managers singleton so the references don't need to be retrieved
- gamemanager weapon lists coudle be reworked


DONE:
- unified hitmarker system (currently every weapon/ability takes hold of the hitmarker UI element on the playerUI canvas independently, I would like to simply have a general system that shows hitmarkers when player deals damage to anything that takes damage and with any source)
- UI Manager
- granade hitmarker
- grapple hook (cool when it works but a bit too buggy, doing it right would probably take a lot more time)
- enemy spawn points and system (very barebones)
- enemy and wepon scripts rework to be instantiated at runtime
- enemy types (same enemy with different health, size, damage)
- weapon durability
- projectile trail
- score system
- weapon spawning management
- stun enemies when hit by weapon
- weapon throw interruption
- main/pause menu
fixed:
- player model intercepting shots
- gravity bug
- force applied to enemies when hit
- weapon recoil effects
- throw collisions with player
- throw ui
- various menus bugs

Out of scope:
- other weapon types
- different enemy types
- basic viewmodel animations
- show/hide enemy healthbar dynamically
- audio mix