namespace BITOJ.Core.Data
{
    using System;

    public sealed class UserDataProvider : IUserDataProvider
    {
        public string Username => throw new NotImplementedException();

        public string Organization
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string ImagePath
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public int Rating
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public UserGroup UserGroup
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public UserSubmissionStatistics SubmissionStatistics
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
