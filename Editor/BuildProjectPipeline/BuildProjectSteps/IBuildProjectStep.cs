
namespace JSLCore.BuildPipeline
{
    public interface IBuildProjectStep
    {
        void Execute(BuildTargetPathTracker buildTargetPathTracker, BuildOptionTracker buildOptionTracker, CommandLineParser commandLineParser);
    }
}