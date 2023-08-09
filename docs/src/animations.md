# Unity Animations

## Quick overview

#### The animator

The animator is the nterface to control the Mecanim animation system. We use it to add the animations and create a flow within them.

#### Animation

The animation component is used to play animation clips, which are movement keyframes of the object we want to animate.

#### Transitions

Animation transitions allow the state machine to switch or blend from one animation state to another. We can use differents parameters to make a transition happend. Such as booleans, floats, etc.

Transitions have diffenrents settings to setup deppending of what you want , these are the most important.

- Has Exit Time: Determinates a fixed time for a animation. Ignores any kind of transition parameter.
- If the Fixed Duration box is checked, the transition time is interpreted in seconds. If the Fixed Duration box is not checked, the transition time is interpreted as a fraction of the normalized time of the source state.
- Transition duration: This determinates how much will it took to transition to the next state.

We need The Animator, the animations and transitions to create solid animations with correct flow and states. Drag the animations inside the animator and create transition ( right click in the state) between them to start using your animations.

## How we handle animations

### Parameters

We use a specific list of parameters for our transitions

- `SkillBasic, Skill1, Skill2, Skill3, Skill4 ( Booleans )` We use them to change the state of the animation and make the transition happend.

- `SkillBasic_start, Skill1c_start, Skill2c_start, Skill3c_start, Skill4c_start ( Booleans )` Not allways necessaries , we use them to concatenate animations.

- `SkillBasicSpeed, Skill1Speed, Skill2Speed, Skill3Speed, Skill4Speed ( Floats )` We use them to control the speed of the animation. To use them you have to set the animation to use multiplier parameter and choose the respective parameter.

- `SkillBasicSpeed_start, Skill1Speed_start, Skill2Speed_start, Skill3Speed_start, Skill4Speed_start ( Floats )` We use them to control the speed of the animation start. To use them you have to set the animation to use multiplier parameter and choose the respective parameter.

![](./videos/parameterCustom.gif)

### Scripts

Lets deep into how we use the animations to match with our backend, the scripts we use and more. The main scripts that participate in the animation flow are:

- `Skillinfo.cs` an ScriptableObject with the skill information, here what we care about are the fields `hasModelAnimation`, `startAnimationDuration` , `executeAnimationDuration` and `animationSpeedMultiplier` fields. Lets talk about them later, keep them in mind.

- `Skill.cs` This is a really importart script, here we control the complete flow of the animations ( Play, Stop, Block movements, etc )

- `SkillAnimationEvents.cs` is in change of changing the active skill playing and end the animation playing.
- `PlayerMovement.cs`
- `CustomLevelManager.cs` Where the buttons and mapped with the respective skill.

### SkillInfo fields

- `hasModelAnimation` allows the animation to start and execute it. If this is not true you would have to start the animation manually. It can be useful to have it false with animations like walking where you want them to play constantly.

- `executeAnimationDuration` Determinates the time that the animation will be played

- `startAnimationDuration` Determinates the time to start the animation
- `animationSpeedMultiplier` Determinate the play speed of the animation ( used in the transition parameters if it is set )

### Skill.cs

In this scripts is where all the magic happends. It has all the scripts to Start, Execute, Stop , Change the animations. I believe this script leads to a separeted guide. So for now have in mind that all the logic of flows happends here.
[Skill.cs docs](./skill.md)
