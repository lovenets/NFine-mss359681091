//-----------------------------------------------------------------------
// <copyright file=" WebContent.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebContent.cs
// * history : Created by T4 01/19/2017 10:59:40 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using NFine.Domain.IRepository.WebManage;
using NFine.Repository.WebManage;
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NFine.Application.WebManage
{
    public class WebContentApp
    {
        private IWebContentRepository service = new WebContentRepository();

        public List<WebContentEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<WebContentEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.FindList(expression, pagination);
        }

        public List<WebContentEntity> GetList(string queryJson)
        {
            var expression = ExtLinq.True<WebContentEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public int GetAllCount(string queryJson)
        {
            var expression = ExtLinq.True<WebContentEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).Count();
        }

        private static System.Linq.Expressions.Expression<Func<WebContentEntity, bool>> FilterParams(System.Linq.Expressions.Expression<Func<WebContentEntity, bool>> expression, Newtonsoft.Json.Linq.JObject queryParam)
        {
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => (t.F_Title.Contains(keyword) || t.F_Author.Contains(keyword) || t.F_Source.Contains(keyword)));
            }
            if (!queryParam["F_NodeId"].IsEmpty())
            {
                string F_NodeId = queryParam["F_NodeId"].ToString();
                expression = expression.And(t => (t.F_NodeId.Equals(F_NodeId)));
            }
            if (!queryParam["F_FeaturesId"].IsEmpty())
            {
                string F_FeaturesId = queryParam["F_FeaturesId"].ToString();
                expression = expression.And(t => (t.F_FeaturesId.Equals(F_FeaturesId)));
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

        public WebContentEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(WebContentEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(WebContentEntity entity, string keyValue)
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

        public void UpdateForm(WebContentEntity entity)
        {
            service.Update(entity);
        }
    }
}