using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Application.WebManage;
using NFine.Code;
using NFine.Domain.Entity.WebManage;

namespace NFine.Web.Areas.WebManage.Controllers
{
    public class NodesController : ControllerBase
    {
        private WebNodeApp nodeApp = new WebNodeApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetParentNodeJson()
        {
            var data = nodeApp.GetList();
            List<object> list = new List<object>();
            foreach (WebNodeEntity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = nodeApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword, string nodetype, string parentId)
        {
            var data = nodeApp.GetList();
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    data = data.Where(d => d.F_FullName.Contains(keyword)).ToList();
            //}
            //if (!string.IsNullOrEmpty(nodetype))
            //{
            //    data = data.Where(d => d.F_NodeType.Equals(nodetype)).ToList();
            //}
            //if (!string.IsNullOrEmpty(parentId))
            //{
            //    data = data.Where(d => d.F_ParentId.Equals(parentId)).ToList();
            //}
            var treeList = new List<TreeGridModel>();
            foreach (WebNodeEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = true;
                treeModel.entityJson = item.ToJson();
                treeModel.param1 = item.F_NodeType;
                treeList.Add(treeModel);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            if (!string.IsNullOrEmpty(parentId))
            {
                treeList = treeList.TreeWhere(t => t.parentId.Equals(parentId), "id", "parentId");
            }
            if (!string.IsNullOrEmpty(nodetype))
            {
                treeList = treeList.TreeWhere(t => t.param1.Equals(nodetype), "id", "parentId");
            }
            return Content(treeList.TreeGridJson());
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nodeApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WebNodeEntity nodeEntity, string keyValue)
        {

            nodeApp.SubmitForm(nodeEntity, keyValue);
            return Success("操作成功");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var data = nodeApp.GetForm(keyValue);
            nodeApp.Delete(data);
            return Success("删除成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledNode(string keyValue)
        {
            WebNodeEntity nodeEntity = new WebNodeEntity();
            nodeEntity.F_Id = keyValue;
            nodeEntity.F_EnabledMark = false;
            nodeApp.UpdateForm(nodeEntity);
            return Success("栏目禁用成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledNode(string keyValue)
        {
            WebNodeEntity nodeEntity = new WebNodeEntity();
            nodeEntity.F_Id = keyValue;
            nodeEntity.F_EnabledMark = true;
            nodeApp.UpdateForm(nodeEntity);
            return Success("栏目启用成功");
        }

    }
}
