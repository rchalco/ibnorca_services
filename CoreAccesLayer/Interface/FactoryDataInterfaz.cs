using CoreAccesLayer.Implement;
using CoreAccesLayer.Implement.MySQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccesLayer.Interface
{
    public static class FactoryDataInterfaz
    {
        public static IRepository CreateRepository<T>(string provider) where T : DbContext, new()
        {
            IRepository repository = new MySQLRepository<T>();            
            return repository;
        }

    }
}
