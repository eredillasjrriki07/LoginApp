using LoginApp.Domain.Users;

namespace LoginApp.Application.Users;

public interface IJwtProvider
{
    string Create(User user, IEnumerable<string> permissions);
}
