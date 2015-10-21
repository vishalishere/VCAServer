using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DemoSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person
            {
                Id = 1,
                Name = "Jake",
                Address = new Address
                {
                    Name = "XinAn"
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        Content = "15895326302"
                    },
                    new Phone
                    {
                        Content = "15172471549"
                    }
                }
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Person));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, p);
            string xml = writer.ToString();

            XmlSerializer serializer2 = new XmlSerializer(typeof(Person2));
            StringReader reader = new StringReader(xml);
            Person2 p2 = (Person2)serializer2.Deserialize(reader);

                
            
            
        }
    }
}
