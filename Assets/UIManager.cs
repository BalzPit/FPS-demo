using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public HealthBar healthBar;
    public GranadeCooldownUI granadeUI;
    
    public hitmarker hitmrkr;
    public hitmarker deathMarker;
    public ThrowForceBar throwForceBar;
    public Image crosshair;
    public Text ammoCountText;
    public GameOverScreen gameOverScreen;
    public PlayerUICanvas uiCanvas;


    public HealthBar getHealthBar()
    {
        return healthBar;
    }

    public GranadeCooldownUI GetGranadeCooldownUI()
    {
        return granadeUI;
    }

    public hitmarker getHitMarker()
    {
        return hitmrkr;
    }

    public hitmarker getDeathHitMarker()
    {
        return deathMarker;
    }

    public ThrowForceBar GetThrowForceBar()
    {
        return throwForceBar;
    }

    public Image getCrosshair()
    {
        return crosshair;
    }

    public Text getAmmoCounter()
    {
        return ammoCountText;
    }

    public void gameOver()
    {
        gameOverScreen.show();
        uiCanvas.hide();
    }
}
