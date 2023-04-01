using Hb.AuthServer.Core.UnitOfWork;
using Hb.AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Data.UnitOfWork
{
    public class UnitofWork : IUnitOfWork
    {

        private readonly  AppDbContext context;

        public UnitofWork(AppDbContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
