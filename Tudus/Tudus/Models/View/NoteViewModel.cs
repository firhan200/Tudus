using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudus.Models.View
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TotalItem { get; set; }
        public string CreatedAt { get; set; }
    }
}
