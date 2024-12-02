using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopTrigger : MonoBehaviour

{
public GameObject[] gameobject1;
public GameObject[] gameobject2;
    // Start is called before the first frame update
    void Start()
    {
        SwapHoops(0);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
    public void OnTriggerEnter(Collider other)   
    {
        if (other.CompareTag("Hoops"))
        {
            int i = 0; // Assuming you want to use the first element of the arrays
            gameobject1[i].SetActive(false);
            Debug.Log("game object 1 is not active");
            gameobject2[i].SetActive(true);
            Debug.Log("game object 2 is active");
        }
    }

    private void SwapHoops(int i)
    {
        gameobject1[i].SetActive(false);
        gameobject2[i].SetActive(true);
    }
} 