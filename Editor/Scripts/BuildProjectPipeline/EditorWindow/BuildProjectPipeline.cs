using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TEDCore.BuildPipeline
{
    [HideReferenceObjectPicker]
    public class BuildProjectPipeline : Pipeline<BuildProjectPipeline, BuildProjectStep>
    {
        public static BuildProjectPipeline Create(BuildTarget buildTarget)
        {
            return new BuildProjectPipeline(buildTarget);
        }

        public BuildTarget buildTarget { get { return m_buildTarget; } }

        [SerializeField, HideInInspector] private SerializationData m_serializationData;
        [SerializeField, ReadOnly] private BuildTarget m_buildTarget;

        public BuildProjectPipeline(BuildTarget buildTarget)
        {
            m_buildTarget = buildTarget;
        }

        public int Execute(CommandLineParser commandLineParser)
        {
            BuildTargetPathTracker buildTargetPathTracker = new BuildTargetPathTracker();
            BuildOptionTracker buildOptionTracker = new BuildOptionTracker();
            foreach (var step in m_steps)
            {
                step.Execute(buildTargetPathTracker, buildOptionTracker, commandLineParser);
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetSceneNames(),
                locationPathName = buildTargetPathTracker.GetTargetPath(),
                targetGroup = UnityEditor.BuildPipeline.GetBuildTargetGroup(m_buildTarget),
                target = m_buildTarget,
                options = buildOptionTracker.GetBuildOptions()
            };

            BuildReport buildReport = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (buildReport.summary.result != BuildResult.Succeeded)
            {
                Debug.LogError(GetBuildReportLog(commandLineParser, buildReport));
                return 1;
            }
            else
            {
                Debug.Log(GetBuildReportLog(commandLineParser, buildReport));
                EditorUtility.RevealInFinder(buildPlayerOptions.locationPathName);
                return 0;
            }
        }

        private static string[] GetSceneNames()
        {
            List<string> sceneNames = new List<string>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (null == scenes[i])
                {
                    continue;
                }

                if (scenes[i].enabled)
                {
                    sceneNames.Add(scenes[i].path);
                }
            }

            return sceneNames.ToArray();
        }

        private string GetBuildReportLog(CommandLineParser commandLineParser, BuildReport buildReport)
        {
            string buildReportLog = $"[Build Project {buildReport.summary.result}]";

            if(commandLineParser != null)
            {
                buildReportLog += "[Command Line Args]";
                buildReportLog += $"\n{commandLineParser}";
            }

            buildReportLog += "\n\n[Build Project Steps]";
            int stepIndex = 0;
            foreach (var step in m_steps)
            {
                buildReportLog += $"\nStep {stepIndex++}, {step}";
            }

            buildReportLog += "\n\n[Build Report]";
            buildReportLog += "\nSteps";
            for (int i = 0; i < buildReport.steps.Length; i++)
            {
                buildReportLog += $"\nStep {buildReport.steps[i].depth}, duration = {buildReport.steps[i].duration.TotalSeconds:f2} seconds, name = {buildReport.steps[i].name}";
            }

            buildReportLog += "\n\nSummary";
            buildReportLog += $"\nplatformGroup = {buildReport.summary.platformGroup}";
            buildReportLog += $"\nplatform = {buildReport.summary.platform}";
            buildReportLog += $"\nbuildStartedAt = {buildReport.summary.buildStartedAt}";
            buildReportLog += $"\nbuildEndedAt = {buildReport.summary.buildEndedAt}";
            buildReportLog += $"\ntotalTime = {buildReport.summary.totalTime.TotalSeconds:f2} seconds";
            buildReportLog += $"\ntotalSize = {buildReport.summary.totalSize / 1024 / 1024} MB";

            return buildReportLog;
        }

        [Button]
        private void Build()
        {
            Execute(null);
        }
    }
}