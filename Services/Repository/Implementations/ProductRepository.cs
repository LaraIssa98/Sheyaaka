using DataAccess.Data;
using Models;
using Services.Repository.Interfaces;
using Sheyaaka.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly SheyaakaDbContext _context;
        public ProductRepository(SheyaakaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
