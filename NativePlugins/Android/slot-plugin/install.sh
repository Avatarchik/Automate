#!/bin/sh
UNITYLIBS="/Applications/Unity/Unity.app/Contents/PlaybackEngines/AndroidPlayer/development/bin/classes.jar"
DSTDIR=../../../Assets/Plugins/Android
export ANT_OPTS=-Dfile.encoding=UTF8
android update project -p .
mkdir -p libs
cp $UNITYLIBS libs
ant release
mkdir -p $DSTDIR
cp -a bin/classes.jar $DSTDIR/slot-plugin.jar
ant clean
#rm -rf libs res proguard-project.txt
