using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rai4.Domain.Models;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.UserFeatures.Commands;

public class RegisterUserCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository) : IRequestHandler<RegisterUserCommand, Result>
{
    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
            return Result.Fail("Password and Confirm Password do not match.");
        
        var existingUser = await userRepository.GetByPredicateAsync(
            query => query
                .Where(u => u.Email == request.Email.ToLower().Trim()), 
            cancellationToken);
        
        if (existingUser is not null)
            return Result.Fail("A user with the provided email already exists.");
        
        var newUser = mapper.Map<User>(request);
        await userRepository.AddAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}