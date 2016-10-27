﻿using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class AppList
    {
        private readonly IAppRepository _appRepository;
        private readonly UserService _userService;

        public AppList(IAppRepository appRepository, UserService userService)
        {
            _appRepository = appRepository;
            _userService = userService;
        }

        public Result Execute(AllAppsRequest request)
        {
            var user = _userService.GetByNameOrEmail(request.CurrentUserName);
            RequireRole.Admin(user);
            var apps = _appRepository.List();

            return new Result(apps);
        }

        public Result Execute(UserAppsRequest request)
        {
            var user = _userService.GetByNameOrEmail(request.CurrentUserName);
            var apps = _appRepository.List(user.Id);

            return new Result(apps);
        }

        public abstract class Request
        {
            public string CurrentUserName { get; }

            protected Request(string currentUserName)
            {
                CurrentUserName = currentUserName;
            }
        }

        public class AllAppsRequest : Request
        {
            public AllAppsRequest(string currentUserName)
                : base(currentUserName)
            {
            }
        }

        public class UserAppsRequest : Request
        {
            public UserAppsRequest(string currentUserName)
                : base(currentUserName)
            {
            }
        }

        public class Result
        {
            public IList<AppResult> Items { get; private set; }

            public Result(IEnumerable<App> apps)
            {
                Items = apps.Select(o => new AppResult(o.Id, o.AppKey, o.Name)).ToList();
            }
        }
    }
}