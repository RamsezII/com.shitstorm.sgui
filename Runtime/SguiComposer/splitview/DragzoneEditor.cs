#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

namespace _SGUI_.composer
{
    [CustomEditor(typeof(Dragzone))]
    internal class DragzoneEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Dragzone.rt_zone)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Dragzone.opposite_dragzone)));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif