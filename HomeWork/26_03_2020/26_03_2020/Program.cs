using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _26_03_2020
{
     static class Program
    {
        static unsafe void Main(string[] args)
        {
            UnsafeHomework @unsafe = new UnsafeHomework();
            ////////TASK_1
            //@unsafe.Task_1();

            ///////////TASK_2
            //fixed (int* arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            //{
            //    int size = 0;
            //    PrintArr(@unsafe.Task_2(arr, 9, out size), size);
            //}

            //////TASK_3
            //fixed (int* arr = new int[] { 2,2,6,6,3,3,3,1,7,9 })
            //{
            //    int size = 0;
            //    PrintArr(@unsafe.Task_3(arr, 10, out size), size);
            //}

            //////TASK_4
            fixed (int* arr1 = new int[] { 2, 0, 6, 0, 3, 0, 3, 4, 4, 4 })
            {
                fixed (int* arr2 = new int[] { 2, 2, 6, 6, 3, 3, 3, 6, 4, 9 })
                {
                    int size = 0;
                    PrintArr(@unsafe.Task_4(arr1, 10, arr2, 10, out size), size);
                }
            }
            Console.ReadKey();
        }
        public static unsafe void PrintArr(int* arr, int size)
        {
            Console.WriteLine("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(*(arr + i));
            }
        }
    }
}
