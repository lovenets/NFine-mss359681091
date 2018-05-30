//-----------------------------------------------------------------------
// <copyright file=" WebPicture.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebPicture.cs
// * history : Created by T4 05/30/2018 10:00:22 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebPictureMap : EntityTypeConfiguration<WebPictureEntity>
    {
		 public WebPictureMap()
        {
            this.ToTable("Web_Picture");
            this.HasKey(t => t.F_Id);
        }
    }
}