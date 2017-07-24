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
    public class WifiScanController : TableController<WifiScan>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<WifiScan>(context, Request);
        }

        // GET tables/WifiScan
        public IQueryable<WifiScan> GetAllWifiScan()
        {
            return Query(); 
        }

        // GET tables/WifiScan/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<WifiScan> GetWifiScan(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/WifiScan/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<WifiScan> PatchWifiScan(string id, Delta<WifiScan> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/WifiScan
        public async Task<IHttpActionResult> PostWifiScan(WifiScan item)
        {
            WifiScan current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/WifiScan/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteWifiScan(string id)
        {
             return DeleteAsync(id);
        }
    }
}
