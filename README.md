# Champions of Mirra

## Table of Contents

- [Champions of Mirra](#champions-of-mirra)
  - [Table of Contents](#table-of-contents)
  - [About](#about)
  - [Licensing](#licensing)
  - [Requirements](#requirements)
  - [Suggested Development Environment](#suggested-development-environment)
  - [Project and Unity Setup](#project-and-unity-setup)
  - [Running the Backend](#running-the-backend)
  - [Documentation](#documentation)
  - [Contact and Socials](#contact-and-socials)

## About

Welcome to the realm of Champions of Mirra, crafted by LambdaClass.

Champions of Mirra is the inaugural game built on our groundbreaking [Game Backend](https://github.com/lambdaclass/game_backend). This open source backend, meticulously developed by Lambda, ensures seamless and reliable gameplay.

Step into a universe where the destinies of heroes from four planets collide in an epic struggle for the favor of Mirra, a capricious deity known for manipulating entire societies by exploiting their deepest desires. Brace yourself for an immersive journey where every decision matters, and the pursuit of victory comes with the ever-present thrill of unpredictability.

Champions of Mirra is more than a game; it's an adventure into a world where strategy, skill, and a dash of chaos converge. Join the battle and confront the challenges that lie ahead in this captivating and dynamic gaming experience. The stage is set, and the Champions of Mirra awaits—embrace the challenge and become a legend!

<div>
  <div float="center">
    <img src="docs/src/images/Champions_of_Mirra_3D_Assets_Muflus.png" alt="Muflus 3D model" width=300px>  
    <img src="docs/src/images/Champions_of_Mirra_3D_Assets_Uma.jpeg" alt="Uma 3D model" width=300px> 
  </div>
  <div float="center">
    <img src="docs/src/images/Champions_of_Mirra_concept_art_Shinko.png" alt="Shinko hero concept art" width=300px>
    <img src="docs/src/images/Champions_of_Mirra_concept_art_Otobi_dog.png" alt="Concept art for a gang member dog in the planet of Otobi" width=300px>
  </div>
<div>

## Licensing

The code is licensed under the Apache 2 license, while the music and graphics are licensed under a CC attribution and share-alike license.

Find our open source 3D models, concept art, music, lore and more in our [Curse of Mirra Open Game Assets](https://github.com/lambdaclass/curse_of_mirra_assets) repository.

## Requirements

- **Rust:** _for docs_
  - [Install Rust](https://www.rust-lang.org/tools/install)
- **Unity Hub:**
  - [Download Unity Hub](https://unity.com/unity-hub)

## Suggested Development Environment

Set up your environment with the following steps:

- Download the [.NET SDK](https://dotnet.microsoft.com/es-es/download/dotnet/thank-you/sdk-7.0.403-macos-arm64-installer) and [Mono](https://www.mono-project.com/download/stable/) for your operating system.
- In VSCode, download the ```C# Dev Kit``` extension. 
  - You must go to the C# extension and set the version to `v1.25.9` in order for it to work
- In Unity preferences, under "External Tools", check the following preferences:
  - Embedded packages
  - Local packages
  - Registry packages
  - Git packages
  - Built-in packages
  Then click on "Regenerate project files"

## Project and Unity Setup
- Open a terminal and clone the project:

```bash
git clone https://github.com/lambdaclass/champions_of_mirra
```

- Open Unity Hub, if this is your first launching the Hub, you can skip the unity editor installation.
- Click on the add project button and select `champions_of_mirra/client` folder.
- Install the Unity editor suggested version (not latest).
  - You can then install optional tools (Android SDK, iOS Build Support, etc.). None of them are mandatory.
- Download the following libraries and include them in the `Assets/ThirdParty` folder:
  - [Top Down Engine](https://assetstore.unity.com/packages/templates/systems/topdown-engine-89636) by [More Mountains](https://moremountains.com). You need to purchase the license to use it.
  - [SineVFX](https://assetstore.unity.com/packages/vfx/particles/spells/top-down-effects-191455). You need to purchase the license to use it.
  - [JMO Assets](https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565).
  - [Top-Down Dungeons](https://assetstore.unity.com/packages/3d/environments/dungeons/top-down-dungeons-7853).
- To test the game, select the scene in `Assets/Scenes/TitleScreen` and run it by clicking the play button.

## Local Testing

For local testing, use the [mirra backend](https://github.com/lambdaclass/mirra_backend).
Follow its README instructions to build and run the application.
Remember to set ```localhost``` as the server in the client.

## Documentation

Explore our documentation [here](https://docs.curseofmirra.com/) or run it locally. To run locally, install:

```
cargo install mdbook
cargo install mdbook-mermaid
```

Then run:

```
make docs
```

Open: [http://localhost:3000/](http://localhost:3000/ios_builds.html)

Some key documentation pages:

- [Message protocol](https://docs.curseofmirra.com/message_protocol.html)
- [Android builds](https://docs.curseofmirra.com/android_builds.html)
- [iOS builds](https://docs.curseofmirra.com/ios_builds.html)

## Contact and Socials

If you have any questions, feedback, or comments:

- **Email:** gamedev@lambdaclass.com

We share our development and creative process in the open, follow us for frequent updates on our game:

- **Twitter:** [@CurseOfMirra](https://twitter.com/curseofmirra)
- **Reddit:** [r/curseofmirra](https://www.reddit.com/r/curseofmirra/)
- **Discord:** [join link](https://discord.gg/hxDRsbCpzC)
- **Telegram:** [t.me/curseofmirra](https://t.me/curseofmirra)
