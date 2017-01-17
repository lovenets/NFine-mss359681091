using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;

namespace NFine.Web.Areas.WebManage.Controllers
{
    public class NodesController : ControllerBase
    {
        private WebNodeApp nodeApp = new WebNodeApp();

        [HttpGet]
        [HandlerAuthorize]
        public override ActionResult Form()
        {
            base.BindParentNode("", true);//绑定父栏目
            return View();
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = nodeApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nodeApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebNodeEntity userEntity, string keyValue)
        {
            nodeApp.SubmitForm(userEntity, keyValue);
            return Success("操作成功");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var data = nodeApp.GetForm(keyValue);
            nodeApp.Delete(data);
            return Success("删除成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledNode(string keyValue)
        {
            WebNodeEntity nodeEntity = new WebNodeEntity();
            nodeEntity.F_Id = keyValue;
            nodeEntity.F_EnabledMark = false;
            nodeApp.UpdateForm(nodeEntity);
            return Success("栏目禁用成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledNode(string keyValue)
        {
            WebNodeEntity nodeEntity = new WebNodeEntity();
            nodeEntity.F_Id = keyValue;
            nodeEntity.F_EnabledMark = true;
            nodeApp.UpdateForm(nodeEntity);
            return Success("栏目启用成功");
        }

    }
}
