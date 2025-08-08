using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Entities
{
    public class FileEntity : Entity
    {
        public string? Name { get; set; } = null;
        public string? Status { get; set; } = null;
        public long Size { get; set; }
    }
}
