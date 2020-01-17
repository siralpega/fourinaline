using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    public Color32 color = new Color32(198, 48, 48, 255);
    public Slider slider;
    public Image displayChip;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void updateColor()
    {
        int val = (int) slider.value;
        if (val == 0)
            color = new Color32(198, 48, 48, 255); //red
        else if (val == 1)
            color = new Color32(222, 58, 212, 255); //pink
        else if (val == 2)
            color = new Color32(81, 97, 51, 255); //lime
        else if (val == 3)
            color = new Color32(130, 197, 51, 255); //green
        else if (val == 4)
            color = new Color32(51, 174, 197, 255); //cyan
        else if (val == 5)
            color = new Color32(43, 64, 210, 255); //blue
        displayChip.color = color;
    }
}
