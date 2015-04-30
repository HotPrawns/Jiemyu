﻿using System.Reflection;
using Microsoft.Xna.Framework;
using CocosDenshion;
using CocosSharp;

namespace Chess_Demo
{
    class AppDelegate : CCApplicationDelegate
    {
        static CCWindow sharedWindow;

        public static CCWindow SharedWindow
        {
            get { return sharedWindow; }
        }

        public static CCSize DefaultResolution;

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            application.ContentRootDirectory = "assets";

            sharedWindow = mainWindow;

            DefaultResolution = new CCSize(
                application.MainWindow.WindowSizeInPixels.Width,
                application.MainWindow.WindowSizeInPixels.Height);

            CCScene scene = new CCScene(sharedWindow);

            sharedWindow.RunWithScene(scene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            application.Paused = true;
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.Paused = false;
        }
    }
}
