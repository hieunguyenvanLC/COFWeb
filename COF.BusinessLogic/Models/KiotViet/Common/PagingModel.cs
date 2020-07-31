using System.Collections.Generic;

namespace COF.BusinessLogic.Models.KiotViet.Common
{
    public class PagingModel<T>
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; }
        public string Timestamp { get; set; }
    }
}
