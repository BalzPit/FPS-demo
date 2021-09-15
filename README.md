The purpose of this project is to develop a small first-person shooter demo as a result of my first experiences and experiments with the Unity game engine.
The result is not supposed to be a fully fledged, balanced or well designed game, because game design is not a field I have experience of, but to merely be a demo of gameplay features that is hopefully somewhat playable as well (I'm trying to make things that make sense, also taking this as a small game design excercise).
Since it's my first time with Unity and making a game in general, I had to follow some tutorials at the start to understand how things work so obviously not everything here was 100% my creation (e.g.: aiming and walking, weaponshooting script, weapon pickup system), but pretty much every script was changed with added features (double jump, crouch, wallcheck on weapon pickup and weapon throwing, etc).

In this game, the player is able to perform the following actions:
- walk and look around
- run
- jump and double-jump
- crouch
- throw granades (granade energy refills like in Destiny)
- pick up weapons
- shoot/reload weapons
- drop/throw weapons (this does damage when weapon collides with enemies)
- loose health when damaged

Advanced movement features:
- wepon recoil actually moves the player character
  - shooting the ground right after a jump lets the player jump higher (only certain weapons have a strong enough recoil to make this effect noticeable)


BACKLOG:

----- general -----
- menus
- a couple of proper levels
  - enemy (and player) spawnpoints


----- gameplay features -----
- melee
- grapple hook
- other enemy types
- other weapon types


----- UI -----
- granade hitmarker
- show/hide enemy healthbar dynamically
- granade UI
- damage direction on hit


----- art -----
- viewmodel animations


----- audio -----
- steps
- granade explosion
- shooting sounds
- hitmarker sound feedback


----- fixes -----
- force applied to enemies when hit
- residual movement when landing
- crouch
- in-air movement
- weapon recoil effects
- bullet holes not rotating correctly


----- gameplay tweaks ------


----- refactors/reworks -----
- unified hitmarker system (currently every weapon/ability takes hold of the hitmarker UI element on the playerUI canvas independently, I would like to simply have a general system that shows hitmarkers when player deals damage to anything that takes damage and with any source)
- tidy up playermovement script
- weapon pickup system is kinda terrible
- ✓ UI Manager



Problem: 
- why would the player choose to ever throw an equipped weapon (it's easier to hit enemies at close range) if they could just melee?
