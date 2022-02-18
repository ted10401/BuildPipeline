using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace JSLCore.BuildPipeline
{
    [HideReferenceObjectPicker]
    public class BuildProjectPipeline : Pipeline<BuildProjectPipeline, IBuildProjectStep>
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

        public BuildReport Execute(CommandLineParser commandLineParser)
        {
            BuildTargetPathTracker buildTargetPathTracker = new BuildTargetPathTracker();
            BuildOptionTracker buildOptionTracker = new BuildOptionTracker();

            foreach(var step in m_steps)
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
                Debug.LogError($"BuildPipeline.BuildPlayer failed.");
            }
            else
            {
                EditorUtility.RevealInFinder(buildPlayerOptions.locationPathName);
            }

            return buildReport;
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

        [Button]
        private void Build()
        {
            Execute(null);
        }
    }
}