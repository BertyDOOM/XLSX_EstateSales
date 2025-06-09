using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLSX_EstateSales
{
    public class PropertyType : BaseModel
    {
        [Column(TypeName = "varchar(100)")] public string? PropertyTypeName { get; set; }
        //public Estate? Estate { get; set; }
        public override string ToString()
        {
            return $"{PropertyTypeName}";
        }
    }
}
