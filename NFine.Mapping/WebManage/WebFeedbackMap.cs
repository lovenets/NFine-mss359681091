//-----------------------------------------------------------------------
// <copyright file=" WebFeedback.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebFeedback.cs
// * history : Created by T4 01/23/2017 10:13:24 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.WebManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.WebManage
{
    public class WebFeedbackMap : EntityTypeConfiguration<WebFeedbackEntity>
    {
		 public WebFeedbackMap()
        {
            this.ToTable("Web_Feedback");
            this.HasKey(t => t.F_Id);
        }
    }
}