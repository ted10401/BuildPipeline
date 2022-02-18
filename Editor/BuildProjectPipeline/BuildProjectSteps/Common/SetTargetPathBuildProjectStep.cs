using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TEDCore.BuildPipeline
{
    public static class SetTargetPathBuildProjectStepUtility
    {
        public static BuildProjectPipeline AddTargetPathBuildProjectStep(this BuildProjectPipeline projectBuilder)
        {
            return projectBuilder.AddStep(new SetTargetPathBuildProjectStep(projectBuilder.buildTarget));
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class SetTargetPathBuildProjectStep : IBuildProjectStep
    {
        [Command("-targetPath")]
        [SerializeField] private string m_targetPath = default;

        private BuildTarget m_buildTarget;

        public SetTargetPathBuildProjectStep(BuildTarget buildTarget)
        {
            m_buildTarget = buildTarget;
        }

        public void Execute(
            BuildTargetPathTracker buildTargetPathTracker,
            BuildOptionTracker buildOptionTracker,
            CommandLineParser commandLineParser)
        {
            commandLineParser?.Parse(this);
            m_targetPath = GetFormattedTargetPath(m_targetPath, m_buildTarget);

            buildTargetPathTracker.SetTargetPath(m_targetPath);
        }

        private string GetFormattedTargetPath(string targetPath, BuildTarget buildTarget)
        {
            string version;
            string fileName = string.Empty;
            string result = targetPath;

            if (string.IsNullOrEmpty(result))
            {
                result = Application.dataPath.Replace("Assets", "Builds");

                switch (buildTarget)
                {
                    case BuildTarget.StandaloneWindows64:
                        version = PlayerSettings.bundleVersion;
                        fileName = string.Format("{0}_v{1}_{2}", PlayerSettings.productName, version, DateTime.Now.ToString("yyyyMMddHHmm"));
                        fileName = Path.Combine(fileName, PlayerSettings.productName + ".exe");
                        break;
                    case BuildTarget.StandaloneOSX:
                        version = PlayerSettings.bundleVersion;
                        fileName = string.Format("{0}_v{1}_{2}", PlayerSettings.productName, version, DateTime.Now.ToString("yyyyMMddHHmm"));
                        fileName = Path.Combine(fileName, PlayerSettings.productName + ".app");
                        break;
                    case BuildTarget.Android:
                        version = PlayerSettings.bundleVersion + "." + PlayerSettings.Android.bundleVersionCode;
                        fileName = string.Format("{0}_v{1}_{2}.apk", PlayerSettings.productName, version, DateTime.Now.ToString("yyyyMMddHHmm"));
                        break;
                }
            }
            else
            {
                switch (buildTarget)
                {
                    case BuildTarget.StandaloneWindows64:
                        if(!result.EndsWith(".exe"))
                        {
                            fileName = PlayerSettings.productName + ".exe";
                        }
                        break;
                    case BuildTarget.StandaloneOSX:
                        if (!result.EndsWith(".app"))
                        {
                            fileName = PlayerSettings.productName + ".app";
                        }
                        break;
                    case BuildTarget.Android:
                        if(!result.EndsWith(".apk"))
                        {
                            version = PlayerSettings.bundleVersion + "." + PlayerSettings.Android.bundleVersionCode;
                            fileName = string.Format("{0}_v{1}_{2}.apk", PlayerSettings.productName, version, DateTime.Now.ToString("yyyyMMddHHmm"));
                        }
                        break;
                }
            }

            if(!string.IsNullOrEmpty(fileName))
            {
                result = Path.Combine(result, fileName);
            }

            result = result.Replace('\\', Path.DirectorySeparatorChar);
            result = result.Replace('/', Path.DirectorySeparatorChar);

            return result;
        }
    }
}