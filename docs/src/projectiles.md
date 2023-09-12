# How we handle projectiles

For this explanation we will be referring to 5 scripts: `ProjectileInfo.cs`, `ProjectileHandler.cs`, `SkillProjectile.cs`, `Battle.cs` and `SkillInfo.cs`.

Our first goal, was to create two prefabs. One for the projectile and another one for its feedback. The first issue to solve was how to associate them for future references. For example, when there are several projectiles being shot, we want to display a specific feedback for each one. For this, we needed a **Scriptable Object**. `ProjectileInfo.cs` inherits from the **ScriptableObject** class. In there we use CreateAssetMenu to make it easier to create custom assets using this class. We also stablished a GameObject reference: projectileFeedback.

```
public GameObject projectileFeedback;
```

This script is attached to each projectile prefab. To assign this projectile to a specific skill, we created a reference for it in `SkillInfo.cs`. This has all the references any skill could need.

```
public GameObject projectilePrefab;
```

`ProjectileHandler.cs` is attached to **Battle Manager** in the scene and handles the creation and instance of the projectiles. We are using **MMSimpleObjectPooler** to have a projectile pooler for each kind of projectile. Feel free to read the [documentation](https://corgi-engine-docs.moremountains.com/API/class_more_mountains_1_1_tools_1_1_m_m_simple_object_pooler.html) on this, but we will give you a quick overview in regards of why we use this. Object Pooling is a great way to optimize your projects and lower the burden that is placed on the CPU when having to rapidly create and destroy GameObjects. With that being said, to set our pooler we use `CreateProjectilesPoolers()`. This requires a **HashSet<SkillInfo> skillInfoSet** to be sent as a parameter. **skillInfoSet** is a HashSet of every skill in which a projectile prefab is assigned. Then, it is only a matter of getting from each skill their projectile to create a pooler.

`SkillProjectile.cs` is a component in each projectile prefab. In this script we have a reference for projectile info and methods meant for each projectile.

```
[SerializeField] public ProjectileInfo projectileInfo;
```

In `Battle.cs` we stablished a HashSet to populate it with all the skills that have a projectile prefab assigned.

```
public HashSet<SkillInfo> skillInfoSet;
```

In `InitializeProjectiles()` we wait for the players to be loaded and check for each player, all their skills and save in the HashSet only the ones with a projectile prefab assigned.

In `UpdateProjectileActions()`, called in `Update()`, if a gameProjectile status is active, we compare the skill triggered with the skills in the HashSet. This is how we determine which skill is being used and get their projectile. Once we know that, we are able to call all the methods implemented by `SkillProjectile.cs` such as `InstanceShoot()`, `ShootLaser()`, `LaserCollision`, `LaserDisappear()`, etc.
