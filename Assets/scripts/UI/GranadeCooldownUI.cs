using UnityEngine;
using UnityEngine.UI;

public class GranadeCooldownUI : MonoBehaviour
{
    public Slider energySlider;
    public Slider secondSlider;
    public Image secondStack;

    Color uncharged = new Color(8, 98, 107, 1);
    Color charged = new Color(0, 242, 185, 1);



    public void setMaxEnergy(float max)
    {
        energySlider.maxValue = max;
        energySlider.value = max;
        secondSlider.maxValue = max;
        secondSlider.value = max;
    }



    /*
     * if sliderflag = 0, the first energyslider will be used
     */
    public void setEnergy(float energy, float slider_flag)
    {
        if(slider_flag == 0)
        {
            energySlider.value = energy;
        }
        if (slider_flag == 1)
        {
            secondSlider.value = energy;
        }
    }



    public void resetSlider(float flag)
    {
        if(flag == 0) 
        {
            // I just used a granade and I'm left wit one, so the active slider becomes the primary one, reset the second one
            secondSlider.value = 0;
        }
    }



    //fill in second stack when both granades are charged up
    public void chargeSecondStack()
    {
        secondStack.color = charged;
    }
    public void useSecondStack()
    {
        secondStack.color = uncharged;
    }
}
