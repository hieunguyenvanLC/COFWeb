using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FV = FluentValidation.Results;

namespace COF.BusinessLogic.Settings
{
    public class BusinessLogicResult<T>
    {
        public bool Success { get; set; }
        public FV.ValidationResult Validations { get; set; }
        public T Result { get; set; }
    }
}
