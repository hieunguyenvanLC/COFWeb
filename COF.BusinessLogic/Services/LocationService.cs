using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using CapstoneProjectServer.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Device.Location;
namespace COF.BusinessLogic.Services
{
    public interface ILocationService : ILogic
    {
        Task<IEnumerable<Supplier>> SearchSupplierWithLocationByCityAndDistrictAsync(string city, string district);
        Task<IEnumerable<Supplier>> SearchSupplierWithLatitudeAndLongitude(string latitude, string longitude, string serviceName);
    }
    public class LocationService : BaseService, ILocationService
    {
        public LocationService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;

        public async Task<IEnumerable<Supplier>> SearchSupplierWithLocationByCityAndDistrictAsync(string city, string district)
        {
            var supplierRepository = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = await supplierRepository.SearchSupplierByLocation(city, district);
            var searchLocationDto = new SupplierSearchLocationDto();
            foreach (var item in result)
            {
                item.Branches = item.Branches.Where(x => x.City.Name.Contains(city) && x.District.Name.Contains(district)).ToList();

                item.Services = item.Services.Where(x => x.ServiceStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.ServiceStatus.Active).ToList();
                foreach (var service in item.Services)
                {
                    service.PromotionDetails = service.PromotionDetails.Where(x => x.Promotion.EffectiveStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.Promotion.EffectiveEndDate.Value.Date).ToList();
                }

            }
            return result;
        }

        public async Task<IEnumerable<Supplier>> SearchSupplierWithLatitudeAndLongitude(string latitude, string longitude, string serviceName)
        {
            var supplierRepository = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = await supplierRepository.SearchSupplierByLatLong(serviceName);
            var allBranches = result.SelectMany(x => x.Branches).ToList();
            allBranches = SortBranchesWithLatLong(allBranches, latitude, longitude);

            List<Supplier> suppliers = new List<Supplier>();
            foreach (var item in allBranches)
            {
                var currentSupplier = result.SingleOrDefault(x => x.SupplierId == item.SupplierId);
                if (!suppliers.Any(x => x.SupplierId == item.SupplierId))
                {

                    currentSupplier.Branches = new List<Branch> { item };
                    var otherBranches = allBranches.Where(x => x.SupplierId == item.SupplierId && x.BranchId != item.BranchId).ToList();
                    foreach (var hihi in otherBranches)
                    {
                        currentSupplier.Branches.Add(hihi);
                    }
                    suppliers.Add(currentSupplier);
                }
            }

            foreach (var item in suppliers)
            {

                item.Services = item.Services.Where(x => x.ServiceStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.ServiceStatus.Active && x.Name.ToLower().Contains(serviceName.ToLower())).ToList();
                foreach (var service in item.Services)
                {
                    service.PromotionDetails = service.PromotionDetails.Where(x => x.Promotion.EffectiveStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.Promotion.EffectiveEndDate.Value.Date).ToList();
                }

            }
            return suppliers;
        }

        private List<Branch> SortBranchesWithLatLong(List<Branch> branchesSrc, string la, string lo)
        {
            var currentLocation = new GeoCoordinate(double.Parse(la), double.Parse(lo));
            branchesSrc
                .Sort((x, y) => DistanceBetween2Points(currentLocation, x.Latitude, x.Longitude).CompareTo(DistanceBetween2Points(currentLocation, y.Latitude, y.Longitude)));
            return branchesSrc;
        }
        private double DistanceBetween2Points(GeoCoordinate currentLocation, string la, string lo)
        {
            var eCoord = new GeoCoordinate(double.Parse(la), double.Parse(lo));

            return currentLocation.GetDistanceTo(eCoord);
        }
    }
}
