using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Properties.Settings.Default.UserID == 0)
            {
                Application.Run(new Form1());
            }
            else if (Properties.Settings.Default.IsEmployee)
            {
                Application.Run(new EmployeeManagmentForm(Properties.Settings.Default.UserID));
            }
            else
            {
                Application.Run(new UserManagmentForm(Properties.Settings.Default.UserID));
            }
        }
    }
}
