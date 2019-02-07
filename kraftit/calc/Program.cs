using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calc
{
    class Program
    {
        public static Dictionary<string, int> variables = new Dictionary<string, int>();

        static void Main(string[] args)
        {

            while(true)
            {
                List<string> data = new List<string>(); // части выражения
                List<char> rawData; //введенные символы

                string line;
                int result = 0;

                line = Console.ReadLine();

                rawData = new List<char>(line); //представляем выражение в символах
                rawData.RemoveAll(ch => ch == ' '); //не зависим от пробелов в записи

                data = ConvertData(data, rawData); //превращаем символы в части выражения

                result = Calculate(data); //считаем

                Console.WriteLine(result);
            }

        }

        public static List<string> ConvertData(List<string> data, List<char> rawData)
        {
            string item;
            int nextIndex;
            for (int i = 0; i < rawData.Count; i++) //перебираем символы
            {
                item = ""; //сюда будем записывать часть выражения

                do
                {
                    if (rawData[i] == '+' || rawData[i] == '-')
                    { //знаки входят в операнды
                        item += rawData[i];
                        nextIndex = i++;
                    }

                    item += rawData[i];
                    nextIndex = i++;
                    if (nextIndex + 1 == rawData.Count) break;
                }
                while (Char.GetUnicodeCategory(rawData[i]) == Char.GetUnicodeCategory(rawData[nextIndex])); //пока символы одного типа они складываются в часть выражения


                data.Add(item);

                i -= 1; //возвращаемся к символу перед разделителем
            }
            return data;
        }

        public static int Calculate(List<string> data)
        {
            int result = 0;
            var equally = data.IndexOf("=");
            if (equally > 0) //если присвоение
            {
                //правая часть 
                for (int i = equally + 1; i < data.Count; i++) //перебираем операнды. используем индекс = чтоб найти откуда начинать
                {
                    result += GetNumber(data[i]); //считаем
                }

                //левая часть
                if (variables.ContainsKey(data[0])) //если есть в словаре перезаписываем
                {
                    variables[data[0]] = result;
                }
                else //если нету добавляем
                {
                    variables.Add(data[0], result);
                }

            }
            else
            { //если просто операция
                for (int i = 0; i < data.Count; i++)
                {
                    result += GetNumber(data[i]); //складываем все
                }
            }

            return result;
        }

        public static int GetNumber(string op) //превращаем операнды в цифры
        {
            int number = 0;
            bool minus = false;

            if (op.Any(char.IsLetter)) //если переменная
            {
                if (op.Contains('-')) minus = true; //запоминаем что там был минус
                op = op.Trim('-', '+'); //очищаем от знаков чтоб найти в словаре

                try
                {
                    number = variables[op]; //берем из словаря
                    if (minus) number = number * -1;
                }
                catch { Console.WriteLine("Попытка использования неинициализированной переменной"); }

            }
            else //если просто число
            {
                number = Convert.ToInt32(op);
            }
            return number;
        }

    }
}
