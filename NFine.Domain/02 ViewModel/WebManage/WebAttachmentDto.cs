//-----------------------------------------------------------------------
// <copyright file=" WebAttachment.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebAttachment.cs
// * history : Created by T4 01/20/2017 13:50:00 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace NFine.Domain.ViewModel
{
    /// <summary>
    /// WebAttachment Entity Model
    /// </summary>
    public class WebAttachmentDto
    {
        public String F_Id { get; set; }
        public String F_FullName { get; set; }
        public String F_FilePath { get; set; }
        public String F_FileType { get; set; }
        //public String F_ImageUrl { get; set; }
        //public String F_IconPath { get; set; }
        public String F_FileSize { get; set; }
        public String F_TableName { get; set; }
        public String F_TableRecordID { get; set; }
        public Int32? F_SortCode { get; set; }
        public Boolean? F_DeleteMark { get; set; }
        public Boolean? F_EnabledMark { get; set; }
        //public String F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public String F_CreatorUserId { get; set; }
        //public DateTime? F_LastModifyTime { get; set; }
        //public String F_LastModifyUserId { get; set; }
        //public DateTime? F_DeleteTime { get; set; }
        //public String F_DeleteUserId { get; set; }
    }
}