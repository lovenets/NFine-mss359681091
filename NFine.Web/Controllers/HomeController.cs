/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Application.WebManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.Entity.WebManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : ControllerBase
    {
     
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
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

            //获取文件完整文件名(包含绝对路径)
            //文件存放路径格式：/files/upload/{日期}/{md5}.{后缀名}
            //例如：/files/upload/20130913/43CA215D947F8C1F1DDFCED383C4D706.jpg
            string fileMD5 = Md5.md5(Filedata.FileName, 16);
            string FileEextension = Path.GetExtension(Filedata.FileName);
            string uploadDate = DateTime.Now.ToString("yyyyMMdd");

            string imgType = Request["imgType"];
            string tablename = Request["tablename"];//表名
            string recordid = Request["recordid"];//记录ID
            string virtualPath = "/";
            if (imgType == "normal")
            {
                virtualPath = string.Format("~/files/upload/{0}/{1}{2}", uploadDate, fileMD5, FileEextension);
            }
            else
            {
                virtualPath = string.Format("~/files/upload2/{0}/{1}{2}", uploadDate, fileMD5, FileEextension);
            }
            string fullFileName = this.Server.MapPath(virtualPath);

            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            if (!System.IO.File.Exists(fullFileName))
            {
                Filedata.SaveAs(fullFileName);
                if (tablename != null && recordid != null)
                {
                    //保存附件记录
                    WebAttachmentEntity attachmentEntity = new WebAttachmentEntity();
                    attachmentEntity.F_TableName = tablename;
                    attachmentEntity.F_TableRecordID = recordid;
                    attachmentEntity.F_FileSize = FileHelper.ToFileSize(FileHelper.GetFileSize(fullFileName));
                    attachmentEntity.F_FileType = FileHelper.GetExtension(fullFileName);
                    attachmentEntity.F_FullName = FileHelper.GetFileName(fullFileName);
                    attachmentEntity.F_FilePath = virtualPath;
                    attachmentEntity.F_EnabledMark = true;
                    new WebAttachmentApp().SubmitForm(attachmentEntity, "");
                }
            }
            var data = new { imgtype = imgType, imgpath = virtualPath.Remove(0, 1) };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
