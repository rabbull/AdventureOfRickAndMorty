using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : Button {
    protected override void OnClickEvent()
    {
        // open brainfuck wikipedia
        Application.OpenURL("https://en.wikipedia.org/wiki/Brainfuck");
    }
}
