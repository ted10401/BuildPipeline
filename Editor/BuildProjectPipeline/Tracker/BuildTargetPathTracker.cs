
namespace TEDCore.BuildPipeline
{
    public class BuildTargetPathTracker
    {
        private string m_targetPath;

        public void SetTargetPath(string targetPath)
        {
            m_targetPath = targetPath;
        }

        public string GetTargetPath()
        {
            return m_targetPath;
        }

        public override string ToString()
        {
            return $"TargetPath = {m_targetPath}";
        }
    }
}