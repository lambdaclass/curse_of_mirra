# Skill.cs Animations

Let's start by explaining the different methods we have in this script, since it handles more things than just animations, let's focus only in the methods and flow of the animations.

`SetSkill` 

It is used to initialize the skills once the character is selected, it maps the skill from the backend, the skill ScriptableObject and the animationEvent.

`ClearAnimator` 

This method clears each skill parameter, setting all booleans to false.

`ChangeCharacterState`

Updates the current playing animation, changes the movement state machine to attacking and sets the respective animation parameter to `true`.

`StartFeedback`

This is in charge of beginning the start animations (parameters with "_start" in their name, ex: "Skill1_start"), calls the methods mentioned before `ClearAnimator` and `ChangeCharacterState` and finally starts a coroutine to end the animation depending of the `startAnimationDuration` time of the skill ScriptableObject.

`ExecuteFeedback`

#### This method is very important because it controls all the flow of the Skill animations.
This method implements the same logic as `StartFeedback` but for the parameters of all the skills without the "_start" in their name (ex: Skill1). It also uses `executeAnimationDuration` instead of `startAnimationDuration` to end the animation.

`EndSkillFeedback`

Changes the movement machine state to Idle and sets the animation parameter to false. This is used in the `SkillAnimationEvents` to end the animations.

## ExecuteFeedback flowchart

![](./images/executeFeedback.png)

### This flowchart only takes into account the animation part of the method
