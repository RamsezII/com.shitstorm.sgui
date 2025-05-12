using _ARK_;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static TMPro.TMP_InputField;

namespace _SGUI_
{
    partial class SguiCustom
    {


        //--------------------------------------------------------------------------------------------------------------

        public void EditJSon_OLD(in string file_path, in string title = null)
        {
            string file_name = Path.GetFileName(file_path);
            trad_title.SetTrad(title ?? file_name);
        }

        public void EditJSon(in object json)
        {
            ArkJSon arkjson = json as ArkJSon;
            trad_title.SetTrad(Path.GetFileName(arkjson.GetFilePath()));

            Type target_type = arkjson.GetType();
            FieldInfo[] target_fields = target_type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            Action on_save = null;
            onAction_confirm += () =>
            {
                on_save?.Invoke();
                arkjson.SaveArkJSon(true);
                return true;
            };

            for (int i = 0; i < target_fields.Length; ++i)
            {
                FieldInfo field = target_fields[i];

                if (field.IsNotSerialized)
                    continue;

                Type type = field.FieldType;
                object value = field.GetValue(arkjson);
                string name = field.Name;

                SguiCustom_Abstract button = null;

                switch (value)
                {
                    case bool _bool:
                        {
                            var toggle = AddButton<SguiCustom_Toggle>();
                            button = toggle;
                            toggle.toggle.isOn = _bool;
                            on_save += () => field.SetValue(arkjson, toggle.toggle.isOn);
                        }
                        break;

                    case int _int:
                        {
                            var input_int = AddButton<SguiCustom_InputField>();
                            button = input_int;
                            input_int.input_field.text = _int.ToString();
                            input_int.input_field.contentType = ContentType.IntegerNumber;
                            on_save += () => field.SetValue(arkjson, int.Parse(input_int.input_field.text));
                        }
                        break;

                    case float _float:
                        {
                            var input_float = AddButton<SguiCustom_InputField>();
                            button = input_float;
                            input_float.input_field.text = _float.ToString();
                            input_float.input_field.contentType = ContentType.DecimalNumber;
                            on_save += () => field.SetValue(arkjson, float.Parse(input_float.input_field.text));
                        }
                        break;

                    case Enum _enum:
                        {
                            var dropdown = AddButton<SguiCustom_Dropdown>();
                            button = dropdown;
                            dropdown.dropdown.ClearOptions();
                            dropdown.dropdown.AddOptions(Enum.GetNames(type).ToList());

                            int enum_i = Convert.ToInt32(_enum);
                            dropdown.dropdown.value = Enum.GetValues(type).Cast<int>().ToList().IndexOf(enum_i);
                            dropdown.dropdown.RefreshShownValue();

                            on_save += () =>
                            {
                                int index = dropdown.dropdown.value;
                                string enum_name = dropdown.dropdown.options[index].text;
                                field.SetValue(arkjson, Enum.Parse(type, enum_name));
                            };
                        }
                        break;

                    case string _str:
                        {
                            var input_str = AddButton<SguiCustom_InputField>();
                            button = input_str;
                            input_str.input_field.text = _str;
                            input_str.input_field.contentType = ContentType.Standard;
                            on_save += () => field.SetValue(arkjson, input_str.input_field.text);
                        }
                        break;
                }

                button?.trad_label.SetTrad(name + ":");
            }
        }
    }
}