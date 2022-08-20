using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Sendler : MonoBehaviour
{
    private ListNode _head;
    private ListRandom rand;
    private int Counter = 10;
    private void Awake()
    {
        _head = new ListNode();
        CreateDetaStructToTest(Counter, _head);
        gameObject.GetComponent<Button>().onClick.AddListener(async ()  =>
        {
            FileStream stream = new FileStream("file.txt", FileMode.Open);
            rand.Head = _head;
            ListNode head = new ListNode();
            head = await rand.Deserialize(stream);
            Debug.Log(head.Data);
        });
    }
    private void CreateDetaStructToTest(int Count, ListNode Current)
    {
        Current.Previous = null;
        for (int i = 0; i < Count; i++)
        {
            var node = new ListNode();
            var currentSave = Current;
            Current.Data = "randomString" + i;
            if (i == Count - 1) 
            {
                Current.Next = null;
                break;
            } 
            else Current.Next = node;
            Current = Current.Next;
            Debug.Log(i);
            if (i != 0) Current.Previous = currentSave;
        }

        Stream s = new MemoryStream();
        rand = new ListRandom();
        rand.Head = _head;
        FileStream stream = new FileStream("file.txt", FileMode.Create);
        rand.Serialize(stream, _head, Counter);
    }

}
