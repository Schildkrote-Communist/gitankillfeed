using Rocket.API;


namespace gitankillfeed
{
    public class gitankillfeedConfiguration : IRocketPluginConfiguration
    {
        public int cooldown;
        public void LoadDefaults()
        {
            cooldown = 8;
        }
    }
}
