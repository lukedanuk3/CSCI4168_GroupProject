using UnityEngine;

public class ButtonItem : MonoBehaviour
{
    public enum tool
    {
        CROWBAR,
        BOLTCUTTER,
        WIRECUTTER
    }

    public tool item;

    public ItemManager itemManager;

    public void OnButtonClick()
    {
        itemManager.updateItems(item.ToString());
        
    }
}
