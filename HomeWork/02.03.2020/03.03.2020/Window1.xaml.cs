using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace _03._03._2020
{
    public partial class Window1 : Window
    {
        public Window1(Process pr)
        {
            InitializeComponent();
            LoadLBox(pr);
        }
        private void LoadLBox(Process pr)
        {
            try
            {
                LBox.Items.Add("PID = "+pr.Id);
            }
            catch
            {
                LBox.Items.Add("PID = " + "??????");
            }
            try
            {
                LBox.Items.Add("Name = "+pr.ProcessName);
            }
            catch
            {
                LBox.Items.Add("Name = " + "??????");
            }
            try
            {
                LBox.Items.Add("MainWindowTitle = " + pr.MainWindowTitle);
            }
            catch
            {
                LBox.Items.Add("MainWindowTitle = " + "??????");
            }
            try
            {
                LBox.Items.Add("StartInfo = " + pr.StartInfo);
            }
            catch
            {
                LBox.Items.Add("StartInfo = " + "??????");
            }
            try
            {
                LBox.Items.Add("BasePriority = " + pr.BasePriority);
            }
            catch
            {
                LBox.Items.Add("BasePriority = " + "??????");
            }
            try
            {
                LBox.Items.Add("HandleCount = " + pr.HandleCount);
            }
            catch
            {
                LBox.Items.Add("HandleCount = " + "??????");
            }
            try
            {
                LBox.Items.Add("StartInfo = " + pr.StartInfo);
            }
            catch
            {
                LBox.Items.Add("StartInfo = " + "??????");
            }
            try
            {
                LBox.Items.Add("VirtualMemorySize = " + pr.VirtualMemorySize);
            }
            catch
            {
                LBox.Items.Add("VirtualMemorySize = " + "??????");
            }
            try
            {
                LBox.Items.Add("PrivateMemorySize = " + pr.PrivateMemorySize);
            }
            catch
            {
                LBox.Items.Add("PrivateMemorySize = " + "??????");
            }
            try
            {
                LBox.Items.Add("SessionId = " + pr.SessionId);
            }
            catch
            {
                LBox.Items.Add("SessionId = " + "??????");
            }
            try
            {
                LBox.Items.Add("StartTime = " + pr.StartTime);
            }
            catch
            {
                LBox.Items.Add("StartTime = " + "??????");
            }
        }
    }
}
