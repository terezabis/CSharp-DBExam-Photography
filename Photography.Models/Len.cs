﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photography.Models
{
    public class Len
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public int FocalLength { get; set; }

        public float MaxAperture { get; set; }
        
        public string CompatibleWith { get; set; }
        
        public int OwnerId { get; set; }
        public virtual Photographer Owner { get; set; }
    }
}
