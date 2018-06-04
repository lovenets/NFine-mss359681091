//-----------------------------------------------------------------------
// <copyright file=" WebSourceFile.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebSourceFile.cs
// * history : Created by T4 06/04/2018 11:27:16 
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
    public class WebSourceFileRepository : RepositoryBase<WebSourceFileEntity>, IWebSourceFileRepository
    {
    }
}