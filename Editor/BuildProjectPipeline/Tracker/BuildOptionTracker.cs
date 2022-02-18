using System.Collections.Generic;
using UnityEditor;

namespace JSLCore.BuildPipeline
{
    public class BuildOptionTracker
    {
        private List<BuildOptions> m_addOptions = new List<BuildOptions>();
        private List<BuildOptions> m_removeOptions = new List<BuildOptions>();

        public void Add(BuildOptions buildOptions)
        {
            m_addOptions.Add(buildOptions);
        }

        public void Remove(BuildOptions buildOptions)
        {
            m_removeOptions.Add(buildOptions);
        }

        public BuildOptions GetBuildOptions()
        {
            BuildOptions buildOptions = BuildOptions.None;

            foreach(BuildOptions option in m_addOptions)
            {
                buildOptions |= option;
            }

            foreach(BuildOptions option in m_removeOptions)
            {
                buildOptions &= ~option;
            }

            return buildOptions;
        }

        public override string ToString()
        {
            string message = string.Empty;
            
            message += "\n\nAdd Options:";
            foreach (BuildOptions option in m_addOptions)
            {
                message += $"\n{option}";
            }

            message += "\n\nRemove Options:";
            foreach (BuildOptions option in m_removeOptions)
            {
                message += $"\n{option}";
            }

            return message;
        }
    }
}