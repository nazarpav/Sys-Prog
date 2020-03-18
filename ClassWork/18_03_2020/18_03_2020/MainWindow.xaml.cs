using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
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
                tvi.Expanded += Tvi_Expanded;
                foreach (var h in i.GetSubKeyNames())
                {
                    tvi.Items.Add(i.OpenSubKey(h));
                }
                TW.Items.Add(tvi);
            }
        }

        private void Tvi_Expanded(object sender, RoutedEventArgs e)
        {
            foreach (var i in ((sender as TreeViewItem).Header as RegistryKey).GetSubKeyNames())
            {
                TreeViewItem tvi = new TreeViewItem();
                tvi.Header = i;
                tvi.IsSelected = false;
                tvi.Expanded += Tvi_Expanded;
                foreach (var h in ((sender as TreeViewItem).Header as RegistryKey).OpenSubKey(i).GetSubKeyNames())
                {
                    tvi.Items.Add(((sender as TreeViewItem).Header as RegistryKey).OpenSubKey(h));
                }
                (sender as TreeViewItem).Items.Add(tvi);
            }
        }
    }
    public class Param
    {

    }
}
