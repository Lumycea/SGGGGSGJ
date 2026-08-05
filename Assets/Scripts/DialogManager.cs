using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private Queue<string> dialog = new();

    void Start()
    {
        for (int i = 0; i < 15; ++i)
            AddDialog("Garry", "Hi!" + i);
    }

    void AddDialog(string source, string line)
    {
        dialog.Enqueue($"[{source}] {line}\n");
        ConstructDialog();
    }

    void ConstructDialog()
    {
        var str = "";
        foreach (var l in dialog) { str += l; }

        text.text = str;
    }

    void Update()
    {
        if (text.isTextOverflowing)
        {
            dialog.Dequeue();
            ConstructDialog();
        }
    }
}
