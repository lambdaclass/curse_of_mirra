# Fluid Gameplay in a Multiplayer Environment

Curse of Myrra is a multiplayer game. As such, every action any player performs (moving, attacking, etc) has to go through a centralized server that changes the state accordingly and eventually sends back the new state to players. This introduces a huge source of problems when trying to render the game smoothly: there is an unreliable network in between.

Below we go over the problems that arise from networking, how we solved some of them, and how we might improve these solutions in the future. This section is going to be long, full of explanations and videos showing different issues that arise from playing through a network. I HIGHLY encourage anyone reading this to actually try this stuff out by themselves. The way we judge whether we have done a good job or not is if the game feels good to play. Ultimately, that is the only metric that matters here.

The main thing we will discuss is *movement*, as it is the most basic element of the game that needs to render smoothly. Constant jitters/stutters in movement are the hallmark of code that is not robust enough to handle multiplayer gameplay.

## Framerate, Tick rate and Action rate

Before continuing, we need to talk about three extremely important concepts.

### Framerate

`Framerate`, sometimes called `FPS` (for `frames per second`), is how many `frames` the game (unity) renders each second. Let's go into this in more detail. As complex as games can be, they can all ultimately be reduced to the following high level code:
```
while true {
    get_user_inputs();
    update_game_state_according_to_these_inputs();
    render_game_state();
}
```

This mental model is so common in games that it has a name; this is the `game loop`. A `frame` is one iteration of this loop.
 
Typically, the more computing power you have, the higher your framerate. In Myrra, we show players their framerate in the bottom left corner of the screen. Its value is usually capped at `300`, and anything below `30` will feel really bad to play. Most games run either at `30`, `60` or at an uncapped `FPS`.

Myrra does not cap framerate, which means it can go up to `300`. This is an important thing to keep in mind, as we don't have control over it, and therefore cannot make assumptions about its value. Some games cap it and then use it as a way to, for example, keep track of time.

It's very important to understand that framerate is a property of the client; the server does not know about it at all. This will matter later on.

### Tick Rate

The `Tick Rate` is the rate at which the server sends game updates to every client. As explained on the [backend architecture section](./backend_architecture.md), the way our backend works is the following:

- It receives commands from clients (move, attack, use ability, etc).
- Commands are applied to the game state, mutating it.
- Every so often, it sends every client the current game state.

This last "every so often" is the tick rate. We usually refer to it in milliseconds, so if we say the tick rate is `50ms`, what we are saying is every 50 milliseconds, the backend will send the current state to clients.

You can think of tick rate as the rate at which we sample the gameplay. A higher tick rate means we sample more frequently, and thus converge to a more continuous experience, while a lower one can make the game look like a slideshow.

Note that we are making tick rate the same for every player, that is, players all receive updates at the same rate. This is not the only way to do things. `Valve`, for example, makes it possible for you to set (within a range) how many ticks you get per second. Typically, you want to set it as high as possible for a more accurate experience, but if your network or computer can't handle a high tick rate, lowering it can help.

These days, computers and internet providers are fast enough that almost nobody would need to lower it, so we don't bother allowing for a variable tick rate.

### Action/Command Rate

The `Action` or `Command` rate is the rate at which the client sends movement commands to the server. It's important to understand that this applies ONLY to movement. Commands related to using abilities do not have this restriction, and can be sent at any moment, at any frequency. This is an unusual choice. The way we handle input in general is unusual, so let's explain it in more detail.

The main reason to introduce an action rate is to reduce pressure on the network, both on the server and the client. It usually works like this:

- Whenever the player presses a key, its associated command is saved into a list/buffer.
- When the action rate time has passed, the client bundles up all the commands and sends them together to the server.

We don't do this. What we do instead is the following:

- For non-movement commands, whenever the player issues them, we immediately send the command to the server.
- For movement commands, every `ActionRate` milliseconds, we check for input on the movement keys/joystick. We then send the appropriate command to the server (move left, move right, etc).

The reason we do this has nothing to do with reducing pressure on the network. It's a consequence of what movement commands look like in our game. When someone sends a "move right" command, the server will move them a fixed amount to the right. What this means is the more "move right" commands you can send per unit of time, the faster you will move. This is a problem, which we fixed in the hackiest way possible. We introduced the action rate to fix the move commands frequency, and thus fix movement speeds.

Keep in mind this isn't final. We may and probably will change this at some point in the future. For now, however, this is how it works. Of course, people could cheat by unlocking their action rate, which is something we will have to address if this is the solution we keep.

The less hacky solution to this is to make move commands tell the server where you are now. Players know their own speed so they can do the math themselves and just tell the server "now I'm here" or "I moved this many units to the right". This allows clients to send as many movement commands as they want. You still have to solve cheating, however, as clients could lie to the server, but we won't concern ourselves with that here.

In Myrra, the action rate is set to be the same as the tick rate. This way, we get one movement per tick. The immediate problem caused by this is that speed is tied to tick rate. Decreasing tick rate slows everything down, increasing it accelerates it. There's also another huge problem this causes, which we'll talk about extensively later on as it affects smooth movement.

## Inescapable fact: Networks are unreliable

## No Perfect Solutions

## High ping vs high ping *Variance*

## Naive movement code (use latest update to set position)

## Framerate same as tick rate example

## Smooth movement through interpolation

## Client prediction

## Entity interpolation

## Action rate, hacky solution
