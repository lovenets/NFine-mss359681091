//-----------------------------------------------------------------------
// <copyright file=" WebFriendLinks.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebFriendLinks.cs
// * history : Created by T4 01/22/2017 15:59:00 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using NFine.Domain.IRepository.WebManage;
using NFine.Repository.WebManage;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NFine.Application.WebManage
{
    public class WebFriendLinksApp
    {
        private IWebFriendLinksRepository service = new WebFriendLinksRepository();

        public List<WebFriendLinksEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<WebFriendLinksEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.FindList(expression, pagination);
        }

        public List<WebFriendLinksEntity> GetList(string queryJson)
        {
            var expression = ExtLinq.True<WebFriendLinksEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).OrderBy(t => t.F_CreatorTime).ToList();
        }

        private static System.Linq.Expressions.Expression<Func<WebFriendLinksEntity, bool>> FilterParams(System.Linq.Expressions.Expression<Func<WebFriendLinksEntity, bool>> expression, Newtonsoft.Json.Linq.JObject queryParam)
        {
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_FullName.Contains(keyword));
            }
            if (!queryParam["nodeid"].IsEmpty())
            {
                string keyword = queryParam["nodeid"].ToString();
                expression = expression.And(t => t.F_NodeID.Equals(keyword));
            }
            if (!queryParam["linktype"].IsEmpty())
            {
                string keyword = queryParam["linktype"].ToString();
                expression = expression.And(t => t.F_LinkType.Equals(keyword));
            }
            if (!queryParam["F_DeleteMark"].IsEmpty())
            {
                string F_DeleteMark = queryParam["F_DeleteMark"].ToString();
                expression = expression.And(t => (t.F_DeleteMark.Equals(F_DeleteMark)));
            }
            if (!queryParam["F_EnabledMark"].IsEmpty())
            {
                string F_EnabledMark = queryParam["F_EnabledMark"].ToString();
                expression = expression.And(t => (t.F_EnabledMark.Equals(F_EnabledMark)));
            }
            return expression;
        }


        public WebFriendLinksEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(WebFriendLinksEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(WebFriendLinksEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
            }
        }
        public void UpdateForm(WebFriendLinksEntity entity)
        {
            service.Update(entity);
        }
    }
}