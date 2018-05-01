using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudus.Models.View
{
    public class TodoItemViewModel
    {
        public int Id { get; set; }
        public int TodoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string OnDateTime { get; set; }
        public bool IsDone { get; set; }
    }
}
