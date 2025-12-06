using UnityEngine;

public class FlashlightBehaviour : MonoBehaviour
{

    public GameObject lightSource;

    void Awake(){
        gameObject.GetComponent<ToolData>().relativePosition = transform.localPosition;
        gameObject.GetComponent<ToolData>().relativeRotation = transform.localEulerAngles;
    }

    void Start(){
        lightSource.SetActive(false);
    }

    public void Toggle(){
        if (lightSource.activeSelf) lightSource.SetActive(false);
        else lightSource.SetActive(true);
    }
}
