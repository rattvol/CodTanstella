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
                    arrOfElements[i] = decimal.Parse(objOfLine[i].Replace('.', separator));
                }
            }
            //массив хранения множества вершин
            List<Element> massiv = new List<Element>(1);
            massiv.Add(new Element(1, 0));//заполняем первичным значением
            int topCounter = k;//ожидаемое количество вершин при следующем расщеплении
            int limit = Convert.ToInt32(Math.Pow(d, n));//ограничение по количеству вершин
            decimal max = 1;//максимальное для поиска
            decimal newmax = 0;
            decimal newProb;

            while (topCounter <= limit)
            {
                List<Element> temp = new List<Element>();
                foreach (Element dec in massiv)
                {
                    if (dec.Key == max & topCounter <= limit)//расщепление для максимального значения
                    {
                        for (int i = 0; i < k; i++)
                        {
                            //заполняем временный массив результатом расщепления
                            newProb = dec.Key * arrOfElements[i];
                            if (i < k - 1) temp.Add(new Element(newProb, dec.Value + 1));
                            else//заменяем значение расщепляемой вершины в основном массиве
                            {
                                dec.Key = newProb;
                                dec.Value += +1;
                            }
                            if (newProb > newmax) newmax = newProb;//вычисляем максимальное
                        }
                        topCounter += k - 1;
                    }
                    else
                    {
                        if (dec.Key > newmax) newmax = dec.Key;//вычисляем максимальное среди неизменяемых элементов
                    }
                }
                foreach (Element dec in temp)
                {
                    massiv.Add(dec);//дополняем массив новыми элементами
                }
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
