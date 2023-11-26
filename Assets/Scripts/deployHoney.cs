using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deployHoney : MonoBehaviour
{
    public GameObject honeyPrefab;
    public float respawnTime = 1.0f;
    private Vector2 screenBounds;
    public Transform zPosition;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(honeyWave());
       
    }

    private void spawnHoney()
    {
        GameObject a = Instantiate(honeyPrefab) as GameObject;
        a.transform.position = new Vector3(screenBounds.x * -2, Random.Range(-1f, 2.8f), zPosition.position.z);
        Debug.Log(a.transform.position);
    }

    IEnumerator honeyWave()
    {
        while (true)

        {
            yield return new WaitForSeconds(respawnTime);
            spawnHoney();
        }
    }
}
