using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaPlayer : MonoBehaviour {

    public GameObject motioncontroller;
    GameObject leht_hand;
    bool placed = false;

	// Use this for initialization
	void Start () {
        //StartCoroutine(Wait_sec());
	}
	
    IEnumerator Wait_sec()
    {
        yield return new WaitForSeconds(1);
        GameObject leht_hand = motioncontroller.transform.Find("LeftController").gameObject;
        if (!placed)
        {
            transform.position = leht_hand.transform.position + new Vector3(0, 0.1f, 0);
            transform.Rotate(new Vector3(45, 90, 0));
            placed = true;
        }
    }

	// Update is called once per frame
	void Update () {
        if (!placed)
        {
            if (leht_hand == null)
            {
                leht_hand = motioncontroller.transform.Find("LeftController").gameObject;
            }
            else
            {
                transform.position = leht_hand.transform.position + new Vector3(0, 0.1f, 0);
                transform.Rotate(new Vector3(0, 0, 0));
                placed = true;
            }
        }
		
	}
}
