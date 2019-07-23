using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Settings
{
    using DAV = System.ComponentModel.DataAnnotations;
    using FV = FluentValidation.Results;
    public static class ValidationResultExtensions
    {
        public static FV.ValidationResult AsFluentValidationResult(this IEnumerable<DAV.ValidationResult> dbValErrors)
        {
            FV.ValidationResult result = new FV.ValidationResult();
            foreach (var dbError in dbValErrors)
            {
                result.Errors.Add(new FV.ValidationFailure(dbError.MemberNames.First(), dbError.ErrorMessage));
            }

            return result;
        }
    }
}
