# Building to WebGL

In order to make a local WebGL build, go through the following steps.

1. In the Unity Editor, File -> Build Settings...
2. In the window that opens, select WebGL and click Switch Platform.
3. Select the Scenes you want to include in the build. In our case, those scenes are (make sure to have them in this order): `Scenes/Lobbies`, `ThirdParty/TopDownEngine/(...)/LoadingScreens/LoadingScreen`, `Scenes/Lobby`, `Scenes/BackendPlayground`.
4. Click Build and Run and select a folder to save the build to.
5. After the build is done, the game should open in your browser.

## Some notes

Unity produces two types of builds, this can be set using the checkbox in the Build Settings window.
- A release build, which includes only what’s necessary to run the application. This is the default build type.
- A development build, which includes scripting debug symbols and the Profiler. Selecting the development build option enables an additional set of options, such as deep profiling support and script debugging, but makes the build larger and slower than a release build.

In Player Settings -> Publishing Settings, you can choose to enable exceptions catching in WebGL, which will make the build slower but will also give you more information in case of an error.
