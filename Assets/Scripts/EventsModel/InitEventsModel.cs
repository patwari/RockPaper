using UnityEngine.Events;

namespace Events {
    public static partial class EventsModel {
        public static UnityAction<string, bool, string> LOAD_SCENE; // <sceneName, isAdditive, message>
        public static UnityAction<string> PRE_SCENE_LOAD_BEGIN; // <sceneName>
        public static UnityAction<string> SCENE_LOAD_COMPLETED; // <sceneName>
    }
}