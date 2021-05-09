using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Swarming
{
    //聊天对话管理器
    public class ChatManager : SingletonBase<ChatManager>
    {
        public TextAsset textAsset;
        public Text text;
        public Dictionary<string, List<string>> chatDic = new Dictionary<string, List<string>>();
        private string currentOrder;//当前要进行对话的段落 1-1  1-2  2-1.......
        private int textIndex = 1;//每一行的下标
        private bool chatTrigger;//对话触发
        private bool isFinish;//单行是否播放玩

        public void Initialize()
        {
            textAsset = Resources.Load("text") as TextAsset;
            GetDataFromAsset();
            MonoManager.Instance.AddUpdateListener(Update, "ChatManager");
        }
        void Update()
        {
            Chat(currentOrder);
        }

        private void Chat(string order)
        {
            if (order != null && chatTrigger)
            {
                if (Input.GetMouseButtonDown(0) && textIndex <= chatDic[order].Count - 1 && isFinish)
                    MonoManager.Instance.StartCoroutine(ChatDely(chatDic[order][textIndex++], 0.1f));
                else if (Input.GetMouseButtonDown(0) && isFinish)
                    EndChat();
            }
        }

        //逐字播放
        private IEnumerator ChatDely(string list,float dely)
        {
            text.text = "";
            isFinish = false;
            for (int i = 0; i < list.Length; i++)
            {
                text.text += list[i];
                yield return new WaitForSeconds(dely);
            }
            isFinish = true;
        }

        //提供给外部的调用方法开始对话
        public void StartChat(string order)
        {
            chatTrigger = true;
            isFinish = true;
            currentOrder = order;
            //text.gameObject.SetActive(true);
            UIManager.Instance.OpenView<DialogView>("Dialog/DialogBarView");
            MonoManager.Instance.StartCoroutine(ChatDely(chatDic[order][textIndex++], 0.1f));
        }

        private void EndChat()
        {
            chatTrigger = false;
            currentOrder = null;
            textIndex = 1;
            text.gameObject.SetActive(false);
        }

        //读取文本数据
        private void GetDataFromAsset()
        {
            var levelTextArray = textAsset.text.Split('\n');
            string chatKey = null;
            foreach (var item in levelTextArray)
            {
                if (item[0] == '[')
                {
                    chatKey = item.Substring(1, 3);
                    if (!chatDic.ContainsKey(chatKey))
                    {
                        chatDic.Add(chatKey, new List<string>());
                    }
                }
                chatDic[chatKey].Add(item);
            }
        }
    }
}
