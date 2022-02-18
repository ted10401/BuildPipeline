using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace TEDCore.Pipeline
{
    public static class SetAllowDebuggingBuildProjectStepUtility
    {
        public static BuildProjectPipeline AddAllowDebuggingBuildProjectStep(this BuildProjectPipeline projectBuilder)
        {
            return projectBuilder.AddStep(new SetAllowDebuggingBuildProjectStep());
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class SetAllowDebuggingBuildProjectStep : IBuildProjectStep
    {
        [Command("-allowDebugging")]
        [SerializeField] private bool m_allowDebugging = true;

        public void Execute(
            BuildTargetPathTracker buildTargetPathTracker,
            BuildOptionTracker buildOptionTracker,
            CommandLineParser commandLineParser)
        {
            commandLineParser?.Parse(this);

            if (m_allowDebugging)
            {
                buildOptionTracker.Add(BuildOptions.AllowDebugging);
            }
            else
            {
                buildOptionTracker.Remove(BuildOptions.AllowDebugging);
            }
        }
    }
}