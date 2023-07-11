# Fileguides

## Overview

All duration related fields are measured in miliseconds by default.  
Units to measure distance and velocity are still undefined.

## Attributes.json
### Overview

This file contains the functionalities for each possible stat on units

### Data Fields
- **Name:** defines an internal name for an attribute
- **Id:** unique numeric ID to identify the stat internally

## Characters.json
### Overview

This file contains the information and base stats from characters in the game

### Data Fields

- **Name:** defines the internal name for a character
- **Id:** unique numeric ID to identify the character
- **Active:** boolean. Defines if the class is currently playable
- **Class:** lookup field to Classes.json
- **Faction:** lookup field to Factions.json
- **BaseSpeed:** defines a numeric value for the character's base movement speed
- **SkillBasic:** lookup field to Skills.json for the character's basic skill
- **SkillActive1:** lookup field to Skills.json for the character's first active skill
- **SkillActive2:** lookup field to Skills.json for the character's second active skill  
- **SkillDash:** lookup field to Skills.json for the character's third active skill
- **SkillUltimate:** lookup field to Skills.json for the character's ultimate skill
- **BodySize** f64 that defines the circumference of how much space the character takes up in the board. Useful for knowing when an attack impacts them, for example.

## Classes.json
### Overview

This file contains information about the four available classes in the game

### Data Fields

- **Name:** defines the internal name for a class
- **Code:** unique identifier used as a reference from other files

## Crates.json
### Overview

This file contains information about the various crates that can randomly spawn in the game

### Data Fields

- **Name:** defines the internal name for a crate
- **Id:** unique numeric ID to identify a crate

## Factions.json
### Overview

This file contains information about the four available factions in the game

### Data Fields

- **Name:** defines the internal name for a faction
- **Code:** unique identifier used as a reference from other files

## GameSettings.json
### Overview

This file contains various settings used by the game

### Data Fields

- **Name:** defines the internal name for a settings group
- **board_width:** defines the width of the playing board
- **board_height:** defines the length of the playing board
- **server_tickrate_ms:** represents how often the server sends updates to the client
- **game_timeout_ms:** represents how much a game session lasts

## Projectiles.json
### Overview

This file contains information about all projectiles in the game

### Data Fields

- **Name:** defines the internal name for a projectile
- **Id:** unique numeric ID
- **DoFunc:** uses an ID value to select a specific behavior called every tick while the projectile exists
- **HitFunc:** uses an ID value to select a specific behavior when the missile's collision is triggered
- **Duration:** how long the projectile will exist as long as it isn't destroyed by a collision
- **Speed:** determines how fast a missile travels
- **CollideType:** determines what kind of objects will trigger a missile's collision
- **CollideKill:** boolean field, if set to 1 the missile will be destroyed once it collides with a matching CollideType

## Skills.json
### Overview

This file contains information about all the skills in the game

### Data Fields

- **Name:** defines the internal name for a skill
- **DoFunc:** uses an ID value to select a specific behavior when a skill is used
- **ButtonType:** uses an ID value to select a specific behavior for the controls of the skill
- **Cooldown:** determines how long one must wait before using a skill at the start of the game and every time it's used
- **Damage:** determines the damage of a skill as a percent of the character's base damage
- **Duration:** determines the duration of a skill/buff, if called by the DoFunc
- **Projectile:** pointer to Projectiles.json, if called by the DoFunc
- **Minion:** pointer to Units.json to summon units, if called by the DoFunc

## Status.json
### Overview

This file contains information about all states a unit can have

### Data Fields

- **Name:** defines the internal name for a state

## Units.json
### Overview

This file contains information about all the non-character units in the game

### Data Fields

- **Name:** defines the internal name for a unit
- **Id:** unique numeric ID
- **Life:** determines the base health of the unit
- **Damage:** determines the base damage of the unit
