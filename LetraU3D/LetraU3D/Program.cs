using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LetraU3D
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var nativeSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "3D Letter U",
            };

            using (var window = new Window(GameWindowSettings.Default, nativeSettings))
            {
                window.Run();
            }
        }
    }
}
