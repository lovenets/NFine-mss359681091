//-----------------------------------------------------------------------
// <copyright file=" WebFeedback.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebFeedback.cs
// * history : Created by T4 01/23/2017 10:13:25 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Data;
using NFine.Domain.Entity.WebManage;
using NFine.Domain.IRepository.WebManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.WebManage
{
    public class WebFeedbackRepository : RepositoryBase<WebFeedbackEntity>, IWebFeedbackRepository
    {
    }
}