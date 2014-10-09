using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mozu.Api.Contracts.AppDev;
using Mozu.Api.Security;
using Application = System.Windows.Forms.Application;

namespace MozuOrderImport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppAuthenticator.Initialize(new AppAuthInfo { ApplicationId = "a9f92fd992674bee8142a35501369335", SharedSecret = "be48336d7ee84bfca0c8a35501369335" }, "https://home.mozu.com");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
