//-----------------------------------------------------------------------
// <copyright file=" WebContent.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebContent.cs
// * history : Created by T4 01/19/2017 10:59:42 
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
    public class WebContentRepository : RepositoryBase<WebContentEntity>, IWebContentRepository
    {
    }
}