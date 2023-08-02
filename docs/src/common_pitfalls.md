# Common Pitfalls

## Floating point arithmetic

## Keeping track of time in Unity

Some parts of the client code need to keep track of how much time has passed since the game started. Most notably, entity interpolation uses this to know which game update from the buffer it should render. There are at least two ways to do this:

- Using the `Time` module provided by unity, with functions like `Time.deltaTime`, which gives you the time passed between frames.
- Using Unix timestamps, keeping an initial timestamp at the beginning and then getting a new one each time we want to know how much time has passed.

It might seem like these are equivalent, but the first one has a problem that the second one doesn't. The problem with using `Time` is that it needs to have the game running for it to apply. If you ever move the game to the background, it will stop running, frames won't be executed, and the time will essentially stop passing. If you alt-tab for one second, when you go back to the game your accumulated time is one second behind.

This is a huge problem. A possible solution is to change the settings on Unity so that the game always runs, even in the background. This does not work, however, because not every platform allows for running in the background (Android, for example, forbids it).

Because of all this, if you have to keep track of accumulated time, go for timestamps, not `Time`.
