using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class SiteController : Controller
    {
        //站点管理
        // GET: /Site/

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        private WebContentApp contentApp = new WebContentApp();

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            pagination.sidx = "F_Top desc,F_SortCode asc,F_CreatorTime desc";
            pagination.sord = "desc";
            var data = contentApp.GetList(pagination, queryJson);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult GetAllCount(string queryJson)
        {
            int count = contentApp.GetAllCount(queryJson);
            return Content(count.ToString());
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Action()
        {
            return View();
        }

    }
}
