using System;
using UnityEngine;

[Serializable]
public class TutorialItem
{
   public string TutorialText;
   public RectTransform TutorialTarget;
   public GameObject[] EnableGameobjects;
   public GameObject[] DisableGameobjects;
}
