using FluentResults;

namespace BSM.Application.Commons.Errors;

public class InternalError(string message) : Error(message);