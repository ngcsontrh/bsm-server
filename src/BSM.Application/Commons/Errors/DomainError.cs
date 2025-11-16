using FluentResults;

namespace BSM.Application.Commons.Errors;

public class DomainError(string message) : Error(message);