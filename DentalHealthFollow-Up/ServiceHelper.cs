using System;

namespace DentalHealthFollow_Up.MAUI
{
    public static class ServiceHelper
    {
        public static IServiceProvider? Services { get; private set; }
        public static void Initialize(IServiceProvider services) => Services = services;

        public static T Resolve<T>() where T : notnull =>
            (T)(Services ?? throw new InvalidOperationException("Services not initialized"))
             .GetService(typeof(T))!;
    }
}






