using UnityEngine;

public class PlayerUICanvas : MonoBehaviour
{
    //pass stuff to this method in case score/results needs to be shown
    public void hide()
    {
        gameObject.SetActive(false);
    }
}
