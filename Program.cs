using EasyAccounting.form;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EasyAccounting
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
            Application.Run(new EasyAccounting());

        }
    }
}