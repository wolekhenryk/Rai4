using FluentResults;
using MediatR;

namespace Rai4.Application.Features.UserFeatures.Commands;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<Result>;