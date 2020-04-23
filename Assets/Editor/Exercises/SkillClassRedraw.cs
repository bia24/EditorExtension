using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/***
****   编辑器脚本，自定义类Skill在可排列数组面板中的重绘制  
***/

[CustomPropertyDrawer(typeof(Skill))]
public class SkillClassRedraw:PropertyDrawer{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            //找到对应的属性值
            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty cd = property.FindPropertyRelative("cd");
            SerializedProperty damage = property.FindPropertyRelative("damage");
            SerializedProperty mp = property.FindPropertyRelative("mp");
            SerializedProperty icon = property.FindPropertyRelative("icon");
            SerializedProperty effect = property.FindPropertyRelative("effect");

            //创建属性的矩形区域

            //position为这一个索引的传入矩形数据
            position.height = EditorGUIUtility.singleLineHeight; //默认所有矩形的高度为一行，可修改;
            EditorGUIUtility.labelWidth =55;//设置自动出现的lable统一其宽度为60;

            //创建icon区域
            Rect iconRect = new Rect(position) { y=position.y+11,width = 68, height = 68 };
            //创建name矩形
            Rect nameRect = new Rect(position) { width = position.width - 80, x = position.x + 80 };
            //创建cd矩形
            Rect cdRect = new Rect(nameRect) { y = nameRect.y + EditorGUIUtility.singleLineHeight + 2 };
            //创建damage矩形
            Rect damageRect = new Rect(cdRect) { y = cdRect.y + EditorGUIUtility.singleLineHeight + 2 };
            //创建mp消耗矩形
            Rect mpRect = new Rect(damageRect) { y = damageRect.y + EditorGUIUtility.singleLineHeight + 2 };
            //创建特效矩形
            Rect effectRect = new Rect(mpRect) { y = mpRect.y + EditorGUIUtility.singleLineHeight + 2 };


            //绘制属性区域，并更新对应的属性值
            EditorGUI.ObjectField(iconRect,icon,typeof(Texture),GUIContent.none); //使用EditorGUI能利用矩形来绘制
            //name.stringValue = EditorGUI.TextField(nameRect, name.displayName, name.stringValue);//error : TextField不存在使用SerializedProperty的API，因此在多选上存在Bug
            EditorGUI.PropertyField(nameRect, name);/*更新：使用PropertyField万能框来解决*/
            EditorGUI.Slider(cdRect,cd,0,120f,new GUIContent(cd.displayName));
            //damage.floatValue = EditorGUI.FloatField(damageRect, damage.displayName, damage.floatValue);//error : FloatField不存在使用SerializedProperty的API，因此在多选上存在Bug
            EditorGUI.PropertyField(damageRect, damage);/*更新：使用PropertyField万能框来解决*/
            //mp.intValue = EditorGUI.IntField(mpRect, mp.displayName, mp.intValue);//error : Popup不存在使用IntField的API，因此在多选上存在Bug
            EditorGUI.PropertyField(mpRect, mp);/*更新：使用PropertyField万能框来解决*/
            EditorGUI.ObjectField(effectRect,effect,new GUIContent(effect.displayName));
        }
    }
}
