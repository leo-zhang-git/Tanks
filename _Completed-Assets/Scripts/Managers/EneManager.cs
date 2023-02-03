using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public static class SpawnPosLimit
    {
        public static float maxX = 35f, minX = -35f;
        public static float maxZ = 30f, minZ = -30f;
        public static float lockY = 0f;
    }

    [Serializable]
    public class DropPair
    {
        public GameObject obj;
        public float rate;
    }
    public class EneManager : MonoSingleton<EneManager>
    {
        public enum EneType
        {
            DashTank,
            ShootTank
        }
        public Dictionary<EneType, Color> colorSet;
        public GameObject m_EneTankPrefab;
        public DropPair[] m_Drops;
        public Transform m_DropNode;
        public event Action<GameObject> EneOnSpawn;
        public int EneNum
        {
            get { return eneNum; }
        }
        [SerializeField]
        private int eneNum;
        public Queue<GameObject> Enemies
        {
            get 
            {
                Queue<GameObject> res = new Queue<GameObject>();
                for(int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).gameObject.activeSelf) res.Enqueue(transform.GetChild(i).gameObject);
                }
                return res;
            }
        }

        [SerializeField]
        private Queue<GameObject> pool;

        protected override void Awake()
        {
            base.Awake();
            pool= new Queue<GameObject>();
            colorSet = new Dictionary<EneType, Color>()
            {
                { EneType.DashTank, Color.red},
                { EneType.ShootTank, Color.black}
            };
        }
        // Start is called before the first frame update
        void Start()
        {
            eneNum= 0;
            EneOnSpawn += (ene) =>
            {
                ene.GetComponent<TankHealth>().EneDie += EnemyDie;
            };
        }
        public void SpawnEne(int number, EneType eneType, int strength)
        {
            for(int i = 0; i < number; i++)
            {
                SpawnEne(eneType, strength);
            }
        }
        public void EnemyDie(GameObject ene)
        {
            pool.Enqueue(ene);
            eneNum--;
            ene.GetComponent<TankHealth>().EneDie -= EnemyDie;

            foreach(var drop in m_Drops)
            {
                if (HaveDrop(drop.obj, drop.rate, ene.transform.position)) break;
            }
        }
        public void KillAllEne()
        {
            TankHealth ene = null;
            do
            {
                if (ene) ene.TakeDamage(float.MaxValue);
                ene = transform.GetChild(0).GetComponent<TankHealth>();
            } while (ene.gameObject.activeSelf);
            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    transform.GetChild(i).gameObject.SetActive(false);
            //}
        }
        public void DestroyDropsOnField()
        {
            for(int i = 0; i < m_DropNode.childCount; i++)
            {
                Destroy(m_DropNode.GetChild(i).gameObject);
            }
        }
        public void FreezingAllEne(float time)
        {
            for(int i = 0; i < EneNum; i++)
            {
               var controller = transform.GetChild(i).GetComponent<DashTankController>();
               if (controller) controller.Pause(time);
               else transform.GetChild(i).GetComponent<ShootTankController>().Pause(time);
            }
        }
        private void SpawnEne(EneType eneType, int strength)
        {
            GameObject ene;
            Vector3 pos = GetSpawnPos();
            Vector3 lookPos = GameManager.Instance.GetPlayerTank().transform.position;
            lookPos.y = pos.y;
            
            if (pool.Count > 0)
            {
                ene = pool.Dequeue();
                ene.transform.position = pos;
                ene.SetActive(true);
            }
            else
            {
                ene = Instantiate(m_EneTankPrefab, pos,Quaternion.Euler(Vector3.zero), gameObject.transform);
            }

            MeshRenderer[] renderers = ene.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = colorSet[eneType];
            }

            ene.transform.SetAsFirstSibling();
            switch (eneType)
            {
                case EneType.DashTank:
                    ene.AddComponent<DashTankController>();
                    break;
                case EneType.ShootTank:
                    ene.AddComponent<ShootTankController>();
                    break;
                default:
                    break;
            }

            TankHealth tankHealth = ene.GetComponent<TankHealth>();
            tankHealth.m_StartingHealth += tankHealth.m_StartingHealth * strength * 2f / 100f;

            eneNum++;
            EneOnSpawn(ene);
        }
        private Vector3 GetSpawnPos()
        {
            Vector3 pos;
            while (true)
            {
                pos = new Vector3(UnityEngine.Random.Range(SpawnPosLimit.minX, SpawnPosLimit.maxX), SpawnPosLimit.lockY, UnityEngine.Random.Range(SpawnPosLimit.minZ, SpawnPosLimit.maxZ));
                if (Physics.OverlapSphere(pos, 1f, 1).Length == 0) break;
            }
            return pos;
        }
        private bool HaveDrop(GameObject obj, float rate, Vector3 pos)
        {
            bool ret = UnityEngine.Random.Range(0f,1f) <= rate;
            if(ret)
            {
                Instantiate<GameObject>(obj, pos + Vector3.up, Quaternion.identity, m_DropNode);
            }
            return ret;
        }
    }
}
