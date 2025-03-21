using MornGlobal;

namespace MornProcess
{
    internal sealed class MornProcessGlobal : MornGlobalPureBase<MornProcessGlobal>
    {
        protected override string ModuleName => nameof(MornProcess);

        public static void Log(string message)
        {
            I.LogInternal(message);
        }

        public static void LogWarning(string message)
        {
            I.LogWarningInternal(message);
        }

        public static void LogError(string message)
        {
            I.LogErrorInternal(message);
        }
    }
}