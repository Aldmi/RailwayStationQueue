using System;
using System.Drawing;

namespace Terminal.View
{
    public interface IMainForm : IView
    {
        bool IsConnect { set; }
        bool IsRunDataExchange{ set; }

        event EventHandler<EventArgs> EhGetInfoVilage;
        event EventHandler<EventArgs> EhGetInfoLong;
    }
}