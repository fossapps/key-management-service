﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API;

public static class Setup
{
    public static void SetupGraphqlServer(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddGraphQLServer()
            .AddApolloFederation()
            .AddApolloTracing()
            .AddErrorFilter<ExceptionHandler>()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();
    }

    public static void AddGraphQlEndpoints(this WebApplication application)
    {
        application.MapGraphQL();
    }
}
