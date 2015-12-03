﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace genturfahi
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static string[] CommandLineArgs { get; private set; }

        public void Startup_App(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0)
                return;
            CommandLineArgs = e.Args;
        }


    }
}
