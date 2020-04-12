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
            try
            {
                TreeViewItem _tvi = Dispatcher.Invoke(() => tvi) as TreeViewItem;
                RegistryKey key = Dispatcher.Invoke(() => _tvi.Header) as RegistryKey;
                foreach (var name in key.GetSubKeyNames())
                {
                    Dispatcher.Invoke(() =>
                    {
                        TreeViewItem newTvi = new TreeViewItem();
                        newTvi.IsExpanded = false;
                        try
                        {
                            newTvi.Header = key.OpenSubKey(name);
                        }
                        catch
                        {

                        }
                        newTvi.Expanded += Tvi_Expanded;
                        //if (!_tvi.Items.Contains(newTvi))
                        //{
                        //    _tvi.Items.Add(newTvi);
                        //}
                        _tvi.Items.Add(newTvi);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Tvi_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (sender as TreeViewItem);
            foreach (TreeViewItem sub in item.Items)
            {
                Task.Run(() => LoadSubKeys(sub));
            }
            item.Expanded -= Tvi_Expanded;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t?.Abort();
        }

        private void TW_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tvi = (TreeViewItem)TW.SelectedItem;
            string val = "Name: " + ((RegistryKey)tvi.Header).Name;
            string name = val;
            val += " || Value: " + ((RegistryKey)tvi.Header).GetValue(name, "???");
            LV.Items.Clear();
            LV.Items.Add(val);
        }
    }
}
