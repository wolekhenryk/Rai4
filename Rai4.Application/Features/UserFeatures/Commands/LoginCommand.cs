using FluentResults;
using MediatR;
using Rai4.Application.Dto.Auth;

namespace Rai4.Application.Features.UserFeatures.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponseDto>>;
