using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public Button weapon1;
    public Button weapon2;
    public Button weapon3;
    public Button weapon4;

    public Button connectLobby;

    private string nickName = "User";

    private const string gameVersion = "1.0";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName = $"{nickName}_{Random.Range(0, 100)}";
        PhotonNetwork.ConnectUsingSettings();

        weapon1.onClick.AddListener(() => OnWeaponSelect(0));
        weapon2.onClick.AddListener(() => OnWeaponSelect(1));
        weapon3.onClick.AddListener(() => OnWeaponSelect(2));
        weapon4.onClick.AddListener(() => OnWeaponSelect(3));
        connectLobby.onClick.AddListener(() => OnConnectedBattleRoom());

        //DontDestroyOnLoad(this.gameObject);
    }

    // 선택무기 저장
    private void OnWeaponSelect(int weaponIndex)
    {
        PlayerPrefs.SetInt("SELECTED_WEAPON", weaponIndex);

        Debug.Log(PlayerPrefs.GetInt("SELECTED_WEAPON"));
    }

    // 로비로 입장 , 씬 변경 ==> Trigger 충돌했을 때
    private void OnConnectedBattleRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        ro.IsOpen = true;
        ro.IsVisible = true;

        PhotonNetwork.CreateRoom("MyRoom", ro);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleScene");
        }
    }
}
