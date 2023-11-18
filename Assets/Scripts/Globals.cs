using UnityEngine;

public class Globals : Singleton<Globals> {
    public int Money = 0;
    public Camera MainCamera;

    public void AddMoney(int amount) {
        Money += amount;
    }
}