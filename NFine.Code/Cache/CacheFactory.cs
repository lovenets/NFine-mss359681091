﻿/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Code
{
    public class CacheFactory
    {
        public static ICache Cache()
        {
            return new Cache();
        }
    }
}
