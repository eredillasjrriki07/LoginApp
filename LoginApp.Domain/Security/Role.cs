namespace LoginApp.Domain.Security;

public sealed class Role
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
}
