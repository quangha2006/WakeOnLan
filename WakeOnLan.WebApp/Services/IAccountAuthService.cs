namespace WakeOnLan.WebApp.Services;

public interface IAccountAuthService
{
    bool Validate(string accountName, string password);
}
