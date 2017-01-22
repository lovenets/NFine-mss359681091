//-----------------------------------------------------------------------
// <copyright file=" WebFriendLinks.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebFriendLinks.cs
// * history : Created by T4 01/22/2017 15:59:01 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebFriendLinksMap : EntityTypeConfiguration<WebFriendLinksEntity>
    {
		 public WebFriendLinksMap()
        {
            this.ToTable("Web_FriendLinks");
            this.HasKey(t => t.F_Id);
        }
    }
}