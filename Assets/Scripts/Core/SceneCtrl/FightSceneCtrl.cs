using UnityEngine;
using System.Collections;


///<summary>
///战斗场景控制
///</summary>
public class FightSceneCtrl : Singleton<FightSceneCtrl>
{
    /// <summary>
    /// 主角出生点
    /// </summary>
    [SerializeField]
    private Transform m_PlayerBornPos;
    [SerializeField]
    private Transform[] enemyBornPos;
    private float enemyBorn = 0;
    public float enemyBornTime = 5f;
    public Object EnemyPrefabs;
    public Character currentPlayer;
    public PlayerBlue playerBlue;
    public PlayerRed playerRed;
    public GameObject cam;

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("CurrentRoleIndex");
        Object obj;
        GameObject player;
        switch (selectedIndex)
        {
            case 0:
                //加载玩家
                obj = Resources.Load("Role/Player/PlayerBlue");
                player = GameObject.Instantiate(obj, m_PlayerBornPos) as GameObject;

                EnemyPrefabs = Resources.Load("Role/Enemy/EnemyBlack");
                playerBlue = player.GetComponent<PlayerBlue>();
                playerBlue.name = playerBlue.name.Replace("(Clone)", "");
                currentPlayer = playerBlue;
                break;
            default:
                //加载玩家
                obj = Resources.Load("Role/Player/PlayerRed");
                player = GameObject.Instantiate(obj, m_PlayerBornPos) as GameObject;

                EnemyPrefabs = Resources.Load("Role/Enemy/EnemyBlack");
                playerRed = player.GetComponent<PlayerRed>();
                playerRed.name = playerRed.name.Replace("(Clone)", "");
                currentPlayer = playerRed;
                break;
        }
    }

    private void Update()
    {
        if (enemyBorn < Time.time)
        {
            enemyBorn = Time.time + enemyBornTime;
            if (enemyBornPos.Length > 0)
            {
                Transform tsf = enemyBornPos[(int)(Random.value * enemyBornPos.Length)];
                GameObject.Instantiate(EnemyPrefabs, tsf.position, tsf.rotation);
            }
        }
    }

    public void SetPlayerBlue()
    {
        if (playerBlue == null)
        {
            Object obj = Resources.Load("Role/Player/PlayerBlue");
            GameObject player = GameObject.Instantiate(obj, m_PlayerBornPos) as GameObject;
            playerBlue = player.GetComponent<PlayerBlue>();
            playerBlue.name = playerBlue.name.Replace("(Clone)", "");
            playerBlue.transform.position = playerRed.transform.position;
            Destroy(playerRed.gameObject);
            playerRed = null;
            currentPlayer = playerBlue;
        }
    }

    public void SetPlayerRed()
    {
        if (playerRed == null)
        {
            Object obj = Resources.Load("Role/Player/PlayerRed");
            GameObject player = GameObject.Instantiate(obj, m_PlayerBornPos) as GameObject;
            playerRed = player.GetComponent<PlayerRed>();
            playerRed.name = playerRed.name.Replace("(Clone)", "");
            playerRed.transform.position = playerBlue.transform.position;
            Destroy(playerBlue.gameObject);
            playerBlue = null;
            currentPlayer = playerRed;
        }
    }

    public void OpenDefeatPanel()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.OpenDefeatPanel();
    }
}