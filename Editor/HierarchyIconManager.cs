using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[InitializeOnLoad]
public static class HierarchyIconManager
{
	static HierarchyIconManager()
	{
		Debug.Log("HierarchyIconManager!");
		EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon;
	}

	private static Dictionary<Type, Texture2D> iconCache = new Dictionary<Type, Texture2D>();

	private static void DrawHierarchyIcon(int instanceID, Rect selectionRect)
	{
		if (!showIcons)
			return;

		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if (go == null)
			return;

		var components = go.GetComponents<Component>();

		float iconSize = 16f;
		float initialXPosition = selectionRect.x + selectionRect.width;

		foreach (var component in components)
		{
			if (component == null)
				continue;

			Type componentType = component.GetType();

			if (!iconCache.TryGetValue(componentType, out Texture2D icon))
			{
				icon = EditorGUIUtility.ObjectContent(component, componentType).image as Texture2D;
				iconCache[componentType] = icon;
			}

			if (icon != null)
			{
				Rect iconRect = new Rect(initialXPosition, selectionRect.y, iconSize, iconSize);
				GUI.color = Color.white;
				GUI.DrawTexture(iconRect, icon);
				initialXPosition -= iconSize;
			}
		}
	}

	private static bool showIcons = true;

	[MenuItem("HierarchyIcons/Show Icons")]
	private static void ToggleShowIcons()
	{
		showIcons = !showIcons;
		Menu.SetChecked("HierarchyIcons/Show Icons", showIcons);
	}
}

#endif