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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _05._03._2020
{
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token;
        public MainWindow()
        {
            InitializeComponent();
            token = cts.Token;
            token.Register(StopedCrypt);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                TBFileName.Text = dlg.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //start
            ThreadPool.QueueUserWorkItem(Start, cts.Token);
        }
        private void Start(object obj)
        {
            try
            {
                if (Dispatcher.Invoke(() => Encrypt.IsChecked) == true)
                {
                    EncryptDectyptFile(Dispatcher.Invoke(() => TBFileName.Text), int.Parse(Dispatcher.Invoke(() => Key.Text)), true, (CancellationToken)obj);
                }
                else
                {
                    EncryptDectyptFile(Dispatcher.Invoke(() => TBFileName.Text), int.Parse(Dispatcher.Invoke(() => Key.Text)), false, (CancellationToken)obj);
                }
            }
            catch
            {
                MessageBox.Show("Eror");
            }
        }
        private string EncryptDecrypt(string Text, int Key, CancellationToken token)
        {
            StringBuilder szInputStringBuild = new StringBuilder(Text);
            StringBuilder szOutStringBuild = new StringBuilder(Text.Length);
            char Textch;
            Dispatcher.Invoke(() => PRB.Maximum = Text.Length - 1);
            for (int i = 0; i < Text.Length; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return null;
                }
                Dispatcher.Invoke(() => PRB.Value = i);
                Textch = szInputStringBuild[i];
                Textch = (char)(Textch ^ Key);
                szOutStringBuild.Append(Textch);
                Thread.Sleep(100);
            }
            MessageBox.Show("Success");
            Dispatcher.Invoke(() => PRB.Value = 0);
            return szOutStringBuild.ToString();
        }
        private void EncryptDectyptFile(string path, int key, bool isEncrypt, CancellationToken token)
        {
            string newFile = string.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                newFile = EncryptDecrypt(sr.ReadToEnd(), key, token);
            }
            using (StreamWriter sw = new StreamWriter(System.IO.Path.GetDirectoryName(path) + (isEncrypt ? "\\Encrypt" : "\\Decrypt") + rnd.Next(int.MinValue, int.MaxValue) + ".txt", false, System.Text.Encoding.Default))
            {
                if (newFile != null && newFile != "")
                {
                    sw.WriteLine(newFile);
                }
            }
        }
        private void StopedCrypt()
        {
            cts = new CancellationTokenSource();
            token = cts.Token;
            token.Register(StopedCrypt);
            Dispatcher.Invoke(() => PRB.Value = 0);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //cancel
            cts.Cancel();
        }
    }
}
