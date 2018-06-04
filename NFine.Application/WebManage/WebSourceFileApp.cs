//-----------------------------------------------------------------------
// <copyright file=" WebSourceFile.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebSourceFile.cs
// * history : Created by T4 06/04/2018 11:27:14 
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
    public class WebSourceFileApp
    {
		private IWebSourceFileRepository service = new WebSourceFileRepository();

		public List<WebSourceFileEntity> GetList(Pagination pagination, string queryJson)
        {
		    var expression = ExtLinq.True<WebSourceFileEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_FullName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

	    public WebSourceFileEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(WebSourceFileEntity entity)
        {
            service.Delete(entity);
        }

	    public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

		public void SubmitForm(WebSourceFileEntity entity, string keyValue)
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
	    public void UpdateForm(WebSourceFileEntity entity)
        {
            service.Update(entity);
        }
    }
}