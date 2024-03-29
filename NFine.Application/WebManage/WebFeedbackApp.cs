//-----------------------------------------------------------------------
// <copyright file=" WebFeedback.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebFeedback.cs
// * history : Created by T4 01/23/2017 10:13:23 
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using NFine.Domain.IRepository.WebManage;
using NFine.Domain.ViewModel;
using NFine.Repository.WebManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.WebManage
{
    public class WebFeedbackApp
    {
        private IWebFeedbackRepository service = new WebFeedbackRepository();

        public List<WebFeedbackDto> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<WebFeedbackEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);

            var lst = service.FindList(expression, pagination);
            return Mapper.Map<List<WebFeedbackEntity>, List<WebFeedbackDto>>(lst);//Dtoӳ��

        }

        public List<WebFeedbackEntity> GetList(string queryJson)
        {
            var expression = ExtLinq.True<WebFeedbackEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public int GetAllCount(string queryJson)
        {
            var expression = ExtLinq.True<WebFeedbackEntity>();
            var queryParam = queryJson.ToJObject();
            expression = FilterParams(expression, queryParam);
            return service.IQueryable(expression).Count();
        }

        private static System.Linq.Expressions.Expression<Func<WebFeedbackEntity, bool>> FilterParams(System.Linq.Expressions.Expression<Func<WebFeedbackEntity, bool>> expression, Newtonsoft.Json.Linq.JObject queryParam)
        {
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Realname.Contains(keyword) || t.F_Mobile.Equals(keyword));
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

        public WebFeedbackEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(WebFeedbackEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(WebFeedbackEntity entity, string keyValue)
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

        public void UpdateForm(WebFeedbackEntity entity)
        {
            service.Update(entity);
        }
    }
}