Steps to generate a build for Android and open it in an Android device:

1. Activate developer mode in your Android device. In order to do that, go to Settings > About phone and tap Build number seven times. Then, go to Settings > Developer options and enable USB debugging. You'll also need to enable Install via USB.
2. Connect your Android device to your computer via USB. Make sure that your computer can detect your device and that you choose the option to transfer photos (PTP) when prompted in your device.
3. Open the project in Unity and go to File > Build Settings.
4. Select Android and click on Switch Platform.
5. Click on Player Settings and go to the Other Settings tab.
6. In Publishing Settings, click on Keystore Manager and create a new keystore. You'll need to provide a password and a name for the keystore. Then, click on Create Key and provide a name for the key, a password and a validity period. Finally, click on Create Key. Save this keystore somewhere in your computer outside of the project folder, it's just for testing purposes.
7. In Publishing Settings, click on Browse and select the keystore you just created.
8. In Publishing Settings, make sure that the Split Application Binary option is enabled.
9. Close the Player Settings window and back in the Build Settings window, find the Run Device selection and select your Android device.
10. Enable the Development Build checkbox, and click Build and Run. This will generate a build and install it in your Android device. You may need to authorize the installation in your device.
11. Once the build is installed, you can open it in your device and test it.
