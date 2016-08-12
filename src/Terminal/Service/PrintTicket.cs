using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

namespace Terminal.Service
{
    public class PrintTicket : IDisposable
    {
        #region Field

        private readonly PrintDocument _printDocument;

        private string _ticketName;
        private string _countPeople;
        private DateTime _dateAdded;
        #endregion




        #region ctor

        public PrintTicket()
        {
            var printersNames = PrinterSettings.InstalledPrinters;
            string printerName = printersNames[1];
            PrinterSettings ps = new PrinterSettings {PrinterName = printerName};

            _printDocument = new PrintDocument {PrinterSettings = ps};
            _printDocument.PrintPage += Pd_PrintPage;
        }

        #endregion




        #region Event

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //ПЕЧАТЬ ЛОГОТИПА
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Picture", "RZD_logo.jpg");
            if (File.Exists(filePath))
                e.Graphics.DrawImage(Image.FromFile(filePath), 265, 15);

            e.Graphics.DrawLine(new Pen(Color.Black), 20, 168, 805, 168);

            //ПЕЧАТЬ ТЕКСТА
            string printText = $"{_ticketName}\r\n";
            Font printFont = new Font("Times New Roman", 30, FontStyle.Regular, GraphicsUnit.Millimeter);
            e.Graphics.DrawString(printText, printFont, Brushes.Black, 270, 225);

            printText =$"перед вами {_countPeople} чел.\r\n";
            printFont = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Millimeter);
            e.Graphics.DrawString(printText, printFont, Brushes.Black, 218, 375);

            printText = "\r\n \r\n \r\n";
            printText += $"{_dateAdded.ToString("T")}                                                               {_dateAdded.ToString("d")}";
            printFont = new Font("Times New Roman", 8, FontStyle.Regular, GraphicsUnit.Millimeter);
            e.Graphics.DrawString(printText, printFont, Brushes.Black, 30, 500);
        }

        #endregion




        #region Methode

        public void Print(string ticketName, string countPeople, DateTime dateAdded)
        {
            _ticketName = ticketName;
            _countPeople = countPeople;
            _dateAdded = dateAdded;

            _printDocument.Print();
        }

        #endregion




        #region IDisposable

        public void Dispose()
        {
            _printDocument?.Dispose();
        }

        #endregion
    }
}