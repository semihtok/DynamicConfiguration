using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DynamicConfiguration.Data.Factory;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Service;

namespace DynamicConfiguration.Core
{
    public class ConfigurationReader : IConfigurationReader
    {
        private string ApplicationName { get; set; }
        private string ConnectionString { get; set; }
        private int RefreshTimerIntervalInMs { get; set; }
        private string ReferencedApp { get; set; }
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// ConfigurationReader is getting & updating newest application configuration(s) from
        /// specified data source(s).
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="connectionString"></param>
        /// <param name="refreshTimerIntervalInMs"></param>
        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            // Getting referenced application name(s) from stack trace
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            ReferencedApp = stackFrame.GetMethod().DeclaringType.Assembly.FullName.Split(",")[0];

            ApplicationName = applicationName;
            ConnectionString = connectionString;
            RefreshTimerIntervalInMs = refreshTimerIntervalInMs;
            
            var cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = cancellationTokenSource.Token;
        }

        /// <summary>
        /// GetValue is getting data from local (litedb) storage. If it can't find
        /// any results from local storage, it checks from remote (redis) storage periodically.
        /// </summary>
        /// <param name="key">Key section of configuration data</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            try
            {
                // Checking is application trying to access authorized config
                if (ApplicationName == ReferencedApp)
                {
                    var liteDbService = new LiteDbService();

                    // Creating new scheduled service with specified time interval.
                    Task.Factory.StartNew(() =>
                    {
                        var isCancelled = _cancellationToken.IsCancellationRequested;

                        if (!isCancelled)
                        {
                            // ConfigFunc is creating job with status
                            async Task<bool> ConfigFunc()
                            {
                                return await ConfigJob(key, TimeSpan.FromMilliseconds(RefreshTimerIntervalInMs));
                            }

                            // Worker is taking job function to use with time interval
                            ConfigJobWorker(
                                ConfigFunc,
                                TimeSpan.FromMilliseconds(RefreshTimerIntervalInMs), _cancellationToken);
                        }
                    }, _cancellationToken);


                    var currentConfig = liteDbService.GetConfig(ApplicationName  +"." + key);

                    if (currentConfig != null)
                    {
                        if (currentConfig.Value is T)
                        {
                            return (T) Convert.ChangeType(currentConfig.Value, typeof(T));
                        }

                        return default(T);
                    }

                    return Find<T>(key);
                }

                return default(T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        /// <summary>
        /// Find is getting data from remote (redis) storage. After this
        /// it writes updated version of data into locaL (litedb) storage
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T Find<T>(string key)
        {
            var liteDbService = new LiteDbService();
            var dynamicConfig = new DynamicConfig
            {
                ApplicationName = ApplicationName,
                Name = key,
                IsActive = 1
            };

            var configurationReaderFactory = new ConfigurationServiceFactory();
            var reader = configurationReaderFactory.ProduceReader("Redis", ConnectionString);

            var redisResult = reader.Read<DynamicConfig>(dynamicConfig);

            // If redis result fits with requested data type
            if (redisResult?.Value is T)
            {
                liteDbService.RemoveConfig(key);
                liteDbService.AddConfig(redisResult);
                return (T) Convert.ChangeType(redisResult.Value, typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// ConfigJob is the definition of scheduled job of worker.
        /// It's comparing local (litedb) data with remote (redis) storage data. Also it's
        /// making neccessary updates on local.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="refreshTimerIntervalInMs"></param>
        /// <returns></returns>
        private async Task<bool> ConfigJob(string key, TimeSpan refreshTimerIntervalInMs)
        {
            var isCancelled = _cancellationToken.IsCancellationRequested;

            // if task not cancelled
            if (!isCancelled)
            {
                await Task.Delay(refreshTimerIntervalInMs, _cancellationToken);

                var liteDbService = new LiteDbService();
                var dynamicConfig = new DynamicConfig
                {
                    ApplicationName = ApplicationName,
                    Name = key,
                    IsActive = 1
                };

                var configurationReaderFactory = new ConfigurationServiceFactory();
                var reader = configurationReaderFactory.ProduceReader("Redis", ConnectionString);

                var redisResult = reader.Read<DynamicConfig>(dynamicConfig);
                var liteDbResult = liteDbService.GetConfig(key);

                if (redisResult != null && liteDbResult != null)
                {
                    // Comparing data changes
                    if (redisResult.Value != liteDbResult.Value)
                    {
                        liteDbService.RemoveConfig(key);
                        liteDbService.AddConfig(redisResult);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// ConfigJobWorker is the definition of scheduled worker.
        /// It's working and managing pre-defined job(s) until the timer end. 
        /// </summary>
        /// <param name="configFunc"></param>
        /// <param name="refreshTimerIntervalInMs"></param>
        /// <param name="cancellationToken"></param>
        private static async void ConfigJobWorker(Func<Task<bool>> configFunc, TimeSpan refreshTimerIntervalInMs,
            CancellationToken cancellationToken)
        {
            await configFunc();
            var isCancelled = cancellationToken.IsCancellationRequested;

            while (!isCancelled)
            {
                isCancelled = cancellationToken.IsCancellationRequested;

                if (isCancelled) continue;
                await Task.Delay(refreshTimerIntervalInMs, cancellationToken);
                await configFunc();
            }
        }
    }
}