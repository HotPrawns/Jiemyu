using CocosSharp;
using System;

namespace Chess_Demo
{
    class Program
    {
        static float WIDTH = 1024f;
        static float HEIGHT = 768f;

        static void Main()
        {
            CCApplication application = new CCApplication(false, new CCSize(WIDTH, HEIGHT));
            application.ApplicationDelegate = new AppDelegate();
            application.StartGame();
        }
    }
}
