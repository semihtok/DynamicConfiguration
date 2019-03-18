using System.Collections.Generic;
using DynamicConfiguration.Data.Model;

namespace DynamicConfiguration.Data.Service.Interface
{
    public abstract class IConfigService
    {
        public abstract bool AddConfig(DynamicConfig dynamicConfiguration);
        public abstract DynamicConfig GetConfig(string name);
        public abstract bool RemoveConfig(string name);
        public abstract IEnumerable<DynamicConfig> GetAllConfig();
    }
}