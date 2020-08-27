using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomEditor : EditorWindow
{
    TextField searchTxt;
    TextField replaceTxt;
    Label numSelectedLbl;

    [MenuItem("Custom Tools/Renamerator")]
    public static void OpenWindow()
    {
        GetWindow<RoomEditor>();
    }

    private void OnEnable()
    {
        var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Renamarator.uxml");
        var ui = template.CloneTree();
        rootVisualElement.Add(ui);
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Renamarator.uss");
        ui.styleSheets.Add(styleSheet);

        numSelectedLbl = ui.Q<Label>("numSelectedLbl");
        updateNumSelectedLabel();
        Selection.selectionChanged += updateNumSelectedLabel;

        searchTxt = ui.Q<TextField>("searchTxt");
        replaceTxt = ui.Q<TextField>("replaceTxt");

        var renameBtn = ui.Q<Button>("renameBtn");
        renameBtn.clicked += renameSelected;
    }

    public void OnDisable()
    {
        Selection.selectionChanged -= updateNumSelectedLabel;
    }

    private void updateNumSelectedLabel()
    {
        var numSelected = Selection.gameObjects.Length;
        numSelectedLbl.text = $"GameObjects Selected: {numSelected}";
    }

    void renameSelected()
    {
        Debug.Log($"Renaming {searchTxt.value} -> {replaceTxt.value}");
    }
}
