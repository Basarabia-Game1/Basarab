using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testControll : MonoBehaviour
{
    public PhotonView photonView;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (Input.GetKey(KeyCode.W))
            transform.Translate(-Time.deltaTime * 5, 0, 0);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Time.deltaTime * 5, 0, 0);
    }
}
