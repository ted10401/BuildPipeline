﻿using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace TEDCore.Pipeline
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
            BuildReport buildReport = projectBuilder.Execute(commandLineParser);
            EditorApplication.Exit(buildReport.summary.result == BuildResult.Succeeded ? 0 : 1);
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