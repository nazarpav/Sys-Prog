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

namespace nazar123
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process[] pr;
        List<Dgrid> dgrids;
        public MainWindow()
        {
            InitializeComponent();
            pr = Process.GetProcesses();
            dgrids = new List<Dgrid>();

            //Timer timer = new Timer(UpdateTasks, num, 0, 2000);
            UpdateTasks();
        }

        public void UpdateTasks()
        {
            pr = Process.GetProcesses();
            dgrids.Clear();
            foreach (var i in pr)
            {
                //Dispatcher.Invoke(() => DGrid.Items.Add(new Dgrid { 
                //Name = i.ProcessName,
                //PID = i.Id,
                //Machine = i.MachineName
                //}));
                dgrids.Add(new Dgrid()
                {
                    Name = i.ProcessName,
                    PID = i.Id,
                    Machine = i.MachineName
                });
            }
            DGrid_.ItemsSource = dgrids;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //pr.Where(q => q.Id == int.Parse(Combo_box.SelectedItem.ToString()));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
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
        public string Name;
        public int PID;
        public string Machine;
    }
}
