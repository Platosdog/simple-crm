﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace SimpleCrm.WebApi.ApiControllers {
    public class AuthController : Controller
    {
            private readonly UserManager < CrmUser > _userManager;
    private readonly JwtFactory _jwtFactory;

    public AuthController(UserManager < CrmUser > userManager, JwtFactory jwtFactory)
        {
            this._userManager = userManager;
            this._jwtFactory = jwtFactory;
        }

        [HttpPost("login")]
          public async Task < IActionResult > Post([FromBody] CredentialsViewModel credentials)
        { 
            if (!ModelState.IsValid) {
                return UnprocessableEntity(ModelState);
            }

            
            var user = await Authenticate(credentials.EmailAddress, credentials.Password);
            if (user == null) {
                return UnprocessableEntity("Invalid username or password.");
            }

            
            var userModel = await GetUserData(user);
            
            return Ok(userModel);
        }
        
    private async Task < CrmUser > Authenticate(string emailAddress, string password)
        {
            if (emailAddress == "" || password == "") {
                return null;
            }

            var currentUser = await _userManager.FindByEmailAsync(emailAddress);
            if (currentUser == null) {
                var rand = new Random(DateTime.Now.Second).Next(2, 38);
                await Task.Delay(rand);
                return null;
            }

            var isUser = await _userManager.CheckPasswordAsync(currentUser, password);
            if (isUser) {
                return currentUser;
            }

            return null;
        }

    private async Task < UserSummaryViewModel > GetUserData(CrmUser user)
        {
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0) {
                roles.Add("prospect");
            }

            var jwt = await _jwtFactory.GenerateEncodedToken(user.UserName, _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString()));
            var userModel = new UserSummaryViewModel
            {
                Id = user.Id,
                    Name = user.UserName,
                    emailAddress = user.Email
            };
        }
    }
}