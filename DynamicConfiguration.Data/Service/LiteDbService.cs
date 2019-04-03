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

        private static readonly LiteDatabase Db = new LiteDatabase(ConnectionString);


        public override bool AddConfig(DynamicConfig dynamicConfiguration)
        {
            lock (_asyncBlock)
            {
                try
                {
                    var configs = Db.GetCollection<DynamicConfig>("configs");
                    dynamicConfiguration.Id = Guid.NewGuid();
                    configs.Insert(dynamicConfiguration);
                    return true;
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
                    var configs = Db.GetCollection<DynamicConfig>("configs");
                    return configs.FindOne(i => i.Name == name);
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
                var configs = Db.GetCollection<DynamicConfig>("configs");
                return configs.FindAll();
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
                    var configs = Db.GetCollection<DynamicConfig>("configs");
                    configs.Delete(i => i.Name == name);
                    return true;
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