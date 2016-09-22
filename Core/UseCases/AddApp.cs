using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Services;

namespace Core.UseCases
{
    public class AddApp
    {
        private readonly AppService _appService;
        private readonly UserService _userService;

        public AddApp(AppService appService, UserService userService)
        {
            _appService = appService;
            _userService = userService;
        }

        public void Execute(Request request)
        {
            var appName = request.AppName;
            var apiKey = Guid.NewGuid().ToString();
            var user = _userService.GetByNameOrEmail(request.UserName);

            var app = new App(0, apiKey, appName, user.Id);

            _appService.Add(app);
        }

        public class Request
        {
            [Required(ErrorMessage = "User Name can't be empty")]
            public string UserName { get; }

            [Required(ErrorMessage = "App Name can't be empty")]
            public string AppName { get; }

            public Request(string userName, string appName)
            {
                UserName = userName;
                AppName = appName;
            }
        }
    }
}
