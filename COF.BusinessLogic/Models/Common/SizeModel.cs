﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Common
{
    public class SizeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UploadImageResult
    {
        public string Url { get; set; }
    }

    public enum UploadFileType
    {
        Product = 1,
        Category = 2
    }
}
