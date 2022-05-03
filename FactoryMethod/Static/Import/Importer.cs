using System;
using System.IO;
namespace FactoryMethod.Static.Import
{
    public class ImportResult
    {
        public bool IsSuccess { get; set; }
        public int TotalObjects { get; set; }
        public int SuccessObjects { get; set; }
        public int ErrorObjects { get; set; }
    }

    public interface IImporter
    {
        public ImportResult ExecuteImport();
    }

    public class ExcelImporter : IImporter
    {
        public ImportResult ExecuteImport()
        {
            //логика по импорту из Excel
            return new ImportResult();
        }
    }

    public class JsonImporter : IImporter
    {
        public ImportResult ExecuteImport()
        {
            //логика по импорту из Json
            return new ImportResult();
        }
    }

    public static class ImportFactory
    {
        public static IImporter GetImporter(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            
            return extension switch
            {
                ".json" => new JsonImporter(),
                ".xlsx" => new ExcelImporter(),
                _ => throw new Exception("Некорректное расширение файла")
            };
        }
    }
}