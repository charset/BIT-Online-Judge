namespace BITOJ.Core.Resolvers
{
    using BITOJ.Core.Data;

    public sealed class CfUrlResolver : IProblemUrlResolver
    {
        public string Resolve(ProblemHandle handle)
        {
            string numericId = handle.ProblemId.Substring(2);
            int adress = 0;
            while (numericId[adress] >= '0' && numericId[adress] <= '9') adress++;
            if(adress==0)
                throw new InvalidProblemException(handle);//不合法
            string nameXu = numericId.Substring(adress);
            numericId = numericId.Substring(0, adress);
            int result;

            if (!int.TryParse(numericId, out result) || !(nameXu[0] >= 'A' && nameXu[0] <= 'Z'))
                throw new InvalidProblemException(handle);//不合法

            return "http://codeforces.com/problemset/problem/" + result + "/" + nameXu;
        }
    }
}
