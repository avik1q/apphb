using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ComputerStore.Models;

namespace ComputerStore.DAL
{
    public class OrdersDAL : DbContext
    {
        public DbSet<Orders> Order { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Orders>().ToTable("Orders");
        }
    }
}