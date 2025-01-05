namespace Gameplay
{
    public class BotProfile
    {
        public string name { get; private set; }

        public BotProfile(string name) => this.name = name;
    }
}