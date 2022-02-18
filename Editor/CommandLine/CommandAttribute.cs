using System;

namespace TEDCore.BuildPipeline
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