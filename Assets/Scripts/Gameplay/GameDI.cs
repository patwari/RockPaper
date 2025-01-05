// it will store the reference to shape view
using UnityEngine;

namespace Gameplay {
    /// <summary>
    /// This class stores references of objects that are needed in the game. <br/>
    /// It gets created from <seealso cref="GameDiCover"/> when GAMEPLAY scene is to be loaded. <br/>
    /// And gets deleted once the scene is unloaded. <br/>
    /// </summary>
    internal class GameDI {
        public static GameDI di { get; private set; }
        public BotManager botManager { get; private set; } = null;
        public TimerManager timerManager { get; private set; } = null;

        private GameDI() {
            shapeImageConfig = Resources.Load<ShapeImageConfig>("ScriptableObjects/ShapeImageConfig");
            Resources.UnloadAsset(shapeImageConfig);
        }

        public static void CreateNew() {
            di = new GameDI();
            Debug.Log($"patt :: GameDi created");
        }

        public static void Delete() {
            if (di != null) Debug.Log($"patt :: GameDi deleted");
            di = null;
        }

        public ShapeImageConfig shapeImageConfig { get; private set; } = null;

        public void SetBotManager(BotManager botManager) => this.botManager = botManager;
        public void SetTimerManager(TimerManager timerManager) => this.timerManager = timerManager;
    }
}