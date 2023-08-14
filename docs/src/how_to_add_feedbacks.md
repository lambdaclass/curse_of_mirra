# How to create a new feedback for State Effects
☝️🤓 Assuming we already have the feedback implemented in the backend

1. First, create the new Feedback prefab with the particle system you want, then add it to the `Client/Asssets/Feedbacks/States` folder.

2. In the `FeedbackContainer` prefab add the new prefab you just created to the list of prefabs.

3. Now you have to add the new state to the `StateEffects` enum and use the value of `PlayerEffect`. It's better with an example, if we would like to add the Effect state `Freeze`:
``` 
    private enum StateEffects
    {
        Poisoned = PlayerEffect.Poisoned,
        Slowed = PlayerEffect.Slowed,
        Freeze = PlayerEffect.Freeze,
    }
```
4. And now you are ready to go! Test it and enjoy the new feedback.🤩
