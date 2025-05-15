using _ARK_;
using System.IO;
using System.Reflection;
using System;
using UnityEngine;
using _UTIL_;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public void EditArkJSon(in string file_path, in Type type)
        {
            ArkJSon arkjson = (ArkJSon)JsonUtility.FromJson(File.ReadAllText(file_path), type);
            ReflectionEditor(arkjson, () => arkjson.SaveArkJSon(true));
        }

        public void ReflectionEditor(in object target, Action on_save, Action<SguiCustom_Abstract, object> on_change = null, Traductions title = default)
        {
            if (title.IsDefault)
                title = new(target.GetType().FullName);

            trad_title.SetTrads(title);
            on_change += (_, _) => trad_title.SetTrad(title + "*");

            FieldInfo[] target_fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            onAction_confirm += () =>
            {
                on_save?.Invoke();
                NUCLEOR.delegates.onApplicationFocus?.Invoke();
                return true;
            };

            for (int i = 0; i < target_fields.Length; ++i)
            {
                FieldInfo field = target_fields[i];

                if (field.IsNotSerialized)
                    continue;

                Type type = field.FieldType;
                object value = field.GetValue(target);

                SguiCustom_Abstract button = value switch
                {
                    bool _bool => AddBool(field, target, _bool, on_change, ref on_save),
                    int _int => AddInt(field, target, _int, on_change, ref on_save),
                    float _float => AddFloat(field, target, _float, on_change, ref on_save),
                    Enum _enum => AddEnum(field, target, _enum, on_change, ref on_save),
                    string _str => AddString(field, target, _str, on_change, ref on_save),
                    _ => null,
                };

                if (button == null)
                {
                    button = AddButton<SguiCustom_Label>();
                    button.trad_label.SetTrads(new()
                    {
                        english = $"Could not parse field: {{ \"{field.Name}\" : \"{value}\" }} ({type})",
                        french = $"Impossible de parser le champ: {{ \"{field.Name}\" : \"{value}\" }} ({type})",
                    });
                }
            }
        }
    }
}