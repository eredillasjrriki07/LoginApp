using LoginApp.Domain.Users;

namespace LoginApp.Application.Users;

public interface ITokenProvider
{
    string Create(User user);
}
