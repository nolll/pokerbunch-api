using System.Collections.Generic;
using System.Linq;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetTimezoneList
{
    private readonly ITimezoneRepository _timezoneRepository;

    public GetTimezoneList(ITimezoneRepository timezoneRepository)
    {
        _timezoneRepository = timezoneRepository;
    }

    public Result Execute()
    {
        var timezones = _timezoneRepository.List().Select(o => new Timezone(o.Id, o.Name)).ToList();

        return new Result(timezones);
    }

    public class Result
    {
        public IList<Timezone> Timezones { get; }

        public Result(IList<Timezone> timezones)
        {
            Timezones = timezones;
        }
    }

    public class Timezone
    {
        public string Id { get; }
        public string Name { get; }

        public Timezone(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

public class GetLocation
{
    private readonly ILocationRepository _locationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IBunchRepository _bunchRepository;

    public GetLocation(ILocationRepository locationRepository, IUserRepository userRepository, IPlayerRepository playerRepository, IBunchRepository bunchRepository)
    {
        _locationRepository = locationRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _bunchRepository = bunchRepository;
    }

    public Result Execute(Request request)
    {
        var location = _locationRepository.Get(request.LocationId);
        var bunch = _bunchRepository.Get(location.BunchId);
        var user = _userRepository.Get(request.UserName);
        var player = _playerRepository.Get(location.BunchId, user.Id);
        RequireRole.Player(user, player);

        return new Result(location.Id, location.Name, bunch.Slug);
    }

    public class Request
    {
        public string UserName { get; }
        public int LocationId { get; }

        public Request(string userName, int locationId)
        {
            UserName = userName;
            LocationId = locationId;
        }
    }

    public class Result
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }

        public Result(int id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }
    }
}