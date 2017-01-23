//-----------------------------------------------------------------------
// <copyright file=" WebAttachment.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebAttachment.cs
// * history : Created by T4 01/20/2017 13:49:59 
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
    public class WebAttachmentApp
    {
        private IWebAttachmentRepository service = new WebAttachmentRepository();

        public List<WebAttachmentEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<WebAttachmentEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.FindList(expression, pagination);
        }

        public List<WebAttachmentEntity> GetList(string queryJson)
        {
            var expression = ExtLinq.True<WebAttachmentEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public int GetAllCount(string queryJson)
        {
            var expression = ExtLinq.True<WebAttachmentEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).Count();
        }

        private static System.Linq.Expressions.Expression<Func<WebAttachmentEntity, bool>> FilterParams(System.Linq.Expressions.Expression<Func<WebAttachmentEntity, bool>> expression, Newtonsoft.Json.Linq.JObject queryParam)
        {
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_FullName.Contains(keyword));
            }
            if (!queryParam["TableName"].IsEmpty())
            {
                string TableName = queryParam["TableName"].ToString();
                expression = expression.And(t => t.F_TableName.Equals(TableName));
            }
            if (!queryParam["TableRecordID"].IsEmpty())
            {
                string TableRecordID = queryParam["TableRecordID"].ToString();
                expression = expression.And(t => t.F_TableRecordID.Equals(TableRecordID));
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

        public WebAttachmentEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(WebAttachmentEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(WebAttachmentEntity entity, string keyValue)
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

        public void UpdateForm(WebAttachmentEntity entity)
        {
            service.Update(entity);
        }
    }
}