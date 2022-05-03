using System;
using FactoryMethod.Classic.Example;
using FactoryMethod.Classic.LogParser;
using FactoryMethod.Static.Import;

namespace FactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            var importer = ImportFactory.GetImporter("import.json");
            Console.WriteLine(importer.ToString());
        }
    }
}
