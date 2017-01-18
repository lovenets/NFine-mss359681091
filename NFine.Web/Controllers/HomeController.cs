/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
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
        public ActionResult View1()
        {
            return View();
        }

        ///// <summary>
        ///// 批量导出本校第一批派位学生
        ///// </summary>
        ///// <returns></returns>
        //public FileResult ExportStu2()
        //{

        //    DataTable dt = new DataTable("myTable");
        //    dt.Columns.Add("username", System.Type.GetType("System.String"));
        //    dt.Columns.Add("password", System.Type.GetType("System.String"));
        //    dt.Columns.Add("nickname", System.Type.GetType("System.String"));
        //    dt.Columns.Add("age", System.Type.GetType("System.String"));
        //    dt.Columns.Add("sex", System.Type.GetType("System.String"));
        //    DataRow dr = dt.NewRow();
        //    dr["username"] = "admin";
        //    dr["password"] = "123567";
        //    dr["nickname"] = "肖奈";
        //    dr["age"] = "18";
        //    dr["sex"] = "男";
        //    dt.Rows.Add(dr);


        //    // 写入到客户端 
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //    ms = new NPOIExcel().ToExcelFileStream(dt);

        //    return File(ms, "application/vnd.ms-excel", "第一批电脑派位生名册.xls");


        //}

        /// <summary>
        /// 批量导出本校第一批派位学生
        /// </summary>
        /// <returns></returns>
        public FileResult ExportStu2()
        {
            DataTable dt = new DataTable("myTable");
            dt.Columns.Add("username", System.Type.GetType("System.String"));
            dt.Columns.Add("password", System.Type.GetType("System.String"));
            //dt.Columns.Add("nickname", System.Type.GetType("System.String"));
            //dt.Columns.Add("age", System.Type.GetType("System.String"));
            //dt.Columns.Add("sex", System.Type.GetType("System.String"));
            DataRow dr = dt.NewRow();
            dr["username"] = "admin";
            dr["password"] = "123567";
            //dr["nickname"] = "肖奈";
            //dr["age"] = "18";
            //dr["sex"] = "男";
            dt.Rows.Add(dr);


            string schoolname = "401";
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            //List<TB_STUDENTINFOModel> listRainInfo = m_BLL.GetSchoolListAATQ(schoolname);

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("电脑号");
            row1.CreateCell(1).SetCellValue("姓名");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(dt.Rows[i]["username"].ToString());
                rowtemp.CreateCell(1).SetCellValue(dt.Rows[i]["password"].ToString());
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "第一批电脑派位生名册.xls");
        }

        public static System.IO.MemoryStream ExportExcel<T>(string title, List<T> objList, params string[] excelPropertyNames)
        {
            NPOI.SS.UserModel.IWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("Sheet1");
            NPOI.SS.UserModel.IRow row;
            NPOI.SS.UserModel.ICell cell;
            NPOI.SS.UserModel.ICellStyle cellStyle;

            int rowNum = 0;
            if (!string.IsNullOrEmpty(title))
            {
                #region 标题
                #region 标题样式
                cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中有问题
                NPOI.SS.UserModel.IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 15;
                cellStyle.SetFont(font);
                #endregion
                row = sheet.CreateRow(rowNum);
                cell = row.CreateCell(0, NPOI.SS.UserModel.CellType.String);
                cell.SetCellValue(title);
                cell.CellStyle = cellStyle;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, excelPropertyNames.Length > 0 ? excelPropertyNames.Length - 1 : 0));
                rowNum++;
                #endregion
            }

            if (objList.Count > 0)
            {
                Type type = objList[0].GetType();
                if (type != null)
                {
                    System.Reflection.PropertyInfo[] properties = type.GetProperties();
                    if (properties.Length > 0)
                    {
                        #region 表头
                        #region 表头样式
                        cellStyle = workbook.CreateCellStyle();
                        cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        #endregion
                        if (excelPropertyNames.Length > 0)
                        {
                            row = sheet.CreateRow(rowNum);
                            int count = 0;
                            for (int m = 0; m < properties.Length; m++)
                            {
                                if (excelPropertyNames.Contains(properties[m].Name))
                                {
                                    cell = row.CreateCell(count, NPOI.SS.UserModel.CellType.String);
                                    string displayName = GetDisplayNameByPropertyName(properties[m].Name);
                                    cell.SetCellValue(displayName == null ? "" : displayName);
                                    cell.CellStyle = cellStyle;
                                    count++;
                                }
                            }
                            rowNum++;
                        }
                        #endregion

                        #region 表体
                        if (excelPropertyNames.Length > 0)
                        {
                            for (int i = 0; i < objList.Count; i++)
                            {
                                row = sheet.CreateRow(i + rowNum);
                                int count = 0;
                                for (int j = 0; j < properties.Length; j++)
                                {
                                    if (excelPropertyNames.Contains(properties[j].Name))
                                    {
                                        cell = row.CreateCell(count);
                                        object obj = properties[j].GetValue(objList[i]);
                                        cell.SetCellValue(obj == null ? "" : obj.ToString());
                                        cell.CellStyle = cellStyle;
                                        count++;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            return ms;
        }

        public static string GetDisplayNameByPropertyName(string propertyName)
        {
            string result = null;
            foreach (KeyValuePair<string, string> dic in NameDictionary())
            {
                if (dic.Key == propertyName)
                {
                    result = dic.Value;
                }
                continue;
            }
            return result;
        }

        public static Dictionary<string, string> NameDictionary()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("AdminID", "编号");

            dic.Add("AdminName", "用户名");

            dic.Add("AdminMobile", "手机号");

            dic.Add("RealName", "真实姓名");

            return dic;
        }

        public ActionResult Test()
        {
            //int totalCount;
            //List<AdminModel> adminModelList = adminBLL.GetPageList(1, 10, out totalCount);

            List<AdminModel> adminModelList = new List<AdminModel>{
                new AdminModel{
                    AdminID="1",
                    AdminName="Administrator",
                    AdminMobile="1235",
                    RealName="李赛赛"
                },
                new AdminModel{
                    AdminID="2",
                    AdminName="lisai",
                    AdminMobile="1375749",
                    RealName="刘亚苹"
                }
            };
            //;
            //AdminModel my = new AdminModel();
            //my.AdminID = "1";
            //my.AdminName = "LSS";
            //my.AdminMobile = "13757493417";
            //my.RealName = "李赛赛";
            //adminModelList.Add(my);

            if (adminModelList == null)
            {
                adminModelList = new List<AdminModel>();
            }
            return File(ExportExcel<AdminModel>("表头", adminModelList, "AdminID", "AdminName", "AdminMobile", "RealName").ToArray(), "application/vnd.ms-excel", "工作簿.xls");
        }


        public class AdminModel
        {
            public string AdminID { get; set; }
            public string AdminName { get; set; }
            public string AdminMobile { get; set; }
            public string RealName { get; set; }

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
            }

            var data = new { imgtype = imgType, imgpath = virtualPath.Remove(0, 1) };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
