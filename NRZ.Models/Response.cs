﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
