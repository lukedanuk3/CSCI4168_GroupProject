using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class CameraToolControl : MonoBehaviour
{
    public Camera fpsCamera;
    public AudioSource openSound;
    public AudioSource closeSound;
    public AudioSource photoTakingSound;
 
    public GameObject uiPanel;
    public UIManager uiManager;
    public bool isOpen;
 
    void Update(){
        if (isOpen){
            uiPanel.SetActive(true);
            if (Input.GetKeyUp(KeyCode.Return)){
                TakePicture();
            }
        }else{
            uiPanel.SetActive(false);
        }
    }
 
    public void OpenClose(){
        isOpen = !isOpen;
        if(isOpen){
            if(closeSound.isPlaying){
                closeSound.Stop();
            }
            openSound.time = 0.5f;
            openSound.Play();
        }
        else{
            if(openSound.isPlaying){
                openSound.Stop();
            }
            closeSound.Play();
        }
    }
 
    public void TakePicture(){
        if(!photoTakingSound.isPlaying){
            photoTakingSound.Play();
        }
 
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
 
        RaycastHit hit;
 
        if (Physics.Raycast(ray, out hit, 100f))
        {
            GameObject item = hit.collider.gameObject;
 
            if (item.CompareTag("FollowMonster"))
            {
                Debug.Log("Enemy seen!");
            }
            else if (item.CompareTag("CameraMonster"))
            {
                item.GetComponentInParent<EnemyBehavior>().FreezeEnemy();
            }
            else if (item.CompareTag("Objective"))
            {
                ItemPhotoHandler handler = item.GetComponent<ItemPhotoHandler>();
 
                if (!handler.CheckIfPhotoAlreadyTaken())
                {
                    handler.PhotoNowTaken();
                    uiManager.increaseCounter();
                }
                else
                {
                    Debug.Log("Object's already had its picture taken");
                }
            }
        }
        else
        {
            Debug.Log("Nothing hit by raycast.");
        }
    }
}
 