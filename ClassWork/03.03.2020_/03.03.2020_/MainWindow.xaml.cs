using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace nazar
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Library";
            dlg.DefaultExt = ".dll";
            dlg.Filter = "Text documents (.dll)|*.dll"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                LibName.Content = dlg.FileName;
                LoadAssembly();
            }
        }
        private void LoadAssembly()
        {
            AppDomain thirdDomain = AppDomain.CreateDomain("Child domain #2");
            thirdDomain.AssemblyLoad += (s, e) => { Console.WriteLine("Domain loaded"); };
            thirdDomain.DomainUnload += (s, a) => { Console.WriteLine("Domain UNLOADED"); };

            Assembly asm2 = thirdDomain.Load(AssemblyName.GetAssemblyName(LibName.Content.ToString()));

            Module module2 = asm2.GetModule("ViewMessageBox.dll");

            var query = thirdDomain.GetAssemblies()
                                    .SelectMany(t => t.GetTypes())
                                    .Where(t => t.IsClass && t.Namespace == "ViewMessageBox")
                                    ;
            foreach (var item in query)
            {
                LBMInFOO.Items.Add(item.FullName);
                //    if (!item.IsPublic)
                //    {
                //        continue;
                //    }

                //    MemberInfo[] members = item.GetMembers(BindingFlags.Public
                //                                          | BindingFlags.Instance
                //                                          | BindingFlags.InvokeMethod);
                //    foreach (MemberInfo member in members)
                //    {
                //        Console.WriteLine(item.Name + "." + member.Name);
                //    }
                //    Console.WriteLine();
                //}
                //AppDomain.Unload(thirdDomain);
            }
        }
    }
}
