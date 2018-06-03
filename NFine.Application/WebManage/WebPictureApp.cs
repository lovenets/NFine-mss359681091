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
            expression = FilterParams(expression, queryParam);
            return service.FindList(expression, pagination);
        }

        private static System.Linq.Expressions.Expression<Func<WebPictureEntity, bool>> FilterParams(System.Linq.Expressions.Expression<Func<WebPictureEntity, bool>> expression, Newtonsoft.Json.Linq.JObject queryParam)
        {
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => (t.F_FullName.Contains(keyword) || t.F_Nick.Contains(keyword) || t.F_FileType.Contains(keyword) || t.F_Alias.Contains(keyword)));
            }
            if (!queryParam["F_Category"].IsEmpty())
            {
                string F_Category = queryParam["F_Category"].ToString();
                expression = expression.And(t => (t.F_Category.Equals(F_Category)));
            }
            if (!queryParam["F_Synchro"].IsEmpty())
            {
                string F_Synchro = queryParam["F_Synchro"].ToString();
                if (F_Synchro == "False")
                {
                    expression = expression.And(t => (t.F_Synchro.ToString().Equals(F_Synchro) || t.F_Synchro.ToString() == ""));
                }
                else
                {
                    expression = expression.And(t => (t.F_Synchro.ToString().Equals(F_Synchro)));
                }

            }
            return expression;
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