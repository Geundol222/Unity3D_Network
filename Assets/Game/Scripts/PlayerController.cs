using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] List<Color> playerColorList;

    [SerializeField] float accelPower;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float fireCoolTime;

    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int count;

    private PlayerInput playerInput;
    private Rigidbody rigid;
    private Vector2 inputDir;
    private float lastFireTime = float.MinValue;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        SetPlayerColor();

        if (!photonView.IsMine)
            Destroy(playerInput);

        if (photonView.IsMine)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 20f, 0);
        }
    }

    private void Update()
    {
        Accelate(inputDir.y);
        Rotate(inputDir.x);
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0, 0);
    }

    private void Accelate(float input)
    {
        rigid.AddForce(input * accelPower * transform.forward, ForceMode.Force);
        if (rigid.velocity.magnitude > maxSpeed)
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
    }

    private void Rotate(float input)
    {
        transform.Rotate(Vector3.up, input * rotateSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient, transform.position, transform.rotation);
    }

    [PunRPC]
    private void RequestCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        // 마스터클라이언트(서버) 입장에서 판정 진행        
        if (Time.time < lastFireTime + fireCoolTime)
            return;
        lastFireTime = Time.time;

        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, position, rotation, sentTime, info.Sender);
    }

    [PunRPC]
    private void ResultCreateBullet(Vector3 position, Quaternion rotation, float sentTime, Player player)
    {
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - sentTime));

        Bullet bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.SetPlayer(player);
        bullet.ApplyLag(lag);
    }

    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();
        if (playerColorList == null || playerColorList.Count <= playerNumber)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material.color = playerColorList[playerNumber];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(count);
        }
        else // stream.IsReading
        {
            count = (int)stream.ReceiveNext();
        }
    }
}
