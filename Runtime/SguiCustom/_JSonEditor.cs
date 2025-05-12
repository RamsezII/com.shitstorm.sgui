using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

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

        public void EditJSon(in object target)
        {
            Type target_type = target.GetType();
            FieldInfo[] target_fields = target_type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < target_fields.Length; ++i)
            {
                FieldInfo field = target_fields[i];

                if (field.IsNotSerialized)
                    continue;

                Type type = field.FieldType;
                object value = field.GetValue(target);
                string name = field.Name;

                SguiCustom_Abstract button = null;

                switch (value)
                {
                    case string str:
                        var input_field = AddButton<SguiCustom_InputField>();
                        input_field.input_field.text = str;
                        button = input_field;
                        break;

                    case bool b:
                        var toggle = AddButton<SguiCustom_Toggle>();
                        toggle.toggle.isOn = b;
                        button = toggle;
                        break;
                }

                if(false)
                switch (type)
                {
                    case Type t when t == typeof(string):
                        var input_field = AddButton<SguiCustom_InputField>();
                        input_field.input_field.text = value.ToString();
                        button = input_field;
                        break;
                }

                button?.trad_label.SetTrad(name + ":");
            }
        }
    }
}