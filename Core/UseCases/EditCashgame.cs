﻿using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class EditCashgame
    {
        private readonly CashgameService _cashgameService;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;
        private readonly LocationService _locationService;
        private readonly EventService _eventService;

        public EditCashgame(CashgameService cashgameService, UserService userService, PlayerService playerService, LocationService locationService, EventService eventService)
        {
            _cashgameService = cashgameService;
            _userService = userService;
            _playerService = playerService;
            _locationService = locationService;
            _eventService = eventService;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var cashgame = _cashgameService.GetById(request.Id);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(cashgame.BunchId, user.Id);
            RequireRole.Manager(user, player);
            var location = _locationService.Get(request.LocationId);
            cashgame = new Cashgame(cashgame.BunchId, location.Id, cashgame.Status, cashgame.Id);
            _cashgameService.UpdateGame(cashgame);

            if (request.EventId > 0)
            {
                _eventService.AddCashgame(request.EventId, cashgame.Id);
            }
            
            return new Result(cashgame.Id);
        }

        public class Request
        {
            public string UserName { get; }
            public int Id { get; }
            [Range(1, int.MaxValue, ErrorMessage = "Please select a location")]
            public int LocationId { get; }
            public int EventId { get; }

            public Request(string userName, int id, int locationId, int eventId)
            {
                UserName = userName;
                Id = id;
                LocationId = locationId;
                EventId = eventId;
            }
        }
        public class Result
        {
            public int CashgameId { get; private set; }

            public Result(int cashgameId)
            {
                CashgameId = cashgameId;
            }
        }
    }
}
