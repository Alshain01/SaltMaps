using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaltCharts
{
    
    public class Utils
    {
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(Point point);

        public static Control GetControlAtMousePoint()
        {
            var hwnd = WindowFromPoint(Control.MousePosition);
            var c = Control.FromHandle(hwnd);
            return c;
        }

        public static Control GetControlAtPoint(Point p)
        {
            var hwnd = WindowFromPoint(p);
            var c = Control.FromHandle(hwnd);
            return c;
        }
    }
}
