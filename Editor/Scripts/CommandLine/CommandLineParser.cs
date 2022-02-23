using System.Linq;
using System.Reflection;

namespace TEDCore.BuildPipeline
{
    public class CommandLineParser
    {
        private readonly string[] m_commandLineArgs;

        public CommandLineParser(string[] commandLineArgs)
        {
            m_commandLineArgs = commandLineArgs;
        }

        public bool HasCommand(string command)
        {
            command = GetFormattedCommand(command);
            return m_commandLineArgs.Contains(command);
        }

        private string GetFormattedCommand(string command)
        {
            if (command != null && command.StartsWith("-"))
            {
                return command;
            }

            return "-" + command;
        }

        public string GetCommandValue(string command)
        {
            if (!HasCommand(command))
            {
                return null;
            }

            command = GetFormattedCommand(command);

            int commandValueIndex = -1;
            for (int i = 0; i < m_commandLineArgs.Length; i++)
            {
                if (m_commandLineArgs[i] == command)
                {
                    commandValueIndex = i + 1;
                    break;
                }
            }

            if (commandValueIndex < 0 || commandValueIndex >= m_commandLineArgs.Length)
            {
                return null;
            }

            return m_commandLineArgs[commandValueIndex];
        }

        public override string ToString()
        {
            return string.Join(" ", m_commandLineArgs);
        }

        internal void Parse(BuildProjectStep buildStep)
        {
            FieldInfo[] fieldInfos = buildStep.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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

                string commandValue = GetCommandValue(attribute.command);

                if (fieldInfo.FieldType == typeof(string))
                {
                    fieldInfo.SetValue(buildStep, commandValue);
                }
                else if (fieldInfo.FieldType == typeof(int))
                {
                    if (int.TryParse(commandValue, out int value))
                    {
                        fieldInfo.SetValue(buildStep, value);
                    }
                }
                else if (fieldInfo.FieldType == typeof(float))
                {
                    if (float.TryParse(commandValue, out float value))
                    {
                        fieldInfo.SetValue(buildStep, value);
                    }
                }
                else if (fieldInfo.FieldType == typeof(bool))
                {
                    if (bool.TryParse(commandValue, out bool value))
                    {
                        fieldInfo.SetValue(buildStep, value);
                    }
                }
            }
        }
    }
}