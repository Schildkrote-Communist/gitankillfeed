using Rocket.API;


namespace gitankillfeed
{
    public class gitankillfeedConfiguration : IRocketPluginConfiguration
    {
        public int cooldown { get; set; }
        public void LoadDefaults()
        {
            cooldown = 8;
        }
    }
}
