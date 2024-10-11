using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientColorCotroller : MonoBehaviour
{
    public Material _gradientMaterial;
    Image _bar;
    [SerializeField]
    Color BaseColor = Color.white;
    [SerializeField]
    Color BaseColor2 = Color.white;
    private void Start()
    {
        _bar = GetComponent<Image>();
        SetBarColor();
    }

    private void SetBarColor()
    {
        Material newMaterial = new Material(_gradientMaterial);
        newMaterial.SetColor("_BaseColor", BaseColor);
        newMaterial.SetColor("_BaseColor2", BaseColor2);

        _bar.material = newMaterial;
    }
}
