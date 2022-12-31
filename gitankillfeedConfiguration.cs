using Rocket.API;


namespace gitankillfeed
{
    public class gitankillfeedConfiguration : IRocketPluginConfiguration
    {
        public int cooldown;
        public string LinesColor;
        public void LoadDefaults()
        {
            cooldown = 8;
            LinesColor = "#323232";
        }
    }
}
