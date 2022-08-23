namespace ConsoleApp.Models
{
    internal class DatabaseObject : ImportedObject
    {
        public string Schema { get; set; }
        public string ParentName { get; set; }
        public string ParentType { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public int NumberOfChildren { get; set; }
    }
}
