namespace BITOJ.Core.Resolvers
{
    using BITOJ.Core.Data;

    public sealed class GymUrlResolver : IProblemUrlResolver
    {
        public string Resolve(ProblemHandle handle)
        {
            string numericId = handle.ProblemId.Substring(3);
            string nameXu = numericId.Substring(6);
            numericId = numericId.Substring(0, 6);
            int result;

            if (!int.TryParse(numericId, out result) || !(nameXu[0] >= 'A' && nameXu[0] <= 'Z')) 
                throw new InvalidProblemException(handle);//不合法

            return "http://codeforces.com/gym/" + result + "/problem/" + nameXu;
        }
    }
}
