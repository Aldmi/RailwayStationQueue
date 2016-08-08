using System;
using System.Drawing;

namespace Terminal.View
{
    public interface IMainForm : IView
    {
        bool IsConnect { set; }
        bool IsRunDataExchange{ set; }
        string ErrorString {set; }

        event EventHandler<EventArgs> EhGetInfoVilage;
        event EventHandler<EventArgs> EhGetInfoLongRoad;
    }
}