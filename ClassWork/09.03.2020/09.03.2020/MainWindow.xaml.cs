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
using System.Windows.Threading;

namespace _09._03._2020
{
    public partial class MainWindow : Window
    {
        Fib c = new Fib();
        ParameterizedThreadStart threadstart = new ParameterizedThreadStart(FibonachiNums);
        Thread[] threads = new Thread[5];
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(threadstart);
                threads[i].Start();
            }
        }
        public void WListW(string message)
        {
            LW.Items.Add(message);
        }
    }
    public class Fib
    {
        int oldNum = 1;
        int newNum = 1;
        public void FibonachiNums(object o)
        {
            long res;
            while (true)
            {
                res = (long)oldNum + (long)newNum;
                if (res > int.MaxValue) return;
                oldNum = newNum;
                newNum = (int)res;
                Dispatcher.Invoke(() => LW.Items.Add(res));
            }
        }
    }
}
