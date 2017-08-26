namespace BITOJ.Core.Resolvers
{
    using BITOJ.Core.Data;

    public sealed class BzojUrlResolver : IProblemUrlResolver
    {
        public string Resolve(ProblemHandle handle)
        {
            string numericId = handle.ProblemId.Substring(4);
            int result;

            if (!int.TryParse(numericId, out result))
                throw new InvalidProblemException(handle);//不合法

            return "http://www.lydsy.com/JudgeOnline/problem.php?id=" + result;
        }
    }
}
