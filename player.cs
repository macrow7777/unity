using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;
using System.Reflection;

public class player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public TextMesh NickNameText;
    NetworkManager NM;

    void Start()
    {
        NM = GameObject.Find("GameManager").GetComponent<NetworkManager>();
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        transform.position = Vector3.zero;
    }

    void Update()
    {
        NickNameText.color = PV.IsMine ? Color.blue : Color.white;
        move();
    }

    void move()
    {
        if (PV.IsMine)
        {
            RB.velocity = new Vector2(4 * Input.GetAxisRaw("Horizontal"), 4 * Input.GetAxisRaw("Vertical"));
            if (Input.GetAxisRaw("Horizontal") == 1) Flip(false); else Flip(true);
        }
    }
    [PunRPC]
    public void Flip(bool flag)
    {
        SR.flipX = flag;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
        }
        else
        {
            
        }
    }
}