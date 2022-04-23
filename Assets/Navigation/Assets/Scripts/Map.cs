using UnityEngine;

public class Map : MonoBehaviour {
    public int[,] gridVals;

    public void Display() {
        string result = "";
        for (int row = 0; row < gridVals.GetLength(0); row++) {
            string rowRep = "";
            for (int col = 0; col < gridVals.GetLength(1); col++) {
                rowRep += gridVals[row,col];
            }
            result += rowRep + "\n";
        }
        Debug.Log(result);
    }


}