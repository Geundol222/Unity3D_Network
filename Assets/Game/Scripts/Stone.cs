using Photon.Pun;
using UnityEngine;

public class Stone : MonoBehaviourPun
{
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (photonView.InstantiationData != null)
        {
            Vector3 force = (Vector3)photonView.InstantiationData[0];
            Vector3 torque = (Vector3)photonView.InstantiationData[1];

            rigid.AddForce(force, ForceMode.Impulse);
            rigid.AddTorque(torque, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (transform.position.magnitude > 200f)
            PhotonNetwork.Destroy(photonView);
    }
}
