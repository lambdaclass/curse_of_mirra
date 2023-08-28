# How to create projectiles

This will guide you step by step in the process of creating a new projectile.

First of all, you should create a projectile prefab. The vital thing is to add a `ProjectileHandler` component to it. For visual purposes, we recommend the prefab is composed of a projectile and its trail. Keep in mind that, if you choose to follow our recommendation, these GameObjects should be children of the prefab to which `ProjectileHandler` was applied to. Then, place the prefab in the `Resources` folder.

In the `ScriptableObjects > Projectiles` folder, create a scriptable object by selecting `Projectile Info`. You have to assign your projectile prefab and its feedback to each reference within the scriptable object.

Then you should go to the sibling folder, `ScriptableObjects > Skills`, and choose which skill, within each character, you would like to assign a projectile to and click on it. Once you do that, drag your projectile scriptable object to the Projectile Info input in that skill.

`game.rs` sets each projectile behaviour. In that file, you should setup the projectile's behaviour, its prefab name and to which skill it belongs to.

On the other hand we created `ProjectileHandler.cs` to set all the projectile's actions. If you were to need a new action for a projectile to perform it should be created here. These actions are called in `Battle.cs`, so the execution of these actions should be called in this script.
