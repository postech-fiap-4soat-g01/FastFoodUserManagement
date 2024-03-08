﻿using AutoMapper;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Exceptions;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateUser;

public class AuthenticateUserHandler(IUserRepository userRepository, IMapper mapper, IUserAuthentication userAuthentication) : IRequestHandler<AuthenticateUserRequest, AuthenticateUserResponse>
{
    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var cpf = request.cpf.Replace(".", string.Empty).Replace("-", string.Empty);

        var user = await userRepository.GetUserByCPFAsync(cpf, cancellationToken)
            ?? throw new ObjectNotFoundException("Usuário não encontrado para esse CPF");

        var token = await userAuthentication.AuthenticateUser(user, cancellationToken);

        var response = mapper.Map<AuthenticateUserResponse>(user);
        response.Token = token;

        return response;
    }
}
