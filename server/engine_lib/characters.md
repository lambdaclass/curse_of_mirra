# Characters

Characters don't have any core mechanics, they are more a set of attributes and skills.

One decision is about the skills, the amount of skills a character can have will most likely be hardcoded

## Skill struct

Configurable fields:
- `name`: Unique name of the character
- `active`: Can the character be picked
- `base_speed`: Base speed of the character
- `size`: Size of the character for collision math
- `skill_1`: Skill 1
- `skill_2`: Skill 2
- `skill_3`: Skill 3
- `skill_4`: Skill 4
- `skill_5`: Skill 5


Non-configurable:
-

## Configuration

Some example configurations

```
[
  {
    "name": "H4ck"
    "active": true,
    "base_speed": 25,
    "size": 80,
    "skill_1": "Slingshot",
    "skill_2": "Multishot",
    "skill_3": "Disarm",
    "skill_4": "Neon Crash",
    "skill_5": "Denial of Service",
  }
]
```
