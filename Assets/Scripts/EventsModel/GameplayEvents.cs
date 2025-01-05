using Gameplay;
using UnityEngine.Events;

namespace Events {
    public static class GameplayEvents {
        public static UnityAction<string, float, UnityAction, UnityAction> SHOW_POPUP; // <message, duration, mid callback, oncomplete callback>
        public static UnityAction<PlayerShape> BOT_MOVE_MADE; // <hand shape>
        public static UnityAction ROUND_TIME_END;
    }
}