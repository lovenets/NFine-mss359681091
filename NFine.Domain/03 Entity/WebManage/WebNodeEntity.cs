//-----------------------------------------------------------------------
// <copyright file=" WebNode.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebNode.cs
// * history : Created by T4 01/17/2017 14:17:47 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.WebManage
{
    /// <summary>
    /// WebNode Entity Model
    /// </summary>
    public class WebNodeEntity : IEntity<WebNodeEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public String F_Id { get; set; }
        public String F_ParentId { get; set; }
        public String F_EnCode { get; set; }
        public String F_FullName { get; set; }
        public String F_NodePath { get; set; }
        public String F_ImageUrl { get; set; }
        public Int32? F_SortCode { get; set; }
        public Boolean? F_DeleteMark { get; set; }
        public Boolean? F_EnabledMark { get; set; }
        public String F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public String F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public String F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public String F_DeleteUserId { get; set; }
    }
}