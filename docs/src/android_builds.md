# Steps to generate a build for Android and open it in an Android device:

1. Activate developer mode in your Android device. In most devices, you should go to *Settings* > *About phone* and tap *Build number* seven times. Then, go to *Settings* > *Developer options* and enable *USB debugging*. You'll also need to enable *Install via USB*. If that's not the way to activate developer mode in your device, you can find help in [the official docs](https://developer.android.com/studio/debug/dev-options).

2. Connect your Android device to your computer via USB. Make sure that your computer can detect your device and that you choose the option to transfer files when prompted in your device.

3. Open the project in Unity and go to *File* > *Build Settings*.

4. Select Android and click on *Switch Platform*. You may need to install the Android Build Support module in Unity beforehand and restart your editor.

5. Click on *Player Settings* and go to *Publishing Settings* (you may use the search bar to get there), click on *Keystore Manager* > (*Keystore...* drop-down menu) > *Create New*. You may save it anywhere in your computer outside of the project folder, it's just for testing purposes. You'll need to provide a password and a name for the keystore. Then, click on *Add Key*.

7. In *Publishing Settings*, click on *Browse* and select the keystore you just created. Also in the same window, at the bottom, make sure that the *Split Application Binary* option is enabled.

9. Close the *Player Settings* window and back in the *Build Settings* window find the *Run Device* selection and select your Android device.

10. Enable the *Development Build* checkbox, click *Build and Run* and choose a location to save the build (outside of the project directory). This will generate a build and install it in your Android device. A dialog will appear in your device asking you to allow the installation of the app, click on *Install*.

11. Once the build is installed, you can open it in your device and test it.
