using System;

namespace Terminal.View
{
    public interface IDialogForm : IView
    {
        string TicketName { set; }
        string CountPeople { set; }

        event EventHandler<EventArgs> EhOk;
        event EventHandler<EventArgs> EhCancel;
    }
}