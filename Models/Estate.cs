using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLSX_EstateSales
{
    public class Estate : BaseModel
    {
        [Column(TypeName = "varchar(100)")]public string? SerialNumber { get; set; }
        public int? ListYear { get; set; }
        public DateTime? DateRecorded { get; set; }
        public Town? Town { get; set; }
        public Address? Address { get; set; }
        public PropertyType? PropertyType { get; set; }
        public override string ToString()
        {
            return $"{SerialNumber} {ListYear} {DateRecorded} {Town.TownName} {Address.Town.TownName}";
        }
    }
}
