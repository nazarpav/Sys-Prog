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

namespace _11_02_2020
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathF;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TBPath.Text = folderBrowserDialog.SelectedPath;
                pathF = TBPath.Text;
            }
        }
        private string[] GetDataFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            Parallel.ForEach<string>(GetDataFiles(pathF),LoadLB);
        }
        private void LoadLB(string data)
        {
            Dispatcher.BeginInvoke(new Action(() => LB.Items.Add(data)));
        }
        /*static void Main(string[] args)
        {
            ParallelLoopResult result = Parallel.ForEach<int>(
                new List<int>() { 1, 3, 5, 8 },
                Factorial);

            Console.ReadLine();
        }
        static void Factorial(int x)
        {
            int result = 1;

            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }
            Console.WriteLine($"Task executing {Task.CurrentId}");
            Console.WriteLine($"Factorial {x} = {result}");
            Thread.Sleep(3000);
        }*/
    }
}
