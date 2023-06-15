# Steps to generate a build for iOS and run it in an iPhone:

1. Create Build in Unity

2. File -> Build Settings -> iOS -> Build. Select new folder, all inside this folder will be overwritten

3. In new folder, open .xcodeproj file with XCode

4. Go to Unity-iPhone -> Signing and Capabilities. Check Automatically manage signing. In team use personal iOS Account (may need free registration in dev program developer.apple.com). Download provisioning profile if needed. Change bundle identifier to something unique that we wont use for the game. For example: com.myGitUsername.myrraV1 (and so on).

5. Plug in iPhone, in iPhone enable developer mode (Settings -> Privacy and Security -> Developer mode -> on). In Xcode Choose iPhone in top bar

6. Trust developer in iPhone (settings -> General -> VPN & Device Management -> Developer app -> Trust)

7. Run Game in iPhone by pressing Play button in XCode
