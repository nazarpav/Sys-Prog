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

namespace _19_03_2020
{
    public class FibonacciCounter
    {
        public int N1 { get; set; }
        public int N2 { get; set; }
        public int EvenCount { get; set; }
        public FibonacciCounter()
        {
            N1 = 1;
            N2 = 1;
            EvenCount = 0;
        }
    }
    public partial class MainWindow : Window
    {
        FibonacciCounter FC;
        const int QUANTITY_ITERATIONS = 10;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CalculateFCNums(object state)
        {
            for (int i = 0; i < QUANTITY_ITERATIONS; i++)
            {
                Monitor.Enter(FC);
                try
                {
                    
                    (state as ListView).Items.Add(FC.EvenCount);
                }
                finally
                {
                    Monitor.Exit(FC);
                }
            }
        }
        private void CalculateFCEvenNums(object state)
        {
            for (int i = 0; i < QUANTITY_ITERATIONS; i++)
            {
                Monitor.Enter(FC);
                try
                {
                    if (FC.N1 % 2 == 0 || FC.N2 % 2 == 0)
                    {
                        ++FC.EvenCount;
                    }
                    (state as ListView).Items.Add(FC.EvenCount);
                }
                finally
                {
                    Monitor.Exit(FC);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FC = new FibonacciCounter();
            N1.Items.Clear();
            N2.Items.Clear();
            EvenCount.Items.Clear();
            ThreadPool.QueueUserWorkItem(CalculateFCNums,N1);
            ThreadPool.QueueUserWorkItem(CalculateFCNums,N2);
            ThreadPool.QueueUserWorkItem(CalculateFCEvenNums,EvenCount);
        }
    }
}
