﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class ModalPartialDoneViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public HttpPostedFileBase File { get; set; }

    }
}