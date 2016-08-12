using System;
using System.Drawing;


namespace Terminal.View
{
    public interface IMainForm : IView
    {
        Color BackgroundColorDataGrid { set; }
        bool BattonEnable{ set; }

        string ErrorString {set; }

        event EventHandler<EventArgs> EhGetInfoVilage;
        event EventHandler<EventArgs> EhGetInfoLongRoad;
    }
}