using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    public class DataReader
    {
        public void ImportAndPrintData(string fileToImport)
        {
            if (!File.Exists(fileToImport))
            {
                Console.Write("Plik nie istnieje"); 
                return;
            }
            List<DatabaseObject> databaseObject = ImportAndClearData(fileToImport);
            PrintData(CalculateNumberOfChildren(databaseObject));
        }

        private void PrintData(List<DatabaseObject> databaseObjects)
        {
            foreach (DatabaseObject database in databaseObjects)
            {
                if (database.Type == "DATABASE")
                {
                    Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                    // print all database's tables
                    foreach (DatabaseObject table in databaseObjects)
                    {
                        if (table.ParentType == database.Type && table.ParentName == database.Name)
                        {
                            Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                            // print all table's columns
                            foreach (DatabaseObject column in databaseObjects)
                            {
                                if (column.ParentType == table.Type && column.ParentName == table.Name)
                                {
                                    Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable ? "accepts nulls" : "with no nulls")}");
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<DatabaseObject> ImportAndClearData(string fileToImport)
        {
            return (from line in File.ReadAllLines(fileToImport)
                 .Skip(1)
                 .Where(line => !String.IsNullOrWhiteSpace(line))
                 let columns = line.Split(';')
                 select new DatabaseObject
                 {
                     Type = columns[0].Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper(),
                     Name = columns[1].Trim().Replace(" ", "").Replace(Environment.NewLine, ""),
                     Schema = columns[2].Trim().Replace(" ", "").Replace(Environment.NewLine, ""),
                     ParentName = columns[3].Trim().Replace(" ", "").Replace(Environment.NewLine, ""),
                     ParentType = columns[4].Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper(),
                     DataType = columns[5].Trim().Replace(" ", "").Replace(Environment.NewLine, ""),
                     IsNullable = columns.ElementAtOrDefault(6) == "1"
                 }).ToList();
        }
        private List<DatabaseObject> CalculateNumberOfChildren(List<DatabaseObject> databaseObjects)
        {
            for (int i = 0; i < databaseObjects.Count(); i++)
            {
                DatabaseObject importedObject = databaseObjects.ToArray()[i];
                foreach (DatabaseObject dbObj in databaseObjects)
                {
                    if (dbObj.ParentType == importedObject.Type && dbObj.ParentName == importedObject.Name)
                    {
                        importedObject.NumberOfChildren++;
                    }
                }
            }

            return databaseObjects;
        }
    }
}
