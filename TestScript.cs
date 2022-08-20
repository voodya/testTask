

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;


public class ListNode
{
    public ListNode Previous;
    public ListNode Next;
    public ListNode Random;
    public string Data;
}

public class ListRandom
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public async void Serialize(FileStream s, ListNode _head, int _count)
    {
        Head = _head;
        Count = _count;
        ListNode current;
        int[] counter = new int[Count];
        current = Head;
        ListNode current2 = current.Next;
        for (int i = 0, j = 0; i < Count && j < Count - 1; i++)
        {
            if (current != current2 && current.Random == current2)
            {
                counter[j] = i;
                current = current.Next;
                current2 = Head;
                j++;
                i = -1;
            }
            else counter[j] = -1;
            current2 = current2.Next;
        }
        byte[] intBytesCount2 = BitConverter.GetBytes(Count);
        await s.WriteAsync(intBytesCount2, 0, intBytesCount2.Length);
        current = current2;
        for (int i = 0; i < Count - 1; i++)
        {
            byte[] bufferText = Encoding.Default.GetBytes(current.Data);
            await s.WriteAsync(bufferText, 0, bufferText.Length);
            byte[] intBytes = BitConverter.GetBytes(counter[i]);
            await s.WriteAsync(intBytes, 0, intBytes.Length);
            current = current.Next;
        }
    }

    public async Task<ListNode> Deserialize(FileStream s)
    {
        Count = s.ReadByte();
        Head = new ListNode();
        ListNode current = Head;
        int[] counter = new int[Count];
        for (int i = 0; i < Count; i++)
        {
            if (i == 0)
            {
                byte[] output = new byte[13];
                s.Seek(4, SeekOrigin.Begin);
                await s.ReadAsync(output, 0, output.Length);
                current.Data = Encoding.Default.GetString(output);
                current.Previous = null;
                current.Next = new ListNode();
                counter[i] = s.ReadByte();
            }
            else
            {
                current.Next.Previous = current;
                current = current.Next;
                s.Seek(3, SeekOrigin.Current);
                byte[] output = new byte[13];
                await s.ReadAsync(output, 0, output.Length);
                current.Data = Encoding.Default.GetString(output);
                if (i != Count - 1)
                    current.Next = new ListNode();
                else
                    Tail = current;
                counter[i] = s.ReadByte();
            }
        }
        return current;
    }   
}
