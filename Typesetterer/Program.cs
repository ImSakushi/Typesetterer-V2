using System;
using System.Windows.Forms;

namespace Typesetterer
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (EntryForm entryForm = new EntryForm())
            {
                Application.Run(entryForm);
            }
        }
    }
}