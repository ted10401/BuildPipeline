using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace TEDCore.BuildPipeline
{
    public static class SetAndroidBundleVersionCodeBuildProjectStepUtility
    {
        public static BuildProjectPipeline AddAndroidBundleVersionCodeBuildProjectStep(this BuildProjectPipeline projectBuilder)
        {
            return projectBuilder.AddStep(new SetAndroidBundleVersionCodeBuildProjectStep());
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class SetAndroidBundleVersionCodeBuildProjectStep : BuildProjectStep
    {
        [Command("-androidBundleVersionCode")]
        [SerializeField] private int m_androidBundleVersionCode = 0;

        public override void Execute(
            BuildTargetPathTracker buildTargetPathTracker,
            BuildOptionTracker buildOptionTracker,
            CommandLineParser commandLineParser)
        {
            commandLineParser?.Parse(this);
            PlayerSettings.Android.bundleVersionCode = m_androidBundleVersionCode;
        }
    }
}