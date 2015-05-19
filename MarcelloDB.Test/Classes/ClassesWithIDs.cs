using System;
using MarcelloDB.Attributes;

namespace MarcelloDB.Test
{
    public class ClassWithID
    {
        public int ID { get; set; }
    }

    public class SubclassWithID : ClassWithID {}

    public class ClassWithId
    {
        public int Id { get; set; }
    }

    public class ClassWithid
    {
        public int id { get; set;}
    }

    public class ClassWithClassID
    {
        public int ClassWithClassIDID { get; set; }
    }

    public class ClassWithClassId
    {
        public int ClassWithClassIdId {get; set; }
    }

    public class ClassWithClassid
    {
        public int ClassWithClassidid {get; set; }
    }

    public class ClassWithAttrID
    {
        [ID]
        public int AttributedID {get; set; }
    }
}

