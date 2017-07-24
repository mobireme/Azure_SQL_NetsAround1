using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using NetsAroundMobileApp.DataObjects;
using NetsAroundMobileApp.Models;

namespace NetsAroundMobileApp.Controllers
{
    [Authorize]
    public class CellDataController : TableController<CellData>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<CellData>(context, Request);
        }

        // GET tables/CellData
        public IQueryable<CellData> GetAllCellData()
        {
            return Query(); 
        }

        // GET tables/CellData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<CellData> GetCellData(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/CellData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<CellData> PatchCellData(string id, Delta<CellData> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/CellData
        public async Task<IHttpActionResult> PostCellData(CellData item)
        {
            CellData current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/CellData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCellData(string id)
        {
             return DeleteAsync(id);
        }
    }
}
