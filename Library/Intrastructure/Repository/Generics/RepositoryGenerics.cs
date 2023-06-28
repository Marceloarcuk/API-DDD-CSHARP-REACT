using Domain.Interfaces.Generics;
using Intrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Intrastructure.Repository.Generics
{
    public class RepositoryGenerics<X> : IGenerics<X>, IDisposable where X : class
    {
        //-------------------------------------------
        //CONTRUTOR
        //-------------------------------------------
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryGenerics()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }
        //-------------------------------------------

         //-------------------------------------------
        // IMPLEMENTAÇAO DO IGenerics
        //-------------------------------------------
        public async Task Add(X Objeto)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                await data.Set<X>().AddAsync(Objeto);
                await data.SaveChangesAsync();
            }
        }
        public async Task Delete(X Objeto)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                data.Set<X>().Remove(Objeto);
                await data.SaveChangesAsync();
            }
        }
        public async Task Update(X Objeto)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                data.Set<X>().Update(Objeto);
                await data.SaveChangesAsync();
            }
        }
        public async Task<X> GetEntityById(int Id)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                return await data.Set<X>().FindAsync(Id);
            }
        }
        public async Task<List<X>> List()
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                return await data.Set<X>().ToListAsync();
            }

        }
        //-------------------------------------------


        //-------------------------------------------
        // IMPLEMENTAÇAO DO IDisposable - SITE MICROSOFT
        //-------------------------------------------
        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);



        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion
        //-------------------------------------------
    }

}
