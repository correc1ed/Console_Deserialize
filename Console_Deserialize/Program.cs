using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleSerialization_1
{

    [Serializable]
    class Program
    {
        public static Person[] JSONReadDown(OpenFileDialog openFileDialog)
        {

            Person[] persons;
            string ObjectJSONfile = File.ReadAllText(openFileDialog.FileName);
            persons = JsonSerializer.Deserialize<Person[]>(ObjectJSONfile);
            Console.WriteLine("JSON Deserialized: "); Console.WriteLine("--------------------------");

            foreach (Person person in persons)
            {
                Console.WriteLine($" Id = {person.id} \n Name = {person.Name}\n Surname = {person.SurName}\n Profession = {person.Profession}");
            }

            Console.WriteLine("--------------------------");

            return persons;
        }


        public static Person[] TRASH_ReadDown(OpenFileDialog openFileDialog)
        {

            Person[] persons = new Person[1];
            return persons;
        }

        public static Person[] XMLReadDown(OpenFileDialog openFileDialog)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person[]));
            Stream stream = openFileDialog.OpenFile();
            Person[] list;

            using (XmlReader reader = XmlReader.Create(openFileDialog.FileName))
            {
                list = (Person[])xmlSerializer.Deserialize(reader);
            }

                Console.WriteLine("XML Deserialized: "); Console.WriteLine("--------------------------"); // ДОДЕЛАТЬ.
                foreach (Person person in list)
                {
                    Console.WriteLine($" Id: {person.id}\n Фамилия: {person.SurName}\n Имя: {person.Name}\n Профессия: {person.Profession}");
                }
               Console.WriteLine("--------------------------");
            

            return list;
        }

        public static Person[] SOAPreadDown(OpenFileDialog openFileDialog)
        {
            SoapFormatter formatter = new SoapFormatter();

            Person[] People;

            using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
            {
                People = (Person[])formatter.Deserialize(fs);
                Console.WriteLine("SOAP Deserialized: "); Console.WriteLine("--------------------------");

                foreach (Person person in People)
                {
                    Console.WriteLine($" Id: {person.id}\n Фамилия: {person.SurName}\n Имя: {person.Name}\n Профессия: {person.Profession}");
                }

                Console.WriteLine("--------------------------");
            }
            return People;
        }

        public static Person[] BINARYreadDown(OpenFileDialog openFileDialog)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Person[] persons;

            using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
            {
                persons = (Person[])formatter.Deserialize(fs);
                Console.WriteLine("BINARY Deserialized: "); Console.WriteLine("--------------------------");

                foreach (Person person in persons)
                {
                    Console.WriteLine($" Id: {person.id}\n Фамилия: {person.SurName}\n Имя: {person.Name}\n Профессия: {person.Profession}");
                }
                Console.WriteLine("--------------------------");
            }

            return persons;
        }

        public static void JSONWriteDown(Person[] personList)
        {
            string ObjectSerialized = JsonSerializer.Serialize(personList);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "deserialization file(*.json)|*.json|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, ObjectSerialized);
            }

        }

        public static void XMLWriteDown(Person[] personList)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "deserialization file(*.xml)|*.xml|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            Stream stream;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person[]));

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    xmlSerializer.Serialize(stream, personList);
                }
            }

        }

        public static void SOAPwriteDown(Person[] personList)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "deserialization file(*.soap)|*.soap|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            Stream stream;

            SoapFormatter soapFormatter = new SoapFormatter();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    soapFormatter.Serialize(stream, personList);
                }
            }


        }

        public static void BINARYwriteDown(Person[] person)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "deserialization file(*.dat)|*.dat|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            Stream stream;

            BinaryFormatter formatter = new BinaryFormatter();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    formatter.Serialize(stream, person);
                }
            }

        }





        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Программа для десериализации и сериализации объекта  \n \n Нажмите Enter для открытия Диалогового окна \n \n ");
            Console.ReadKey();

            //Person[] persons;
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {                    
                    openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\";
                    openFileDialog.Filter = "deserialize files (*.json)|*.json|deserialize files (*.xml)|*.xml|deserialize files (*.soap)|*.soap|deserialize files (*.dat)|*.dat|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.ShowDialog();
                    
                    Person[] persons = TRASH_ReadDown(openFileDialog);

                    switch (openFileDialog.FileName.Substring(openFileDialog.FileName.Length - 4))
                    {
                        case "json":
                            persons = JSONReadDown(openFileDialog);
                            break;
                        case ".xml":
                            persons = XMLReadDown(openFileDialog);
                            break;
                        case "soap":
                            persons = SOAPreadDown(openFileDialog);
                            break;
                        case ".dat":
                            persons = BINARYreadDown(openFileDialog);
                            break;

                    }

                    Console.ReadKey();

                    Console.Clear();

                    string promt_for_YESorNO = "Хотите-ли вы дополнить данный файл?";
                    string[] options_for_YESorNO = { "Да", "Нет" };
                    Menu menu_for_YESorNO = new Menu(promt_for_YESorNO, options_for_YESorNO);

                    int selectedIndex_for_YESorNO = menu_for_YESorNO.Vote();

                    if (selectedIndex_for_YESorNO == 0)
                    {
                        Person person = new Person();

                        Console.Write("Введите Id: "); person.id = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Введите SurName: "); person.SurName = Console.ReadLine();
                        Console.Write("Введите Name: "); person.Name = Console.ReadLine();
                        Console.Write("Введите Profession: "); person.Profession = Console.ReadLine();

                        Array.Resize(ref persons, persons.Length + 1);
                        persons[persons.Length - 1] = person;
                    }

                    string[] options_for_Formats = { ".json", ".xml", ".soap", ".dat" };
                    Menu menu_for_Formats = new Menu("Выберите формат, в котором хотите десериализовать файл", options_for_Formats);

                    int selectedIndex_for_Formats = menu_for_Formats.Vote();

                    switch (selectedIndex_for_Formats)
                    {
                        case 0:
                            JSONWriteDown(persons);
                            break;
                        case 1:
                            XMLWriteDown(persons);
                            break;
                        case 2:
                            SOAPwriteDown(persons);
                            break;
                        case 3:
                            BINARYwriteDown(persons);
                            break;
                    }

                    Console.Clear();
                    Console.WriteLine("Объект десериализован!");
                    Console.ReadKey();

                }
            }

            catch (Exception exception) { Console.WriteLine(exception.Message); }

        }

    }

    [Serializable]
    public class Person
    {
        public int id { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }

        public Person() { }
        public Person(int ID, string surname, string name, string profession)
        {
            this.id = ID;
            this.SurName = surname;
            this.Name = name;
            this.Profession = profession;
        } //не пригодилось. :/  



    }


    public class Menu
    {
        public int SelectedIndex { get; set; }
        private string[] Options { get; set; }
        private string Promt { get; set; }

        public Menu(string promt, string[] options)
        {
            this.Promt = promt;
            this.Options = options;
            this.SelectedIndex = 1;

        }




        public int Vote()
        {

            ConsoleKey consoleKey;

            do
            {
                Console.Clear();
                Display();

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                consoleKey = consoleKeyInfo.Key;

                if (consoleKey == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex < 0 || SelectedIndex > Options.Length - 1)
                    {
                        SelectedIndex = 0;
                    }
                }
                else if (consoleKey == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex < 0 || SelectedIndex > Options.Length - 1)
                    {
                        SelectedIndex = 0;
                    }
                }
            }
            while (consoleKey != ConsoleKey.Enter);


            return SelectedIndex;
        }

        private void Display()
        {
            Console.WriteLine(Promt);


            for (int i = 0; i < Options.Length; i++)
            {
                string prefix;

                if (i == SelectedIndex)
                {
                    prefix = "*";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = string.Empty;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{prefix} {Options[i]}");
            }
            Console.ResetColor();
        }

    }


}

