
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.Generic;
using System.IO.Compression;

namespace Lab_1
{
    class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            //задание 1
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    Console.WriteLine($"Имя диска: {drive.Name}");
                    Console.WriteLine($"Файловая система: {drive.DriveFormat}");
                    Console.WriteLine($"Тип диска: {drive.DriveType}");
                    Console.WriteLine($"Общий объём свободного места, доступного на диске: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Размер диска: {drive.TotalSize}");
                    Console.WriteLine($"Метка тома диска: {drive.VolumeLabel}");
                }
                catch { }

                Console.WriteLine();
            }

            //задание 2
            string path = @"C:\csharp";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            // запись в файл
            using (FileStream fstream = new FileStream($"{path}/note.txt", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }
            // чтение из файла
            using (FileStream fstream = File.OpenRead($"{path}/note.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }
            string surfile = @"C:\csharp\note.txt";
            Console.WriteLine(File.Exists(surfile) ? "Файл существует" : "Файл не существует");
            {
                File.Delete(surfile);
                Console.WriteLine("Файл удалён");
                Console.WriteLine();
            }

            //Задание номер 3
            Student tom = new Student { Name = "Ivan", Age = 19 };
            string fileName = "Student.json";
            string json = JsonSerializer.Serialize<Student>(tom);
            File.WriteAllText(fileName, json);
            Console.WriteLine(json);
            Student restoredStudent = JsonSerializer.Deserialize<Student>(json);

            string jsfile = @"C:\Users\dan_i\source\repos\Lab_1_1\Lab_1_1\bin\Debug\net6.0\Student.json";//необходимо создать файл с расширением .json и указать полный путь
            Console.WriteLine(File.Exists(jsfile) ? "Файл существует" : "Файл не существует");
            {
                File.Delete(jsfile);
                Console.WriteLine("Файл удалён");
                Console.WriteLine();
            }

            //Задание номер 4
            List<Student> users = new List<Student>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\dan_i\source\repos\Lab_1_1\Lab_1_1\bin\Debug\net6.0\users.xml");//необходимо создать файл с расширением .xml и указать полный путь
            XmlElement xRoot = xDoc.DocumentElement;

            // создаем новый элемент student
            XmlElement userElem = xDoc.CreateElement("user");
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            XmlElement ageElem = xDoc.CreateElement("age");
            Console.WriteLine("Создание нового узла");
            Console.WriteLine("Введите имя пользователя: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите возраст пользователя: ");
            string age = Console.ReadLine();
            XmlText nameText = xDoc.CreateTextNode(name);
            XmlText ageText = xDoc.CreateTextNode(age);

            //добавляем узлы
            nameAttr.AppendChild(nameText);
            ageElem.AppendChild(ageText);
            userElem.Attributes.Append(nameAttr);
            userElem.AppendChild(ageElem);
            xRoot.AppendChild(userElem);
            xDoc.Save("users.xml");
            Console.WriteLine("Данные сохранены");
            //Вывод данных
            Console.WriteLine("Вывод данных");
            foreach (XmlElement xnode in xRoot)
            {
                Student user = new Student();
                XmlNode attr = xnode.Attributes.GetNamedItem("name");
                if (attr != null)
                    user.Name = attr.Value;

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "age")
                        user.Age = Int32.Parse(childnode.InnerText);
                }
                users.Add(user);
            }
            foreach (Student u in users)
                Console.WriteLine($"Имя: {u.Name} Возраст: {u.Age}");
            Console.ReadLine();

            // удаление файла
            string xmlfile = @"C:\Users\dan_i\source\repos\Lab_1_1\Lab_1_1\bin\Debug\net6.0\users.xml";//
            Console.WriteLine(File.Exists(xmlfile) ? "Файл существует" : "Файл не существует");
            {
                File.Delete(xmlfile);
                Console.WriteLine("Файл удалён");
            }
            
            //Задание номер 5
            string sourceFile = @"C:\csharp\user.txt";
            string compressedFile = @"C:\csharp\user.gz";
            string targetFile = @"C:\csharp\user_new.txt";

            Compress(sourceFile, compressedFile);

            Decompress(compressedFile, targetFile);
            Console.ReadLine();

            // удаление файла
            File.Delete(@"C:\csharp\user_new.txt");
            File.Delete(@"C:\csharp\user.gz");
            Console.WriteLine("Файлы удалены");
            Console.ReadLine();
        }

        static void Compress(string sourceFile, string compressedFile)
        {
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }
        ///Разархивирование
        static void Decompress(string compressedFile, string targetFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", targetFile);
                    }
                }
            }
        }
    }
}
