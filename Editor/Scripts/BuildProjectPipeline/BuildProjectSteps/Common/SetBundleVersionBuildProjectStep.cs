using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace TEDCore.BuildPipeline
{
    public static class SetBundleVersionBuildProjectStepUtility
    {
        public static BuildProjectPipeline AddBundleVersionBuildProjectStep(this BuildProjectPipeline projectBuilder)
        {
            return projectBuilder.AddStep(new SetBundleVersionBuildProjectStep());
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class SetBundleVersionBuildProjectStep : BuildProjectStep
    {
        [Command("-bundleVersion")]
        [SerializeField] private string m_bundleVersion = "0.0.1";

        public override void Execute(
            BuildTargetPathTracker buildTargetPathTracker,
            BuildOptionTracker buildOptionTracker,
            CommandLineParser commandLineParser)
        {
            commandLineParser?.Parse(this);
            PlayerSettings.bundleVersion = m_bundleVersion;
        }
    }
}