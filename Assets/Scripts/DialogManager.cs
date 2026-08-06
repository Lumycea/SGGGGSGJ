using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    [SerializeField] private TMP_Text text;

    private Queue<string> dialog = new();

    void Start()
    {
        Instance = this;
    }

    public void AddDialog(string source, string line)
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
