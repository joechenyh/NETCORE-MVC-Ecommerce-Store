using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCORE_CA_8A.Models;
using NETCORE_CA_8A.DB;
using Microsoft.AspNetCore.Http;


namespace NETCORE_CA_8A.DB
{
    public class DBTester
    {
        protected StoreDbContext dbcontext;
        public DBTester(StoreDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
     
    }
}
