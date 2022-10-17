//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SenkouCards
//{
//    public partial class users : IValidatableObject
//    {
//        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//        {
//            if (username == "")
//            {
//                yield return new ValidationResult(
//                    "Username must not be empty", new[] { nameof(username) }
//                    );
//            }
//        }
//    }
//}
