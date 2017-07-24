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
    public class WifiScanDetailController : TableController<WifiScanDetail>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<WifiScanDetail>(context, Request);
        }

        // GET tables/WifiScanDetail
        public IQueryable<WifiScanDetail> GetAllWifiScanDetail()
        {
            return Query(); 
        }

        // GET tables/WifiScanDetail/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<WifiScanDetail> GetWifiScanDetail(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/WifiScanDetail/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<WifiScanDetail> PatchWifiScanDetail(string id, Delta<WifiScanDetail> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/WifiScanDetail
        public async Task<IHttpActionResult> PostWifiScanDetail(WifiScanDetail item)
        {
            WifiScanDetail current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/WifiScanDetail/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteWifiScanDetail(string id)
        {
             return DeleteAsync(id);
        }
    }
}
