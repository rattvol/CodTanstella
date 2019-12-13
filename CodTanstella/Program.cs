using System;
using System.Collections.Generic;
using System.IO;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal[] arrOfElements;
            int d, k, n;
            using (StreamReader streamReader = new StreamReader("Tanstall.in"))
            {
                string line = streamReader.ReadLine();
                string[] objOfLine = line.Split(' ');
                d = Convert.ToInt32(objOfLine[0]);//кратность кода
                k = Convert.ToInt32(objOfLine[1]);//количество слов
                n = Convert.ToInt32(objOfLine[2]);//длина слова
                                                  //первичное заполнение массива степеней вершин
                arrOfElements = new decimal[k];
                line = streamReader.ReadLine();
                objOfLine = line.Split(' ');
                for (int i = 0; i < k; i++)//заполняем массив вероятностей
                {
                    arrOfElements[i] = Convert.ToDecimal(objOfLine[i].Replace('.', ','));
                }
            }
            List<Element> massiv = new List<Element>();
            massiv.Add(new Element(1, 0));
            int topCounter = k;
            Console.WriteLine("1");
            while (topCounter <= Math.Pow(d, n))
            {
                List<Element> temp = new List<Element>();
                decimal max = 0;
                //поиск максимального значения вероятности
                foreach (Element dec in massiv) if (dec.Key > max) max = dec.Key;
                foreach (Element dec in massiv)
                {
                    if (dec.Key == max)//расщепление для максимального значения
                    {
                        for (int i = 0; i < arrOfElements.Length; i++)
                        {
                            //заполняем временный массив результатом расщепления
                            temp.Add(new Element(dec.Key * arrOfElements[i], dec.Value+1));
                            Console.Write(" " + dec.Key * arrOfElements[i]+"-"+dec.Value);
                        }
                        max = 0;
                    }
                    else
                    {
                        temp.Add(new Element(dec.Key, dec.Value));
                        Console.Write(" " + dec.Key+ "-" + dec.Value);
                    }
                }
                topCounter += k-1;
                massiv.Clear();
                foreach (Element dec in temp) massiv.Add(new Element(dec.Key, dec.Value));
                Console.WriteLine();
            }
            decimal result = 0;
            decimal summ = 0;
            foreach (Element dec in massiv)
            {
                result += dec.Key * dec.Value;
                summ += dec.Key;
            }
            Console.WriteLine("result=" + result + " summ=" + summ);
            Console.ReadKey();

            //Расчет средней длины сообщения
            ////вывод в файл
            using (StreamWriter streamWriter = new StreamWriter("Tanstall.out", false))
                streamWriter.Write(result);
        }
    }
    class Element 
    {
        public decimal Key { get; set; }
        public int Value { get; set; }
        public Element(decimal key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
