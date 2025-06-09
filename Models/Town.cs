using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLSX_EstateSales
{
    public class Town : BaseModel
    {
        [Column(TypeName = "varchar(100)")] public string? TownName {  get; set; }
        public List<Address>? Address { get; set; }
    }
}
