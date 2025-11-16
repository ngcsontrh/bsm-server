using FluentResults;

namespace BSM.Application.Commons.Errors;

public class BusinessError(string message) : Error(message);