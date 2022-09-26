using Attribute;
using System;

namespace ClassModel
{
    public class ClassAdd
    {
        [PrimaryKey]
        public Guid? ClassAddID { get; set; }
        [AddDatabase]
        public int Interger1 { get; set; }
        [AddDatabase]
        public String String1 { get; set; }

        public string Extension { get; set; }
    }
}
