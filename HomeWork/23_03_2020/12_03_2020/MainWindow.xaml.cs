using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        Task task1;
        CancellationTokenSource cts;
        CancellationToken token;
        public MainWindow()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            token = cts.Token;
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
            try
            {
                cts = new CancellationTokenSource();
                token = cts.Token;
                int q = 0;
                if (!int.TryParse(QuantityCopies.Text, out q))
                {
                    System.Windows.MessageBox.Show("Error QuantityCopies type");
                    return;
                }
                ProgBar.Maximum = q;
                task1 = Task.Run(() => FCopy(), cts.Token);
            }
            catch
            {
                System.Windows.MessageBox.Show("Eror _step1!");
            }
        }
        private void FCopy()
        {
            for (int i = 0; i < Dispatcher.Invoke(() => (int)ProgBar.Maximum); i++)
            {
                if (Dispatcher.Invoke(() => token.IsCancellationRequested))
                {
                    Console.WriteLine("Stoped");
                    Dispatcher.Invoke(() => ProgBar.Value = 0);
                    return;
                }
                Thread.Sleep(70);
                Dispatcher.Invoke(() => ProgBar.Value++);
                try
                {
                    File.Copy(Dispatcher.Invoke(() => From.Text), Dispatcher.Invoke(() => To.Text) + Revers_(GetFileName()));
                }
                catch
                {
                    Dispatcher.Invoke(() => ProgBar.Value = 0);
                }
            }
            EndParallel();
        }
        private void EndParallel()
        {
            System.Windows.MessageBox.Show("Success!");
            Dispatcher.Invoke(() => ProgBar.Value = 0);
        }
        private string Revers_(string s)
        {
            string str = string.Empty;
            foreach (var i in s.Reverse())
            {
                str += i;
            }
            return str;
        }
        private string GetFileName()
        {
            string FName = string.Empty;
            Random rnd = new Random();
            foreach (var i in Dispatcher.Invoke(() => From.Text).Reverse())
            {
                FName += i;
                if (i == '.')
                {
                    FName += rnd.Next(int.MinValue, int.MaxValue);
                }
                if (i == '/' || i.ToString() == @"\")
                {
                    return FName;
                }
            }
            return FName;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (task1?.Status == TaskStatus.Running)
            {
                cts.Cancel();
                cts.Dispose();
                Dispatcher.Invoke(() => ProgBar.Value = 0);
            }
        }
    }
}
