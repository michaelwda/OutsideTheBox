using System.Collections.Generic;
using System.Threading.Tasks;
using OTB.Core.Hook;

namespace OTB.Core
{
    public interface IOTBClient
    {
        Task ScreenConfigUpdate(IEnumerable<VirtualScreen> screens);
        Task Clipboard(string value);
        Task MouseMove(double x, double y);
        Task MouseWheel(int x, int y);
        Task MouseDown(MouseButton button);
        Task MouseUp(MouseButton button);
        Task KeyDown(Key key);
        Task KeyUp(Key key);
    }
}