using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Helpers
{
    public class SuperuserFilter : IAsyncResultFilter
    {
        private readonly UserManager<User> _userManager;

        public SuperuserFilter(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(context.HttpContext.User);

            if (user.IsSuperuser)
            {
                await next();
            }
            else
            {
                context.Cancel = true;
            }
        }
    }
}
