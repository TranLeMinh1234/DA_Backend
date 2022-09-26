using System;

namespace Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : System.Attribute
    {
        public PrimaryKey()
        { 
            
        }
    }
}
