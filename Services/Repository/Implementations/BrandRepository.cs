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
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly SheyaakaDbContext _context;
        public BrandRepository(SheyaakaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
