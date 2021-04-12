using UnityEngine;
using UnityEngine.UI;

public class hitmarker : MonoBehaviour
{
    bool fade;
    public float fadespeed;
    float alpha;

    public Image hitmarkerImg;
    Color hit_color;

    // Start is called before the first frame update
    void Start()
    {
        fade = false;

        //set hitmarker as invisible
        hit_color = hitmarkerImg.color;
        hit_color.a = 0;
        hitmarkerImg.color = hit_color;
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            if (alpha > 0)
            {
                alpha -= Time.deltaTime * fadespeed;
                hit_color.a = alpha;
                hitmarkerImg.color = hit_color;
            }
            else
            {
                fade = false;
            }
        }
    }

    public void showHitMarker()
    {
        //show hitmarker

        hit_color = hitmarkerImg.color;
        //make image completely visible
        alpha = 1;
        hit_color.a = alpha;
        hitmarkerImg.color = hit_color;

        fade = true;
    }
}
