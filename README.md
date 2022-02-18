# BuildPipeline
[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](https://github.com/dauxio/daux.io/blob/master/LICENSE.md)
[![Releases](https://img.shields.io/github/release/ted10401/BuildPipeline.svg)](https://github.com/ted10401/BuildPipeline/releases)

The **BuildPipeline** module provides an easy way to customize the build pipeline. The module is referenced by [**Mimiron Lite**](https://gitlab.com/rayark/mimiron-lite) and creates the Edtior Window with [**Odin Inspector**](https://odininspector.com/).

**Reference**<br>
[**Mimiron Lite**](https://gitlab.com/rayark/mimiron-lite)

**Reguire**<br>
[**Odin Inspector**](https://odininspector.com/)

## Menu Items
TEDCore/Pipeline/Build Project Pipeline

## Example - Set BuildOptions.Development Step
```csharp
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace TEDCore.Pipeline
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
```

## Example - Build a Windows64 Project
```csharp
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace TEDCore.Pipeline
{
    public class BuildProjectPipelineWindows64
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
            return BuildProjectPipeline.Create(BuildTarget.StandaloneWindows64)
                .AddBundleVersionBuildProjectStep()
                .AddTargetPathBuildProjectStep()
                .AddDevelopmentBuildProjectStep()
                .AddAllowDebuggingBuildProjectStep();
        }
    }
}
```

## Exmaple - Create Editor Window
After you have created a new custom build project pipeline, if you want to create an Editor Window, you can add **[CreateBuildProjectPipeline]** attribute to the static method which returns **BuildProjectPipeline** and the Editor Window would create directly.
```csharp
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace TEDCore.Pipeline
{
    public class BuildProjectPipelineWindows64
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
            return BuildProjectPipeline.Create(BuildTarget.StandaloneWindows64)
                .AddBundleVersionBuildProjectStep()
                .AddTargetPathBuildProjectStep()
                .AddDevelopmentBuildProjectStep()
                .AddAllowDebuggingBuildProjectStep();
        }
    }
}
```
<img src="GithubResources/01.Build_Project_Pipeline_Editor_Window.jpg" />

## Example - Batch Script
```batch
@echo off

setlocal

set UNITY_PATH=C:\Program Files\Unity\Hub\Editor\2019.2.12f1\Editor\Unity.exe
set PROJECT_PATH=D:\YourProject
set TARGET_PATH=C:\Users\UserName\Desktop\Build

"%UNITY_PATH%" -batchmode -projectPath "%PROJECT_PATH%" -executeMethod TEDCore.BuildPipeline.BuildProjectPipelineWindows64.Build -development true -allowDebugging true -buildVersion 1.0.0 -targetPath "%TARGET_PATH%"

endlocal

pause
```