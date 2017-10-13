using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Domain;

namespace NFine.Web.Areas.WebManage.Controllers
{
    public class ConentsController : ControllerBase
    {
        // 内容管理
        // GET: /WebManage/Conents/

        private WebContentApp contentApp = new WebContentApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = contentApp.GetList(pagination, queryJson);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = contentApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        //[OutputCache(CacheProfile = "SqlDependencyCache")]
        [HttpGet]
        [HandlerAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebContentEntity contentEntity, string keyValue)
        {
            contentApp.SubmitForm(contentEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            contentApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledConents(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                WebContentEntity contentEntity = new WebContentEntity();
                contentEntity.F_Id = keyValue;
                contentEntity.F_EnabledMark = false;
                contentApp.UpdateForm(contentEntity);
                return Success("禁用成功");
            }
            return Success("请选择禁用项");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledConents(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                WebContentEntity contentEntity = new WebContentEntity();
                contentEntity.F_Id = keyValue;
                contentEntity.F_EnabledMark = true;
                contentApp.UpdateForm(contentEntity);
                return Success("启用成功");
            }
            return Success("请选择启用项");
        }



    }
}
