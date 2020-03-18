using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _26_03_2020
{
    class UnsafeHomework
    {
        public unsafe void Task_1()
        {
            const int SIZE = 2;
            int[] arr1 = new int[SIZE];
            int[] arr2 = new int[SIZE];
            for (int i = 0; i < arr1.Length; i++)
            {
            eeeem:
                try
                {
                    arr1[i] = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Errror!!!");
                    goto eeeem;
                    //////i--;
                }
            }
            for (int i = 0; i < arr2.Length; i++)
            {
            eeeem2:
                try
                {
                    arr2[i] = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Errror!!!");
                    goto eeeem2;
                    //////i--;
                }
            }
            int* arr3 = stackalloc int[arr1.Length + arr2.Length];
            for (int i = 0; i < arr1.Length; i++)
                *(arr3 + i) = arr1[i];
            for (int i = 0; i < arr2.Length; i++)
                *(arr3 + i + arr1.Length) = arr2[i];
            Console.WriteLine("Result => VVV -----------------------------");
            for (var i = 0; i < arr1.Length + arr2.Length; i++)
            {
                Console.WriteLine(*(arr3 + i));
            }
        }
        public unsafe int* Task_2(int* arr, int size, out int sizeRezArr)
        {
            int count = 0;
            for (int i = 0; i < size; i++)
            {
                if (*(arr + i) % 2 == 0)
                {
                    ++count;
                }
            }
            sizeRezArr = count;
            int* arrRes = stackalloc int[count];
            count = 0;
            for (int i = 0; i < size; i++)
            {
                if (*(arr + i) % 2 == 0)
                {
                    *(arrRes + count++) = *(arr + i);
                }
            }
            Console.WriteLine("Task_2 Array in Method=> VVV");
            Program.PrintArr(arrRes, sizeRezArr);
            return arrRes;
        }
        public unsafe int* Task_3(int* arr, int size, out int sizeRezArr)//2,2,6,6,3,3,3,1,7,9
        {
            int count = 0;
            for (int i = 0; i < size; i++)
            {
                int LCount = 0;
                for (int g = 0; g < size; g++)
                {
                    if (i != g && *(arr + i) == *(arr + g))
                    {
                        LCount++;
                    }
                }
                if (LCount == 0)
                {
                    count++;
                }
            }
            sizeRezArr = count;
            int* arrRes = stackalloc int[count];
            count = 0;
            for (int i = 0; i < size; i++)
            {
                int LCount = 0;
                for (int g = 0; g < size; g++)
                {
                    if (i != g && *(arr + i) == *(arr + g))
                    {
                        LCount++;
                    }
                }
                if (LCount == 0)
                {
                    *(arrRes + count++) = *(arr + i);
                }
            }
            Console.WriteLine("Task_3 Array in Method=> VVV");
            Program.PrintArr(arrRes, sizeRezArr);
            return arrRes;
        }
        public unsafe int* Task_4(int* arr1, int size1, int* arr2, int size2, out int sizeRezArr)
        {
            int counter = 0;
            for (int i = 0; i < size1; i++)
            {
                int LCount = 0;
                for (int g = 0; g < size2; g++)
                {
                    if (*(arr1 + i) == *(arr2 + g))
                    {
                        ++LCount;
                    }
                }
                if (LCount > 0)
                {
                    ++counter;
                }
            }
            //////////////////////////////////////////
            int counter2 = 0;
            for (int i = 0; i < size2; i++)
            {
                int LCount = 0;
                for (int g = 0; g < size1; g++)
                {
                    if (*(arr2 + i) == *(arr1 + g))
                    {
                        ++LCount;
                    }
                }
                if (LCount > 0)
                {
                    ++counter2;
                }
            }
            sizeRezArr = counter + counter2;
            int* arrRes = stackalloc int[sizeRezArr];
            counter = 0;
            for (int i = 0; i < size1; i++)
            {
                int LCount = 0;
                for (int g = 0; g < size2; g++)
                {
                    if (*(arr1 + i) == *(arr2 + g))
                    {
                        ++LCount;
                    }
                }
                if (LCount > 0)
                {
                    *(arrRes + counter++) = arr1[i];
                }
            }
            counter = 0;
            for (int i = 0; i < size2; i++)
            {
                int LCount = 0;
                for (int g = 0; g < size1; g++)
                {
                    if (*(arr2 + i) == *(arr1 + g))
                    {
                        ++LCount;
                    }
                }
                if (LCount > 0)
                {
                    *(arrRes + (counter+ sizeRezArr-counter2)) = arr2[i];
                    counter++;
                }
            }
            Console.WriteLine("Task_4 Array in Method=> VVV");
            Program.PrintArr(arrRes, sizeRezArr);
            return arrRes;
        }
    }
}
