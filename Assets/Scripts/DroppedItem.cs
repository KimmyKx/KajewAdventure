using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private ItemInfoScriptable itemInfo;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider);
        if(collider.gameObject.tag == "Player")
        {
            UIManager.Instance.ReceiveItem(itemInfo);
            Destroy(gameObject);
        }
    }
}
