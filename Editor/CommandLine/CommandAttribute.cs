using System;

namespace TEDCore.Pipeline
{
    public class CommandAttribute : Attribute
    {
        public string command { get; private set; }

        public CommandAttribute(string tag)
        {
            this.command = tag;
        }
    }
}