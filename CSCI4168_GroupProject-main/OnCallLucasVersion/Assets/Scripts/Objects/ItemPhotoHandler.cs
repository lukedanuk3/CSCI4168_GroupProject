using UnityEngine;

public class ItemPhotoHandler : MonoBehaviour
{
    private bool photoAlreadyTaken = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool CheckIfPhotoAlreadyTaken(){
        return photoAlreadyTaken;
    }

    public void PhotoNowTaken(){
        photoAlreadyTaken = true;
    }
}
