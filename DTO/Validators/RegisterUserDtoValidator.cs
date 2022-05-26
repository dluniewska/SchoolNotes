//using FluentValidation;
//using School.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace School.DTO.Validators
//{
//    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
//    {
//        public RegisterUserDtoValidator(ApiContext apicontext)
//        {
//            RuleFor(x => x.Email).NotEmpty().EmailAddress();
//            RuleFor(x => x.Password).MinimumLength(6);
//            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
//            RuleFor(e => e.Email).Custom((value, context) =>
//            {
//                var emailInUse = apicontext.Users.Any(u => u.Email == value);
//                if (emailInUse)
//                {
//                    context.AddFailure("Email", "That email is taken!");
//                }
//            });
//        }
//    }
//}
