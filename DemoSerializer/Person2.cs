using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DemoSerializer
{

    [XmlRootAttribute("Person")]
    public class Person2
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [XmlElement("Address")]
        public Address2 Address { get; set; }

        [XmlArrayItem("Phone")]
        public Phone2[] Phones { get; set; }

    }

    public class Address2
    {
        public string Name { get; set; }

    }

    public class Phone2
    {
        public string Content { get; set; }

    }
}
