using System;

namespace Welt.Types
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class StringEnumAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string m_PositionalString;

        // This is a positional argument
        public StringEnumAttribute(string positionalString)
        {
            m_PositionalString = positionalString;
        }

        public string String
        {
            get { return m_PositionalString; }
        }
    }
}
