using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace TEDCore.BuildPipeline
{
    public abstract class Pipeline<TPipeline, TStep> : ISerializationCallbackReceiver
        where TPipeline : Pipeline<TPipeline, TStep>
    {
        [SerializeField, ListDrawerSettings(Expanded = true)] protected List<TStep> m_steps = new List<TStep>();

        public TPipeline AddStep(TStep step)
        {
            m_steps.Add(step);
            return (TPipeline)this;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            OnAfterDeserialize();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            OnBeforeSerialize();
        }

        protected virtual void OnAfterDeserialize()
        {
        }

        protected virtual void OnBeforeSerialize()
        {
        }
    }
}