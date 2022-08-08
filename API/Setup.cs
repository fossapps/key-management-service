using API.types;
using Microsoft.AspNetCore.Builder;
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
            .AddType<User>()
            .AddQueryType<Query>();
    }

    public static void AddGraphQlEndpoints(this WebApplication application)
    {
        application.MapGraphQL();
    }
}
