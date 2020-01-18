
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;

namespace WebApplication2.Models
{
    public class WebApplication2Context : DbContext
    {
        public WebApplication2Context(DbContextOptions<WebApplication2Context> options)
            : base(options)
        {
            //essa linha cria o banco 
            //this.Database.;
        }

        public DbSet<WebApplication2.Models.Fruit> Fruit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fruitdev>();
        }
        /// <summary>  
        /// Get product by ID store procedure method.  
        /// </summary>  
        /// <param name="productId">Product ID value parameter</param>  
        /// <returns>Returns - List of product by ID</returns>  
        public  IQueryable<Fruitdev> Getsp(int productId)
        {
            // Initialization.  
            IQueryable<Fruitdev> lst;//= new IQueryable<Fruit>();

            try
            {
                // Settings.  
                SqlParameter usernameParam = new SqlParameter("@product_ID", productId.ToString() ?? (object)DBNull.Value);

                // Processing.  
                string sqlQuery = "EXEC [dbo].[sp_dev] " +
                                    "@product_ID";

                lst = this.Set<Fruitdev>().FromSqlRaw(sqlQuery, usernameParam).ToList().AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

    }
}
