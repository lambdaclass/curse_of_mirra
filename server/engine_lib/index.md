# LAGE (Lambda's Awesome Game Engine)

- [Effects](./effects.md)
- [Loots](./loots.md)
- [Projectiles](./projectiles.md)
- [Skills](./skills.md)
- [Characters](./characters.md)

## The Engine

- Everything has to be configurable
    - Maybe read JSON (or something) with serde
- Define units, what they represent, and precision
    - Should we keep using "board" points or move to integer coordinate system
    - Time units
    - Damage units
    - Speed units
- Define external structs, this structs will act as one of the APIs
    - Although this is backend focus, they should provide enough information for clients
    - Similar will be the enums we display (e.g. player actions)
- Engine API
- Design of Skills/Loots/etc, to support the most broad of usages we need to be flexible and easy to compose, but at the same time having the ability to over really custom behavior for a skill might be useful
    - Have core attributes for skills that define certain generic behavior
    - (somehow) support completely custom behavior for a skill


## Additions for the library
- Metrics, specifically on how long function calls take
- Tests
- Load testing and benchmarks

## Per file stuff

### board.rs
- Do we need this? We technically no longer have a board, it could just be fields in a game struct

### character.rs
- `Name`, `Faction`, `Class` enums should be removed, instead replace by a configurable field in the character

### game.rs
- Too much things jajaja

### loot.rs

- Loot type should instead be behavior parameters (e.g heals, boost damage, etc)

### player.rs

- Extract `Effect` into their own module, this effects should be composed of `EffectAttribute` which would be the things defining their behavior so `Effect` could in theory just be a config thing (e.g. `FieryRampage` has attributes `IncreaseDamage` and `IncreaseSpeed` with certain config values)

### projectile.rs

- `ProjectileType` will probably end up as a kind of `Effect`, but for projectiles

### skills.rs
- Make the behavior of skills parameter based (e.g. auto-target, leap, dash, etc)
