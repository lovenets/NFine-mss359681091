/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class ModuleButtonApp
    {
        public string cacheKey = "moduleButtonCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IModuleButtonRepository service = new ModuleButtonRepository();

        public List<ModuleButtonEntity> GetList(string moduleId = "")
        {
            cacheKey = cacheKey + "0_" + moduleId ;//拼接有参key值
            var cacheList = cache.GetCache<List<ModuleButtonEntity>>(cacheKey);
            if (cacheList == null)
            {
                var expression = ExtLinq.True<ModuleButtonEntity>();
                if (!string.IsNullOrEmpty(moduleId))
                {
                    expression = expression.And(t => t.F_ModuleId == moduleId);
                }
                cacheList = service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();
                cache.WriteCache<List<ModuleButtonEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_ModuleButton");
            }
            return cacheList;
        }
        public ModuleButtonEntity GetForm(string keyValue)
        {
            cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            var cacheEntity = cache.GetCache<ModuleButtonEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(keyValue);
                cache.WriteCache<ModuleButtonEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_ModuleButton");
            }
            return cacheEntity;
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleButtonEntity.Modify(keyValue);
                service.Update(moduleButtonEntity);
            }
            else
            {
                moduleButtonEntity.Create();
                service.Insert(moduleButtonEntity);
            }
        }
        public void SubmitCloneButton(string moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data = this.GetList();
            List<ModuleButtonEntity> entitys = new List<ModuleButtonEntity>();
            foreach (string item in ArrayId)
            {
                ModuleButtonEntity moduleButtonEntity = data.Find(t => t.F_Id == item);
                moduleButtonEntity.F_Id = Common.GuId();
                moduleButtonEntity.F_ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            service.SubmitCloneButton(entitys);
        }
    }
}
