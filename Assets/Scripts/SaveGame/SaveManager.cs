using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;
using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Types;


namespace Swarming
{
    public class SaveManager : SingletonBase<SaveManager>
    {
        [System.Serializable]
        public class SaveData
        {
            public Vector3 m_RobotPos;

            public Vector3 m_WindPos;

            public bool m_visualAbility;

            public bool m_charAbility;

            public int m_golsNum;

            public int m_life;

            public SaveData()
            {
                m_RobotPos = new Vector3(0, 2f, 0);
                m_WindPos = new Vector3(0, 2f, 0);
                m_visualAbility = false;
                m_charAbility = false;
                m_golsNum = 0;
                m_life = 3;
            }
        }

        private SaveData m_saveData = new SaveData();

        public SaveData saveData { get { return m_saveData; } }
        
        public string identifier = "Save.dat";


        public void AddEvent()
        { 
            EventCenter.Instance.AddEventListener(EventDefine.SAVE_GAME, Save);

        }

        public void SaveGold()
        { 
            
        }

        public void Save(object obj)
        {
            m_saveData.m_RobotPos = GameManager.Instance.RobotPlayer.transform.position;
            m_saveData.m_WindPos = GameManager.Instance.RobotPlayer.transform.position;
            //m_saveData.m_golsNum = GameManager.Instance.GoldNum;
            //m_saveData.m_life = GameManager.Instance.Life;
            //m_saveData.m_visualAbility = CharactorManager.Instance.canChangeVisual;
            //m_saveData.m_charAbility = CharactorManager.Instance.canChangeChar;
            

            SaveGame.Save<SaveData>(identifier, m_saveData);
        }

        public void Load(bool reSet = false)
        {
            SaveData saveData = SaveGame.Load<SaveData>(identifier, new SaveData());

            if (reSet)
            {
                GameManager.Instance.GoldNum = saveData.m_golsNum;
                GameManager.Instance.Life = saveData.m_life;
                CharactorManager.Instance.canChangeVisual = saveData.m_visualAbility;
                CharactorManager.Instance.canChangeChar = saveData.m_charAbility;
            }
            else
            {
                GameManager.Instance.RobotPlayer.transform.position = saveData.m_RobotPos;
                GameManager.Instance.WindPlayer.transform.position = saveData.m_WindPos;
                Physics.SyncTransforms();
            }
            
        }

        public void Clear()
        {
            SaveGame.Clear();
        }

    }
}
