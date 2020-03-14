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

namespace _04._03._2020
{
    public partial class MainWindow : Window
    {
        Thread thread1;
        Thread thread2;
        Thread thread3;
        public MainWindow()
        {
            InitializeComponent();

            ParameterizedThreadStart T1 = new ParameterizedThreadStart(FibonachiNums);
            ParameterizedThreadStart T2 = new ParameterizedThreadStart(FacktorialNums);
            ParameterizedThreadStart T3 = new ParameterizedThreadStart(SimplNums);

            thread1 = new Thread(T1);
            thread1.Start(new Param("Fibonachi",int.MaxValue,100));
            thread2 = new Thread(T2);
            thread2.Start(new Param("Facktorial", int.MaxValue, 100));
            //thread3 = new Thread(T3);
            //thread1.Start(new Param("SimpleNums", int.MaxValue, 100));

        }
        public void FibonachiNums(object o)
        {
            int count = 0;
            int oldNum=1;
            int newNum=1;
            long res;
            while (true)
            {
                Thread.Sleep(((Param)o).Delay);
                res = (long)oldNum + (long)newNum;
                if (res > int.MaxValue) return;
                oldNum = newNum;
                newNum = (int)res;
                Dispatcher.Invoke(() => Fib.Items.Add(res));
                Dispatcher.Invoke(() => Fib.Items.Add(count++));
            }
        }
        public void FacktorialNums(object o)
        {
            long res;
            for (int i = 1; i < int.MaxValue; i++)
            {
                Thread.Sleep(((Param)o).Delay);
                res = 1;
                for (int j = 1; j <= i; j++)
                {
                    res *= j;
                }
                if (res > int.MaxValue) return;
                Dispatcher.Invoke(() => Fakt.Items.Add(res));
            }
        }
        public void SimplNums(object o)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (thread1 == null) return;
            if(thread1.ThreadState == ThreadState.Suspended)
            {
                b1.Content = "Start";
                thread1.Resume();
            }
            else if (thread1.ThreadState == ThreadState.WaitSleepJoin)
            {
                b1.Content = "Pause";
                thread1.Suspend();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (thread1 == null) return;
            if (thread1.ThreadState == ThreadState.Suspended)
            {
                b2.Content = "Start";
                thread2.Resume();
            }
            else if (thread1.ThreadState == ThreadState.WaitSleepJoin)
            {
                b2.Content = "Pause";
                thread2.Suspend();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (thread1 == null) return;
            if (thread1.ThreadState == ThreadState.Suspended)
            {
                b3.Content = "Start";
                thread3.Resume();
            }
            else if (thread1.ThreadState == ThreadState.WaitSleepJoin)
            {
                b3.Content = "Pause";
                thread3.Suspend();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            thread1.Abort();
            thread2.Abort();
            thread3.Abort();
        }
    }
    class Param
    {
        public Param(string n,int q,int d)
        {
            Name = n;
            Quantity = 1;
            Delay = d;
        }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Delay { get; set; }
    }
}
