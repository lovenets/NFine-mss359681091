//-----------------------------------------------------------------------
// <copyright file=" WebContent.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebContent.cs
// * history : Created by T4 01/19/2017 10:59:41 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebContentMap : EntityTypeConfiguration<WebContentEntity>
    {
		 public WebContentMap()
        {
            this.ToTable("Web_Content");
            this.HasKey(t => t.F_Id);
        }
    }
}