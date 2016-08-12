using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminal.Presenter;
using Terminal.View;

namespace Terminal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var presenter = new MainPresenter(new MainForm()))
            {
                presenter.Run();
            }
        }
    }
}
