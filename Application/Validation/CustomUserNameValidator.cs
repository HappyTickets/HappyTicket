using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Validation
{
    public class CustomUserNameValidator<TUser> : UserValidator<TUser> where TUser : class
    {
        public CustomUserNameValidator(IdentityErrorDescriber describer = null!) : base(describer) { }


        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var username = await manager.GetUserNameAsync(user);

            if (string.IsNullOrEmpty(username) || !IsValidUserName(username))
            {
                var error = new IdentityError
                {
                    Code = "Invalid UserName!",
                    Description = "UserName can only contain English letters, Arabic letters, whitespace, dashes, and underscores."
                };

                return IdentityResult.Failed(error);
            }

            return IdentityResult.Success;
        }


        private bool IsValidUserName(string username)
        {
            var regex = new Regex(@"^[a-zA-Z\u0600-\u06FF\s_-]+$"); // Regex to allow Arabic letters, English letters, whitespace, dashes, and underscores

            return regex.IsMatch(username);
        }
    }
}
