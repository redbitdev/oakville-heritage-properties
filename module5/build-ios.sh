#!/bin/sh
# This is the server build script. It expects the following environment variables to be set outside 
# of this build script:
#
export TESTCLOUD_API_KEY=“YOUR_API_KEY”
export IOS_DEVICE_ID="92028d88"


### Grab all the nuget packages
/usr/bin/nuget restore HeritageProperties.sln

### This will have to be updated when xut-console is updated.
export XUTCONSOLE=./packages/Xamarin.UITest.0.6.5/tools/test-cloud.exe

### You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./HeritageProperties/HeritageProperties.UITest/bin/Release/
export IPA=./HeritageProperties/HeritageProperties.iOS/bin/iPhone/Debug/FormsTemplateiOS-1.0.ipa

### Uploading the dSYM files is optional - but it can help with troubleshooting 
export DSYM=./HeritageProperties/HeritageProperties.iOS/bin/iPhone/Debug/HeritagePropertiesiOS.app.dSYM

### Remove the old directories
rm -rf ./HeritageProperties/HeritageProperties.iOS/obj
rm -rf ./HeritageProperties/HeritageProperties.iOS/bin

## UITest: Build the UITest project
/usr/bin/xbuild /p:Configuration=Release ./HeritageProperties/HeritageProperties.UITest/HeritageProperties.UITest.csproj

## Core Lib: Build the Xamarin Forms project
/usr/bin/xbuild /p:Configuration=Release ./HeritageProperties/HeritageProperties/HeritageProperties.csproj

### iOS : build and submit the iOS app for testing
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./HeritageProperties.sln
/usr/bin/mono $XUTCONSOLE submit $IPA $TESTCLOUD_API_KEY --devices $IOS_DEVICE_ID --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Heritage Properties Validator (iOS)" --dsym $DSYM
