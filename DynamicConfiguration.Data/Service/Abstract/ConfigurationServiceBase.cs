using System.Collections;
using System.Collections.Generic;
using DynamicConfiguration.Data.Model;

namespace DynamicConfiguration.Data.Service.Abstract
{
    public abstract class ConfigurationServiceBase
    {
        public string ConnectionString { get; set; }
        protected abstract T GetConfig<T>(DynamicConfig config);
        public abstract IEnumerable<DynamicConfig> GetAllConfig();
        public abstract bool UpdateConfig(DynamicConfig config);

        public T Read<T>(DynamicConfig config)
        {
            return GetConfig<T>(config);
        }

        public abstract bool AddConfig(DynamicConfig config);
        public abstract DynamicConfig GetOne(string key);
    }
}