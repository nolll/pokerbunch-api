using Core.Services;

namespace Api.Auth;

public interface IAuth
{
    IPrincipal Principal { get; }
}