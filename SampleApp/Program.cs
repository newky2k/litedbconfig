using LiteDB;
using System;

namespace SampleApp
{
    class Program
    {
        private static string testPath = @"D:\test.db";

        static void Main(string[] args)
        {
            var mapper = BsonMapper.Global;

            mapper.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

            using (var aDb = new LiteDB.LiteDatabase(testPath))
            {
                var aCol = aDb.GetCollection<DataClass>("Datas");

                var items = aCol.FindAll();

                var newItem = new DataClass()
                {
                    Name = "Test",
                    IsActive = true,
                    Phones = new string[] { "12", "34", "56" },
                };

                aCol.Upsert(newItem);

                

            }
        }
    }
}
