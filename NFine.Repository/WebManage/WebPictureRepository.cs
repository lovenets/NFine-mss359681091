//-----------------------------------------------------------------------
// <copyright file=" WebPicture.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: WebPicture.cs
// * history : Created by T4 05/30/2018 10:00:22 
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
    public class WebPictureRepository : RepositoryBase<WebPictureEntity>, IWebPictureRepository
    {
    }
}