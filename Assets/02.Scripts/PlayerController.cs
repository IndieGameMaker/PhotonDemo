using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    private const string SELECTED_WEAPON = "SELECTED_WEAPON";
    private const string BODY_COLOR = "BODY_COLOR";

    // 미리 제작된 무기 프리팹
    public List<GameObject> weapons = new List<GameObject>();

    // 로비에서 선택한 무기
    public int selectedWeapon;
    // 입장 순서 (바디 색상)
    public int playerIndex;

    // 자신의 무기 저장 변수
    public GameObject playerWeapon;

    // Weapon Anchor
    public Transform weaponAnchor;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        // 자신의 속성(무기, 색상)만 저장
        if (pv.IsMine)
        {
            SetPlayerInfo();
            StartCoroutine(MakeWeapon(selectedWeapon));

            // 카메라 위치 변경
            Camera.main.transform.parent = this.transform;
            Camera.main.transform.position = this.transform.position - (Vector3.forward * 5.0f) + (Vector3.up * 4.0f);
            Camera.main.transform.LookAt(this.transform.position + (Vector3.up * 2.0f));
        }
    }

    // 커스텀 프로퍼티에 저장하는 함수
    void SetPlayerInfo()
    {
        // 로비에서 선택한 무기
        selectedWeapon = PlayerPrefs.GetInt(SELECTED_WEAPON);
        // 바디 색상을 위한 인덱스
        playerIndex = PhotonNetwork.CurrentRoom.PlayerCount;

        Hashtable ht = new Hashtable();
        ht.Add(SELECTED_WEAPON, selectedWeapon);
        ht.Add(BODY_COLOR, playerIndex);

        // 커스텀 프로퍼티 저장
        pv.Owner.SetCustomProperties(ht);
    }

    // 무기 생성
    IEnumerator MakeWeapon(int weaponIndex)
    {
        // 생성위치
        Vector3 pos = transform.position + new Vector3(0, 0.9f, 1.0f);

        yield return new WaitForSeconds(0.1f);
        playerWeapon = PhotonNetwork.Instantiate(weapons[weaponIndex].name, pos, Quaternion.identity);

        //playerWeapon = PhotonNetwork.InstantiateRoomObject(weapons[weaponIndex].name, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            float r = Input.GetAxis("Mouse X");

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            transform.Translate(moveDir * Time.deltaTime * 10.0f);

            transform.Rotate(Vector3.up * Time.deltaTime * r * 100.0f);
        }
    }

    // 플레이어 커스텀 프로퍼티 업데이트 시 호출되는 콜백함수
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // 변경된 프로퍼티를 불러와서 저장하기
        int selectedWeapon = (int)changedProps[SELECTED_WEAPON];
        int bodyColor = (int)changedProps[BODY_COLOR];

        Debug.Log($"플레이어 : {targetPlayer.NickName} / 선택 무기 : {selectedWeapon}/ 바디 색상 : {bodyColor}");
    }

}
