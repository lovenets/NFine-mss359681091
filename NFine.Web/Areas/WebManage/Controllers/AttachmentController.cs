using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NFine.Web.Areas.WebManage.Controllers
{
    public class AttachmentController : ControllerBase
    {
        // 附件管理
        // GET: /WebManage/Attachment/

        private WebAttachmentApp attachmentApp = new WebAttachmentApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = attachmentApp.GetList(pagination, queryJson);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = attachmentApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebAttachmentEntity attachmentEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(attachmentEntity.F_FilePath))
            {
                var path = Server.MapPath(attachmentEntity.F_FilePath);
                if (FileHelper.IsExistFile(path))
                {
                    attachmentEntity.F_FileSize =FileHelper.ToFileSize(FileHelper.GetFileSize(path));
                    attachmentEntity.F_FileType = FileHelper.GetExtension(path);
                    if (string.IsNullOrEmpty(attachmentEntity.F_FullName))
                    {
                        attachmentEntity.F_FullName = FileHelper.GetFileName(path);
                    }
                }
            }

            attachmentApp.SubmitForm(attachmentEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            attachmentApp.DeleteForm(keyValue);
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
                WebAttachmentEntity attachmentEntity = new WebAttachmentEntity();
                attachmentEntity.F_Id = keyValue;
                attachmentEntity.F_EnabledMark = false;
                attachmentApp.UpdateForm(attachmentEntity);
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
                WebAttachmentEntity attachmentEntity = new WebAttachmentEntity();
                attachmentEntity.F_Id = keyValue;
                attachmentEntity.F_EnabledMark = true;
                attachmentApp.UpdateForm(attachmentEntity);
                return Success("启用成功");
            }
            return Success("请选择启用项");
        }
    }
}
