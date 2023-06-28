using Domain.Interfaces;
using Entities.Entities;
using Intrastructure.Configuration;
using Intrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intrastructure.Repository.Repositories
{
    public class RepositoryMessage : RepositoryGenerics<Message>, IMessage
    {
        //-------------------------------------------
        //CONTRUTOR
        //-------------------------------------------
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryMessage()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }
        //-------------------------------------------

    }
}
