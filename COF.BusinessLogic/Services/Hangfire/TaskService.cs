using COF.BusinessLogic.Models.KiotViet.Common;
using COF.BusinessLogic.Models.KiotViet.Customers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;

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

            //Thread.Sleep(TimeSpan.FromMinutes(20));

            //for (int i = 0; i < 1000; i++)
            //{
            //    await AddRandomUser();
            //}

            //Thread.Sleep(TimeSpan.FromMinutes(4));

            //for (int i = 0; i < 1000; i++)
            //{
            //    await AddRandomUser();
            //}

            //Thread.Sleep(TimeSpan.FromMinutes(4));

            //for (int i = 0; i < 1000; i++)
            //{
            //    await AddRandomUser();
            //}

            //Thread.Sleep(TimeSpan.FromMinutes(4));

            //for (int i = 0; i < 2000; i++)
            //{
            //    await AddRandomUser();
            //}

            //Thread.Sleep(TimeSpan.FromMinutes(4));

            //for (int i = 0; i < 3000; i++)
            //{
            //    await AddRandomUser();
            //}




            var canBeFetch = true;
            var pageSize = 200;
            var currentItem = 0;
            var listData = new List<KiotVietCustomerModel>();
            //var customers = await _customerService.GetAllAsync();
            //var customerCodes = customers.Select(x => x.Code).ToList();
            while (canBeFetch)
            {
                var client = new RestClient("https://public.kiotapi.com");
                var access_token = @"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE1OTYyNzE2MjcsImV4cCI6MTU5NjM1ODAyNywiaXNzIjoiaHR0cDovL2lkLmtpb3R2aWV0LnZuIiwiYXVkIjpbImh0dHA6Ly9pZC5raW90dmlldC52bi9yZXNvdXJjZXMiLCJLaW90VmlldC5BcGkuUHVibGljIl0sImNsaWVudF9pZCI6IjJmYThlMDY3LTViMjYtNGIzMC1iNDJhLTBjMDE0YjJhM2I2YyIsImNsaWVudF9SZXRhaWxlckNvZGUiOiJob2FuZ3BoYW4xMjMiLCJjbGllbnRfUmV0YWlsZXJJZCI6IjU0ODk4NyIsImNsaWVudF9Vc2VySWQiOiI4MzE5OCIsImNsaWVudF9TZW5zaXRpdmVBcGkiOiJUcnVlIiwic2NvcGUiOlsiUHVibGljQXBpLkFjY2VzcyJdfQ.Zc32aeVR-noh3a4doxX3mywXn5fJFdK96SkC-ISOosY__uxSwbOOl-OYQzLl_HYI2XHrVwikJXpkBJaJ9BVPoW2q1yeyNvHUuiw3JKU9LXLFgSqFuVkLZOLW7V671QBvJsIUWlStynXM_wm5_B1sLXxNty39ZUP1QHmTm98TMszTWvO34YTZnKqrUQafUvxYg0G88sgH078KMwKJE_MdUCXjSI4oJd5ZbJXZdgT5Do68CGEnZ5oKQx9kiswcgZP6sJrLej_0eaRrKcE-2aLsh9EOOJ5zM1-4BCORhlNPL30TVnpWUOM_jZo-S2YpujxdZECXsyinbv9r46YCSmtp1w";
                var request = new RestRequest("customers", DataFormat.Json);
                request.AddParameter("Authorization", $"Bearer {access_token}", ParameterType.HttpHeader);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Retailer", "hoangphan123");
                request.Parameters.Add(new Parameter("pageSize",    , ParameterType.QueryString));
                request.Parameters.Add(new Parameter("currentItem", currentItem, ParameterType.QueryString));
                var customerData = await client.GetAsync<PagingModel<KiotVietCustomerModel>>(request);

                var dataaa = customerData.Data.Where(x => x.CreatedDate.Date == DateTime.Now.Date).ToList();
                if (dataaa.Any())
                {
                    break;
                }

                foreach (var data in customerData.Data.OrderByDescending(x => x.CreatedDate).ToList())
                {
                    await CreateCustomer(data);
                }

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
            foreach (var data in createData.OrderByDescending(x => x.CreatedDate).ToList())
            {
                await CreateCustomer(data);
            }
        }

        public async Task CreateCustomer(KiotVietCustomerModel data)
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

        public async Task AddRandomUser()
        {
            var client = new RestClient("https://public.kiotapi.com");
            var access_token = @"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE1OTYxNzA0NDEsImV4cCI6MTU5NjI1Njg0MSwiaXNzIjoiaHR0cDovL2lkLmtpb3R2aWV0LnZuIiwiYXVkIjpbImh0dHA6Ly9pZC5raW90dmlldC52bi9yZXNvdXJjZXMiLCJLaW90VmlldC5BcGkuUHVibGljIl0sImNsaWVudF9pZCI6IjJmYThlMDY3LTViMjYtNGIzMC1iNDJhLTBjMDE0YjJhM2I2YyIsImNsaWVudF9SZXRhaWxlckNvZGUiOiJob2FuZ3BoYW4xMjMiLCJjbGllbnRfUmV0YWlsZXJJZCI6IjU0ODk4NyIsImNsaWVudF9Vc2VySWQiOiI4MzE5OCIsImNsaWVudF9TZW5zaXRpdmVBcGkiOiJUcnVlIiwic2NvcGUiOlsiUHVibGljQXBpLkFjY2VzcyJdfQ.bfHz9ZrwrG5GEzKYp0hjRSHOhJWGIIlagRLw-Pkujidvy5c43SuhGK7Sw_3ebkZoFI02BRFheHFIyHAZUjLbehpFD63RjAxl0S8icmGO2jzoNKGNI8XwYPfZ256rC1TN9hUCfWuu3Bw6luQXpkJbbmam1G1V3R-sUWJGJeBx44NNBq-nPXg_9-KfZmTrO__BniJf1bWvRb4faAQ1q3J4pS4NNX8B_2cM14K2q7RSsvmQitpWSw0WvFh_5YbFFWqFZEYQ3qk6GjZBB9v1BWL790QFupbQxMORpJlizrQfgQTxwUApAozbj9YcT-PHCD8jZneTLrCKDZh2_78-Oio9Lg";
            var request = new RestRequest("customers", DataFormat.Json);
            request.AddJsonBody(new
            {
                code = Guid.NewGuid().ToString(),
                name = GenerateName(15),
                gender = true,
                birthDate = RandomBirthDay(),
                contactNumber = RandomDigits(10),
                address = Guid.NewGuid().ToString(),
                email = $"random.{RandomDigits(10)}@gmail.com",
                branchId = 39234
            }) ;
            request.AddParameter("Authorization", $"Bearer {access_token}", ParameterType.HttpHeader);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Retailer", "hoangphan123");
            var customerData = await client.PostAsync<object>(request);
        }

        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = "0";
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public DateTime RandomBirthDay()
        { 
            Random gen = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
        
    }
}
