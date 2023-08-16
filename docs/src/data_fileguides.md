# Fileguides

## Characters.json
### Overview

This file contains the information and base stats from characters in the game

### Data Fields

- **Name:** defines the internal name for a character
- **Id:** unique numeric ID to identify the character
- **Active:** boolean. Defines if the class is currently playable
- **Class:** character class (war, hun, ass, wiz)
- **Faction:** their origin (oto, mer, ara, kal)
- **BaseSpeed:** defines a numeric value for the character's base movement speed
- **SkillBasic:** lookup field to Skills.json for the character's basic skill
- **SkillActive1:** lookup field to Skills.json for the character's first active skill
- **SkillActive2:** lookup field to Skills.json for the character's second active skill
- **SkillDash:** lookup field to Skills.json for the character's third active skill
- **SkillUltimate:** lookup field to Skills.json for the character's ultimate skill
- **BodySize:** f64 that defines the circumference of how much space the character takes up in the board. Useful for knowing when an attack impacts them, for example.

## GameSettings.json
### Overview

This file contains various settings used by the game

### Data Fields

- **Name:** defines the internal name for a settings group
- **board_width:** defines the width of the playing board
- **board_height:** defines the length of the playing board
- **server_tickrate_ms:** represents how often the server sends updates to the client
- **game_timeout_ms:** represents how much a game session lasts
- **map_shrink_wait_ms:** Represents the amount of time players have to wait until the zone starts to shrink
- **map_shrink_interval:** It's the interval time between shrinks
- **map_shrink_minimum_radius:** Mininum radius for the playable area, how small can the playable zone can get
- **out_of_area_damage:** Represents the damage the area does

## Skills.json
### Overview

This file contains information about all the skills in the game. NOTE: whether each field is used or not depends on the function of each skill. So adding damage to a buff does nothing unless the buff is referencing that field.

### Data Fields

- **Name:** defines the internal name for a skill
- **Cooldown:** determines how long one must wait between uses of a skill
- **Damage:** determines the damage of a skill as a percent of the character's base damage
- **Duration:** determines the duration of a skill/buff
- **SkillRange:** represents the attack's range of an skill
- **Par1-5:** arbitrary parameters depending on the skill func
- **Par1-5Desc:** comment fields, describing the above
- **Angle** represents an angle modifier for the skill
