using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace desafio_zipersoft.Models
{
    public class Contexto : DbContext
    {

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
            Database.EnsureCreated(); //assegurar que a tabela foi criada
        }

        public DbSet<Usuario> Usuario { get; set; } //refenciada a tabela usuário

    }
}
