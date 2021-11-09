using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace TxtToXmlConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter file path (including 'filename.txt'): ");
            string fileName = Console.ReadLine();

            string path = Path.Combine(fileName);
            string[] source = File.ReadAllLines(path);

            var xmlTree = new XElement("people");
            XElement personElem = null;
            XElement familyElem = null;
            bool fromPerson = false;

            foreach (var line in source)
            {
                string[] slices = line.Split("|");
                if (slices[0] == "P" && slices.Length == 3)
                {
                    XElement person =
                        new XElement("person",
                            new XElement("firstname", slices[1]),
                            new XElement("lastname", slices[2]));
                    personElem = person;
                    xmlTree.Add(person);
                    fromPerson = true;
                }
                else if (slices[0] == "T" && slices.Length == 3)
                {
                    XElement phone =
                        new XElement("phone",
                            new XElement("mobile", slices[1]),
                            new XElement("landline", slices[2]));
                    if (fromPerson)
                    {
                        personElem.Add(phone);
                    }
                    else
                    {
                        familyElem.Add(phone);
                    }
                }
                else if (slices[0] == "A" && slices.Length == 4)
                {
                    XElement address =
                        new XElement("address",
                            new XElement("street", slices[1]),
                            new XElement("city", slices[2]),
                            new XElement("zipcode", slices[3]));
                    if (fromPerson)
                    {
                        personElem.Add(address);
                    }
                    else
                    {
                        familyElem.Add(address);
                    }
                }
                else if (slices[0] == "F" && slices.Length == 3)
                {
                    XElement family =
                        new XElement("family",
                            new XElement("name", slices[1]),
                            new XElement("born", slices[2]));
                    familyElem = family;
                    fromPerson = false;
                    personElem.Add(family);
                }
            }

            using var writer = XmlWriter.Create("output.xml", new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true });
            xmlTree.Save(writer);

            Console.WriteLine();
            Console.WriteLine($"Output file 'output.xml' will be saved at: {Environment.CurrentDirectory}");
            Console.WriteLine("Press any key to save and exit!");
            Console.ReadLine();
        }
    }
}