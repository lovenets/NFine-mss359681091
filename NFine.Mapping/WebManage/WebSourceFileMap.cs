//-----------------------------------------------------------------------
// <copyright file=" WebSourceFile.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebSourceFile.cs
// * history : Created by T4 06/04/2018 11:27:16 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebSourceFileMap : EntityTypeConfiguration<WebSourceFileEntity>
    {
		 public WebSourceFileMap()
        {
            this.ToTable("Web_SourceFile");
            this.HasKey(t => t.F_Id);
        }
    }
}