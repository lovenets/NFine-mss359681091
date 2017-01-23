using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.WebManage.Controllers
{
    public class FeedBackController : ControllerBase
    {
        // 反馈管理
        // GET: /WebManage/FeedBack/

        private WebFeedbackApp feedbackApp = new WebFeedbackApp();


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = feedbackApp.GetList(pagination, queryJson);
            //return Content(data.ToJson());
            return Json(data, JsonRequestBehavior.AllowGet);//前台AJAX如果是GET用这句
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = feedbackApp.GetForm(keyValue);
            if (data != null)
            {
                data.F_IsRead = true;
                feedbackApp.SubmitForm(data, keyValue);
            }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebFeedbackEntity feedbackEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                feedbackEntity.F_IsRead = false;
            }
            feedbackApp.SubmitForm(feedbackEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            feedbackApp.DeleteForm(keyValue);
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
                WebFeedbackEntity feedbackEntity = new WebFeedbackEntity();
                feedbackEntity.F_Id = keyValue;
                feedbackEntity.F_EnabledMark = false;
                feedbackApp.UpdateForm(feedbackEntity);
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
                WebFeedbackEntity feedbackEntity = new WebFeedbackEntity();
                feedbackEntity.F_Id = keyValue;
                feedbackEntity.F_EnabledMark = true;
                feedbackApp.UpdateForm(feedbackEntity);
                return Success("启用成功");
            }
            return Success("请选择启用项");
        }
    }
}
