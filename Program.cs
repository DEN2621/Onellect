using Newtonsoft.Json.Linq;

using System;
using System.IO;
using System.Net;

namespace ONELLECT
{
    class Program
    {
        static HttpWebRequest request;
        delegate int[] MethodsArray(int[] array);
        static int[] Input()
        {
            int[] array = new int[new Random().Next(20, 101)];
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = new Random().Next(-100, 101);
            }
            return array;
        }
        static void Print(int[] array)
        {
            using StreamWriter stream = new(request.GetRequestStream());
            for (int i = 0; i < array.Length; ++i)
            {
                Console.Write($"{array[i]} ");
                stream.Write($"{array[i]} ");
            }
            Console.WriteLine();
            stream.WriteLine();
        }
        static int[] BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                    {
                        (array[i], array[j]) = (array[j], array[i]);
                    }
                }
            }
            Console.WriteLine("Выполнена сортировка пузырьком:");
            return array;
        }
        static int[] GnomeSort(int[] array)
        {
            int i = 1, j = 2;
            while (i < array.Length)
            {
                if (array[i - 1] < array[i])
                {
                    i = j++;
                }
                else
                {
                    (array[i - 1], array[i]) = (array[i], array[i - 1]);
                    --i;
                    if (i == 0)
                    {
                        i = j++;
                    }
                }
            }
            Console.WriteLine("Выполнена гномья сортировка:");
            return array;
        }
        static int[] ShakerSort(int[] array)
        {
            int left = 0, right = array.Length - 1, count = 0;
            while (left < right)
            {
                for (int i = left; i < right; i++)
                {
                    ++count;
                    if (array[i] > array[i + 1])
                    {
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                    }
                }
                --right;
                for (int i = right; i > left; i--)
                {
                    ++count;
                    if (array[i - 1] > array[i])
                    {
                        (array[i - 1], array[i]) = (array[i], array[i - 1]);
                    }
                }
                ++left;
            }
            Console.WriteLine("Выполнена шейкерная сортировка:");
            return array;
        }
        static void Main()
        {
            dynamic config = JObject.Parse(new StreamReader("config.json").ReadToEnd());
            request = (HttpWebRequest)WebRequest.Create(config.Url.ToString());
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "text/plain";
            MethodsArray[] methods = new MethodsArray[] { BubbleSort, GnomeSort, ShakerSort };
            int[] array = Input();
            Console.WriteLine("Исходный массив:");
            Print(array);
            Print(methods[new Random().Next(0, 3)](array));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
            Console.WriteLine(response.StatusCode);
        }
    }
}