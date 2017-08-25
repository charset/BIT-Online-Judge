using System.Web.Mvc;

namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Models;
    using System.Collections.Generic;

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        
        // Get: Admin/User
        public ActionResult Users(string username, string organization, string usergroup)
        {
            // 从数据库查询信息。
            // 创建查询参数对象。
            UserQueryParameter query = new UserQueryParameter();
            if (!string.IsNullOrEmpty(username))
            {
                query.QueryByUsername = true;
                query.Username = username;
            }
            if (!string.IsNullOrEmpty(organization))
            {
                query.QueryByOrganization = true;
                query.Organization = organization;
            }
            if (!string.IsNullOrEmpty(usergroup))
            {
                query.QueryByUsergroup = true;
                query.UserGroup = UsergroupConvert.ConvertFromString(usergroup);
            }

            // 进行查询，收集查询结果。
            IList<UserHandle> handles = UserManager.Default.QueryUsers(query);

            // 创建模型对象接收查询结果。
            UserQueryModel model = UserQueryModel.FromUserQueryResult(handles);
            model.IsPostBack = true;
            model.Username = username;
            model.Organization = organization;
            model.UserGroupName = usergroup;

            return View(model);
        }
    }
}