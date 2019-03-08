using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp2020.Models;

namespace WebApp2020.Data
{
    public class DummyData
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();
            String adminId1 = "";
            String adminId2 = "";

            String role1 = "Admin";
            String desc1 = "Este é o perfil do administrador";

            String role2 = "Member";
            String desc2 = "Este é o perfil do membro";

            string password = "123";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            }
            if (await userManager.FindByNameAsync("webscore") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "webscore",
                    Email = "celso.ravaneda@gmail.com",
                    FirstName = "Celso",
                    LastName = "Ravaneda",
                    Street = "Av.Rep.Argentina",
                    City = "Curitiba",
                    Province = "Paraná",
                    PostalCode = "80610-260",
                    Country = "Brasil",
                    PhoneNumber = "41-99226-0037"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId1 = user.Id;

            }

            if (await userManager.FindByNameAsync("visitor") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "visitor",
                    Email = "celso.ravaneda@gmail.com",
                    FirstName = "Celso",
                    LastName = "Ravaneda",
                    Street = "Av.Rep.Argentina",
                    City = "Curitiba",
                    Province = "Paraná",
                    PostalCode = "80610-260",
                    Country = "Brasil",
                    PhoneNumber = "41-99226-0037"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
                adminId2 = user.Id;

            }

        }

    }
}
