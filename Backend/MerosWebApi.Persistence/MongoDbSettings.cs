using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MerosWebApi.Persistence
{
    public class MongoDbSettings
    {
        public string ConnectionURI { get; set; }

        public string DatabaseName { get; set; }
    }
}
