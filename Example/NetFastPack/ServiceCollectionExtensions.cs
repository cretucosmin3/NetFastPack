namespace NetFastPack;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNetFastPack(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            // [PackedBytesResponse]
            options.Filters.Add<BytesPackedResponseFilter>(int.MaxValue);

            // [FromPackedBytes]
            options.ModelBinderProviders.Insert(0, new FromPackedBytesModelBinderProvider());
        });

        return services;
    }
}