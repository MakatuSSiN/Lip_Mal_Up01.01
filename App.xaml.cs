using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.utilities;
using WpfApp2.viewModel;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        Navigation mainVM = new Navigation();
        protected override void OnStartup(StartupEventArgs e)
        {
            FileManager.create_json();
            FileManager.create_images_folder();
            // при запуске
            base.OnStartup(e);
            new MainWindow() { DataContext = mainVM }.Show();
        }
    }
}
