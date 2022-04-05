using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatFollower : MonoBehaviour
{
    public List<Image> slots;

    public Button unload;
    public Button depart;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
        depart.gameObject.SetActive(Boat.instance.isDocked);
        unload.gameObject.SetActive(Boat.instance.isDocked);
        unload.interactable = Boat.instance.contents.Count > 0;

        for(int i = 0; i < slots.Count; i++) {
            if(Boat.instance.contents.Count > i) {
                slots[i].enabled = true;
                slots[i].sprite = Boat.instance.contents[i].GetSprite();
                var c = Boat.instance.contents[i].GetColor();
                c.a = slots[i].color.a;
                slots[i].color = c;
            } else {
                slots[i].sprite = null;
                slots[i].enabled = false;
            }
        }
    }
}
