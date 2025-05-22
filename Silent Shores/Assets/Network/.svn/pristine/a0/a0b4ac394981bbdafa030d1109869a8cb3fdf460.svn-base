using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(NetworkController), true)]
public class NetworkControllerEditor : Editor {
    public void OnEnable(){
    }
	
    public override void OnInspectorGUI() {
	NetworkController controller = (NetworkController) target;

        EditorGUILayout.Foldout(true, "Automatically Registered Scripts"); 
        EditorGUI.indentLevel++;
	foreach (NetworkListener listener in controller.getListeners()){
		if (listener is UnityEngine.Object){
			EditorGUILayout.ObjectField((UnityEngine.Object)listener, typeof(NetworkListener), false);
		}
	}
        EditorGUI.indentLevel--;
    }
}
