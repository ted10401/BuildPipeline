using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace TEDCore.BuildPipeline
{
    public class BuildProjectPipelineAndroid
    {
        public static void Build()
        {
            Build(Environment.GetCommandLineArgs());
        }

        private static void Build(string[] commandLineArgs)
        {
            CommandLineParser commandLineParser = new CommandLineParser(commandLineArgs);

            BuildProjectPipeline projectBuilder = CreatePipeline();
            int result = projectBuilder.Execute(commandLineParser);
            EditorApplication.Exit(result);
        }

        [CreateBuildProjectPipeline]
        private static BuildProjectPipeline CreatePipeline()
        {
            return BuildProjectPipeline.Create(BuildTarget.Android)
                .AddBundleVersionBuildProjectStep()
                .AddAndroidBundleVersionCodeBuildProjectStep()
                .AddTargetPathBuildProjectStep()
                .AddDevelopmentBuildProjectStep()
                .AddAllowDebuggingBuildProjectStep()
                .AddAndroidKeyStoreBuildStepBuildProjectStep();
        }
    }
}