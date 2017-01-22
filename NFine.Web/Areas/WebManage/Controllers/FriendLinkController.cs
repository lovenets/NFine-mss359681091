using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.WebManage.Controllers
{
    public class FriendLinkController : ControllerBase
    {
        // 内容管理
        // GET: /WebManage/Conents/

        private WebFriendLinksApp linksApp = new WebFriendLinksApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = linksApp.GetList(pagination, queryJson);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = linksApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebFriendLinksEntity friendLinksEntity, string keyValue)
        {
            linksApp.SubmitForm(friendLinksEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            linksApp.DeleteForm(keyValue);
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
                WebFriendLinksEntity friendLinksEntity = new WebFriendLinksEntity();
                friendLinksEntity.F_Id = keyValue;
                friendLinksEntity.F_EnabledMark = false;
                linksApp.UpdateForm(friendLinksEntity);
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
                WebFriendLinksEntity friendLinksEntity = new WebFriendLinksEntity();
                friendLinksEntity.F_Id = keyValue;
                friendLinksEntity.F_EnabledMark = true;
                linksApp.UpdateForm(friendLinksEntity);
                return Success("启用成功");
            }
            return Success("请选择启用项");
        }
    }
}
