using System;
using System.Collections.Generic;
using System.Drawing;

namespace SaltCharts
{
    interface IMapItem
    {
        Point GetLocation();
        Image GetImage();
    }
}
