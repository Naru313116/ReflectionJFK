using System;

namespace JfkCallable
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        public string Description { get; }

        public string Copyright { get; set; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
