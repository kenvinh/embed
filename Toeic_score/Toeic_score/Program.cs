using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Toeic_score
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /* 
             * Init the main code 
             */
            if (Directory.Exists(SystemProperties.TestDataFolderName) != true)
            {
                Directory.CreateDirectory(SystemProperties.TestDataFolderName);
            }

            if (Directory.Exists(SystemProperties.UserFolderName) != true)
            {
                Directory.CreateDirectory(SystemProperties.UserFolderName);
            }

            /* 
             * Starting GUI 
             */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
