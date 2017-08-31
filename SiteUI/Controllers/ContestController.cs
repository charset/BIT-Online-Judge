
namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Core.Resolvers;
    using BITOJ.SiteUI.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;


    public class ContestController : Controller
    {
        private const int ItemsPerPage = 50;

        // GET: Contest
        public ActionResult Index(int page = 1)
        {
            ContestModel model = new ContestModel()
            {
                Catalog = 0,
                CurrentPage = page,
            };
            return View(model);
        }
    }
}