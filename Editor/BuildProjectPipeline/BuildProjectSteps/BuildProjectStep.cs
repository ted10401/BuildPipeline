using System.Linq;
using System.Reflection;

namespace TEDCore.BuildPipeline
{
    public abstract class BuildProjectStep
    {
        public abstract void Execute(BuildTargetPathTracker buildTargetPathTracker, BuildOptionTracker buildOptionTracker, CommandLineParser commandLineParser);
        public override string ToString()
        {
            string result = string.Empty;

            FieldInfo[] fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                CommandAttribute attribute = fieldInfo
                    .GetCustomAttributes(inherit: false)
                    .FirstOrDefault(attr => attr is CommandAttribute)
                                                 as CommandAttribute;

                if (attribute == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(result))
                {
                    result += "\n";
                }

                result += $"{attribute.command} = {fieldInfo.GetValue(this)}";
            }

            return result;
        }
    }
}