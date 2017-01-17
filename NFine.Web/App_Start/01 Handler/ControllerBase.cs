﻿using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web
{
    [HandlerLogin]
    public abstract class ControllerBase : Controller
    {
        private OrganizeApp organizeApp = new OrganizeApp();
        private RoleApp roleApp = new RoleApp();
        private DutyApp dutyApp = new DutyApp();

        public Log FileLog
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Details()
        {
            return View();
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }

        /// <summary>
        /// 绑定公司
        /// </summary>
        /// <param name="IsDefault"></param>
        /// <param name="Value"></param>
        public void BindOrganizeId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var data = organizeApp.GetList();
            foreach (OrganizeEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_OrganizeId = new SelectList(list, "Value", "Text", Value);
        }

        /// <summary>
        /// 绑定部门
        /// </summary>
        /// <param name="IsDefault"></param>
        /// <param name="Value"></param>
        public void BindDepartmentId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var data = organizeApp.GetList();
            foreach (OrganizeEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_DepartmentId = new SelectList(list, "Value", "Text", Value);
        }

        /// <summary>
        /// 绑定角色
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="IsDefault"></param>
        public void BindRoleId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var data = roleApp.GetList();
            foreach (RoleEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_RoleId = new SelectList(list, "Value", "Text", Value);
        }

        /// <summary>
        /// 绑定职责
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="IsDefault"></param>
        public void BindDutyId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var data =dutyApp.GetList();
            foreach (RoleEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_DutyId = new SelectList(list, "Value", "Text", Value);
        }

    }
}
