using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLSX_EstateSales
{
    public class Excel
    {
        [Name("Serial Number")] public string SerialNumber { get; set; }
        [Name("List Year")] public string? ListYear { get; set; }
        [Name("Date Recorded")] public string DateRecorded { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        [Name("Assessed Value")] public string AssessedValue { get; set; }
        [Name("Sale Amount")] public string SaleAmount { get; set; }
        [Name("Sales Ratio")] public string SalesRatio { get; set; }
        [Name("Property Type")] public string PropertyType { get; set; }
        [Name("Residential Type")] public string ResidentialType { get; set; }
        [Name("Non Use Code")] public string NonUseCode { get; set; }
        [Name("Assessor Remarks")] public string AssessorRemarks { get; set; }
        [Name("OPM remarks")] public string OPMRemarks { get; set; }
        [Name("Location;;;;;")] public string Location { get; set; }
        public override string ToString()
        {
            return $"{SerialNumber} {ListYear} {DateRecorded} {Town} {Address} {AssessedValue} {SaleAmount} " +
                       $"{SalesRatio} {PropertyType} {ResidentialType} {NonUseCode} {AssessorRemarks} {OPMRemarks} {Location}";
        }
    }
}
