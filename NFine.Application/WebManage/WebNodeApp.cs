//-----------------------------------------------------------------------
// <copyright file=" WebNode.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebNode.cs
// * history : Created by T4 01/17/2017 14:17:46 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Code;
using NFine.Domain.Entity.WebManage;
using NFine.Domain.IRepository.WebManage;
using NFine.Repository.WebManage;
using System.Collections.Generic;
using System.Linq;
namespace NFine.Application.WebManage
{
    public class WebNodeApp
    {
        public string cacheKey = "webNodeCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IWebNodeRepository service = new WebNodeRepository();

        public List<WebNodeEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<WebNodeEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_FullName.Contains(keyword));
            }
            if (!queryParam["parentId"].IsEmpty())
            {
                string parentId = queryParam["parentId"].ToString();
                expression = expression.And(t => t.F_ParentId.Equals(parentId));
            }
            if (!queryParam["nodeType"].IsEmpty())
            {
                string nodeType = queryParam["nodeType"].ToString();
                expression = expression.And(t => t.F_NodeType.Equals(nodeType));
            }

            return service.FindList(expression, pagination);
        }

        public List<WebNodeEntity> GetList()
        {
            var cacheList = cache.GetCache<List<WebNodeEntity>>(cacheKey);
            if (cacheList == null)
            {
                cacheList = service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
                cache.WriteCache<List<WebNodeEntity>>(cacheList, cacheKey, "UserCacheDependency", "Web_Node");
            }
            return cacheList;
        }

        public WebNodeEntity GetForm(string keyValue)
        {
            cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            var cacheEntity = cache.GetCache<WebNodeEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(keyValue);
                cache.WriteCache<WebNodeEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Web_Node");
            }
            return cacheEntity;
        }

        public void Delete(WebNodeEntity entity)
        {
            service.Delete(entity);
        }

        public void SubmitForm(WebNodeEntity entity, string keyValue)
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

        public void UpdateForm(WebNodeEntity entity)
        {
            service.Update(entity);
        }
    }
}