//-----------------------------------------------------------------------
// <copyright file=" WebNode.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebNode.cs
// * history : Created by T4 01/17/2017 14:17:47 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebNodeMap : EntityTypeConfiguration<WebNodeEntity>
    {
        public WebNodeMap()
        {
            this.ToTable("Web_Node");
            this.HasKey(t => t.F_Id);
        }
    }
}