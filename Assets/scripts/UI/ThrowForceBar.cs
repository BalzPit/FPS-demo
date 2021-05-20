using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowForceBar : MonoBehaviour
{
    float fadein_speed = 4;
    float fadeout_speed = 2;
    float alpha;
    bool hide;
    public Slider forceSlider;
    public Image barImg;
    public Image contourImg;
    Color forceBarColor;

    void Start()
    {
        hide = true;

        //set the color variable
        forceBarColor = barImg.color;
        //make bar invisible
        forceBarColor.a = 0;
        barImg.color = forceBarColor;
        contourImg.color = forceBarColor;
    }

    private void Update()
    {
        if (hide && alpha > 0)
        {
            //hide the bar
            alpha -= fadeout_speed*Time.deltaTime;
            forceBarColor.a = alpha;
            barImg.color = forceBarColor;
            contourImg.color = forceBarColor;

            if(alpha <= 0)
            {
                setForce(0);
            }
        }
        else if (!hide && alpha < 1)
        {
            //show the bar
            alpha += fadein_speed * Time.deltaTime;
            forceBarColor.a = alpha;
            barImg.color = forceBarColor;
            contourImg.color = forceBarColor;
        }
    }

    public void setForce(float force)
    {
        forceSlider.value = force;
    }

    public void setMaxForce(float maxForce)
    {
        forceSlider.maxValue = maxForce;
    }

    public void setMinForce(float minForce)
    {
        forceSlider.minValue = minForce;
    }

    public void hideBar()
    {
        hide = true;
    }
    public void showBar()
    {
        hide = false;
    }
}
