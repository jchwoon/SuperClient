using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaGradient : MonoBehaviour
{
    Image _gradientImage;
    [SerializeField]
    Color BaseColor = Color.white;
    [SerializeField]
    Color BaseColor2 = Color.white;
    private void Start()
    {
        _gradientImage = GetComponent<Image>();
        SetBarColor();
    }

    private void SetBarColor()
    {

        Color gradientColor = Color.Lerp(BaseColor, BaseColor2, 0.5f);

        _gradientImage.color = gradientColor;
    }
}
