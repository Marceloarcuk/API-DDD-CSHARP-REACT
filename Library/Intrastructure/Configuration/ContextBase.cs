using Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intrastructure.Configuration
{
    public class ContextBase : IdentityDbContext<ApplicationUser>
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { set; get; }
        public DbSet<Message> Message { set; get; }
        /*
        public DbSet<Categoria> Categoria { set; get; }
        public DbSet<Despesa> Despesa { set; get; }
        */



        //-------------------------------------------
        // CONEXAO COM BANCO DE DADOS
        // (caso nao encontre a string em outro lugar ele usa esta)
        //-------------------------------------------
        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
            optionsbuilder.UseSqlServer("Data Source=MACSAMSUNGNOTE\\SQLEXPRESS;Initial Catalog=API_DDD_2022;Integrated Security=False;User ID=sa;Password=12345;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            base.OnConfiguring(optionsbuilder);
        }
        //-------------------------------------------

        //-------------------------------------------
        // MAPEAR O ID DA TABELA  PARA TABELA
        //-------------------------------------------
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(x => x.Id);
            base.OnModelCreating(builder);
        }
        //-------------------------------------------

        //-------------------------------------------
        // STRING DE CONEXAO DO BANCO DE DADOS
        //-------------------------------------------
        public string ObterStringConexao()
        {
            return ("Data Source=MACSAMSUNGNOTE\\SQLEXPRESS;Initial Catalog=API_DDD_2022;Integrated Security=False;User ID=sa;Password=12345;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
        }
        //-------------------------------------------
    }
}
