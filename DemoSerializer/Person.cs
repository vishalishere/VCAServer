﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSerializer
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public List<Phone> Phones { get; set; }

    }

    public class Address
    {
        public string Name { get; set; }

    }

    public class Phone
    {
        public string Content { get; set; }

    }
}
