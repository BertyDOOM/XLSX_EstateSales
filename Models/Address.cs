using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLSX_EstateSales
{
    public class Address : BaseModel 
    {
        [Column(TypeName = "varchar(100)")] public string? AddressName { get; set; }
        public Town? Town { get; set; }
    }
}
