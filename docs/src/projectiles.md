# How we handle projectiles

For this explanation we will be refearing to 5 scripts: `ProjectileInfo.cs`, `ProjectileHandler.cs`, `Battle.cs`, `Skill.cs` and `SkillInfo.cs`.

Our first goal, is to create a prefab for the projectile and another one for its feedback. The first issue to solve would be to associate them for future references, for example when there are several projectiles being shot and we want to display a specific feedback for each one. For this, we needed a ScriptableObject. `ProjectileInfo.cs` inherits from the **ScriptableObject** class. In there we use CreateAssetMenu to make it easier to create custom assets using this class. We also stablished two GameObjects to reference: projectile and projectileFeedback.

```
public GameObject projectile;
public GameObject projectileFeedback;
```

To be able to assign this projectile scriptable object to a specific skill, we created a reference for it in `SkillInfo.cs`, which has all the references any skill could need. That's why we added there a reference such as:

```
public ProjectileInfo projectileInfo;
```

In addition, taking into account that each skill inherits from `Skill.cs,` we created `GetProjectileFromSkill()`, which returns the ProjectileInfo associated with the given skill through its scriptable object.

On the other hand we created `ProjectileHandler.cs` to set all the projectile's actions. This script is attached to every projectile prefab. We are using **MMSimpleObjectPooler** to have a projectiles pooler. Feel free to read the [documentation](https://corgi-engine-docs.moremountains.com/API/class_more_mountains_1_1_tools_1_1_m_m_simple_object_pooler.html) on this but we eill give you a quick overview in regards of why we use this. Object Pooling is a great way to optimize your projects and lower the burden that is placed on the CPU when having to rapidly create and destroy GameObjects. To set our pooler we created `SetProjectilePrefab()`. For this we need to send 3 values through paratemer: a name for the GameObject that will be created with an **MMSimpleObjectPooler** component attached to it; the position where the **SimpleObjectPooler** list will be created; and the projectile prefab's name that will be used to fill the previously mentioned list, by default with a length of 10;

In `Battle.cs` we stablished three variables to handle the projectiles.

```
public IEnumerable<ProjectileInfo> skillProjectiles;
public ProjectileInfo projectileUsed;
```

We set **skillProjectiles** using the `GetProjectileFromSkill()` method mentioned before. For each **skillProjectile** we set the projectile to be pooled with `SetProjectilePrefab()`. Therefore, there will be a pooler for each projectile.

In `UpdateProjectileActions()`, called in `Update()`, if a gameProjectile status is active, we only take the projectile which has the same name as the gameProjectile and assign it to the variable **projectileUsed**; Once we know which projectile is being used we are able to call all the functions needed within the `ProjectHandler.cs` such as `.InstanceShoot()`, `ShootLaser()`, `LaserCollision`, `LaserDisappear()`, etc.
