namespace Sgms.Backend
{
    public static class ApplicationBuilderExtentions
    {
        public static WinServiceCardListenerClient listenerClient { get; set; }
        public static WebApplication UseRabbitListener(this WebApplication app)
        {
            listenerClient=app.Services.GetService<WinServiceCardListenerClient>();
            var lifetime = app.Services.GetService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(listenerClient.Start);
            lifetime.ApplicationStopped.Register(listenerClient.Dispose);
            return app;
        }
    }
}
