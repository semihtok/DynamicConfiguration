using System;
using LiteDB;

namespace DynamicConfiguration.Data.Model
{
    public class DynamicConfig
    {
        [BsonId]
        public Guid Id { get; set; }
        // Definition of APP (Example : Weather Service)
        public string ApplicationName { get; set; }
        // Config Key
        public string Name { get; set; }
        // Config Value
        public string Value { get; set; }
        // Status of key
        public int IsActive { get; set; }
    }
}