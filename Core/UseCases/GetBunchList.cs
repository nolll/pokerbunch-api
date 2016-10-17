﻿using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Services;

namespace Core.UseCases
{
    public class GetBunchList
    {
        private readonly BunchService _bunchService;
        private readonly UserService _userService;

        public GetBunchList(BunchService bunchService, UserService userService)
        {
            _bunchService = bunchService;
            _userService = userService;
        }

        public Result Execute(AllBunchesRequest request)
        {
            var user = _userService.GetByNameOrEmail(request.UserName);
            RequireRole.Admin(user);

            var bunches = _bunchService.GetList();
            return new Result(bunches);
        }

        public Result Execute(UserBunchesRequest request)
        {
            var user = _userService.GetByNameOrEmail(request.UserName);
            var bunches = user != null ? _bunchService.GetByUserId(user.Id) : new List<Bunch>();
            
            return new Result(bunches);
        }

        public class AllBunchesRequest
        {
            public string UserName { get; }

            public AllBunchesRequest(string userName)
            {
                UserName = userName;
            }
        }

        public class UserBunchesRequest
        {
            public string UserName { get; }

            public UserBunchesRequest(string userName)
            {
                UserName = userName;
            }
        }

        public class Result
        {
            public IList<ResultItem> Bunches { get; private set; }

            public Result(IEnumerable<Bunch> bunches)
            {
                Bunches = bunches.Select(o => new ResultItem(o)).ToList();
            }
        }

        public class ResultItem
        {
            public string Slug { get; }
            public string Name { get; }
            public string Description { get; }

            public ResultItem(Bunch bunch)
            {
                Slug = bunch.Slug;
                Name = bunch.DisplayName;
                Description = bunch.Description;
            }
        }
    }
}