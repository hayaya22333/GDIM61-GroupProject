using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storynode : ScriptableObject
{
    public string speaker;
    [TextArea(2,4)]
    public string[] line;
}
