using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] float objectSpeed = 1f;
    [SerializeField] private float resetPosition = -96f;
    [SerializeField] private float startPosition = 127.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!GameManager.instance.GameOver)
        {
            transform.Translate(Vector3.left * (objectSpeed * Time.deltaTime));



            if (transform.localPosition.x <= resetPosition)
            {
                Vector3 newPos = new Vector3(startPosition, transform.position.y, transform.position.z);
                transform.position = newPos;

            }
        }   
    }
}
