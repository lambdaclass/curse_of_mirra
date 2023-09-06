# Effects

Effects are mostly buffs and debuffs players can have. They will given by skills or loot.

An important aspect of effects is that they have a core mechanic which determines how they act, the rest of its behavior will be configurable

## Struct

Configurable fields:
- `name`: unique name for the effect
- `duration_ms`: Duration of the effect in ms. If duration is not provided it will mean the duration is forever and effect is never removed
- `trigger_interval_ms`: Intervals between triggers of the effect (e.g. gain health every X). `duration_ms` MUST be divisible by this amount. If not provided or is 0 effect will trigger immediately, otherwise it will first trigger after the interval time has passed
- `mechanic`: Core mechanic of the effect (e.g. damage increase %)
- `change_value`: The mechanics doing a value change will store it here

Non-configurable:
- `remaining_trigger_counts`: This number is defined by `duration_ms/trigger_interval_ms` and acts as a failsafe to make sure the effect is triggered the amount of times we wanted

## Core mechanics

This are the mechanics so far, most are simplified into a single `Change` mechanic which can either increase or decrease something.

Every mechanic will add a configuration field with the same name of it, the difference is that most only expect a single config value so it is expressed directly, others where a more complex configuration is possible will expect a nested object

- `HealthChange`: Player health will change by X
  * `change_value`
- `MaxHealthPercentageChange`: Player health will change by X% of their max health
  * `change_value`
- `DamagePercentageChange`: Player damage will increase/decrease by X%
  * `change_value`
- `DefensePercentageChange`: Damage received by the player is increased/decreased by X%
  * `change_value`
- `SpeedPercentageChange`: Player speed changes by X%
  * `change_value`
- `SizePercentageChange`: Player hitbox and model size increases/decreases by X%
  * `change_value`
- `CooldownPercentageChange`: Player cooldowns change by X%
  * `change_value`
- `Piercing`: Projectiles from the player don't dissappear on collision and keep going
- `Disarm`: Player is unable to use skills

## Configuration

Examples of the JSON defining effects

```
[
  {
    "name": "Gain health 10 3s"
    "duration": 3000,
    "trigger_interval_ms": 1000,
    "mechanic": "HealthChange",
    "HealthChange": 10
  },
  {
    "name": "Gigantify"
    "duration": 10000,
    "trigger_interval_ms": 0,
    "mechanic": "SizePercentageChange",
    "SizePercentageChange": 50
  }
]
```
