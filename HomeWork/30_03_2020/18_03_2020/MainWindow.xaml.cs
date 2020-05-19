using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using MessageBox = System.Windows.MessageBox;

namespace _18_03_2020
{
    public partial class MainWindow : Window
    {
        private static MainWindow _thisWindow;
        ParameterizedThreadStart PStart;
        Thread t;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static HookProc proc = HookCallback;
        private static IntPtr hook = IntPtr.Zero;
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private IntPtr SetHook(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (wParam == (IntPtr)WM_KEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);

                //if ((Keys)vkCode == Keys.Y)
                //{
                //    Console.WriteLine("{0} change to {1}!", (Keys)vkCode, Keys.N);
                //    lParam = (IntPtr)Keys.N;
                //    //return (IntPtr)1;
                //}

                if (((Keys)vkCode == Keys.F5))
                {
                    _thisWindow.ReadReg();
                    MessageBox.Show("F5");
                    return (IntPtr)1;
                }
                else if (((Keys)vkCode == Keys.Escape))
                {
                    _thisWindow.Close();
                    MessageBox.Show("Escape");
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        public MainWindow()
        {
            InitializeComponent();
            _thisWindow = this;
            PStart = new ParameterizedThreadStart(LoadSubKeys);
            ReadReg();
            hook = SetHook(proc);
        }

        public void ReadReg()
        {
            TW.Items.Clear();
            RegistryKey[] regs = new[]
                    {
                    Registry.ClassesRoot,
                    Registry.CurrentUser,
                    Registry.LocalMachine,
                    Registry.Users,
                    Registry.CurrentConfig
                };
            foreach (var i in regs)
            {
                TreeViewItem tvi = new TreeViewItem();
                tvi.Header = i;
                tvi.IsSelected = false;
                tvi.IsExpanded = false;
                TW.Items.Add(tvi);
                LoadSubKeys(tvi);
                tvi.Expanded += Tvi_Expanded;
            }
        }
        private void LoadSubKeys(object tvi)
        {
            try
            {
                TreeViewItem _tvi = Dispatcher.Invoke(() => tvi) as TreeViewItem;
                RegistryKey key = Dispatcher.Invoke(() => _tvi.Header) as RegistryKey;
                foreach (var name in key.GetSubKeyNames())
                {
                    Dispatcher.Invoke(() =>
                    {
                        TreeViewItem newTvi = new TreeViewItem();
                        newTvi.IsExpanded = false;
                        try
                        {
                            newTvi.Header = key.OpenSubKey(name);
                        }
                        catch
                        {

                        }
                        newTvi.Expanded += Tvi_Expanded;
                        //if (!_tvi.Items.Contains(newTvi))
                        //{
                        //    _tvi.Items.Add(newTvi);
                        //}
                        _tvi.Items.Add(newTvi);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Tvi_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (sender as TreeViewItem);
            foreach (TreeViewItem sub in item.Items)
            {
                Task.Run(() => LoadSubKeys(sub));
            }
            item.Expanded -= Tvi_Expanded;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnhookWindowsHookEx(hook);
            t?.Abort();
        }

        private void TW_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                TreeViewItem tvi = (TreeViewItem)TW.SelectedItem;
                string val = "Name: " + ((RegistryKey)tvi.Header).Name;
                string name = val;
                val += " || Value: " + ((RegistryKey)tvi.Header).GetValue(name, "???");
                LV.Items.Clear();
                LV.Items.Add(val);
            }
            catch
            {

            }
        }
    }
}
