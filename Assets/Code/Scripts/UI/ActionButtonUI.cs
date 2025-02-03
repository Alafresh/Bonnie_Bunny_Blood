using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Image imageBtn;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private List<IconActionButtonImage> actionButtonImagesList;
    
    [Serializable]
    public struct IconActionButtonImage
    {
        public IconActionButton iconActionButton;
        public Sprite icon;
    }

    public enum IconActionButton
    {
        Move,
        Spin,
        Sword,
        Shoot,
        Grenade,
        Interact
    }
    
    private BaseAction _baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        _baseAction = baseAction;
        textMesh.text = baseAction.GetActionName().ToUpper();

        foreach (IconActionButtonImage actionButtonImage in actionButtonImagesList)
        {
            if (baseAction.GetActionName() == actionButtonImage.iconActionButton.ToString())
            {
                imageBtn.sprite = actionButtonImage.icon;
            }
        }
        
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(_baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedAction == _baseAction);
    }
}
