using System;
using UnityEditor;

namespace TEDCore.BuildPipeline
{
    public class BuildProjectPipelineMacOSX
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
            return BuildProjectPipeline.Create(BuildTarget.StandaloneOSX)
                .AddBundleVersionBuildProjectStep()
                .AddTargetPathBuildProjectStep()
                .AddDevelopmentBuildProjectStep()
                .AddAllowDebuggingBuildProjectStep();
        }
    }
}