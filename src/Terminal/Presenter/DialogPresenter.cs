using Terminal.View;

namespace Terminal.Presenter
{
    public enum Act
    {
        Ok, Cancel, Undefined
    }

    public class DialogPresenter : IPresenter
    {
        #region field

        private readonly IDialogForm _view;

        #endregion




        #region prop

        public Act Act { get; set; }

        #endregion




        #region ctor

        public DialogPresenter(IDialogForm view, string ticketName, string countPeople)
        {
            _view = view;

            _view.EhCancel += _view_EhCancel;
            _view.EhOk += _view_EhOk;

            _view.TicketName = ticketName;
            _view.CountPeople = countPeople;
        }

        #endregion




        #region EventHandler

        private void _view_EhOk(object sender, System.EventArgs e)
        {
            Act= Act.Ok;
            _view.Close();
        }

        private void _view_EhCancel(object sender, System.EventArgs e)
        {
            Act = Act.Cancel;
            _view.Close();
        }

        #endregion





        #region Methode

        public void Run()
        {
            _view.Show();
        }

        #endregion
    }
}