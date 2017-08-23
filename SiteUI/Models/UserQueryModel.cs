namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 为用户数据查询提供数据模型。
    /// </summary>
    public class UserQueryModel
    {
        /// <summary>
        /// 获取或设置一个值，该值指示当前页面是否是一个结果回送页面。
        /// </summary>
        public bool IsPostBack { get; set; }

        /// <summary>
        /// 获取或设置查询的用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置查询的用户权限集。
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// 获取或设置查询的用户组织名称。
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 获取或设置查询结果。
        /// </summary>
        public ICollection<UserProfileModel> Users { get; set; }

        /// <summary>
        /// 创建 UserQueryModel 类的新实例。
        /// </summary>
        public UserQueryModel()
        {
            IsPostBack = false;
            Username = string.Empty;
            UserGroupName = string.Empty;
            Organization = string.Empty;
            Users = new List<UserProfileModel>();
        }

        /// <summary>
        /// 从查询结果创建 UserQueryModel 类的新实例。
        /// </summary>
        /// <param name="handles">查询结果。</param>
        /// <returns>查询结果的 UserQueryModel 包装。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static UserQueryModel FromUserQueryResult(IList<UserHandle> handles)
        {
            if (handles == null)
                throw new ArgumentNullException(nameof(handles));

            UserQueryModel model = new UserQueryModel();
            foreach (UserHandle hd in handles)
            {
                model.Users.Add(UserProfileModel.FromUserHandle(hd));
            }

            return model;
        }
    }
}