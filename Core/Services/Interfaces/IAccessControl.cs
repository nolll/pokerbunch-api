namespace Core.Services;

public interface IAccessControl
{
    bool CanClearCache { get; }
    bool CanSendTestEmail { get; }
}