using Core.Services;

namespace Tests.Core.TestClasses;

public class AccessControlInTest(bool returnValue) : IAccessControl
{
    public static IAccessControl Allow => new AccessControlInTest(true);
    public static IAccessControl Deny => new AccessControlInTest(false);
    
    public bool CanClearCache => returnValue;
    public bool CanSendTestEmail => returnValue;
}