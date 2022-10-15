using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public GameObject playerManager;

    private void Start()
    {
        SpawnMyPlayer();
    }

    void SpawnMyPlayer()
    {
        Vector3 randomPosition = transform.position;
        GameObject myPlayer = (GameObject)PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        myPlayer.transform.SetParent(playerManager.transform);
        //ENABLED SO THAT EACH CLIENT HAS THEIR OWN VERSION
        myPlayer.GetComponent<TpMovement>().enabled = true;
        myPlayer.GetComponent<PlayerInput>().enabled = true;
        myPlayer.GetComponent<CharacterAiming>().enabled = true;
        myPlayer.transform.Find("Camera Holder").gameObject.SetActive(true);
        myPlayer.transform.Find("Zoomed Camera").gameObject.SetActive(true);
        myPlayer.transform.Find("Virtual Camera").gameObject.SetActive(true);
    }
}
