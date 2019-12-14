using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal[] arrOfElements;
            int d, k, n;  
            Char separator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            using (StreamReader streamReader = new StreamReader("Tunstall.in"))
            {
                string line = streamReader.ReadLine();
                string[] objOfLine = line.Split(' ');
                d = Convert.ToInt32(objOfLine[0]);//кратность кода
                k = Convert.ToInt32(objOfLine[1]);//количество слов
                n = Convert.ToInt32(objOfLine[2]);//длина слова
                 //чтение и заполнение массива вероятностей
                arrOfElements = new decimal[k];
                line = streamReader.ReadLine();
                objOfLine = line.Split(' ');
                for (int i = 0; i < k; i++)//заполняем массив вероятностей
                {
                    arrOfElements[i] = Convert.ToDecimal(objOfLine[i].Replace('.', separator));
                }
            }
            //массив хранения множества вершин
            List<Element> massiv = new List<Element>();
            massiv.Add(new Element(1, 0));//заполняем первичным значением
            int topCounter = k;//ожидаемое количество вершин при следующем расщеплении
            int limit = (Int32)Math.Pow(d, n);//ограничение по количеству вершин
            decimal max = 1;//максимальное для поиска
            decimal newmax = 0;//максимальное для заполнения
            while (topCounter <= limit)
            {
                List<Element> temp = new List<Element>();
                foreach (Element dec in massiv)
                {
                    if (dec.Key == max)//расщепление для максимального значения
                    {
                        for (int i = 0; i < arrOfElements.Length; i++)
                        {
                            //заполняем временный массив результатом расщепления
                            decimal newProb = dec.Key * arrOfElements[i];
                            temp.Add(new Element(newProb, dec.Value+1));
                            if (newProb > newmax) newmax = newProb;//вычисляем максимальное
                        }
                        max = 0;//скидываем максимальное, что бы не дублировать расщепление для вершины с равным значением вероятности
                    }
                    else
                    {
                        temp.Add(new Element(dec.Key, dec.Value));//остальное просто переписываем
                        if (dec.Key > newmax) newmax = dec.Key;//вычисляем максимальное
                    }
                }
                topCounter += k-1;
                massiv = temp;
                max = newmax;
                newmax = 0;
            }
            //Расчет средней длины сообщения
            decimal result = 0;
            foreach (Element dec in massiv)
            {
                result += dec.Key * dec.Value;
            }
            ////вывод в файл
            string resultString = Math.Round(result, 3).ToString().Replace(separator, '.');
            using (StreamWriter streamWriter = new StreamWriter("Tunstall.out", false))
                streamWriter.Write(resultString);
        }
    }
    class Element //хранение пары значений: вероятность и длина
    {
        public decimal Key { get; set; }//вероятность
        public int Value { get; set; }//длина сообщения
        public Element(decimal key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
