namespace BITOJ.Core
{
    using DatabaseParticipationMode = BITOJ.Data.Models.ContestParticipationMode;

    /// <summary>
    /// 表示比赛的参与模式。
    /// </summary>
    public enum ContestParticipationMode : int
    {
        /// <summary>
        /// 仅允许个人账号参赛。
        /// </summary>
        IndividualOnly = DatabaseParticipationMode.Individual,

        /// <summary>
        /// 仅允许队伍参赛。
        /// </summary>
        TeamworkOnly = DatabaseParticipationMode.Teamwork,

        /// <summary>
        /// 既允许个人参赛，也允许队伍参赛。
        /// </summary>
        Both = DatabaseParticipationMode.IndividualAndTeamwork,
    }
}
