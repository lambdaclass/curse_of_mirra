# Skill.cs

Lets start by explaining the differents methods we have in this script, no all of them but the ones we care to handle animations.

`SetSkill` 

It is use to initialize the skills once the character is selected,  it maps the skill from the backend, the skill scripteableObject, the animationEvent.

`ClearAnimator` 

This method clear each skill parameter, setting all booleans to false

`ChangeCharacterState`

Updates the current playing animation, changes the movement state machine to attacking and sets the respective animation parameter to `true`.

`StartFeedback`

This is in charge of begin the start animations (parameters with "_start" in their name, ex: "Skill1_start"), calls the methods mencioned before `ClearAnimator` and `ChangeCharacterState` and finally starts a coroutine to end the animation depending of the `startAnimationDuration` time of the skill scripteableObject.

`ExecuteFeedback`

#### This method is very importart due its the one which controls all the flow of the Skill animations.
This method implements the same logic as `StartFeedback` but for the parameters of all the skills without the "_start" in their name (ex: Skill1) and also use `executeAnimationDuration` instead of the `startAnimationDuration` to end the animation.

`EndSkillFeedback`

Changes the movement machine state to Idle and set the animation parameter to false. This is used in the `SkillAnimationEvents` to end the animations.
