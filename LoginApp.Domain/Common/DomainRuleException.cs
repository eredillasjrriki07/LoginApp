namespace LoginApp.Domain.Common;

public class DomainRuleException(string userMessage) : Exception(userMessage);
