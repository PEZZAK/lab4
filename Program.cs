using System;

namespace Lab4Time
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Меню (Задания 6-7) ===");
                Console.WriteLine("1. Вычитание Time из Time");
                Console.WriteLine("2. Операторы ++ и --");
                Console.WriteLine("3. Приведение к byte (часы)");
                Console.WriteLine("4. Приведение к bool (не 00:00)");
                Console.WriteLine("5. Time + uint (минуты)");
                Console.WriteLine("6. Time + Time");
                Console.WriteLine("0. Выход");

                int choice = ReadInt("Выберите пункт: ");
                switch (choice)
                {
                    case 1: RunSubtract(); break;
                    case 2: RunIncrementDecrement(); break;
                    case 3: RunCastToByte(); break;
                    case 4: RunCastToBool(); break;
                    case 5: RunAddMinutes(); break;
                    case 6: RunAddTime(); break;
                    case 0: exit = true; break;
                    default: Console.WriteLine("Нет такого пункта."); break;
                }
            }
        }

        private static void RunSubtract()
        {
            Time left = ReadTime("Введите уменьшаемое время");
            Time right = ReadTime("Введите вычитаемое время");
            Time result = left.Subtract(right);
            Console.WriteLine($"{left} - {right} = {result}");
        }

        private static void RunIncrementDecrement()
        {
            Time time = ReadTime("Введите время");
            Console.WriteLine($"{time} ++ = {++time}");
            Time time2 = ReadTime("Введите другое время");
            Console.WriteLine($"{time2} -- = {--time2}");
        }

        private static void RunCastToByte()
        {
            Time time = ReadTime("Введите время");
            byte hours = (byte)time;
            Console.WriteLine($"byte({time}) = {hours}");
        }

        private static void RunCastToBool()
        {
            Time time = ReadTime("Введите время");
            bool flag = time;
            Console.WriteLine($"bool({time}) = {flag}");
        }

        private static void RunAddMinutes()
        {
            Time time = ReadTime("Введите время");
            uint minutes = (uint)ReadNonNegativeInt("Введите количество минут: ");
            Console.WriteLine($"{time} + {minutes} = {time + minutes}");
            Console.WriteLine($"{minutes} + {time} = {minutes + time}");
        }

        private static void RunAddTime()
        {
            Time left = ReadTime("Введите первое время");
            Time right = ReadTime("Введите второе время");
            Console.WriteLine($"{left} + {right} = {left + right}");
        }

        private static Time ReadTime(string title)
        {
            Console.WriteLine(title + ":");
            byte hours = (byte)ReadRangeInt("  Часы (0–23): ", 0, 23);
            byte minutes = (byte)ReadRangeInt("  Минуты (0–59): ", 0, 59);
            return new Time(hours, minutes);
        }

        private static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                {
                    return value;
                }
                Console.WriteLine("Некорректный ввод. Введите целое число.");
            }
        }

        private static int ReadNonNegativeInt(string message)
        {
            while (true)
            {
                int value = ReadInt(message);
                if (value >= 0)
                {
                    return value;
                }
                Console.WriteLine("Число должно быть неотрицательным.");
            }
        }

        private static int ReadRangeInt(string message, int min, int max)
        {
            while (true)
            {
                int value = ReadInt(message);
                if (value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Число должно быть от {min} до {max}.");
            }
        }
    }
}