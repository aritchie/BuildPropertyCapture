// using Microsoft.Extensions.DependencyInjection;
//
// namespace BuildPropertyCapture;
//
//
// public static class ServiceCollectionExtensions
// {
//     public static IServiceCollection AddBuildPropertyCapture(this IServiceCollection services)
//     {
//         // this could generate multiple types and be a problem - this class should be private but our lib should be able to read it...
//         if (Type.GetType("BuildPropertyCapture.__SentryBuildProperties") != null)
//         {
//             
//         }
//         return services;
//     }
// }