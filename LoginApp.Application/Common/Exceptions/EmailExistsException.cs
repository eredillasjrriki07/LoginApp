namespace LoginApp.Application.Common.Exceptions;

public class EmailExistsException(string email) : Exception($"A user with email '{email}' already exists.");
