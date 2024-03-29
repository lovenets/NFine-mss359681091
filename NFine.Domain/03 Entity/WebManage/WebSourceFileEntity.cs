//-----------------------------------------------------------------------
// <copyright file=" WebSourceFile.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebSourceFile.cs
// * history : Created by T4 06/04/2018 11:27:15 
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
    /// WebSourceFile Entity Model
    /// </summary>
    public class WebSourceFileEntity : IEntity<WebSourceFileEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
						public  String  F_Id { get; set; }
					public  String  F_FullName { get; set; }
					public  String  F_FilePath { get; set; }
					public  String  F_ThumbnailPath { get; set; }
					public  String  F_FileSize { get; set; }
					public  Int32?  F_SortCode { get; set; }
					public  Boolean?  F_DeleteMark { get; set; }
					public  Boolean?  F_EnabledMark { get; set; }
					public  String  F_Description { get; set; }
					public  DateTime?  F_CreatorTime { get; set; }
					public  String  F_CreatorUserId { get; set; }
					public  DateTime?  F_LastModifyTime { get; set; }
					public  String  F_LastModifyUserId { get; set; }
					public  DateTime?  F_DeleteTime { get; set; }
					public  String  F_DeleteUserId { get; set; }
					public  String  F_FileType { get; set; }
					public  String  F_Category { get; set; }
					public  String  F_Nick { get; set; }
					public  Boolean?  F_Synchro { get; set; }
					public  String  F_Alias { get; set; }
		    }
}