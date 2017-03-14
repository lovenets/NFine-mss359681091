//-----------------------------------------------------------------------
// <copyright file=" WebContent.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebContent.cs
// * history : Created by T4 01/19/2017 10:59:41 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace NFine.Domain.ViewModel
{
    /// <summary>
    /// WebContent Entity Model
    /// </summary>
    public class WebContentDto
    {
        public String F_Id { get; set; }
        public String F_NodeId { get; set; }
        public String F_FeaturesId { get; set; }
        public String F_Title { get; set; }
        //public String F_SubTitle { get; set; }
        //public String F_Contents { get; set; }
        //public String F_Tags { get; set; }
        //public String F_ImageUrl { get; set; }
        //public String F_LinkUrl { get; set; }
        public String F_Author { get; set; }
        public String F_Source { get; set; }
        public Boolean? F_Recommend { get; set; }
        public Boolean? F_Hot { get; set; }
        public Boolean? F_Top { get; set; }
        public Boolean? F_Color { get; set; }
        //public Boolean? F_LoginFilter { get; set; }
        //public Int32? F_SortCode { get; set; }
        //public Boolean? F_DeleteMark { get; set; }
        public Boolean? F_EnabledMark { get; set; }
        public String F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public String F_CreatorUserId { get; set; }
        //public DateTime? F_LastModifyTime { get; set; }
        //public String F_LastModifyUserId { get; set; }
        //public DateTime? F_DeleteTime { get; set; }
        //public String F_DeleteUserId { get; set; }
    }
}