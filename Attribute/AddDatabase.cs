using System;
using System.Collections.Generic;
using System.Text;

namespace Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AddDatabase : System.Attribute
    {
        public AddDatabase()
        {

        }
    }
}
