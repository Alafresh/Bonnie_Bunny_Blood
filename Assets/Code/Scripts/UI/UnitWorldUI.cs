using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionsPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamage += HealthSystem_OnDamage;
        UpdateActionPointsText();
        UpdateHealthBarImage();
    }
    private void OnDestroy()
    {
        Unit.RemoveAllEventListeners();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionsPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void HealthSystem_OnDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBarImage();
    }
    private void UpdateHealthBarImage()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
