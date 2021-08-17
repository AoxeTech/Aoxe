using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Zaaby.Server
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;

        public UnitOfWorkMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, IDbTransaction dbTransaction)
        {
            await _next(context);
            try
            {
                dbTransaction.Commit();
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }
    }
}