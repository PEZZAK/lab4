using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab4Tasks
{
    public static class Lab4Tasks
    {
        // ================================================================
        // ЗАДАНИЕ 1 (List)
        // Оставить в списке только первые вхождения одинаковых элементов.
        // ================================================================
        public static void Task1()
        {
            Console.WriteLine("Задание 1: оставить только первые вхождения элементов.");
            List<int> list = ReadIntList("Введите элементы списка через пробел: ");

            KeepFirstOccurrences(list);

            Console.WriteLine("Результат: " + string.Join(", ", list));
        }

        public static void KeepFirstOccurrences(List<int> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            HashSet<int> seen = new HashSet<int>();
            int writeIndex = 0;

            for (int readIndex = 0; readIndex < list.Count; readIndex++)
            {
                int value = list[readIndex];
                if (seen.Add(value))
                {
                    list[writeIndex] = value;
                    writeIndex++;
                }
            }

            if (writeIndex < list.Count)
            {
                list.RemoveRange(writeIndex, list.Count - writeIndex);
            }
        }

        // ================================================================
        // ЗАДАНИЕ 2 (LinkedList)
        // Если у элемента E «соседи» не равны, заменить на F.
        // ================================================================
        public static void Task2()
        {
            Console.WriteLine("Задание 2: заменить E на F, если у E соседи не равны.");
            List<int> values = ReadIntList("Введите элементы списка через пробел: ");

            LinkedList<int> list = new LinkedList<int>(values);
            int e = ReadInt("Введите значение E: ");
            int f = ReadInt("Введите значение F (замена): ");

            ReplaceIfNeighborsDiffer(list, e, f);

            Console.WriteLine("Результат: " + string.Join(", ", list));
        }

        public static void ReplaceIfNeighborsDiffer(LinkedList<int> list, int e, int f)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            LinkedListNode<int> node = list.First;
            while (node != null)
            {
                LinkedListNode<int> next = node.Next;

                if (node.Value == e)
                {
                    bool hasPrev = node.Previous != null;
                    bool hasNext = node.Next != null;

                    if (hasPrev && hasNext && node.Previous.Value != node.Next.Value)
                    {
                        node.Value = f;
                    }
                }

                node = next;
            }
        }

        // ================================================================
        // ЗАДАНИЕ 3 (HashSet)
        // Языки: какие знает каждый работник, какие хотя бы один, какие никто.
        // ================================================================
        public static void Task3()
        {
            Console.WriteLine("Задание 3: анализ знания языков работниками.");

            int languageCount = ReadPositiveInt("Сколько языков в перечне? ");
            HashSet<string> allLanguages = new HashSet<string>();
            for (int i = 1; i <= languageCount; i++)
            {
                string language = ReadNonEmptyString($"Название языка {i}: ");
                allLanguages.Add(language);
            }

            int workerCount = ReadPositiveInt("Сколько работников? ");
            List<HashSet<string>> workers = new List<HashSet<string>>();
            for (int i = 1; i <= workerCount; i++)
            {
                Console.WriteLine($"Работник {i}:");
                int knownCount = ReadNonNegativeInt("  Сколько языков знает? ");

                HashSet<string> known = new HashSet<string>();
                for (int j = 1; j <= knownCount; j++)
                {
                    string language = ReadNonEmptyString($"  Язык {j}: ");
                    if (!allLanguages.Contains(language))
                    {
                        Console.WriteLine("  Такого языка нет в перечне, пропускаем.");
                        continue;
                    }
                    known.Add(language);
                }

                workers.Add(known);
            }

            var (knownByAll, knownBySome, knownByNone) = AnalyzeLanguages(workers, allLanguages);

            Console.WriteLine("\nЗнают все работники: " + FormatSet(knownByAll));
            Console.WriteLine("Знает хотя бы один: " + FormatSet(knownBySome));
            Console.WriteLine("Не знает никто: " + FormatSet(knownByNone));
        }

        public static (HashSet<string> KnownByAll, HashSet<string> KnownBySome, HashSet<string> KnownByNone)
            AnalyzeLanguages(List<HashSet<string>> workersLanguages, HashSet<string> allLanguages)
        {
            if (workersLanguages == null)
            {
                throw new ArgumentNullException(nameof(workersLanguages));
            }
            if (allLanguages == null)
            {
                throw new ArgumentNullException(nameof(allLanguages));
            }

            HashSet<string> knownByAll = new HashSet<string>();
            HashSet<string> knownBySome = new HashSet<string>();
            HashSet<string> knownByNone = new HashSet<string>();

            if (workersLanguages.Count == 0)
            {
                knownByNone.UnionWith(allLanguages);
                return (knownByAll, knownBySome, knownByNone);
            }

            Dictionary<string, int> counts = new Dictionary<string, int>();

            foreach (HashSet<string> worker in workersLanguages)
            {
                foreach (string language in worker)
                {
                    counts[language] = counts.TryGetValue(language, out int c) ? c + 1 : 1;
                }
            }

            int totalWorkers = workersLanguages.Count;

            foreach (string language in allLanguages)
            {
                if (!counts.TryGetValue(language, out int count))
                {
                    knownByNone.Add(language);
                }
                else
                {
                    if (count == totalWorkers)
                    {
                        knownByAll.Add(language);
                    }
                    if (count < totalWorkers)
                    {
                        knownBySome.Add(language);
                    }
                }
            }

            return (knownByAll, knownBySome, knownByNone);
        }

        // ================================================================
        // ЗАДАНИЕ 4 (HashSet)
        // С каких букв начинаются слова? (без алфавитного порядка)
        // ================================================================
        public static void Task4()
        {
            Console.WriteLine("Задание 4: с каких букв начинаются слова?");
            string text = ReadNonEmptyString("Введите текст на русском языке: ");

            HashSet<char> firstLetters = FirstLettersOfWords(text);

            Console.WriteLine("Первые буквы слов: " + string.Join(", ", firstLetters));
        }

        public static HashSet<char> FirstLettersOfWords(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            string[] words = text.Split(
                new[] { ' ', '\t', '\r', '\n', '.', ',', '!', '?', ';', ':', '-', '—', '«', '»', '(', ')' },
                StringSplitOptions.RemoveEmptyEntries);

            HashSet<char> firstLetters = new HashSet<char>();

            foreach (string word in words)
            {
                char first = char.ToLowerInvariant(word[0]);
                if (char.IsLetter(first))
                {
                    firstLetters.Add(first);
                }
            }

            return firstLetters;
        }

        // ================================================================
        // ЗАДАНИЕ 5
        // Два ученика школы №50, которые лучше всех сдали информатику.
        // Данные берутся из текстового файла "students.txt".
        // ================================================================
        public static void Task5()
        {
            Console.WriteLine("Задание 5: лучшие ученики школы №50.");

            string fileName = "students.txt";
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            if (!File.Exists(filePath))
            {
                CreateStudentsFile(filePath);
                Console.WriteLine($"Создан исходный файл: {fileName}");
            }
            FindTopTwoSchool50FromFile(filePath);
        }
        public static void CreateStudentsFile(string path)
        {
            string[] lines =
            {
                "8",
                "Иванов Сергей 50 87",
                "Сергеев Иван 50 95",
                "Петров Петр 50 95",
                "Сидоров Сидор 50 87",
                "Смирнов Алексей 50 75",
                "Кузнецов Олег 49 99",
                "Попов Дмитрий 50 90",
                "Васильев Игорь 50 85"
            };

            File.WriteAllLines(path, lines);
        }

        private static void FindTopTwoSchool50FromFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл с данными не найден.");
                return;
            }

            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                Console.WriteLine("Файл пуст.");
                return;
            }

            if (!int.TryParse(lines[0].Trim(), out int n) || n <= 0)
            {
                Console.WriteLine("Некорректная первая строка (N).");
                return;
            }

            if (lines.Length - 1 < n)
            {
                Console.WriteLine("В файле меньше строк, чем заявлено.");
                return;
            }

            List<Student> students = new List<Student>();

            for (int i = 1; i <= n; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4) continue;

                if (int.TryParse(parts[2], out int school) &&
                    int.TryParse(parts[3], out int score))
                {
                    students.Add(new Student(parts[0], parts[1], school, score));
                }
            }

            FindTopTwo(students);
        }

        private class Student
        {
            public string Surname { get; }
            public string Name { get; }
            public int School { get; }
            public int Score { get; }

            public Student(string surname, string name, int school, int score)
            {
                Surname = surname;
                Name = name;
                School = school;
                Score = score;
            }
        }

        private static void FindTopTwo(List<Student> students)
        {
            int max1 = -1, max2 = -1;
            List<Student> top1 = new List<Student>();
            List<Student> top2 = new List<Student>();

            foreach (Student s in students)
            {
                if (s.School != 50) continue;

                if (s.Score > max1)
                {
                    max2 = max1;
                    top2 = new List<Student>(top1);

                    max1 = s.Score;
                    top1.Clear();
                    top1.Add(s);
                }
                else if (s.Score == max1)
                {
                    top1.Add(s);
                }
                else if (s.Score > max2)
                {
                    max2 = s.Score;
                    top2.Clear();
                    top2.Add(s);
                }
                else if (s.Score == max2)
                {
                    top2.Add(s);
                }
            }

            if (top1.Count > 1)
            {
                Console.WriteLine($"\nНаибольший балл набрали более двух человек: {top1.Count}");
            }
            else if (top1.Count == 1)
            {
                if (top2.Count > 1)
                {
                    Console.WriteLine($"\n{top1[0].Surname} {top1[0].Name}");
                }
                else if (top2.Count == 1)
                {
                    Console.WriteLine($"\n{top1[0].Surname} {top1[0].Name}");
                    Console.WriteLine($"{top2[0].Surname} {top2[0].Name}");
                }
                else
                {
                    Console.WriteLine($"\n{top1[0].Surname} {top1[0].Name}");
                }
            }
            else
            {
                Console.WriteLine("\nНет учеников из школы №50.");
            }
        }

        // ================================================================
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ВВОДА
        // ================================================================

        private static string ReadNonEmptyString(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (input != null && input.Trim() != "")
                {
                    return input.Trim();
                }
                Console.WriteLine("Строка не может быть пустой.");
            }
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

        private static int ReadPositiveInt(string message)
        {
            while (true)
            {
                int value = ReadInt(message);
                if (value > 0) return value;
                Console.WriteLine("Число должно быть положительным.");
            }
        }

        private static int ReadNonNegativeInt(string message)
        {
            while (true)
            {
                int value = ReadInt(message);
                if (value >= 0) return value;
                Console.WriteLine("Число должно быть неотрицательным.");
            }
        }

        private static List<int> ReadIntList(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (input == null || input.Trim() == "")
                {
                    Console.WriteLine("Список не может быть пустым.");
                    continue;
                }

                string[] tokens = input.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> result = new List<int>();
                bool ok = true;

                foreach (string token in tokens)
                {
                    if (int.TryParse(token, out int value))
                        result.Add(value);
                    else
                    {
                        Console.WriteLine($"«{token}» — не целое число.");
                        ok = false;
                        break;
                    }
                }

                if (ok) return result;
            }
        }

        private static string FormatSet<T>(IEnumerable<T> set)
        {
            string joined = string.Join(", ", set);
            return joined == "" ? "(пусто)" : joined;
        }
    }
}