namespace BITOJ.Core.Resolvers
{
    using BITOJ.Core.Data;

    public sealed class PojUrlResolver : IProblemUrlResolver
    {
        public string Resolve(ProblemHandle handle)
        {
            string numericId = handle.ProblemId.Substring(3);
            int result;

            if (!int.TryParse(numericId, out result))
                throw new InvalidProblemException(handle);//不合法

            return "http://poj.org/problem?id=" + result;
        }
    }
}
