//-----------------------------------------------------------------------
// <copyright file=" WebFeedback.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebFeedback.cs
// * history : Created by T4 01/23/2017 10:13:24 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace NFine.Domain.ViewModel
{
    /// <summary>
    /// WebFeedback Entity Model
    /// </summary>
    public class WebFeedbackDto 
    {
        public String F_Id { get; set; }
        //public String F_Contents { get; set; }
        public String F_Mobile { get; set; }
        public String F_Email { get; set; }
        public Boolean? F_IsRead { get; set; }
        public String F_Realname { get; set; }
        public String F_Company { get; set; }
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