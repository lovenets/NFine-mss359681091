//-----------------------------------------------------------------------
// <copyright file=" WebAttachment.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebAttachment.cs
// * history : Created by T4 01/20/2017 13:50:00 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebAttachmentMap : EntityTypeConfiguration<WebAttachmentEntity>
    {
        public WebAttachmentMap()
        {
            this.ToTable("Web_Attachment");
            this.HasKey(t => t.F_Id);
        }
    }
}