using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum RadiusType { normal, spread, straight, up };

// 타워 오브젝트입니다.
// mouseCursor에서 타워 설치하기 전에는 비활성화 상태입니다.
// 타워를 설치하는 순간 활성화됩니다.
public class Tower : MonoBehaviour
{
    public int idx; // 타워 인덱스 (종류를 나타냄)
    private TowerManager.TowerData data;

    // 보너스 (중첩된 보너스는 합연산으로 적용)
    private int radiusBonus; // 퍼센트 증가량
    private int delayBonus; // 퍼센트 증가량
    private int damageBonus; // 수치 증가량
    private int speedBonus; // 퍼센트 증가량

    private Dictionary<int, int> stackedTower = new Dictionary<int, int>(); // 쌓인 타워의 목록 (맨 아래 타워에만 존재함). key: idx, value: 개수
    private HashSet<int> activatedSynergies = new HashSet<int>(); // 현재 활성화된 시너지

    private GameObject radiusSphere; // 반경을 나타내는 투명한 구
    private MouseCursor mouseCursor; // 마우스 커서 오브젝트
    private TowerManager towerManager;
    private EnemyManager enemyManager;
    private SynergyManager synergyManager;
    private bool select; // 오브젝트가 선택되었는지 나타냄
    protected bool attackable; // 공격 딜레이가 지나서 공격 가능한가
    private int level; // 현재 타워 레벨 (0 ~ 4)

    private GameObject towerStack;
    private Tower upperTower;
    private Tower lowerTower;
    private List<GameObject> bulletList;

    public int GetLevel() { return level; }
    public string GetTowerName() { return data.towerName; }
    public RadiusType GetRadiusType() { return data.radiusType; }
    public int GetCost() { return data.cost[level]; }
    public int GetNextCost() { return data.cost[level + 1]; }
    public int GetSellCost() { return data.cost.GetRange(0, level + 1).Sum() * 4 / 5; }
    public float GetRadius() { return data.radius[level] * (1 + radiusBonus / 100f); }
    public float GetDelay() { return data.delay[level] * (1 + delayBonus / 100f); }
    public GameObject GetBullet() { return data.bullet[level]; }
    public int GetDamage() { return data.damage[level] + damageBonus; }
    public float GetSpeed() { return data.speed[level] * (1 + speedBonus / 100f); }
    public bool GetTargeting() { return data.targeting[level]; }
    public float GetLife() { return data.life[level]; }
    public int GetBulletHp() { return data.bulletHp[level]; }

    protected IEnumerator SetAttackable()
    {
        yield return new WaitForSeconds(GetDelay());
        attackable = true;
    }

    public void SetSelect(bool b)
    {
        select = b;
        radiusSphere.SetActive(b);
    }

    public void SetTowerStack(bool b)
    {
        towerStack.SetActive(b);
    }

    public Dictionary<int, int> GetStackedTower()
    {
        if (lowerTower == null) return stackedTower;
        else return lowerTower.GetStackedTower();
    }
    public void SetUpperTower(Tower t) { upperTower = t; }
    public Tower GetUpperTower() { return upperTower; }

    public void AddLevel() { level++; UpdateRadiusSphere(); }

    void Awake()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
        enemyManager = FindObjectOfType<EnemyManager>();
        towerManager = FindObjectOfType<TowerManager>();
        synergyManager = FindObjectOfType<SynergyManager>();
        radiusSphere = transform.GetChild(1).gameObject;
        attackable = true;
        level = 0;
        towerStack = transform.GetChild(2).gameObject;
        data = towerManager.towerInfo[idx];
        bulletList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSynergy(); // Start가 OnInstallTower보다 늦게 불려서 OnInstallTower에서 UpdateSynergy를 호출하면 안 되더라...
    }

    // Update is called once per frame
    void Update()
    {
        if (attackable)
        {
            // todo: 타겟 설정 바꾸는 기능
            Enemy target = GetTarget(e => e.GetLocation()); // 가장 앞에 있는 적
                                                            // Enemy target = GetTarget(e => -(e.transform.position - transform.position).magnitude); // 가장 가까운 적
            if (target != null)
            {
                // 투사체를 자식 오브젝트로 생성
                Bullet newBullet = Instantiate(GetBullet(), transform.position, transform.rotation, transform).GetComponent<Bullet>();
                bulletList.Add(newBullet.gameObject);
                newBullet.SetTarget(target);
                newBullet.SetBulletInfo(GetDamage(), GetSpeed(), GetTargeting(), GetLife(), GetBulletHp());
                StartCoroutine(SetAttackable());
                attackable = false;
            }
        }
    }

    private void OnDestroy()
    {
        // 투사체 제거
        foreach (GameObject bullet in bulletList)
        {
            Destroy(bullet.gameObject);
        }
    }

    public void UpdateRadiusSphere()
    {
        float scale = 2 * GetRadius();
        switch (GetRadiusType())
        {
            case RadiusType.normal:
            case RadiusType.straight:
                radiusSphere.transform.localScale = (scale == 0 ? 1 : scale) * new Vector3(1, 1, 1);
                break;
            case RadiusType.spread:
                radiusSphere.transform.localScale = new Vector3(scale, radiusSphere.transform.localScale.y, scale);
                break;
        }
    }

    private Enemy GetTarget(Func<Enemy, float> func) // func 함수값이 최대인 적을 선택
    {
        HashSet<Enemy> set = enemyManager.GetEnemySet();
        Enemy target = null;
        float targetKey = Mathf.NegativeInfinity;
        if (set.Count > 0)
        {
            foreach (Enemy e in set)
            {
                float distance = (e.transform.position - transform.position).magnitude;
                float key = func(e);
                if ((GetRadius() == 0 || distance <= GetRadius()) && key > targetKey)
                {
                    target = e;
                    targetKey = key;
                }
            }
        }

        return target;
    }

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            mouseCursor.OnClickTower(gameObject);
    }

    public void OnInstallTower()
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Tower");
        // 자기 자신의 Mesh를 감지하는 것을 막기 위해서 0.4만큼 아래서 Raycast
        if (Physics.Raycast(transform.position + new Vector3(0, -0.4f, 0), new Vector3(0, -1, 0), out hit, 1, layerMask))
        {
            lowerTower = hit.transform.GetComponent<Tower>();
            lowerTower.SetUpperTower(this);
        }
        AddTowerToStackedTower(idx);
    }

    public void OnSell()
    {
        // 아래 타워의 Tower Stack 비활성화
        if (lowerTower != null)
        {
            lowerTower.RemoveTowerToStackedTower(idx);
        }
        UpdateSynergy();
    }

    public void AddTowerToStackedTower(int idx)
    {
        if (lowerTower != null) lowerTower.AddTowerToStackedTower(idx);
        else
        {
            if (stackedTower.ContainsKey(idx)) stackedTower[idx] += 1;
            else stackedTower.Add(idx, 1);
        }
    }

    public void RemoveTowerToStackedTower(int idx)
    {
        if (lowerTower != null) lowerTower.RemoveTowerToStackedTower(idx);
        else
        {
            stackedTower[idx] -= 1;
            if (stackedTower[idx] == 0) stackedTower.Remove(idx);
        }
    }

    public void UpdateSynergy()
    {
        if (lowerTower != null) lowerTower.UpdateSynergy();
        Dictionary<int, int> dict = GetStackedTower();
        activatedSynergies.Clear();
        foreach (var synergyInfoPair in synergyManager.synergyInfo)
        {
            SynergyManager.Synergy synergy = synergyInfoPair.Value;
            bool check1 = true; // 해당 시너지를 위한 타워를 모두 쌓았는가?
            bool check2 = false; // 타워 종류가 시너지를 구성하는가?
            foreach (var idxCountPair in synergy.idxCountPairs)
            {
                int towerIdx = idxCountPair.Key;
                int towerCount = idxCountPair.Value;
                if (!dict.ContainsKey(towerIdx) || dict[towerIdx] < towerCount) check1 = false;
                if (towerIdx == idx) check2 = true;
            }
            if (check1 && check2) activatedSynergies.Add(synergyInfoPair.Key);
        }

        // 스탯 보너스 처리
        radiusBonus = 0;
        delayBonus = 0;
        damageBonus = 0;
        speedBonus = 0;

        foreach (int synergyIdx in activatedSynergies)
        {
            radiusBonus += synergyManager.synergyInfo[synergyIdx].bonus.radiusBonus;
            delayBonus += synergyManager.synergyInfo[synergyIdx].bonus.delayBonus;
            damageBonus += synergyManager.synergyInfo[synergyIdx].bonus.damageBonus;
            speedBonus += synergyManager.synergyInfo[synergyIdx].bonus.speedBonus;
        }

        UpdateRadiusSphere();
    }

    public string GetStatusText()
    {
        string s = string.Format("공격력 {0}", GetDamage());
        if (idx == 6)
        {
            s = string.Format("적 속도 -{0}.{1}", GetDamage() / 10, GetDamage() % 10);
        }

        if (idx == 1 || idx == 2 || idx == 3)
        {
            s += String.Format("\n사거리 {0}", GetRadius());
        }
        if (idx != 6 && idx != 7)
        {
            s += String.Format("\n딜레이 {0}", GetDelay());
        }
        return s;
    }

    public string GetStatusPreviewText()
    {
        string s = "";
        if (level == 4) return s;

        if (data.damage[level] != data.damage[level + 1])
        {
            if (idx != 6)
            {
                s += string.Format("→ {0}", data.damage[level + 1] + damageBonus);
            }
            else
            {
                s += string.Format("→ -{0}.{1}", (data.damage[level + 1] + damageBonus) / 10, (data.damage[level + 1] + damageBonus) % 10);
            }
        }

        if (idx == 1 || idx == 2 || idx == 3)
        {
            s += "\n";
            if (data.radius[level] != data.radius[level + 1])
            {
                s += string.Format("→ {0}", data.radius[level + 1] * (1 + radiusBonus / 100f));
            }
        }

        if (idx != 6 && idx != 7)
        {
            s += "\n";
            if (data.delay[level] != data.delay[level + 1])
            {
                s += string.Format("→ {0}", data.delay[level + 1] * (1 + delayBonus / 100f));
            }
        }

        return s;
    }
}
