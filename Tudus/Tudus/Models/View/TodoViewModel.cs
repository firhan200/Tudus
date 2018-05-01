using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudus.Models.View
{
    public class TodoViewModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string TotalTodo { get; set; }
        public bool IsUpcoming { get; set; }
    }
}
