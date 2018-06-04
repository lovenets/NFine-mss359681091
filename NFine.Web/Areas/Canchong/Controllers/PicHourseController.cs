using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.Canchong.Controllers
{
    public class PicHourseController : ControllerBase
    {
        private WebPictureApp picApp = new WebPictureApp();

        public ActionResult WebUploader()
        {
            return View();
        }

        #region public ActionResult UpLoadProcess() 后台处理程序
        /// <summary>
        /// webUploader后台处理程序
        /// </summary>
        /// <param name="id">自生成id</param>
        /// <param name="name">文件名称</param>
        /// <param name="type">文件类型</param>
        /// <param name="lastModifiedDate">最后修改日期</param>
        /// <param name="size">文件大小</param>
        /// <param name="file">文件流</param>
        /// <param name="param_uploader">前台参数</param>
        /// <returns></returns>
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file, string param_uploader)
        {
            string cacheKey = param_uploader;//缓存键值
            ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存

            string filePathName = string.Empty;
            string localPath = Path.Combine(param_uploader, Configs.GetValue("ImgFolder"));
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }
            //string ex = Path.GetExtension(file.FileName);
            //filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            file.SaveAs(Path.Combine(localPath, file.FileName));
            cache.RemoveCache(cacheKey);//根据键值移除该类别数据集缓存
            return Json(new
            {
                jsonrpc = "2.0",
                id = id
            });
        }
        #endregion

        #region public ActionResult GetDriveJson() 获取磁盘信息
        [HttpGet]
        [HandlerAjaxOnly]
        /// <summary>
        /// 获取磁盘信息
        /// </summary>
        public ActionResult GetDriveJson()
        {
            List<Tuple<string, string>> listPerson = new List<Tuple<string, string>>();
            DriveInfo[] allDirves = DriveInfo.GetDrives();
            //检索计算机上的所有逻辑驱动器名称
            foreach (DriveInfo item in allDirves)
            {
                ////Fixed 硬盘
                ////Removable 可移动存储设备，如软盘驱动器或USB闪存驱动器。
                if (item.IsReady)
                {
                    var length = FileHelper.ToFileSize(item.TotalFreeSpace);//获取磁盘可用大小
                    listPerson.Add(new Tuple<string, string>(item.Name, length));
                }
            }
            return Json(listPerson.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  public ActionResult GetImglstJson() 获取原始磁盘资源分页列表
        [HttpGet]
        [HandlerAjaxOnly]
        /// <summary>
        /// 获取原始磁盘资源分页列表
        /// </summary>
        /// <param name="dash">所在盘符</param>
        /// <param name="page">当前页数</param>
        /// <returns></returns>
        public ActionResult GetImglstJson(string dash, int page = 1)
        {
            //参数0：索引，参数1：名称，参数2：图片路径，参数3：图片大小，参数4：图片扩展名
            List<Tuple<int, string, string, string, string>> imgLst = new List<Tuple<int, string, string, string, string>>();
            string cacheKey = dash;//缓存键值
            ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
            var cacheImgLst = cache.GetCache<List<Tuple<int, string, string, string, string>>>(cacheKey);
            if (cacheImgLst == null)
            {
                dash = Path.Combine(dash, Configs.GetValue("ImgFolder"));//默认固定文件夹为ImgList
                if (FileHelper.IsExistDirectory(dash))
                {
                    DirectoryInfo di = new DirectoryInfo(dash);
                    FileInfo[] files = di.GetFiles();
                    int count = 0;//图片个数
                    foreach (FileInfo item in files)
                    {
                        count++;
                        var length = FileHelper.ToFileSize(item.Length);//获取图片大小
                        imgLst.Add(new Tuple<int, string, string, string, string>(count, item.Name, item.FullName, length.ToString(), item.Extension));
                    }
                    cache.WriteCache<List<Tuple<int, string, string, string, string>>>(imgLst, cacheKey);//写入缓存
                }
                cacheImgLst = imgLst;
            }

            var pageImgLst = GetpageImgLst(cacheImgLst, page);//分页并处理图片移动

            return Json(pageImgLst.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region private List<Tuple<int, string, string, string, string>> GetpageImgLst() 分页并处理图片移动
        /// <summary>
        /// 分页并处理图片移动
        /// </summary>
        /// <param name="imgLst"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private List<Tuple<int, string, string, string, string>> GetpageImgLst(List<Tuple<int, string, string, string, string>> imgLst, int page)
        {
            List<Tuple<int, string, string, string, string>> pageImgLst = new List<Tuple<int, string, string, string, string>>();//实例化一个容器接收处理过的数据

            var returnLst = imgLst.Skip((page - 1) * 12).Take(12).ToList();
            for (int i = 0; i < returnLst.Count; i++)
            {
                var newPath = "/Temp/" + returnLst[i].Item2;//移到项目临时文件夹
                if (FileHelper.IsExistFile(returnLst[i].Item3))
                {
                    if (!FileHelper.IsExistFile(Server.MapPath(newPath)))
                    {
                        FileHelper.CopyFilePhysical(returnLst[i].Item3, Server.MapPath(newPath));   //文件转移
                    }
                }
                pageImgLst.Add(new Tuple<int, string, string, string, string>(returnLst[i].Item1, returnLst[i].Item2, newPath, returnLst[i].Item4, returnLst[i].Item5));
            }

            return pageImgLst;
        }
        #endregion

        #region  public ActionResult FnFile()  图片归档
        [HandlerAjaxOnly]
        /// <summary>
        /// 图片归档
        /// </summary>
        /// <param name="dash">所在盘符</param>
        /// <param name="category">类别</param>
        /// <param name="dat">所选图片集json</param>
        /// <param name="cvalue">类别名称</param>
        /// <param name="synchro">是否同步</param>
        /// <returns></returns>
        public ActionResult FnFile(string dash, string category, string dat, string cvalue, string synchro)
        {
            List<ToJsonFile> paramdat = NFine.Code.Json.ToObject<List<ToJsonFile>>(dat);//接收参数
            string originalPath = Path.Combine(dash, Configs.GetValue("ImgFolder"));//获取源文件坐在盘符

            string cacheKey = dash;//缓存键值
            ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存

            //第一步：判断是否有类别文件夹，没有则创建
            string categoryFolder = "/Temp/" + cvalue + "/";
            string thumbnailFolder = "/Temp/" + cvalue + "_s/";
            FileHelper.CreateDirectory(Server.MapPath(categoryFolder));
            FileHelper.CreateDirectory(Server.MapPath(thumbnailFolder));
            if (paramdat.Count > 0)
            {
                for (int i = 0; i < paramdat.Count; i++)
                {
                    var filename = paramdat[i].filename;//文件名称
                    var filetype = paramdat[i].filetype;//文件扩展名
                    var filesize = paramdat[i].filesize;//文件大小
                    var filenick = paramdat[i].filenick;//文件昵称

                    //第二步：将文件剪切至类别文件夹
                    if (!FileHelper.IsExistFile(Server.MapPath(categoryFolder + filename)))
                    {
                        FileHelper.MoveFile("/Temp/" + filename, categoryFolder + filename); //移动文件，将文件从临时文件夹转移至归档文件

                        //第三步：生成缩略图
                        NFine.Code.Common.MakeThumbnail(Server.MapPath(categoryFolder + filename), Server.MapPath(thumbnailFolder + filename), 600, 350, "W", "jpg");
                    }

                    if (string.IsNullOrEmpty(filenick))
                    {
                        filenick = Path.GetFileNameWithoutExtension(Server.MapPath(categoryFolder + filename));//获取昵称
                    }

                    //第四步：入数据库形成记录
                    WebPictureEntity model = new WebPictureEntity();
                    model.F_FullName = filename;//图片全称
                    model.F_Nick = filenick;//图片昵称
                    model.F_FilePath = categoryFolder + filename;//图片路径
                    model.F_ThumbnailPath = thumbnailFolder + filename;//图片缩略图
                    model.F_FileType = filetype;//文件类型
                    model.F_Category = category;//所属类别
                    model.F_FileSize = filesize;//文件大小
                    model.F_DeleteMark = false;//未删除
                    model.F_EnabledMark = true;//可用
                    if (synchro == "True")
                    {
                        model.F_Synchro = true;
                    }
                    string keyValue = string.Empty;
                    picApp.SubmitForm(model, keyValue);//入库

                    //第五步：同步至外网，调用webservice服务
                    if (synchro == "True")
                    {
                        //同步到外网
                    }
                    //第六步：删除原始文件（备：此源文件不是归档文件）
                    FileHelper.DeleteFile(originalPath + "/" + filename);//删除源文件
                    cache.RemoveCache(cacheKey);//根据键值移除该类别数据集缓存
                }
            }
            return View();
        }
        #endregion

        #region uploadify 上传【弃用】

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

            var dash = Request["dash"];
            dash = dash + Configs.GetValue("ImgFolder");//默认固定文件夹为ImgList
            FileHelper.CreateDirectory(dash);//创建盘符存放图片文件夹

            string fullpath = dash + "/" + Filedata.FileName;
            if (!System.IO.File.Exists(fullpath))
            {
                Filedata.SaveAs(fullpath);
            }

            var data = new { };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public struct ToJsonFile
        {
            public string filename { get; set; }
            public string filetype { get; set; }
            public string filesize { get; set; }
            public string filenick { get; set; }
        }

    }
}
