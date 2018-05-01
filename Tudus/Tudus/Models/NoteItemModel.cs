using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudus.Models
{
    public class NoteItemModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int NoteId { get; set; }
        public string Note { get; set; }
    }
}
