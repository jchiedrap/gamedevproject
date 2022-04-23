using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Curtain : MonoBehaviour
{
    public bool fadeOut, fadeIn;
    public Image image;

    private void Start() {
        image = GetComponent<Image>();
    }

    private void Update() {
        Color objColor = image.color;
        if (fadeOut) {
            float fadeAmt = objColor.a - (Time.deltaTime);
            objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmt);
            image.color = objColor;
            if (fadeAmt <= 0.01f) fadeOut = false;
        }
        if (fadeIn) {
            float fadeAmt = objColor.a + (Time.deltaTime);
            objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmt);
            image.color = objColor;
            if (fadeAmt >= 1.00f) fadeIn = false;
        }
    }

    public void Fade(bool i) {
        if (i) fadeIn = true;
        else fadeOut = true;
    }
}
