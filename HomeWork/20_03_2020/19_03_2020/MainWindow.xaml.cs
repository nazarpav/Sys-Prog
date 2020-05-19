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

namespace HWThreadsResetEvents
{
    public partial class MainWindow : Window
    {
        public Thread MainThread;
        public Thread FibonachiThread;
        public Thread FactorialThread;
        public Thread PreliminarThread;
        public ManualResetEvent resetEvent1 = new ManualResetEvent(false);
        public ManualResetEvent resetEvent2 = new ManualResetEvent(false);
        public ManualResetEvent resetEvent3 = new ManualResetEvent(false);
        public ManualResetEvent waitEvent1 = new ManualResetEvent(true);
        public ManualResetEvent waitEvent2 = new ManualResetEvent(true);
        public ManualResetEvent waitEvent3 = new ManualResetEvent(true);
        public MainWindow()
        {
            InitializeComponent();
            FibonachiThread = new Thread(FibonachiLoop);
            FactorialThread = new Thread(FactorialLoop);
            PreliminarThread = new Thread(PreliminarLoop);
            MainThread = new Thread(MainLoop);
            MainThread.Start();
            FibonachiThread.Start();
            FactorialThread.Start();
            PreliminarThread.Start();
        }
        public void MainLoop()
        {
            for (int i = 0; i < 3; i++)
            {
                Dispatcher.Invoke(() => Threads.Items.Add("WaitForAll"));
                waitEvent1.Reset();
                waitEvent3.Reset();
                waitEvent2.Reset();
                WaitHandle.WaitAll(new WaitHandle[] { resetEvent1, resetEvent2, resetEvent3 });
                Dispatcher.Invoke(() => Threads.Items.Add("Set"));
                resetEvent1.Reset();
                resetEvent2.Reset();
                resetEvent3.Reset();
                waitEvent1.Set();
                waitEvent2.Set();
                waitEvent3.Set();
            }
        }
        void FibonachiLoop(object name)
        {
            List<int> partSize = new List<int> { 2, 4, 6, 0 };
            int part = 0;
            int count = 0;

            int num = 1;
            int num2 = 1;
            while (true)
            {
                int r = num + num2;
                num = num2;
                num2 = r;
                if (r < 0)
                {
                    MessageBox.Show("Success!");
                    return;
                }
                Dispatcher.Invoke(() => FibonachiList.Items.Add(r));
                count++;
                try
                {
                    if (count >= partSize[part])
                    {
                        count = 0;
                        part++;
                        resetEvent1.Set();
                        waitEvent1.WaitOne();
                    }
                }
                catch
                {

                }
                Thread.Sleep(150);

            }
        }

        void FactorialLoop(object name)
        {
            int part = 0;
            int count = 0;
            List<int> partSize = new List<int> { 1, 2, 3, 0 };
            List<int> list = new List<int>() { 1, 2 };
            while (true)
            {
                int r = 1;
                foreach (var i in list)
                {
                    r = r * i;
                }
                if(r<0)
                {
                    return;
                }
                list.Add(r);
                if (r.ToString() == "0")
                {
                    list.Clear();
                    list.Add(2);
                    list.Add(3);
                }
                Dispatcher.Invoke(() => FactorialList.Items.Add(r));
                count++;
                try
                {
                    if (count >= partSize[part])
                    {
                        count = 0;
                        part++;
                        resetEvent2.Set();
                        waitEvent2.WaitOne();
                    }
                }
                catch { }
                Thread.Sleep(100);

            }
        }
        void PreliminarLoop(object name)
        {
            int part = 0;
            int count = 0;
            List<int> partSize = new List<int> { 3, 8, 12, 0 };
            List<int> primes = new List<int>();
            primes.Add(2);
            int curTest = 3;
            while (true)
            {
                int sqrt = (int)Math.Sqrt(curTest);
                bool isPrime = true;
                for (int i = 0; i < primes.Count && primes[i] <= sqrt; ++i)
                {
                    if (curTest % primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (part >3)
                {
                    return;
                }
                if (isPrime)
                {
                    primes.Add(curTest);
                    Dispatcher.Invoke(() => SimpleList.Items.Add(curTest));
                    count++;
                    try
                    {
                        if (count >= partSize[part])
                        {
                            count = 0;
                            part++;
                            resetEvent3.Set();
                            waitEvent3.WaitOne();
                        }
                    }
                    catch { }
                }
                curTest += 2;
                Thread.Sleep(50);
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            MainThread.Abort();
            FibonachiThread.Abort();
            FactorialThread.Abort();
            PreliminarThread.Abort();
        }
    }
}
