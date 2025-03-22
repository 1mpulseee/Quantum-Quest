using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HintManager : MonoBehaviour
{
    public static Action<string> DisplayHintEvent;
    public static Action HideHintEvent;
    public static Action<GameObject> EnableOutlineEvent;
    public static Action<GameObject> DisableOutlineEvent;

    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private float _hintAnimationDuration = 0.3f;

    private void OnEnable()
    {
        DisplayHintEvent += DisplayHint;
        HideHintEvent += HideHint;
        EnableOutlineEvent += EnableOutline;
        DisableOutlineEvent += DisableOutline;
    }

    private void OnDisable()
    {
        DisplayHintEvent -= DisplayHint;
        HideHintEvent -= HideHint;
        EnableOutlineEvent -= EnableOutline;
        DisableOutlineEvent -= DisableOutline;
    }

    private void DisplayHint(string text)
    {
        _hintText.text = text;
        _hintText.transform.DOScale(1f, _hintAnimationDuration).From(0).SetEase(Ease.OutBounce);
    }

    private void HideHint()
    {
        _hintText.transform.DOScale(0f, _hintAnimationDuration).From(1f).SetEase(Ease.InQuad);
    }

    private void EnableOutline(GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        Transform modelTransform = parent.Find("model");
        modelTransform.gameObject.layer = LayerMask.NameToLayer("Outline");
    }

    private void DisableOutline(GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        Transform modelTransform = parent.Find("model");
        modelTransform.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}