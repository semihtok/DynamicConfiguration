using System;
using System.Collections.Generic;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Service.Interface;
using LiteDB;

namespace DynamicConfiguration.Data.Service
{
    /// <summary>
    /// Necessary data operations for LiteDB
    /// </summary>
    public class LiteDbService : IConfigService
    {
        private const string ConnectionString = "Filename=fileconfig.db;Mode=Exclusive";
        private readonly object _asyncBlock = new object();

        public override bool AddConfig(DynamicConfig dynamicConfiguration)
        {
            lock (_asyncBlock)
            {
                try
                {
                    using (var db = new LiteDatabase(ConnectionString))
                    {
                        var customers = db.GetCollection<DynamicConfig>("configs");
                        dynamicConfiguration.Id = Guid.NewGuid();
                        customers.Insert(dynamicConfiguration);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public override DynamicConfig GetConfig(string name)
        {
            lock (_asyncBlock)
            {
                try
                {
                    using (var db = new LiteDatabase(ConnectionString))
                    {
                        var customers = db.GetCollection<DynamicConfig>("configs");
                        return customers.FindOne(i => i.Name == name);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }
        
        /// <summary>
        /// Not locking operations because using by UI.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DynamicConfig> GetAllConfig()
        {
            try
            {
                using (var db = new LiteDatabase(ConnectionString))
                {
                    var customers = db.GetCollection<DynamicConfig>("configs");
                    return customers.FindAll();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public override bool RemoveConfig(string name)
        {
            lock (_asyncBlock)
            {
                try
                {
                    using (var db = new LiteDatabase(ConnectionString))
                    {
                        var customers = db.GetCollection<DynamicConfig>("configs");
                        customers.Delete(i => i.Name == name);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }
    }
}