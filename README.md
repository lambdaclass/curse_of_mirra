# Curse of Myrra

<img src="docs/src/images/Curse_of_Myrra_logo.png" alt="Curse of Myrra logo">

Welcome to the realm of Curse of Myrra, crafted by LambdaClass.

Curse of Myrra is the inaugural game built on our groundbreaking [Game Backend](https://github.com/lambdaclass/game_backend). This open source backend, meticulously developed by Lambda, ensures seamless and reliable gameplay.

Step into a universe where the destinies of heroes from four planets collide in an epic struggle for the favor of Myrra, a capricious deity known for manipulating entire societies by exploiting their deepest desires. Brace yourself for an immersive journey where every decision matters, and the pursuit of victory comes with the ever-present thrill of unpredictability.

Curse of Myrra is more than a game; it's an adventure into a world where strategy, skill, and a dash of chaos converge. Join the battle and confront the challenges that lie ahead in this captivating and dynamic gaming experience. The stage is set, and the Curse of Myrra awaits—embrace the challenge and become a legend!

<div>
  <div float="center">
    <img src="docs/src/images/Curse_of_Myrra_3D_Assets_Muflus.png" alt="Muflus 3D model" width=300px>  
    <img src="docs/src/images/Curse_of_Myrra_3D_Assets_Uma.jpeg" alt="Uma 3D model" width=300px> 
  </div>
  <div float="center">
    <img src="docs/src/images/Curse_of_Myrra_concept_art_Shinko.png" alt="Shinko hero concept art" width=300px>
    <img src="docs/src/images/Curse_of_Myrra_concept_art_Otobi_dog.png" alt="Concept art for a gang member dog in the planet of Otobi" width=300px>
  </div>
<div>

The code is licenced with an Apache 2 license. The music and graphs are licenced with a CC attribution and share alike license.

## Requirements

- Rust:
  - https://www.rust-lang.org/tools/install
- Elixir and Erlang:
  - https://thinkingelixir.com/install-elixir-using-asdf/
  - Erlang/OTP 26
  - Elixir 1.15.4
- Unity
  - Download link: https://unity.com/unity-hub
- Docker

## Suggested environment

- Download the [.NET SDK](https://dotnet.microsoft.com/es-es/download/dotnet/thank-you/sdk-7.0.403-macos-arm64-installer) for your operating system
- In VSCode, download the .NET extension. Once installed, change the version to 1.26 (the option to change versions is in a dropdown next to the Uninstall button in the extension window)/
  To check if the previous steps worked, go to the VSCode's console, select the Output tab and pick Omnisharp Log in the dropdown. If you don't get error logs in that tab and you can see that Omnisharp is scanning the project, then the config is OK.

## Setup project

Make sure Docker is running.

```
git clone https://github.com/lambdaclass/curse_of_myrra
cd curse_of_myrra/server
make db
make setup
```

## Setup Unity

- On Unity Hub, click on the add project button and select the `client` folder.
- Select the correct version of the editor, by default it will show the version needed for the project but you need to select if you want to download the Intel or Silicon version.
- It's also needed to download the [Top Down Engine](https://assetstore.unity.com/packages/templates/systems/topdown-engine-89636) by [More Mountains](https://moremountains.com) and include it in the `Assets/ThirdParty` folder. For this it's necessary to purchase the license of the engine.
- Once we run Unity if you want to test the game you can select the scene on `Assets/Scenes/TitleScreen` and then running it by clicking the play button.

## Run backend

Make sure Docker is running.

```
make start
```

To test locally using the [game backend](https://github.com/lambdaclass/game_backend), temporarily edit the `mix.exs` file to point to the path to your _local_ copy of the game backend, for example:
`{:game_backend, path: "/Users/MyUsername/lambda/game_backend"}`

To test using a remote server, point to the _github URL_ instead and specify the desired branch:
`{:game_backend, git: "https://github.com/lambdaclass/game_backend", branch: "main"}`

## Useful commands

```
make tests
```

Will run Elixir and Rust tests

```
make format
```

Will format Elixir and Rust code.

```
make prepush
```

Will format you code, and run Credo check and tests.

## Documentation

You can find our documentation [here](https://docs.curseofmyrra.com/) or by running it locally.

For that you have to install:

```
cargo install mdbook
cargo install mdbook-mermaid
```

Then run:

```
make docs
```

Open:
[http://localhost:3000/](http://localhost:3000/ios_builds.html)

Some useful links

- [Backend architecture](https://docs.curseofmyrra.com/backend_architecture.html)
- [Message protocol](https://docs.curseofmyrra.com/message_protocol.html)
- [Android build](https://docs.curseofmyrra.com/android_builds.html)
- [IOs builds](https://docs.curseofmyrra.com/ios_builds.html)
