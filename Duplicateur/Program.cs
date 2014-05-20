using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Duplicateur
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Usb tempUsb = new Usb('E');
            tempUsb.EjectDrive();
            //tempUsb.FormatDrive("test", "EXFAT");
            Application.Run(new Form1());
        }
    }
}
