using NFine.Application.SystemManage;
using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.Canchong.Controllers
{
    public class PicListController : ControllerBase
    {
        private WebPictureApp pictureApp = new WebPictureApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = pictureApp.GetList(pagination, queryJson);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = pictureApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebPictureEntity pictureEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(pictureEntity.F_FilePath))
            {
                var path = Server.MapPath(pictureEntity.F_FilePath);
                if (FileHelper.IsExistFile(path))
                {
                    pictureEntity.F_FileType = FileHelper.GetExtension(path);
                }
            }
            if (pictureEntity.F_Synchro == true)
            {
                //执行同步操作
            }
            pictureApp.SubmitForm(pictureEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            WebPictureEntity pictureEntity = pictureApp.GetForm(keyValue);
            if (pictureEntity.F_FilePath.Length > 0)
            {
                //删除原图
                string fullFileName = Server.MapPath(pictureEntity.F_FilePath);
                Thread t1 = new Thread(new ParameterizedThreadStart(DeleteFile));
                t1.Start(fullFileName);
                //删除缩略图
                string thumbnailPath = Server.MapPath(pictureEntity.F_ThumbnailPath);
                Thread t2 = new Thread(new ParameterizedThreadStart(DeleteFile));
                t2.Start(thumbnailPath);
            }
            pictureApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 删除物理文件
        /// </summary>
        /// <param name="fullFileName"></param>
        private static void DeleteFile(object fullFileName)
        {
            FileHelper.DeleteFile(fullFileName.ToString());
        }


        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledConents(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                WebPictureEntity pictureEntity = new WebPictureEntity();
                pictureEntity.F_Id = keyValue;
                pictureEntity.F_EnabledMark = false;
                pictureApp.UpdateForm(pictureEntity);
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
                WebPictureEntity pictureEntity = new WebPictureEntity();
                pictureEntity.F_Id = keyValue;
                pictureEntity.F_EnabledMark = true;
                pictureApp.UpdateForm(pictureEntity);
                return Success("启用成功");
            }
            return Success("请选择启用项");
        }



        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConents(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                //进行同步操作
                WebPictureEntity pictureEntity = new WebPictureEntity();
                pictureEntity.F_Id = keyValue;
                pictureEntity.F_Synchro = true;
                pictureApp.UpdateForm(pictureEntity);

                return Success("同步成功");
            }
            return Success("请选择同步项");
        }

        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            var data = pictureApp.GetForm(keyValue);
            string filename = Server.UrlDecode(data.F_FullName);
            string filepath = Server.MapPath(data.F_FilePath);
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase Filedata)
        {
            // 没有文件上传，直接返回
            if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
            {
                return HttpNotFound();
            }

            var category = System.Web.HttpUtility.UrlDecode(Request["category"]);
            string name = Path.GetFileName(Filedata.FileName);//图片名称
            string nickname = Path.GetFileNameWithoutExtension(Filedata.FileName);//图片昵称
            string categoryFolder = "/Temp/" + category + "/";
            string thumbnailFolder = "/Temp/" + category + "_s/";
            FileHelper.CreateDirectory(Server.MapPath(categoryFolder));
            FileHelper.CreateDirectory(Server.MapPath(thumbnailFolder));

            string fullpath = Server.MapPath(categoryFolder + name);//图片物理路径

            if (!System.IO.File.Exists(fullpath))
            {
                Filedata.SaveAs(fullpath);
            }
            NFine.Code.Common.MakeThumbnail(fullpath, Server.MapPath(thumbnailFolder + name), 600, 350, "W", "jpg");//生成缩略图

            var length = FileHelper.ToFileSize(Filedata.ContentLength);//获取图片大小 
            var data = new { F_Nick = nickname, F_FilePath = categoryFolder + name, F_FullName = name, F_FileSize = length, F_ThumbnailPath = thumbnailFolder + name };
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ImgShow()
        {
            return View();
        }
    }
}
