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
            thread1.Start(new Param("Fibonachi", int.MaxValue, 500));
            thread2 = new Thread(T2);
            thread2.Start(new Param("Facktorial", int.MaxValue, 500));
            thread3 = new Thread(T3);
            thread3.Start(new Param("SimpleNums", int.MaxValue, 500));

        }
        public void FibonachiNums(object o)
        {
            int oldNum = 1;
            int newNum = 1;
            long res;
            while (true)
            {
                Thread.Sleep(((Param)o).Delay);
                res = (long)oldNum + (long)newNum;
                if (res > int.MaxValue) return;
                oldNum = newNum;
                newNum = (int)res;
                Dispatcher.Invoke(() => Fib.Items.Add(res));
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
        /*
         bool prost = true;
            Console.WriteLine("Введите число\n");
            int n =int.Parse(Console.ReadLine());
            for (int i = 2; i <= n / 2; i++)
            {
                if (n % i == 0)
                {
                    prost = false;
                    break;
                }
            }
            if (prost)
            {
                Console.WriteLine("Число простое");
            }
            else
            {
                Console.WriteLine("Число не простое");
            }
             */
        public void SimplNums(object o)
        {
            bool flag = false;
            for (long i = 2; i <= int.MaxValue; i++)
            {
                Thread.Sleep(((Param)o).Delay);
                flag = false;
                for (int h = 2; h <= Math.Sqrt(i); h++)
                {
                    if(i%h==0)
                    {
                        flag = true;
                        break;
                    }
                }
                if(!flag)
                {
                    Dispatcher.Invoke(() => Simpl.Items.Add(i));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (thread1 == null) return;
                if (thread1.ThreadState == ThreadState.Suspended)
                {
                    b1.Content = "Pause";
                    thread1.Resume();
                }
                else if (thread1.ThreadState == ThreadState.WaitSleepJoin)
                {
                    b1.Content = "Start";
                    thread1.Suspend();
                }
            }
            catch
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (thread2 == null) return;
                if (thread2.ThreadState == ThreadState.Suspended)
                {
                    b2.Content = "Pause";
                    thread2.Resume();
                }
                else if (thread1.ThreadState == ThreadState.WaitSleepJoin)
                {
                    b2.Content = "Start";
                    thread2.Suspend();
                }
            }
            catch
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (thread3 == null) return;
                if (thread3.ThreadState == ThreadState.Suspended)
                {
                    b3.Content = "Pause";
                    thread3.Resume();
                }
                else if (thread1.ThreadState == ThreadState.WaitSleepJoin)
                {
                    b3.Content = "Start";
                    thread3.Suspend();
                }
            }
            catch
            {

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (thread1 != null && thread1.ThreadState != ThreadState.Suspended)
                thread1?.Abort();
            if (thread2 != null&& thread2.ThreadState!= ThreadState.Suspended)
                thread2?.Abort();
            if (thread3 != null && thread3.ThreadState != ThreadState.Suspended)
                thread3?.Abort();
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
