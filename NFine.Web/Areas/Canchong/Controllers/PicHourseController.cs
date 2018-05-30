using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.Canchong.Controllers
{

    public class PicHourseController : Controller
    {
        private WebPictureApp picApp = new WebPictureApp();

        //
        // GET: /Canchong/Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

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
                //Console.Write(item.Name + "---" + item.DriveType);
                if (item.IsReady)
                {
                    var strgb = item.TotalSize / 1024 / 1024 / 1024;//B转GB
                    listPerson.Add(new Tuple<string, string>(item.Name, strgb.ToString() + "GB"));
                    //Console.Write(",容量：" + item.TotalSize + "，可用空间大小：" + item.TotalFreeSpace);
                }
            }
            return Json(listPerson.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        /// <summary>
        /// 获取原始磁盘资源分页列表
        /// </summary>
        public ActionResult GetImglstJson(string dash, int page = 1)
        {
            //参数0：索引，参数1：名称，参数2：图片路径，参数3：图片大小，参数4：图片扩展名
            List<Tuple<int, string, string, string, string>> imgLst = new List<Tuple<int, string, string, string, string>>();
            string cacheKey = dash;//缓存键值
            ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
            var cacheImgLst = cache.GetCache<List<Tuple<int, string, string, string, string>>>(cacheKey);
            if (cacheImgLst == null)
            {
                dash = dash + Configs.GetValue("ImgFolder");//默认固定文件夹为ImgList
                if (FileHelper.IsExistDirectory(dash))
                {
                    DirectoryInfo di = new DirectoryInfo(dash);
                    FileInfo[] files = di.GetFiles();
                    int count = 0;//图片个数
                    foreach (FileInfo item in files)
                    {
                        count++;
                        var length = System.Math.Ceiling(item.Length / 1024.0);//获取图片大小
                        imgLst.Add(new Tuple<int, string, string, string, string>(count, item.Name, item.FullName, length.ToString(), item.Extension));
                    }
                    cache.WriteCache<List<Tuple<int, string, string, string, string>>>(imgLst, dash);//写入缓存
                }
                cacheImgLst = imgLst;
            }

            var pageImgLst = GetpageImgLst(cacheImgLst, page);//分页并处理图片移动

            return Json(pageImgLst.ToList(), JsonRequestBehavior.AllowGet);
        }

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

        /// <summary>
        /// 图片归档
        /// </summary>
        /// <param name="dash">所在盘符</param>
        /// <param name="category">类别</param>
        /// <param name="dat">所选图片集json</param>
        /// <returns></returns>
        public ActionResult FnFile(string dash, string category, string dat, string cvalue)
        {
            List<ToJsonMy> my = NFine.Code.Json.ToObject<List<ToJsonMy>>(dat);
            string originalPath = dash + Configs.GetValue("ImgFolder");//获取源文件坐在盘符

            string cacheKey = dash;//缓存键值
            ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存

            //第一步：判断是否有类别文件夹，没有则创建
            string categoryFolder = "/Temp/" + cvalue + "/";
            string thumbnailFolder = "/Temp/" + cvalue + "_s/";
            FileHelper.CreateDirectory(Server.MapPath(categoryFolder));
            FileHelper.CreateDirectory(Server.MapPath(thumbnailFolder));
            if (my.Count > 0)
            {
                for (int i = 0; i < my.Count; i++)
                {
                    var filename = my[i].filename;//文件名称
                    var filetype = my[i].filetype;//文件扩展名
                    //第二步：将文件剪切至类别文件夹
                    FileHelper.MoveFile("/Temp/" + filename, categoryFolder + filename); //移动文件，将文件从临时文件夹转移至归档文件

                    //第三步：生成缩略图
                    NFine.Code.Common.MakeThumbnail(Server.MapPath(categoryFolder + filename), Server.MapPath(thumbnailFolder + filename), 600, 350, "W", "jpg");

                    //第四步：入数据库形成记录
                    WebPictureEntity model = new WebPictureEntity();
                    model.F_FullName = filename;//图片全称
                    model.F_FilePath = categoryFolder + filename;//图片路径
                    model.F_ThumbnailPath = thumbnailFolder + filename;//图片缩略图
                    model.F_FileType = filetype;//文件类型
                    model.F_Category = category;//所属类别
                    string keyValue = string.Empty;
                    picApp.SubmitForm(model, keyValue);//入库

                    //第五步：同步至外网，调用webservice服务

                    //第六步：删除原始文件（备：此源文件不是归档文件）
                    FileHelper.DeleteFile(originalPath + "/" + filename);//删除源文件
                    cache.RemoveCache(cacheKey);//根据键值移除该类别数据集缓存
                }
            }


            return View();
        }

        public struct ToJsonMy
        {
            public string filename { get; set; }
            public string filetype { get; set; }
        }

    }
}
