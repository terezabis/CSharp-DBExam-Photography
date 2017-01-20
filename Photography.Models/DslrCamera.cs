using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photography.Models
{
    public class DslrCamera : Camera
    {
        public int? MaxShutterSpeed { get; set; }
    }
}
