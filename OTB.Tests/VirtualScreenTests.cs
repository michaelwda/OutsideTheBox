using Microsoft.VisualStudio.TestTools.UnitTesting;
using OTB.Core;

namespace OTB.Tests
{
    [TestClass]
    public class VirtualScreenTests
    {
        [TestMethod]
        public void AddScreen()
        {
            var config=new ScreenConfiguration();
            var mainMonitor=new VirtualScreen(){
                Client = "TEST",

            };
        }

    }
}
