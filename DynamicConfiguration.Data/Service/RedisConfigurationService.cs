using System;
using System.Collections.Generic;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Service.Abstract;
using ServiceStack.Redis;

namespace DynamicConfiguration.Data.Service
{
    public sealed class RedisConfigurationService : ConfigurationServiceBase
    {
        private readonly RedisManagerPool _manager;
        private static bool _isRedisInitialized;

        public RedisConfigurationService(string connectionString)
        {
            _manager = new RedisManagerPool(connectionString);
            if (_isRedisInitialized)
            {
                RedisInitializer();
            }
        }

        protected override T GetConfig<T>(DynamicConfig config)
        {
            try
            {
                using (var client = _manager.GetClient())
                {
                    return client.Get<T>(config.ApplicationName + "." + config.Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return default(T);
        }


        public override bool AddConfig(DynamicConfig config)
        {
            try
            {
                using (var client = _manager.GetClient())
                {
                    return client.Add(config.ApplicationName + "." + config.Name, config);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public override IEnumerable<DynamicConfig> GetAllConfig()
        {
            try
            {
                using (var client = _manager.GetClient())
                {
                    var keys = client.GetAllKeys();
                    return client.GetAll<DynamicConfig>(keys).Values;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public override DynamicConfig GetOne(string key)
        {
            try
            {
                using (var client = _manager.GetClient())
                {
                    return client.Get<DynamicConfig>(key);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public override bool UpdateConfig(DynamicConfig config)
        {
            if (!ConnectionStatus()) return false;
            try
            {
                using (var client = _manager.GetClient())
                {
                    client.Delete(config.ApplicationName + "." + config.Name);
                    client.Set(config.ApplicationName + "." + config.Name, config);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private bool ConnectionStatus()
        {
            try
            {
                _manager.GetClient();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RedisInitializer()
        {
            if (!ConnectionStatus()) return;
            using (var client = _manager.GetClient())
            {
                var firstKey = new DynamicConfig
                {
                    Name = "SERVICE-A.SiteName",
                    Value = "Boyner.com.tr",
                    IsActive = 1,
                    ApplicationName = "SERVICE-A"
                };

                var secondKey = new DynamicConfig
                {
                    Name = "SERVICE-A.IsBasketEnabled",
                    Value = "1",
                    IsActive = 1,
                    ApplicationName = "SERVICE-B"
                };

                var thirdKey = new DynamicConfig
                {
                    Name = "SERVICE-A.MaxItemCount",
                    Value = "50",
                    IsActive = 0,
                    ApplicationName = "SERVICE-A"
                };

                var serviceB = new DynamicConfig
                {
                    Name = "SERVICE-B.SiteName",
                    Value = "boyner.com",
                    IsActive = 0,
                    ApplicationName = "SERVICE-B"
                };

                client.Set(firstKey.Name, firstKey);
                client.Set(secondKey.Name, secondKey);
                client.Set(thirdKey.Name, thirdKey);

                _isRedisInitialized = true;
            }
        }
    }
}