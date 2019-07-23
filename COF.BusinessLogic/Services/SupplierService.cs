using COF.BusinessLogic.Settings;
using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using CapstoneProjectServer.Models.dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapstoneProjectServer.Models.Enumrations.Enum;

namespace COF.BusinessLogic.Services
{
    public interface ISupplierService : ILogic
    {
        Task<Supplier> GetSupplierByAccountId(int accountId);
        Supplier GetSupplierByAccountIds(int accountId);
        Task<IEnumerable<Branch>> GetAllBranch(int supplierId, string brandName);
        Task<Branch> GetBranchById(int branchId);
        Task<IEnumerable<City>> GetAllCity();
        Task<IEnumerable<District>> GetDistrictByCityId(int cityId);
        Task<BusinessLogicResult<bool>> AddBranchAsync(Branch branchDto);
        Task<BusinessLogicResult<bool>> UpdateBranchAsync(Branch branchDto);
        Task<BusinessLogicResult<Supplier>> AddSypplierAsync(SupplierRegisterDto supplierRegisterDto);
        Task<BusinessLogicResult<Supplier>> UpdateSypplierAsync(Supplier supplierRegisterDto);
        Task<IEnumerable<SupplierStatus>> GetSupplierStatus(List<int> ids = null);
        Task<IEnumerable<Supplier>> GetAllSupplier(SupplierSearchDto supplierSearchDto);
        Task<BusinessLogicResult<bool>> DeleteBranchById(int id);
        Task<Supplier> GetSupplierInfoById(int id);
        Task<BusinessLogicResult<bool>> ChangeSupplierStatus(int supplierId, int supplierStatusId, string description = "");
        Task<IEnumerable<Supplier>> GetAllSupplierBySupplierName(string supplierName, string sortKey = "price", string sort = "asc");
        Task<IEnumerable<Supplier>> GetAllSupplierBySupplierByService(string service);
        Task<IEnumerable<Feedback>> GetFeedbacksBySupplierId(int supplierId);
        Task<Supplier> GetSupplierAvatarAndName(int supplierId);
        Task<BusinessLogicResult<bool>> UpdateAvatar(string url, int supplierId);
        Task<BusinessLogicResult<bool>> UpdatePaymentEmail(string username, string password, string paymentEmail, int supplierId);
        Task<IEnumerable<Order>> GetAllOrderToDay(int supplierId);
        Task<SupplierReportDto> GetReportToDay(int supplierId, SupplierReportDto reportDto);
        Task<decimal> GetTotalRevenueInMonth(int supplierId);

    }
    public class SupplierService : BaseService, ISupplierService
    {
        public SupplierService(IRepositoryHelper repositoryHelper, IAccountService accountService)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
            this.AccountService = accountService;
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IAccountService AccountService;

        public async Task<IEnumerable<Supplier>> GetAllSupplier(SupplierSearchDto supplierSearchDto)
        {
            var repo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = await repo.GetAllSuppier(supplierSearchDto);
            return result;
        }

        public async Task<Supplier> GetSupplierByAccountId(int accountId)
        {
            var repo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = await repo.GetFirstAsync(x => x.AccountId == accountId);
            return result;
        }

        public async Task<IEnumerable<Branch>> GetAllBranch(int supplierId, string brandName)
        {
            var repo = RepositoryHelper.GetRepository<IBranchRepository>(UnitOfWork);
            var result = await repo.GetAllBranches(supplierId, brandName);
            return result;
        }

        public async Task<Branch> GetBranchById(int branchId)
        {
            var repo = RepositoryHelper.GetRepository<IBranchRepository>(UnitOfWork);
            var result = await repo.GetSimpleByIdId(branchId);
            return result;
        }

        public async Task<IEnumerable<City>> GetAllCity()
        {
            var city = await UnitOfWork.Context.Set<City>().ToListAsync();
            return city;
        }

        public async Task<IEnumerable<District>> GetDistrictByCityId(int cityId)
        {
            var district = await UnitOfWork.Context.Set<District>().Where(x => x.City.CityId == cityId).ToListAsync();
            return district;
        }

        public async Task<BusinessLogicResult<bool>> AddBranchAsync(Branch branchDto)
        {
            var context = UnitOfWork.Context.Set<Branch>();
            branchDto.IsDeleted = false;
            context.Add(branchDto);
            var saveResult = await UnitOfWork.SaveChangesAsync();
            if (saveResult.Any())
            {
                return new BusinessLogicResult<bool>()
                {
                    Success = false,
                    Validations = saveResult.AsFluentValidationResult()
                };
            }

            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };


        }

        public async Task<BusinessLogicResult<bool>> UpdateBranchAsync(Branch branchDto)
        {
            var context = UnitOfWork.Context.Set<Branch>();
            var branch = await context.SingleOrDefaultAsync(x => x.BranchId == branchDto.BranchId);

            // set City
            branch.CityId = branchDto.CityId;
            branch.City = null;

            // set District
            branch.DistrictId = branchDto.DistrictId;
            branch.District = null;

            branch.Address = branchDto.Address;
            branch.Name = branchDto.Name;

            branch.Longitude = branchDto.Longitude;
            branch.Latitude = branchDto.Latitude;
            branch.GoogleMapSearchKey = branchDto.GoogleMapSearchKey;

            if (UnitOfWork.Context.Entry(branch).State == EntityState.Detached)
            {
                context.Attach(branch);
            }
            UnitOfWork.Context.Entry(branch).State = EntityState.Modified;

            var saveResult = await UnitOfWork.SaveChangesAsync();
            if (saveResult.Any())
            {
                return new BusinessLogicResult<bool>()
                {
                    Success = false,
                    Validations = saveResult.AsFluentValidationResult()
                };
            }

            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };

        }


        public async Task<BusinessLogicResult<Supplier>> AddSypplierAsync(SupplierRegisterDto supplierRegisterDto)
        {
            var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var hasExistedAccount = await accountRepo.HasExistedUserName(supplierRegisterDto.Username);

            if (hasExistedAccount)
            {
                return new BusinessLogicResult<Supplier>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Tên tài khoản", "Tên tài khoản đã tồn tại trong hệ thống.") })
                };
            }
            var hasExistedSupplierName = await supplierRepo.HasExistedSupplierName(supplierRegisterDto.SupplierName);
            if (hasExistedSupplierName)
            {
                return new BusinessLogicResult<Supplier>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Tên nhà nhà cung cấp dịch vụ", "Tên nhà cung cấp dịch vụ đã tồn tại trong hệ thống.") })
                };
            }
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    var account = new Account()
                    {
                        AccountTypeId = (int)CapstoneProjectServer.Models.Enumrations.Enum.AccountType.Supplier,
                        Username = supplierRegisterDto.Username,
                        Password = supplierRegisterDto.Password,
                    };
                    accountRepo.Create(account);

                    var dbValresults = await UnitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult(),
                        };
                    }
                    var supply = new Supplier()
                    {
                        Name = supplierRegisterDto.SupplierName,
                        SupplierStatusId = 3,
                        CreatedBy = "system",
                        CreatedDate = DateTime.Now,
                        AccountId = account.AccountId
                    };
                    supplierRepo.Create(supply, account.AccountId.ToString());
                    dbValresults = await UnitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult()
                        };
                    }

                    transaction.Commit();
                    account.CreatedBy = account.AccountId.ToString();
                    accountRepo.Update(account, account.AccountId.ToString());
                    dbValresults = await UnitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult()
                        };
                    }
                    return new BusinessLogicResult<Supplier>
                    {
                        Success = true,
                        Result = supply
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BusinessLogicResult<Supplier>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                    };
                }
            }

        }

        public async Task<BusinessLogicResult<Supplier>> UpdateSypplierAsync(Supplier supplierRegisterDto)
        {
            try
            {
                var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
                var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
                var currentSupplier = await supplierRepo.GetSimpleById(supplierRegisterDto.SupplierId);
                currentSupplier.Name = supplierRegisterDto.Name;
                currentSupplier.Email = supplierRegisterDto.Email;
                var lastUpdated = await accountRepo.GetSimpleByIdAsync(int.Parse(currentSupplier.UpdatedBy));
                if (currentSupplier.SupplierStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.Inactive && lastUpdated.AccountTypeId == (int)CapstoneProjectServer.Models.Enumrations.Enum.AccountType.Admin)
                {
                    return new BusinessLogicResult<Supplier>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Quản trị viên đã tạm ngưng hoạt động của công ty trên hệ thống.") })
                    };
                }
                if (supplierRegisterDto.SupplierStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.Active)
                {
                    if (currentSupplier.SupplierStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.WaitingReview)
                    {
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Bạn phải đợi quản trị viên xét duyệt mới được chuyển thành trạng thái hoạt động") })
                        };
                    }

                    if (currentSupplier.Services.Count(x => x.ServiceStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.ServiceStatus.Active) == 0)
                    {
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Không thể chuyển trạng thái thành hoạt động khi chưa có dịch vụ nào nào đang hoạt động") })
                        };
                    }

                    if (currentSupplier.Branches.Count() == 0)
                    {
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Không thể chuyển trạng thái thành hoạt động khi chưa tạo chi nhánh nào") })
                        };
                    }
                    if (String.IsNullOrEmpty(currentSupplier.Avatar))
                    {
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Bạn phải cập nhật logo của công ty") })
                        };
                    }
                }
                if (supplierRegisterDto.SupplierStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.Inactive)
                {
                    if (currentSupplier.SupplierStatusId != (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.Inactive && currentSupplier.SupplierStatusId != (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.Active)
                    {
                        return new BusinessLogicResult<Supplier>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Bạn không thể chuyển thành trạng thái ngưng hoạt động khi chưa kích hoạt.") })
                        };
                    }
                }
                if (supplierRegisterDto.SupplierStatusId.HasValue)
                {
                    currentSupplier.SupplierStatusId = supplierRegisterDto.SupplierStatusId;
                }

                currentSupplier.MainBranchId = supplierRegisterDto.MainBranchId;
                currentSupplier.PhoneNumber = supplierRegisterDto.PhoneNumber;
                supplierRepo.Update(currentSupplier, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<Supplier>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }

                return new BusinessLogicResult<Supplier>
                {
                    Success = true,
                    Result = currentSupplier
                };
            }
            catch (Exception ex)
            {

                return new BusinessLogicResult<Supplier>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhật thông tin nhà cung cấp", ex.Message) })
                };
            }

        }

        public async Task<IEnumerable<SupplierStatus>> GetSupplierStatus(List<int> ids = null)
        {
            if (ids == null)
            {
                return await UnitOfWork.Context.Set<SupplierStatus>().ToListAsync();
            }
            return await UnitOfWork.Context.Set<SupplierStatus>().Where(x => ids.Contains(x.SupplierStatusId)).ToListAsync();
        }

        public async Task<BusinessLogicResult<bool>> DeleteBranchById(int id)
        {
            var context = UnitOfWork.Context.Set<Branch>();
            var branch = await context.SingleOrDefaultAsync(x => x.BranchId == id);
            branch.IsDeleted = true;

            if (UnitOfWork.Context.Entry(branch).State == EntityState.Detached)
            {
                context.Attach(branch);
            }
            UnitOfWork.Context.Entry(branch).State = EntityState.Modified;

            var saveResult = await UnitOfWork.SaveChangesAsync();
            if (saveResult.Any())
            {
                return new BusinessLogicResult<bool>()
                {
                    Success = false,
                    Validations = saveResult.AsFluentValidationResult()
                };
            }

            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };
        }

        public async Task<Supplier> GetSupplierInfoById(int id)
        {
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var supplier = await supplierRepo.GetSimpleById(id);
            return supplier;

        }

        public async Task<BusinessLogicResult<bool>> ChangeSupplierStatus(int supplierId, int supplierStatusId, string description = "")
        {
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var currentSupplier = await supplierRepo.GetSimpleById(supplierId);
            if (currentSupplier == null)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập trạng thái nhà cung cấp", "Không tìm thấy nhà cung cấp") })
                };
            }

            if (supplierStatusId == (int)SupplierStatusEnum.Active)
            {
                if (String.IsNullOrEmpty(currentSupplier.Name))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập trạng thái nhà cung cấp", "Không thể cập nhập trạng thái thành 'Hoạt động' khi mà nhà cung cấp chưa điền tên Nhà cung cấp") })
                    };
                }


                if (String.IsNullOrEmpty(currentSupplier.Email))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập trạng thái nhà cung cấp", "Không thể cập nhập trạng thái thành 'Hoạt động' khi mà nhà cung cấp chưa điền thông tin Email") })
                    };
                }

                if (String.IsNullOrEmpty(currentSupplier.PhoneNumber))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập trạng thái nhà cung cấp", "Không thể cập nhập trạng thái thành 'Hoạt động' khi mà nhà cung cấp chưa điền thông tin số điện thoại liên hệ") })
                    };
                }

                if (!currentSupplier.Branches.Any(x => x.IsDeleted != true))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập trạng thái nhà cung cấp", "Không thể cập nhập trạng thái thành 'Hoạt động' khi mà nhà cung cấp chưa tạo chi nhánh nào") })
                    };
                }
            }




            if (currentSupplier.SupplierStatusId != supplierStatusId)
            {
                currentSupplier.SupplierStatu = null;
                currentSupplier.SupplierStatusId = supplierStatusId;
            }
            currentSupplier.Description = "";
            if (supplierStatusId == (int)SupplierStatusEnum.Inactive)
            {
                currentSupplier.Description = description;
            }
            try
            {
                supplierRepo.Update(currentSupplier, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>()
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }

            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhật thông tin nhà cung cấp", ex.Message) })
                };
            }
            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };



        }

        public async Task<IEnumerable<Supplier>> GetAllSupplierBySupplierName(string supplierName, string sortKey = "price", string sort = "asc")
        {
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var data = await supplierRepo.GetAllSuppierWithSort(new SupplierSearchDto { Name = supplierName });
            if (sortKey == "price")
            {
                foreach (var item in data)
                {
                    //item.Services = item.Services.Where(x => x.ServiceStatusId == 1);
                    if (sort == "asc")
                    {
                        item.Services = item.Services.OrderBy(x => x.Price).ToList();
                    }
                    else
                    {
                        item.Services = item.Services.OrderByDescending(x => x.Price).ToList();
                    }

                }
            }
            return data;
        }

        public Supplier GetSupplierByAccountIds(int accountId)
        {
            var repo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = repo.GetFirst(x => x.AccountId == accountId);
            return result;
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplierBySupplierByService(string service)
        {
            var repo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = await repo.SearchSupplierByService(service);
            foreach (var item in result)
            {
                item.Services = item.Services.Where(x => x.ServiceStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.ServiceStatus.Active && item.Name.Contains(service)).ToList();
                foreach (var ser in item.Services)
                {
                    ser.PromotionDetails = ser.PromotionDetails.Where(x => x.Promotion.EffectiveStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.Promotion.EffectiveEndDate.Value.Date).ToList();
                }
            }
            return result;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksBySupplierId(int supplierId)
        {
            var feedbacks = await UnitOfWork.Context.Set<Feedback>()
                .Include(x => x.Customer)
                .Where(x => x.SupplierId == supplierId).OrderByDescending(x => x.CreatedDate).ToListAsync();
            return feedbacks;
        }

        public async Task<Supplier> GetSupplierAvatarAndName(int supplierId)
        {
            var repo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var result = await repo.GetByIdAsync(supplierId);
            return result;
        }

        public async Task<BusinessLogicResult<bool>> UpdateAvatar(string url, int supplierId)
        {
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var currentSupplier = await supplierRepo.GetSimpleById(supplierId);
            try
            {
                currentSupplier.Avatar = url;
                supplierRepo.Update(currentSupplier, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>()
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }

            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhật thông tin nhà cung cấp", ex.Message) })
                };
            }
            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };
        }

        public async Task<BusinessLogicResult<bool>> UpdatePaymentEmail(string username, string password, string paymentEmail, int supplierId)
        {
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var currentSupplier = await supplierRepo.GetSimpleById(supplierId);
            try
            {
                var account = await accountRepo.GetUserByUserNameAndPassword(username, password);
                if (account == null || account.AccountId != currentSupplier.AccountId)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập email", "Tên đăng nhập hoặc mật khẩu chưa đúng.") })
                    };
                }
                var dupplicatedEmail = await supplierRepo.GetFirstAsync(x => x.PaymentEmail == paymentEmail && x.SupplierId != supplierId);
                if (dupplicatedEmail != null)
                {
                    var data = await supplierRepo.GetFirstAsync(x => x.PaymentEmail == paymentEmail && x.SupplierId != supplierId);
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhập email", "Email đã được đăng kí cho một nhà cung cấp khác. Vui lòng thử lại.") })
                    };
                }
                currentSupplier.PaymentEmail = paymentEmail;
                supplierRepo.Update(currentSupplier, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>()
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }

            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhật thông tin nhà cung cấp", ex.Message) })
                };
            }
            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };
        }

        public async Task<IEnumerable<Order>> GetAllOrderToDay(int supplierId)
        {

            return null;
        }

        public async Task<SupplierReportDto> GetReportToDay(int supplierId, SupplierReportDto reportDto)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var data = await orderRepo.GetAllOrderToDay(supplierId);
            List<OrderDto> orderDtos = new List<OrderDto>();
            List<OrderDto> order4daysDtos = new List<OrderDto>();
            var order_4Days = await orderRepo.GetAllOrderIn4daysBySupplierIdAsync(supplierId);

            foreach (var item in data.OrderBy(x => x.ImplementDate))
            {
                var orderDto = new OrderDto()
                {
                    CustomerName = item.Customer.Name,
                    WorkTime = item.ImplementDate.Value.ToString("dd-MM-yyyy HH:mm"),
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address
                };
                orderDtos.Add(orderDto);
            }
            foreach (var item in order_4Days.OrderBy(x => x.ImplementDate))
            {
                var orderDto = new OrderDto()
                {
                    CustomerName = item.Customer.Name,
                    WorkTime = item.ImplementDate.Value.ToString("dd-MM-yyyy HH:mm"),
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address
                };
                order4daysDtos.Add(orderDto);
            }

            reportDto.TodayJobs = orderDtos;
            reportDto._4DaysAfteJobs = order4daysDtos;
            var totalMoney = await orderRepo.GetMoneyTotalInmonth(supplierId);
            var totalOrders = await orderRepo.GetAllOrdersInMonth(supplierId);
            reportDto.TotalRevenue = totalMoney;
            reportDto.TotalOrder = totalOrders;
            reportDto.NewCustomer = await orderRepo.GetNewCustomerInmonth(supplierId);
            reportDto.ReportInMonth = await GetReportOrderDetailInMonth(supplierId);
            reportDto.ReportInQuarter = await GetOrderReportInYear(supplierId);

            return reportDto;
        }

        public async Task<decimal> GetTotalRevenueInMonth(int supplierId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var totalMoney = await orderRepo.GetMoneyTotalInmonth(supplierId);
            return totalMoney;
        }
        private async Task<List<OrderReportInMonth>> GetReportOrderDetailInMonth(int supplierId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var orders = await orderRepo.GetAllOrderInMonth(supplierId);

            var groupbyResult = orders.GroupBy(x => x.CreatedDate.Value.Day);
            List<OrderReportInMonth> result = new List<OrderReportInMonth>();
            for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                var day = new OrderReportInMonth();
                day.Day = i + 1;
                var orderInDay = groupbyResult.SingleOrDefault(x => x.Key == i + 1);
                if (orderInDay != null)
                {
                    day.Total = orderInDay.Count();
                    day.Cancel = orderInDay.Count(x => x.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.Cancel);
                    day.Finish = orderInDay.Count(x => x.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.Done);
                    day.Inprogress = orderInDay.Count(x => x.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.InProcessing);
                }
                else
                {
                    day.Total = 0;
                    day.Cancel = 0;
                    day.Finish = 0;
                    day.Inprogress = 0;
                }
                result.Add(day);
            }

            return result;
        }

        private async Task<List<OrderReportInQuarter>> GetOrderReportInYear(int supplierId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var orders = await orderRepo.GetAllOrderBySupplierIdAsync(supplierId);
            List<OrderReportInQuarter> orderReportInQuarters = new List<OrderReportInQuarter>();
            var report = orders.GroupBy(x => x.CreatedDate.Value.Month);
            for (int i = 0; i < 12; i++)
            {
                var month = new OrderReportInQuarter();
                month.Month = i + 1;
                var orderInMonth = report.SingleOrDefault(x => x.Key == i + 1);
                if (orderInMonth != null)
                {
                    month.Total = orderInMonth.Count();
                    month.Cancel = orderInMonth.Count(x => x.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.Cancel);
                    month.Finish = orderInMonth.Count(x => x.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.Done);
                    month.Inprogress = orderInMonth.Count(x => x.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.InProcessing);
                }
                else
                {
                    month.Total = 0;
                    month.Cancel = 0;
                    month.Finish = 0;
                    month.Inprogress = 0;
                }
                orderReportInQuarters.Add(month);
            }
            return orderReportInQuarters;
        }
    }
}

