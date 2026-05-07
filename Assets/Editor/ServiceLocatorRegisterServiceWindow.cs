using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using Layer.Infrastructure;

public class ServiceLocatorRegisterServiceWindow : EditorWindow
{
    [MenuItem("Tools/ServiceLocatorRegisterServiceWindow")]
    public static void ShowWindow()
    {
        GetWindow<ServiceLocatorRegisterServiceWindow>("ServiceLocatorRegisterServiceWindow");
    }

    private Vector2 _scrollPosition;

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Registered Services", EditorStyles.boldLabel);

        if (!UnityEngine.Application.isPlaying)
        {
            EditorGUILayout.HelpBox("再生モード中のみ登録されているサービスを表示できます。", MessageType.Info);
            return;
        }

        // リフレクションで非公開の _services を取得
        var servicesField = typeof(ServiceLocator).GetField("_services", BindingFlags.NonPublic | BindingFlags.Static);
        var servicesDict = servicesField?.GetValue(null) as Dictionary<Type, object>;

        if (servicesDict == null || servicesDict.Count == 0)
        {
            EditorGUILayout.LabelField("登録されているサービスはありません。");
            return;
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            foreach (var kvp in servicesDict)
            {
                DrawServiceItem(kvp.Key, kvp.Value);
            }
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Repaint"))
        {
            Repaint();
        }
    }

    private void DrawServiceItem(Type type, object instance)
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.BeginVertical();
            // 型名を表示
            EditorGUILayout.LabelField($"Type: {type.Name}", EditorStyles.boldLabel);
            // インスタンスの実際の型（実装クラス）とハッシュコードを表示
            EditorGUILayout.LabelField($"Implementation: {instance.GetType().FullName}", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();

            // 登録解除ボタン（デバッグ用）
            if (GUILayout.Button("Unregister", GUILayout.Width(80)))
            {
                // Generic Method の UnregisterService<T> を動的に呼び出す
                var method = typeof(ServiceLocator).GetMethod("UnregisterService");
                var genericMethod = method.MakeGenericMethod(type);
                genericMethod.Invoke(null, null);
            }
        }
    }

    // 定期的にウィンドウを更新して最新の状態を保つ
    private void OnInspectorUpdate()
    {
        if (UnityEngine.Application.isPlaying)
        {
            Repaint();
        }
    }
}