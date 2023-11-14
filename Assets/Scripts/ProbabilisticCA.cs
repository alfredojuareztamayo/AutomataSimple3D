
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProbabilisticCA : MonoBehaviour {
    [SerializeField] int width, height;
    int cellSize, height2;
    bool[,] automaton, buffer;
    float lastTime, interval, delta;
    int cols, rows;
    [SerializeField] GameObject col;
    // Start is called before the first frame update
    void Start() {
        cellSize = 20;
        cols = 20;
        rows = 20;
        automaton = new bool[cols, rows];
        buffer = new bool[cols, rows];
        Debug.Log(automaton.Length);
        automaton[cols-1, 0] = true;
        interval = 300;
        initial();
        InvokeRepeating("evolution", 1.5f, 2f);
    }

    // Update is called once per frame
    void Update() {

    }

    void initial() {
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                automaton[i, j] = Random.Range(0, 2) > 0.5f;
                printEvolution(i, j, automaton[i, j]);
            }
        }
    }



    void evolution() {
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                int vecinosVivos = 0;
                for (int k = i - 1; k <= i + 1; k++) {
                    for (int l = j - 1; l <= j + 1; l++) {
                        if ((k < 0) || (k >= cols) || (l < 0) || (l >= rows)) {
                            continue;
                        }
                        if ((i == k) && (j == l)) {
                            continue;
                        }
                        if (automaton[k,l]) {
                            vecinosVivos++;
                        }
                    }
                }
                if (vecinosVivos > 6) {
                    if (automaton[i,j]) {
                        if (Random.Range(0,5) < 3) {
                            automaton[i,j] = false;
                        }
                    }
                }
                if (vecinosVivos == 1 || vecinosVivos == 2) {
                    if (!automaton[i,j]) {
                        automaton[i,j] = true;
                    }
                }
                if (vecinosVivos == 3) {
                    automaton[i,j] = false;
                }
                printEvolution(i, j, automaton[i, j]);
            }
        }
    }
    void printEvolution(int x, int z, bool state) {
        GameObject cube = Instantiate(col, new Vector3(x, 0, z), Quaternion.identity);
        cube.GetComponent<Renderer>().material.color = state ? Color.black : Color.green;
    }

}

