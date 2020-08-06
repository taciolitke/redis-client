using System;
using System.Collections.Generic;
using System.Text;

namespace redis_client.Model
{
    public class ItemKey
    {
        public string Key { get; set; }

        public DateTime DateSave { get; set; }

        public static ItemKey Create(string key) => new ItemKey { 
            DateSave = DateTime.Now, 
            Key = key 
        };

        public override string ToString()
        {
            return $"Save on {DateSave} - Create {(DateTime.Now - DateSave).TotalSeconds} seconds ago.";
        }
    }
}
