using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopTrigger : MonoBehaviour

{
public GameObject[] gameobject1;
public GameObject[] gameobject2;
    // Start is called before the first frame update
    void Start(int i)
    {
        gameobject1[i].SetActive(true);
        gameobject2[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void OnTriggerEnter(int i)   
    {
        if (CompareTag("Hoops"))
        {
            gameobject1[i].SetActive(false);
            Debug.Log("game object 1 if not active");
            gameobject2[i].SetActive(true);
            Debug.Log("game object 2 is active");
        }

    }
} 