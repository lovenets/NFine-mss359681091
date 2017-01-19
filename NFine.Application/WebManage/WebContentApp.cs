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
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Title.Contains(keyword));
            }
            return service.FindList(expression, pagination);
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