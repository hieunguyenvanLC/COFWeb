using COF.BusinessLogic.Settings;
using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using CapstoneProjectServer.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IAccountService : ILogic
    {
        Task<BusinessLogicResult<Account>> GetUserByUserNameAndPassword(LoginDto loginDto);
        Task<BusinessLogicResult<Customer>> CreateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<BusinessLogicResult<bool>> UpdateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<BusinessLogicResult<Customer>> CheckLoginForCustomer(LoginDto loginDto);
        Task<BusinessLogicResult<Customer>> GetCusomterInfoByIdAsync(int customerId);

    }
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;

        public async Task<BusinessLogicResult<Account>> GetUserByUserNameAndPassword(LoginDto loginDto)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var account = await repo.GetUserByUserNameAndPassword(loginDto.Username, loginDto.Password);
            return new BusinessLogicResult<Account>
            {
                Success = true,
                Result = account
            };
        }

        public async Task<BusinessLogicResult<Customer>> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var hasExistedAccount = await accountRepo.HasExistedUserName(customerCreateDto.Username);
            if (hasExistedAccount)
            {
                return new BusinessLogicResult<Customer>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", "Tên đăng nhập đã tồn tại.") })
                };
            }
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    var account = new Account();

                    account.AccountTypeId = (int)CapstoneProjectServer.Models.Enumrations.Enum.AccountType.Customer;
                    account.Username = customerCreateDto.Username;
                    account.Password = customerCreateDto.Password;
                    account.CreatedDate = DateTime.Now;
                    account.CreatedBy = "system";
                    accountRepo.Create(account, AccountId.ToString());
                    var dbValresults = await UnitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<Customer>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult(),
                        };
                    }
                    var customer = new Customer()
                    {
                        AccountId = account.AccountId,
                        CreatedDate = DateTime.Now,
                        Email = customerCreateDto.Email,
                        Name = customerCreateDto.FullName,
                        CreatedBy = "system"

                    };
                    customerRepo.Create(customer, AccountId.ToString());
                    dbValresults = await UnitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<Customer>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult()
                        };
                    }

                    transaction.Commit();
                    return new BusinessLogicResult<Customer>
                    {
                        Success = true,
                        Result = customer
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BusinessLogicResult<Customer>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                    };
                }


            }
        }

        public async Task<BusinessLogicResult<bool>> UpdateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var currentCustomer = await customerRepo.GetByIdAsync(customerCreateDto.CustomerId);
            try
            {
                currentCustomer.Email = customerCreateDto.Email;
                currentCustomer.PhoneNumber = customerCreateDto.PhoneNumber;
                currentCustomer.Name = customerCreateDto.FullName;
                currentCustomer.Address = customerCreateDto.Address;
                currentCustomer.DateOfBirth = customerCreateDto.DateOfBirth;
                customerRepo.Update(currentCustomer, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                };
            }

        }

        public async Task<BusinessLogicResult<Customer>> CheckLoginForCustomer(LoginDto loginDto)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var cusomerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var account = await repo.GetUserByUserNameAndPassword(loginDto.Username, loginDto.Password);
            if (account == null)
            {
                return new BusinessLogicResult<Customer>
                {
                    Success = true,
                    Result = null
                };
            }
            var data = await cusomerRepo.GetFirstAsync(x => x.AccountId == account.AccountId);
            return new BusinessLogicResult<Customer>
            {
                Success = true,
                Result = data
            };
        }

        public async Task<BusinessLogicResult<Customer>> GetCusomterInfoByIdAsync(int customerId)
        {
            var cusomerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var customer = await cusomerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                return new BusinessLogicResult<Customer>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("GetCustomerInfo", "Không tồn tại khách hàng với customerId trên") })
                };
            }

            return new BusinessLogicResult<Customer>
            {
                Success = true,
                Result = customer
            };
        }


    }
}
