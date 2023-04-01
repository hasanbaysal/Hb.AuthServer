﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Common.Exceptions
{
    public class CustomException:Exception
    {
        public CustomException():base()
        {

        }

        public CustomException(string message):base(message)
        {

        }
        public CustomException(string message, Exception innerExceptipn):base(message, innerExceptipn)
        {

        } 
    }
}
