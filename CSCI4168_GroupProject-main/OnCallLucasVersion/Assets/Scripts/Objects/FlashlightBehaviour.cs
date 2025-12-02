using UnityEngine;

public class FlashlightBehaviour : MonoBehaviour
{

    public GameObject lightSource;

    void Start(){
        lightSource.SetActive(false);
    }

    public void Toggle(){
        if (lightSource.activeSelf) lightSource.SetActive(false);
        else lightSource.SetActive(true);
    }
}
