
namespace JSLCore.Pipeline
{
    public interface IBuildProjectStep
    {
        void Execute(BuildTargetPathTracker buildTargetPathTracker, BuildOptionTracker buildOptionTracker, CommandLineParser commandLineParser);
    }
}