using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Repositories;

namespace Core.UseCases
{
    public class AddApp
    {
        private readonly IAppRepository _appRepository;
        private readonly IUserRepository _userRepository;

        public AddApp(IAppRepository appRepository, IUserRepository userRepository)
        {
            _appRepository = appRepository;
            _userRepository = userRepository;
        }

        public AppResult Execute(Request request)
        {
            var appName = request.AppName;
            var apiKey = Guid.NewGuid().ToString();
            var user = _userRepository.Get(request.UserName);

            var app = new App(0, apiKey, appName, user.Id);

            var id = _appRepository.Add(app);
            return new AppResult(id, apiKey, appName);
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
