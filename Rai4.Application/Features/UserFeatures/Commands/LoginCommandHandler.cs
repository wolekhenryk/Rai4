using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rai4.Application.Dto.Auth;
using Rai4.Application.Services.Interfaces;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.UserFeatures.Commands;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByPredicateAsync(
            query => query.Where(u => u.Email == request.Email.ToLower().Trim()),
            cancellationToken);

        if (user is null)
            return Result.Fail<LoginResponseDto>("Invalid email or password.");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            return Result.Fail<LoginResponseDto>("Invalid email or password.");

        var token = jwtTokenService.GenerateToken(user);

        var response = new LoginResponseDto(token, user.Email, user.FirstName, user.LastName);
        return Result.Ok(response);
    }
}
