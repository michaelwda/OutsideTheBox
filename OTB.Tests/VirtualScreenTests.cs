using Microsoft.VisualStudio.TestTools.UnitTesting;
using OTB.Core;

namespace OTB.Tests
{
    [TestClass]
    public class VirtualScreenTests
    {
        private ScreenConfiguration InitialSetup()
        {
            var config = new ScreenConfiguration();
            var mainMonitor = new VirtualScreen()
            {
                LocalX = 0,
                LocalY = 0,
                Width = 3840,
                Height = 2160,
            };
            var secondaryMonitor = new VirtualScreen()
            {
                LocalX = 3840,
                LocalY = -1680,
                Width = 2160,
                Height = 3840,
            };
            var laptop = new VirtualScreen()
            {
                LocalX = 0,
                LocalY = 0,
                Width = 1980,
                Height = 1020,
            };

            config.AddScreen(mainMonitor, "1", "1");
            config.AddScreen(secondaryMonitor, "1", "1");
            config.AddScreen(laptop, "2", "2");

            return config;
        }
        [TestMethod]
        public void TestAddScreen()
        {
            var config = InitialSetup();
            Assert.AreEqual(2, config.Screens.Count);
            Assert.AreEqual(2, config.GetScreensForConnection("1").Count);
            Assert.AreEqual(1, config.GetScreensForConnection("2").Count);
            var virtualScreen=config.GetScreenForVirtualCoordinate(4000, 500);
            Assert.AreEqual("1",virtualScreen.ConnectionId);
            Assert.AreEqual(2160, virtualScreen.Width);
        }

        
    }
}
