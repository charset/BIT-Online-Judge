namespace BITOJ.SiteUI.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 为用户注册提供数据模型。
    /// </summary>
    public class UserRegisterModel
    {
        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "The length of username should be between 1 and 32, inclusive.")]
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置密码。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters long.")]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置确认的密码。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password confirmation is required.")]
        public string PasswordConfirmation { get; set; }

        /// <summary>
        /// 获取或设置用户性别的字符串表示。
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 获取或设置用户权限集名称。
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// 获取或设置关联到 Username 字段的错误信息。
        /// </summary>
        public string UsernameErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置关联到 Password 字段的错误信息。
        /// </summary>
        public string PasswordErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置关联到 PasswordConfirmation 字段的错误信息。
        /// </summary>
        public string PasswordConfirmationErrorMessage { get; set; }

        /// <summary>
        /// 重置当前模型上的错误消息。
        /// </summary>
        public void ResetErrorMessages()
        {
            UsernameErrorMessage = string.Empty;
            PasswordErrorMessage = string.Empty;
            PasswordConfirmationErrorMessage = string.Empty;
        }

        /// <summary>
        /// 创建 UserRegisterModel 类的新实例。
        /// </summary>
        public UserRegisterModel()
        {
            Username = string.Empty;
            Password = string.Empty;
            PasswordConfirmation = string.Empty;
            Sex = "Female";
            UserGroupName = "Guest";

            UsernameErrorMessage = string.Empty;
            PasswordErrorMessage = string.Empty;
            PasswordConfirmationErrorMessage = string.Empty;
        }
    }
}