using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryCA : MonoBehaviour {
    [SerializeField] GameObject cubeAlive, cubeDead;
    int[] ruleSet;
    bool[] grid, buffer;

    int posX = 0;
    GameObject[] cubes;
    public int limitEvole;
    public int grid_y;
    public int ruleBinary;
    public bool initialCondition;
    // public TMP_InputField ruleInput;
    int ruleNumber;

    // Start is called before the first frame update
    void Start() {
        cubes = new GameObject[grid_y * limitEvole];
        ruleSet = new int[8];
        grid = new bool[grid_y];
        buffer = new bool[grid_y];
        InitialCondition(initialCondition);
        ToBinary(ruleBinary);
        // Evolve();
    }



    // Update is called once per frame
    void Update() {

    }

    void InitialCondition(bool isSimple) {
        for (int i = 0; i < grid.Length; i++) {
            grid[i] = false;
        }
        if (isSimple) {
            grid[grid.Length / 2] = true;
        }
        if (!isSimple) {
            for (int i = 0; i < grid.Length; i++) {
                grid[i] = Random.Range(0, 2) == 1;
            }
           
        }
    }

    public void StartEvolveButtom() {
        // ruleNumber = Mathf.Clamp(inputRuleNumber, 0, 255);
        StartCoroutine(StartEvolve());
    }

    void Evolve() {
        int j = 0;

        bool res;
        buffer = new bool[grid_y];
        //ClearBuff();
        for (int i = 0; i < grid.Length; i++) {
            if (i == 0) {
                res = Rules(grid[grid.Length - 1], grid[i], grid[i + 1]);
            } else if (i == grid.Length - 1) {
                res = Rules(grid[i - 1], grid[i], grid[0]);
            } else {
                res = Rules(grid[i - 1], grid[i], grid[i + 1]);
            }
            buffer[i] = res;
        }
        PrintGrid();
        grid = buffer; // Actualiza grid con los nuevos valores
                       //j++;

    }


    void PrintGrid() {
        int posZ = 0;
        //GameObject cube;
        for (int i = 0; i < grid.Length; i++) {
            if (grid[i]) {
                Instantiate(cubeAlive, new Vector3(posX, 0, posZ), Quaternion.identity);
            } else {
                Instantiate(cubeDead, new Vector3(posX, 0, posZ), Quaternion.identity);
            }
            posZ++;
        }
        posX++;
    }

    void DestroyCubes() {
        foreach (GameObject cube in cubes) {
            Destroy(cube);
        }
    }

    void ClearBuff() {
        for (int i = 0; i < buffer.Length; i++) {
            buffer[i] = false;
        }
    }

    void copyBuff() {
        for (int i = 0; i < buffer.Length; i++) {
            grid[i] = buffer[i];
        }
    }

    bool Rules(bool a, bool b, bool c) {
        if (a && b && c) {
            return ruleSet[0] == 1;
        }
        if (a && b && !c) {
            return ruleSet[1] == 1;
        }
        if (a && !b && c) {
            return ruleSet[2] == 1;
        }
        if (a && !b && !c) {
            return ruleSet[3] == 1;
        }
        if (!a && b && c) {
            return ruleSet[4] == 1;
        }
        if (!a && b && !c) {
            return ruleSet[5] == 1;
        }
        if (!a && !b && c) {
            return ruleSet[6] == 1;
        }
        if (!a && !b && !c) {
            return ruleSet[7] == 1;
        }
        return false;
    }


    void ToBinary(int number) {
        int[] numberarray = new int[8];
        for (int i = 7; i >= 0; i--) {
            ruleSet[i] = number % 2;
            number = number / 2;
        }
        Debug.Log("Reglas: " + string.Join(",", ruleSet));
    }



    IEnumerator StartEvolve() {

        while (true) {
            Evolve();
            yield return new WaitForSeconds(2);
        }
    }
}