using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace TEDCore.BuildPipeline
{
    public static class SetDevelopmentBuildProjectStepUtility
    {
        public static BuildProjectPipeline AddDevelopmentBuildProjectStep(this BuildProjectPipeline projectBuilder)
        {
            return projectBuilder.AddStep(new SetDevelopmentBuildProjectStep());
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class SetDevelopmentBuildProjectStep : IBuildProjectStep
    {
        [Command("-development")]
        [SerializeField] private bool m_development = true;

        public void Execute(
            BuildTargetPathTracker buildTargetPathTracker,
            BuildOptionTracker buildOptionTracker,
            CommandLineParser commandLineParser)
        {
            commandLineParser?.Parse(this);

            if (m_development)
            {
                buildOptionTracker.Add(BuildOptions.Development);
            }
            else
            {
                buildOptionTracker.Remove(BuildOptions.Development);
            }
        }
    }
}