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
                 //первичное заполнение массива вероятностей
                arrOfElements = new decimal[k];
                line = streamReader.ReadLine();
                objOfLine = line.Split(' ');
                for (int i = 0; i < k; i++)//заполняем массив вероятностей
                {
                    arrOfElements[i] = Convert.ToDecimal(objOfLine[i].Replace('.', ','));
                }
            }
            //массив хранения множества вершин
            List<Element> massiv = new List<Element>();
            massiv.Add(new Element(1, 0));//заполняем первичным значением
            int topCounter = k;//ожидаемое количество вершин при следующем расщеплении
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
                        }
                        max = 0;//скидываем максимальное, что бы не дублировать расщепление для вершины с равным значением вероятности
                    }
                    else
                    {
                        temp.Add(new Element(dec.Key, dec.Value));//остальное просто переписываем
                    }
                }
                topCounter += k-1;
                massiv.Clear();//очищаем и переписываем всё в основной массив
                foreach (Element dec in temp) massiv.Add(new Element(dec.Key, dec.Value));
                Console.WriteLine();
            }
            //Расчет средней длины сообщения
            decimal result = 0;
            foreach (Element dec in massiv)
            {
                result += dec.Key * dec.Value;
            }
            ////вывод в файл
            result = Math.Round(result, 3);
            using (StreamWriter streamWriter = new StreamWriter("Tanstall.out", false))
                streamWriter.Write(result);

        }
    }
    class Element //хранение пары значений: вероятность и длина
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
