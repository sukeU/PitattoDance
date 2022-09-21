using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    DollCollision dollCollision;
    // Start is called before the first frame update
    void Start()
    {
        dollCollision = GameObject.FindGameObjectWithTag("Pose").GetComponent<DollCollision>();
    }

    //G‚ê‚Ä‚¢‚é‚Æ‚«AŒÄ‚Î‚ê‚é
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("G‚ê‚½");
        var HDollName = other.gameObject.name; //G‚ê‚½ƒh[ƒ‹‚Ì‰ÓŠ‚Ì–¼‘O‚ğæ“¾
        var HName = this.gameObject.name; //G‚ê‚ç‚ê‚½Œ^‚Ì‰ÓŠ‚Ì–¼‘O‚ğæ“¾
        dollCollision.checkHitName(HDollName, HName);
    }

    //—£‚ê‚½‚Æ‚«‚ÉŒÄ‚Î‚ê‚é
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("—£‚ê‚½");
        dollCollision.checkoutHitName();
    }
}