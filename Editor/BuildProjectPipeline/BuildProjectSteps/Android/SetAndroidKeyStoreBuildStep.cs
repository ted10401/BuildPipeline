using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace JSLCore.Pipeline
{
    public static class SetAndroidKeyStoreBuildProjectStepUtility
    {
        public static BuildProjectPipeline AddAndroidKeyStoreBuildStepBuildProjectStep(this BuildProjectPipeline buildPipeline)
        {
            return buildPipeline.AddStep(new SetAndroidKeyStoreBuildStepBuildProjectStep());
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class SetAndroidKeyStoreBuildStepBuildProjectStep : IBuildProjectStep
    {
        [Command("-keystoreName")]
        [SerializeField] private string m_keystoreName = default;

        [Command("-keystorePass")]
        [SerializeField] private string m_keystorePass = default;

        [Command("-keyaliasName")]
        [SerializeField] private string m_keyaliasName = default;

        [Command("-keyaliasPass")]
        [SerializeField] private string m_keyaliasPass = default;

        public void Execute(
            BuildTargetPathTracker buildTargetPathTracker,
            BuildOptionTracker buildOptionTracker,
            CommandLineParser commandLineParser)
        {
            commandLineParser?.Parse(this);

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = m_keystoreName;
            PlayerSettings.Android.keystorePass = m_keystorePass;
            PlayerSettings.Android.keyaliasName = m_keyaliasName;
            PlayerSettings.Android.keyaliasPass = m_keyaliasPass;
        }
    }
}