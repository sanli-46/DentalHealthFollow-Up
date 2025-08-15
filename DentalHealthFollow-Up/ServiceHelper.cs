using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up.MAUI
{
    public static class ServiceHelper
    {
        public static T Resolve<T>() where T : notnull
        {
            var sp = Microsoft.Maui.Controls.Application.Current?
                     .Handler?.MauiContext?.Services;

            if (sp is null) throw new InvalidOperationException("MAUI service provider hazır değil.");
            return sp.GetRequiredService<T>();
        }
    }
}




