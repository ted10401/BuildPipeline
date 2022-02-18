using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JSLCore.BuildPipeline
{
    public class BuildProjectPipelineEditorWindow : OdinEditorWindow
    {
        [MenuItem("JSLCore/Pipeline/Build Project Pipeline")]
        private static void Init()
        {
            EditorWindow editorWindow = GetWindow<BuildProjectPipelineEditorWindow>();
            editorWindow.Show();
            editorWindow.Focus();
        }

        [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, Expanded = true)] private BuildProjectPipeline[] m_projectBuilders = null;

        protected override void OnEnable()
        {
            base.OnEnable();

            MethodInfo[] methodInfos = GetAllCreateWindowMethods();
            if(methodInfos == null ||
                methodInfos.Length == 0)
            {
                return;
            }

            m_projectBuilders = new BuildProjectPipeline[methodInfos.Length];

            for (int i = 0; i < methodInfos.Length; i++)
            {
                m_projectBuilders[i] = methodInfos[i].Invoke(null, null) as BuildProjectPipeline;
            }
        }

        private static MethodInfo[] GetAllCreateWindowMethods()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(methodInfo => methodInfo.GetCustomAttributes<CreateBuildProjectPipelineAttribute>().Count() > 0 && methodInfo.ReturnType == typeof(BuildProjectPipeline))
                .ToArray();
        }
    }
}