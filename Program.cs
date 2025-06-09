using CsvHelper;
using CsvHelper.Excel;
using CsvHelper.Excel.EPPlus;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.IO.Packaging;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;
using XLSX_EstateSales.ApplicationDbContext.XLSX_EstateSales;
namespace XLSX_EstateSales
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var smt = ExcelRaw();
            var townList = TownList(smt);
            var addressList = AddressList(smt, townList);
            var propertyTypeList = PropertyTypeList(smt);
            var estateList = EstateList(smt, townList, propertyTypeList, addressList);

            using var context = new AppDbContext();

            // Добавяне само ако не съществуват (може да доразвиеш логика)
            await context.Towns.AddRangeAsync(townList);
            await context.Addresses.AddRangeAsync(addressList);
            await context.PropertyTypes.AddRangeAsync(propertyTypeList);
            await context.Estates.AddRangeAsync(estateList);

            await context.SaveChangesAsync();

            Console.WriteLine("Данните са записани успешно в базата.");
        }
        public static List<Excel> ExcelRaw()
        {
            using var parser = new CsvHelper.Excel.EPPlus.ExcelParser("Real_Estate_Sales_2001-2021_GL.xlsx", null, new CsvConfiguration(CultureInfo.InvariantCulture));
            using var reader = new CsvReader(parser);
            return reader.GetRecords<Excel>().ToList();
        }
        public static List<Town> TownList(List<Excel> Raw)
        {
            List<Town> townList = new List<Town>();
            var Distinct = Raw.Select(x => x.Town).Distinct().ToList();
            for (int i = 0; i < Distinct.Count; i++)
            {
                townList.Add(new Town
                {
                    TownName = Distinct[i]
                });
            }
            return townList;
        }
        public static List<Address> AddressList(List<Excel> Raw, List<Town> Towns)
        {
            List<Address> addressList = new List<Address>();
            var townDict = Towns.GroupBy(x => x.TownName).Select(x => x.First()).ToDictionary(x => x.TownName);
            for (int i = 0; i < Raw.Count; i++)
            {
                townDict.TryGetValue(Raw[i].Town, out var currentTown);
                addressList.Add(new Address
                {
                    AddressName = Raw[i].Address,
                    Town = currentTown
                });
            }
            return addressList;
        }
        public static List<PropertyType> PropertyTypeList(List<Excel> raw)
        {
            List<PropertyType> propertyTypeList = new List<PropertyType>();
            var distinctTypes = raw.Select(x => x.PropertyType).Distinct().ToList();
            distinctTypes.RemoveAll(x => x.Contains(","));
            distinctTypes.RemoveAll(x => Regex.IsMatch(x, "[0-9]"));
            distinctTypes.RemoveAll(x => x == "");
            for (int i = 0; i < distinctTypes.Count; i++)
            {
                propertyTypeList.Add(new PropertyType
                {
                    PropertyTypeName = distinctTypes[i]
                });
            }

            return propertyTypeList;
        }
        public static List<Estate> EstateList(List<Excel> raw, List<Town> townList, List<PropertyType> propertyTypeList, List<Address> addressList)
        {
            var townDict = townList.GroupBy(a => a.TownName).Select(g => g.First()).ToDictionary(x => x.TownName);
            var propertyTypeDict = propertyTypeList.GroupBy(a => a.PropertyTypeName).Select(g => g.First()).ToDictionary(p => p.PropertyTypeName);
            var addressDict = addressList.GroupBy(a => a.AddressName).Select(g => g.First()).ToDictionary(p => p.AddressName);

            List<Estate> estateList = new List<Estate>();
            for (int i = 0; i < raw.Count; i++)
            {
                raw[i].SerialNumber = Regex.Replace(raw[i].SerialNumber, "[^0-9]", "");

                int yearResult = 0;
                int.TryParse(raw[i].ListYear, out yearResult);

                string[] formats = { "MM/dd/yyyy" };

                DateTime dateRecorded;
                DateTime.TryParseExact(raw[i].DateRecorded, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateRecorded);

                //var searchTown = Smt[i].Town;
                //var town = townList.FirstOrDefault(x => x.TownName.Equals(searchTown));
                //var searchAddress = Smt[i].Address;
                //var address = addressList.FirstOrDefault(x => x.AddressName.Equals(searchAddress));
                //var searchPropertyType = Smt[i].PropertyType;
                //var propertyType = propertyTypeList.FirstOrDefault(x => x.PropertyTypeName.Equals(searchPropertyType));

                townDict.TryGetValue(raw[i].Town, out var town);
                addressDict.TryGetValue(raw[i].Address, out var address);
                propertyTypeDict.TryGetValue(raw[i].PropertyType, out var propertyType);

                estateList.Add(new Estate
                {
                    SerialNumber = raw[i].SerialNumber,
                    ListYear = yearResult,
                    DateRecorded = dateRecorded,
                    Town = town,
                    Address = address,
                    PropertyType = propertyType
                });
                
            }
            return estateList.ToList();
        }
    }

}
