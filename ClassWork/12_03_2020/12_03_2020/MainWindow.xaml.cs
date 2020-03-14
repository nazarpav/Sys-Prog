using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _12_03_2020
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                From.Text = dlg.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                To.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int q=0;
            if(!int.TryParse(QuantityCopies.Text,out q))
            {
                System.Windows.MessageBox.Show("Error");
            }
            Parallel.For(0, q, FCopy);
            ProgBar.Maximum = q;
        }
        private void FCopy(int x)
        {
            File.Copy(Dispatcher.Invoke(()=> From.Text), Dispatcher.Invoke(() => To.Text) + Revers_(GetFileName(x)));
        }
        private string Revers_(string s)
        {
            string str=string.Empty;
            foreach (var i in s.Reverse())
            {
                str += i;
            }
            return str;
        }
        private string GetFileName(int x)
        {
            Dispatcher.BeginInvoke(new Action(() => ProgBar.Value++));
            string FName= string.Empty;
            foreach (var i in Dispatcher.Invoke(() => From.Text).Reverse())
            {
                FName += i;
                if(i == '.')
                {
                    FName += x.ToString();
                }
                if (i=='/'||i.ToString()==@"\")
                {
                    return FName;
                }
            }
            return FName;
        }
    }
}
