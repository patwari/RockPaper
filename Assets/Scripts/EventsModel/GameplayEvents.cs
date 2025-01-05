using Gameplay;
using UnityEngine.Events;

namespace Events {
    public static class GameplayEvents {
        public static UnityAction<string, float, UnityAction> SHOW_POPUP; // <message, duration, callback>
        public static UnityAction<PlayerShape> BOT_MOVE_MADE; // <hand shape>
    }
}