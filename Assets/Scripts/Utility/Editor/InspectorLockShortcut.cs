using UnityEditor;

namespace IlanGreuter.Utility.Editor
{
    public class InspectorLockShortcut
    {
        [MenuItem("Edit/Inspector Lock %`")]
        public static void ToggleLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

        [MenuItem("Edit/Inspector Lock %`", true)]
        public static bool InspectorValid() => ActiveEditorTracker.sharedTracker.activeEditors.Length > 0;
    }
}