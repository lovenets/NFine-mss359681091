//-----------------------------------------------------------------------
// <copyright file=" WebPicture.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebPicture.cs
// * history : Created by T4 05/30/2018 10:00:20 
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
    public class WebPictureApp
    {
		private IWebPictureRepository service = new WebPictureRepository();

		public List<WebPictureEntity> GetList(Pagination pagination, string queryJson)
        {
		    var expression = ExtLinq.True<WebPictureEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_FullName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

	    public WebPictureEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(WebPictureEntity entity)
        {
            service.Delete(entity);
        }

	    public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

		public void SubmitForm(WebPictureEntity entity, string keyValue)
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
	    public void UpdateForm(WebPictureEntity entity)
        {
            service.Update(entity);
        }
    }
}