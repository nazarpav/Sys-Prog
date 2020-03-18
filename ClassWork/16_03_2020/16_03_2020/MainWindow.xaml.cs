using System;
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

namespace _16_03_2020
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestUnsafe();
        }
        unsafe void TestUnsafe()
        {
            const int SIZE = 5;
            int[] arr = new int[] { 1, 2, 3, 4, 5 };
            fixed (int* PtrArr = arr)
            {
                ShowArr(PtrArr, SIZE);
                MConsole.Items.Add("________________________");
                ShowArr(InitArr(PtrArr, SIZE), SIZE);
                MConsole.Items.Add("________________________");
                ShowArr(ReversArr(PtrArr, SIZE), SIZE);
                MConsole.Items.Add("________________________");
                ShowArr(Task2(arr, SIZE), SIZE);
            }
        }
        unsafe void ShowArr(int* arrPtr, int size)
        {
            for (int i = 0; i < size; i++)
            {
                MConsole.Items.Add(*(arrPtr + i));
            }
        }
        unsafe void Task_1()
        {
            const int SIZE=5;
            int* arr1 = new int[size];
        }
    }
}
