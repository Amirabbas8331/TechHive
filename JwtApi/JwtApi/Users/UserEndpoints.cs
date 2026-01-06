namespace JwtApi.Api.Users;

public static class UserEndpoints
{
    private const string Tag = "Users";
    public const string VerifyEmail = "VerifyEmail";

    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("users/register", async (RegisterUser.Request request, RegisterUser useCase) =>
        {
            User user = await useCase.Handle(request);
            return Results.Ok(user);
        }
          ).WithTags(Tag);

        builder.MapPost("users/login", async (LoginUser.Request request, LoginUser useCase) =>
        {
            var result = await useCase.Handle(request);
            return Results.Ok(new
            {
                accessToken = result.Token,
                role = result.role,
                expiresIn = 3600,

            });
        }).WithTags(Tag);


        builder.MapGet("users/verify-email", async (Guid token, VerifyEmail useCase) =>
        {
            bool success = await useCase.Handle(token);

            return success ? Results.Ok() : Results.BadRequest("Verification token expired");
        })
        .WithTags(Tag)
        .WithName(VerifyEmail);

        builder.MapGet("users/{id:guid}", async (Guid id, GetUser useCase) =>
        {
            GetUser.UserResponse? user = await useCase.Handle(id);

            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithTags(Tag)
        .RequireAuthorization();

        return builder;
    }
}
