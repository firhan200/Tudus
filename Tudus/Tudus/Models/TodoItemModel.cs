using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudus.Models
{
    public class TodoItemModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TodoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime OnDateTime { get; set; }
        public bool IsDone { get; set; }
    }
}
