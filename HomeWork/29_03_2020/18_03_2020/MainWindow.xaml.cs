using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _18_03_2020
{
    public partial class MainWindow : Window
    {
        ParameterizedThreadStart PStart;
        Thread t;
        public MainWindow()
        {
            InitializeComponent();
            PStart = new ParameterizedThreadStart(LoadSubKeys);
            ReadReg();
        }
        public void ReadReg()
        {
            RegistryKey[] regs = new[]
                    {
                    Registry.ClassesRoot,
                    Registry.CurrentUser,
                    Registry.LocalMachine,
                    Registry.Users,
                    Registry.CurrentConfig
                };
            foreach (var i in regs)
            {
                TreeViewItem tvi = new TreeViewItem();
                tvi.Header = i;
                tvi.IsSelected = false;
                tvi.IsExpanded = false;
                TW.Items.Add(tvi);
                LoadSubKeys(tvi);
                tvi.Expanded += Tvi_Expanded;
            }
        }
        private void LoadSubKeys(object tvi)
        {
            TreeViewItem _tvi = Dispatcher.Invoke(() => tvi) as TreeViewItem;
            RegistryKey key = Dispatcher.Invoke(() => _tvi.Header) as RegistryKey;
            foreach (var name in key.GetSubKeyNames())
            {
                Dispatcher.Invoke(() =>
                {
                    TreeViewItem newTvi = new TreeViewItem();
                    newTvi.IsExpanded = false;
                    newTvi.Header = key.OpenSubKey(name);
                    //newTvi.Expanded += Tvi_Expanded;
                    _tvi.Items.Add(newTvi);
                });
            }
            try
            {
            }
            catch
            {
            }

        }
        private void Tvi_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (sender as TreeViewItem);
            foreach (TreeViewItem sub in item.Items)
            {
                t?.Abort();
                t = new Thread(PStart);
                t.SetApartmentState(ApartmentState.STA);
                t.Start(sub);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t?.Abort();
        }
    }
}
