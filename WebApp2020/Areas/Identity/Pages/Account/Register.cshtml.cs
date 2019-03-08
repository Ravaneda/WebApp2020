using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApp2020.Models;

namespace WebApp2020.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(50)]
            public string Street { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(50)]
            public string City { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(50)]
            public string Province { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(15)]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(35)]
            public string Country { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Street = Input.Street,
                    City = Input.City,
                    Province = Input.Province,
                    PostalCode = Input.PostalCode,
                    Country = Input.Country
                };



                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário criou uma nova conta com senha.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirme seu e-mail",
                        $"Por favor, confirme sua conta por <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "MenuInicial");
                }

//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
