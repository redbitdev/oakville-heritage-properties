#!/bin/sh
# This is the server build script. It expects the following environment variables to be set outside 
# of this build script:
#
export TESTCLOUD_API_KEY=“YOUR_API_KEY”
export ANDROID_DEVICE_ID="bfa77759"


### Grab all the nuget packages
/usr/bin/nuget restore HeritageProperties.sln

### This will have to be updated when xut-console is updated.
export XUTCONSOLE=./packages/Xamarin.UITest.0.6.5/tools/test-cloud.exe

### You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./HeritageProperties/HeritageProperties.UITest/bin/Release/
export APK=./HeritageProperties/HeritageProperties.Android/bin/Release/HeritageProperties.Droid.apk

### Remove the old directories
rm -rf ./HeritageProperties/HeritageProperties.Android/obj
rm -rf ./HeritageProperties/HeritageProperties.Android/bin

## UITest: Build the UITest project
/usr/bin/xbuild /p:Configuration=Release ./HeritageProperties/HeritageProperties.UITest/HeritageProperties.UITest.csproj

## Core Lib: Build the Xamarin Forms project
/usr/bin/xbuild /p:Configuration=Release ./HeritageProperties/HeritageProperties/HeritageProperties.csproj

### Android: Build and submit the Android app for testing using the default keystore
/usr/bin/xbuild /t:Package /p:Configuration=Release ./HeritageProperties/HeritageProperties.Android/HeritageProperties.Android.csproj
/usr/bin/mono $XUTCONSOLE submit $APK $TESTCLOUD_API_KEY --devices $ANDROID_DEVICE_ID --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Heritage Properties Validator (Android)"