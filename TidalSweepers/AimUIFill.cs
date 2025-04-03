using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUIFill : MonoBehaviour
{
    public Image sliderFill;
    public Sprite[] reticleSet;

    public void SetSlider(Color color) {
        sliderFill.color = color;
    }

    public Sprite SetReticle(int i) {
        return reticleSet[i];
    }
}
