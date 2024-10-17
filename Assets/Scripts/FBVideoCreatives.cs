using UnityEngine;

public class FBVideoCreatives : MonoBehaviour
{ 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Animator>().SetTrigger("show");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Animator>().SetTrigger("show1");
        }
    }
}
