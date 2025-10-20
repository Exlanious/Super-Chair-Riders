using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = false,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message; // must have value type. 

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);

        };
    }

    private void Update()
    {

        if (!IsOwner) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            /* //This is a synchronized variable. 
            randomNumber.Value = new MyCustomData
            {
                _int = Random.Range(0, 100),
                _bool = true,
                message = "THIS IS A MULTIPLAYER TEST"
            };
            */

            //Test1ServerRpc("test message");
            //Test2ServerRpc(new ServerRpcParams());
            //Test1ClientRpc();
            //Test2ClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } }); //player 2 not the host only

            /* //This will only work on the host/server. To have this work, call this through a server Rpc. 
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            */

            TestSpawnObjectServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true); //destroy the network object but keep instance alive. 
            Destroy(spawnedObjectTransform.gameObject);

            //A note on destroying: 
            //By default, the spawned object will be destroyed with the client that spawned it. 
            //This can be changed in the inspector by checking don't destroy with owner. 
        }

        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.y = -1f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = +1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = +1f;

        float moveSpeed = -3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    //RPC = Remove Procedure Call
    //client -> server. This code is run on the server but called from the client. 
    //All RPCs must end in the respecting RPC. (Server RPC must end in ServerRpc)
    //All params must be value types. (exception: string)
    [ServerRpc()]
    private void Test1ServerRpc(string message)
    {
        Debug.Log("TestServerRpc" + OwnerClientId + ";" + message);
    }

    [ServerRpc()]
    private void Test2ServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc" + OwnerClientId + ";" + serverRpcParams.Receive.SenderClientId);
    }

    //server -> client. This code is run on the client but called from the server.
    [ClientRpc()]
    private void Test1ClientRpc()
    {
        Debug.Log("test1ClientRpc");
    }

    //Use this if want to have control over which client to send data to. 
    [ClientRpc()]
    private void Test2ClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("test2ClientRpc");
    }


    [ServerRpc]
    private void TestSpawnObjectServerRpc()
    {
        spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    //To connet animations, add the network animator and connect the same object animator. 
}
