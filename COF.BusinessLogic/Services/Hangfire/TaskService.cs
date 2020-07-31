using COF.BusinessLogic.Models.KiotViet.Common;
using COF.BusinessLogic.Models.KiotViet.Customers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.Hangfire
{
    public interface ITaskService
    {
        Task ImportCustomerData();
    }
    public class TaskService : ITaskService
    {   
        private readonly ICustomerService _customerService;
        public TaskService(
            //ICustomerService customerService
            )
        {
           // _customerService = customerService;
        }

        public async Task ImportCustomerData()
        {
            var canBeFetch = true;
            var pageSize = 2;
            var currentItem = 0;
            var listData = new List<KiotVietCustomerModel>();
            //var customers = await _customerService.GetAllAsync();
            //var customerCodes = customers.Select(x => x.Code).ToList();
            while (canBeFetch)
            {
                var client = new RestClient("https://public.kiotapi.com");
                var access_token = @"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE1OTYxNzA0NDEsImV4cCI6MTU5NjI1Njg0MSwiaXNzIjoiaHR0cDovL2lkLmtpb3R2aWV0LnZuIiwiYXVkIjpbImh0dHA6Ly9pZC5raW90dmlldC52bi9yZXNvdXJjZXMiLCJLaW90VmlldC5BcGkuUHVibGljIl0sImNsaWVudF9pZCI6IjJmYThlMDY3LTViMjYtNGIzMC1iNDJhLTBjMDE0YjJhM2I2YyIsImNsaWVudF9SZXRhaWxlckNvZGUiOiJob2FuZ3BoYW4xMjMiLCJjbGllbnRfUmV0YWlsZXJJZCI6IjU0ODk4NyIsImNsaWVudF9Vc2VySWQiOiI4MzE5OCIsImNsaWVudF9TZW5zaXRpdmVBcGkiOiJUcnVlIiwic2NvcGUiOlsiUHVibGljQXBpLkFjY2VzcyJdfQ.bfHz9ZrwrG5GEzKYp0hjRSHOhJWGIIlagRLw-Pkujidvy5c43SuhGK7Sw_3ebkZoFI02BRFheHFIyHAZUjLbehpFD63RjAxl0S8icmGO2jzoNKGNI8XwYPfZ256rC1TN9hUCfWuu3Bw6luQXpkJbbmam1G1V3R-sUWJGJeBx44NNBq-nPXg_9-KfZmTrO__BniJf1bWvRb4faAQ1q3J4pS4NNX8B_2cM14K2q7RSsvmQitpWSw0WvFh_5YbFFWqFZEYQ3qk6GjZBB9v1BWL790QFupbQxMORpJlizrQfgQTxwUApAozbj9YcT-PHCD8jZneTLrCKDZh2_78-Oio9Lg";
                var request = new RestRequest("customers", DataFormat.Json);
                request.AddParameter("Authorization", $"Bearer {access_token}", ParameterType.HttpHeader);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Retailer", "hoangphan123");
                request.Parameters.Add(new Parameter("pageSize", pageSize, ParameterType.QueryString));
                request.Parameters.Add(new Parameter("currentItem", currentItem, ParameterType.QueryString));
                var customerData = await client.GetAsync<PagingModel<KiotVietCustomerModel>>(request);
                listData.AddRange(customerData.Data);
                if (customerData.Data.Count == 0)
                {
                    break;
                }
                currentItem += customerData.Data.Count;
                if (currentItem > customerData.Total)
                {
                    break;
                }
                else if (currentItem + 1 == customerData.Total)
                {
                    pageSize = 1;
                }
            }

            //var createData = listData.Where(x => !customerCodes.Contains(x.Code));

            var createData = listData.ToList();
            foreach (var data in createData.OrderByDescending(x => x.CreatedDate))
            {
                var client = new RestClient("http://localhost:10000");
                var request = new RestRequest("/api/account/register", DataFormat.Json)
                    .AddJsonBody(new CustomerRegisterModel
                    {
                        Email = data.Email,
                        FullName = data.Name,
                        PhoneNumber = data.ContactNumber,
                        Address = data.AddressDatail,
                        Gender = data.Gender,
                        Password = "User@12345",
                        UserName = data.ContactNumber,
                        Code = data.Code,
                        BirthDate = data.BirthDate
                    });
                var response = await client.ExecutePostAsync(request);
                if (!response.IsSuccessful)
                {
                    Console.WriteLine("Error");
                }
            }
        }
    }
}
