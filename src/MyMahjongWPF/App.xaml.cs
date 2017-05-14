using MyMahjongWPF.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyMahjongWPF
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// メイン画面インスタンス
        /// </summary>
        private View.MainWindow mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            mainWindow = new View.MainWindow();
            mainWindow.Show();
        }
    }
}
