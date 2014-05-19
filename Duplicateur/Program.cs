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
            Usb.EjectDrive('E');
            //Usb.FormatDrive('E', "test", "EXFAT");
            Application.Run(new Form1());
        }
    }
}
