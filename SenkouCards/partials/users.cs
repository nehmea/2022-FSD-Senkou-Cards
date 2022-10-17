using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenkouCards
{
    [MetadataType(typeof(UsersMetadata))]
    public partial class users
    {
        private string _confirmPassword;
        [NotMapped]
        public string confirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                if (value == "")
                {
                    throw new ArgumentException("You must confirm the password you wrote.");
                }
                else if (value != password)
                {

                    throw new ArgumentException("Passwords Must Match");
                }
                else
                {
                    _confirmPassword = value;
                }
            }
        }
    }

    public class UsersMetadata
    {


        //[Required(AllowEmptyStrings=false,ErrorMessage ="Must enter a username")]
        [StringLength(16, ErrorMessage = "Username must at least be 6 characters", MinimumLength = 6)]
        public string username
        {
            get;set;
        }

        [StringLength(16, ErrorMessage = "Password must at least be 4 characters", MinimumLength = 4)]
        public string password
        {
            get; set;
        }
    }


}
