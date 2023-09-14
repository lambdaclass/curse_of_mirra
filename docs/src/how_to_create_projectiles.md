# How to create projectiles

This will guide you step by step in the process of creating a new projectile.

First of all, you should create a projectile prefab. The vital thing is to add a `SkillProjectile.cs` component to it. For visual purposes, we recommend the prefab has a projectile and a trail. Keep in mind that, if you choose to follow our recommendation, these GameObjects should be children of the prefab to which `SkillProjectile.cs` was applied to. Then, place the prefab in the `Prefab > Projectiles` folder.

In the `ScriptableObjects > Projectiles` folder, create a scriptable object by selecting `Projectile Info`. You have to assign your projectile's feedback to it's reference.

Now that we have a scriptable object with the information required by our projectile prefab, we should attach one to the other. You should assign this scriptable object to its reference in `SkillProjectile.cs` within the projectile prefab.

At this point, our projectile should be good to go. The only step left is to assign our projectile to the skill desired. Each `SkillInfo.cs` has a reference for a **projectilePrefab**. Therefore, in the sibling folder, `ScriptableObjects > Skills` choose which skill, within each character, you would like to assign a projectile to and click on it. Once you do that, drag your projectile scriptable object to the **Projectile Prefab** input in that skill.

On the other hand, `game.rs` sets each projectile behaviour. In that file, you should set the projectile's behaviour and to which skill it belongs to.
