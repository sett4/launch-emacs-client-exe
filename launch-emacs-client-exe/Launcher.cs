using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace jp.jaro68.emacslauncher
{
    class Launcher
    {
        static void Main(string[] args)
        {
            string targetExecutableName = System.Configuration.ConfigurationManager.AppSettings["targetExecutableName"];
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Process where ExecutablePath like '%"+targetExecutableName+"'");
            bool isRunning = false;

            foreach (ManagementObject service in searcher.Get())
            {
                isRunning = true;
            }

            string executable;
            string executableArgs;
            if (isRunning == false)
            {
                executable = System.Configuration.ConfigurationManager.AppSettings["editor"];
                if (System.IO.File.Exists(executable) == false)
                {
                    executable = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["editorDefault"];
                }
                executableArgs = System.Configuration.ConfigurationManager.AppSettings["editorArgs"];

                if (args.Length == 0)
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(executable, executableArgs);
                    startInfo.UseShellExecute = false;
                    System.Diagnostics.Process.Start(startInfo);

                    return;
                }
            }
            else
            {
                executable = System.Configuration.ConfigurationManager.AppSettings["emacsClient"];
                if (System.IO.File.Exists(executable) == false)
                {
                    executable = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["emacsClientDefault"];
                }
                executableArgs = System.Configuration.ConfigurationManager.AppSettings["emacsClientArgs"];
            }

            if (System.IO.File.Exists(args[0]) == false)
            {
                args = new String[1]{String.Join(" ", args)};
            }

            foreach (string arg in args)
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(executable, executableArgs + " \"" + arg + "\"");
                startInfo.UseShellExecute = false;
                System.Diagnostics.Process.Start(startInfo);
            }
        }
    }
}
