
using System;

using UnityEngine;

[Serializable] public enum ActionType { MOVEMENT_PRESS, TAB_PRESSED, DASH_PRESSED, SHOOT_PRESSED, SPRINT_PRESSED, BRAKE_PRESSED, AUGMENT_BUY }

[CreateAssetMenu(menuName = "Tutorial")]
public class Tutorial : ScriptableObject {
    public ActionType Action;
    public string Text;
    public float TimeOut = 5f;
}