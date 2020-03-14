using System;
using System.Diagnostics;
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
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using _03._03._2020;

namespace qwe
{
    public partial class MainWindow : Window
    {
        Process[] pr;
        ObservableCollection<Dgrid> dgrids;
        //List<Dgrid> dgrids;
        Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            pr = Process.GetProcesses();
            dgrids = new ObservableCollection<Dgrid>();
            int i=0;
            DG.ItemsSource = dgrids;
            timer = new Timer(UpdateTasks, i, 0, 1000);
        }

        public void UpdateTasks(object o)
        {
            Dispatcher.Invoke(() => pr = Process.GetProcesses());
            //Dispatcher.Invoke(() => dgrids.Clear());
            List<Dgrid> l = new List<Dgrid>();
            foreach (var g in pr)
            {
                bool Flag = false;
                foreach (var h in dgrids)
                {
                    if(h.PID==g.Id)
                    {
                        Flag = true;
                        break;
                    }
                }
                if(!Flag)
                {
                    Dispatcher.Invoke(() => dgrids.Add(new Dgrid(g.ProcessName,g.Id,g.MachineName,g.PagedMemorySize)));
                }
            }
            List<int> delElem= new List<int>();
            foreach (var g in dgrids)
            {
                bool Flag = false;
                foreach (var h in pr)
                {
                    if (h.Id == g.PID)
                    {
                        Flag = true;
                        break;
                    }
                }
                if (!Flag)
                {
                    delElem.Add(g.PID);
                }
            }
            foreach (var r in delElem)
            {
                Dispatcher.Invoke(() => dgrids.Remove( dgrids.Where(q=>q.PID==r).FirstOrDefault()));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in DG.SelectedItems)
            {
                pr.Where(q => q.Id == ((Dgrid)s).PID).FirstOrDefault().Kill();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var s in DG.SelectedItems)
            {
                Window1 w = new Window1(pr.Where(q => q.Id == ((Dgrid)s).PID).FirstOrDefault());
                w.Show();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process calcProc = Process.Start(ProcesPath.Text);
            if (calcProc.WaitForExit(2000))
            {
                MessageBox.Show("Process is exited...");
            }
        }
    }
    public class Dgrid
    {
        public Dgrid(string Name, int PID, string Machine,int PagedMemorySize)
        {
            this.Name = Name;
            this.PID = PID;
            this.Machine = Machine;
            this.PagedMemorySize = PagedMemorySize;
        }
        public string Name { get; set; }
        public int PID { get; set; }
        public string Machine { get; set; }
        public int PagedMemorySize { get; set; }
    }
}
