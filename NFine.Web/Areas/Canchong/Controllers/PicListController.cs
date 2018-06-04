using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (!string.IsNullOrEmpty(keyValue))
            {
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                for (int i = 0; i < lstid.Count; i++)
                {
                    WebPictureEntity pictureEntity = pictureApp.GetForm(lstid[i]);
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
                    pictureApp.DeleteForm(lstid[i]);
                }
                return Success("删除成功");
            }
            return Success("请选择删除项");
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
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                for (int i = 0; i < lstid.Count; i++)
                {
                    WebPictureEntity pictureEntity = new WebPictureEntity();
                    pictureEntity.F_Id = lstid[i];
                    pictureEntity.F_EnabledMark = false;
                    pictureApp.UpdateForm(pictureEntity);
                }
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
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                for (int i = 0; i < lstid.Count; i++)
                {
                    WebPictureEntity pictureEntity = new WebPictureEntity();
                    pictureEntity.F_Id = lstid[i];
                    pictureEntity.F_EnabledMark = true;
                    pictureApp.UpdateForm(pictureEntity);
                }
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
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                for (int i = 0; i < lstid.Count; i++)
                {
                    //进行同步操作

                    //更改同步标识
                    WebPictureEntity pictureEntity = new WebPictureEntity();
                    pictureEntity.F_Id = lstid[i];
                    pictureEntity.F_Synchro = true;
                    pictureApp.UpdateForm(pictureEntity);
                }
                return Success("同步成功");
            }
            return Success("请选择同步项");
        }

        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                if (lstid.Count == 1)
                {
                    var data = pictureApp.GetForm(lstid[0]);
                    //将压缩包下载
                    FileDownHelper.DownLoadold(Server.MapPath(data.F_FilePath), data.F_FullName);//单个图片直接下载
                    return;
                }

                //将图片拷贝至指定临时路径并压缩打包
                string newFilename = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFolder = "/Temp/" + newFilename + "/";
                string newZip = Server.MapPath("/Temp/" + newFilename + ".zip");//压缩包
                FileHelper.CreateDirectory(Server.MapPath(newFolder));//创建一个临时文件夹

                for (int i = 0; i < lstid.Count; i++)
                {
                    var data = pictureApp.GetForm(lstid[i]);
                    string filename = Server.UrlDecode(data.F_FullName);
                    string filepath = Server.MapPath(data.F_FilePath);
                    if (FileDownHelper.FileExists(filepath))
                    {
                        FileHelper.Copy(filepath, Server.MapPath(newFolder + data.F_FullName));//将文件拷贝到临时文件夹
                    }
                }
                //将文件夹进行GZip压缩
                ZipFloClass Zc = new ZipFloClass();
                Zc.ZipFile(Server.MapPath(newFolder), newZip);//生成压缩包
                if (FileDownHelper.FileExists(newZip))
                {
                    FileDownHelper.DownLoadold(newZip, newFilename + ".zip");//下载压缩包
                }
                //删除文件夹
                FileHelper.DeleteDirectory(newFolder);//删除文件夹
                FileHelper.DeleteFile(newZip);//删除临时压缩包
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

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetImglstJson(string category, string page, string keyword)
        {
            string queryJson = "{ \"F_Category\":\"" + category + "\",\"keyword\":\"" + keyword + "\"}";
            Pagination pagination = new Pagination();
            pagination.page = Convert.ToInt32(page);
            pagination.rows = 18;//每页21条
            pagination.sord = "asc";
            pagination.sidx = "F_Category desc";
            var data = pictureApp.GetList(pagination, queryJson);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WebUploader()
        {
            return View();
        }

        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file, string param_uploader)
        {
            string filePathName = string.Empty;
            param_uploader= System.Web.HttpUtility.UrlDecode(param_uploader);//改格式
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }
            string filename = name;//图片名称
            string nickname = Path.GetFileNameWithoutExtension(file.FileName);//图片昵称
            string categoryFolder = "/Temp/" + param_uploader + "/";
            string thumbnailFolder = "/Temp/" + param_uploader + "_s/";
            FileHelper.CreateDirectory(Server.MapPath(categoryFolder));
            FileHelper.CreateDirectory(Server.MapPath(thumbnailFolder));

            string fullpath = Server.MapPath(categoryFolder + filename);//图片物理路径

            if (!System.IO.File.Exists(fullpath))
            {
                file.SaveAs(fullpath);
            }
            NFine.Code.Common.MakeThumbnail(fullpath, Server.MapPath(thumbnailFolder + filename), 600, 350, "W", "jpg");//生成缩略图

            var length = FileHelper.ToFileSize(file.ContentLength);//获取图片大小 

            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                F_Nick = nickname,
                F_FilePath = categoryFolder + filename,
                F_FullName = filename,
                F_FileSize = length,
                F_ThumbnailPath = thumbnailFolder + filename
            });

        }
    }
}
