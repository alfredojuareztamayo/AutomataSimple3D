using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase que genera un tablero y controla la generación y visualización de cubos en Unity.
/// </summary>
public class GenerateBoard : MonoBehaviour
{
   [SerializeField] int grid_x;
   [SerializeField] int grid_y;
    [SerializeField] GameObject cubeAlive;
    [SerializeField] GameObject cubeDead;
    GameObject[] cubes;
    Tiles[,] tiles;
    float timeInScreen = 4f;
    int[] defaultBuffer;


    void Start ()
    {
        defaultBuffer = new int[grid_y];
        GenerateDefaultArray();
        cubes = new GameObject[grid_x*grid_y];
        tiles = new Tiles[grid_x, grid_y];
        StartCoroutine(ReGenerateGridRotine());
       // GenerateGrid(tiles);
       // PrintGrid(tiles);
        //FillArrayCubes();
       // InstanceCubes();
       // TurnCubesAndSetPosition();
        //SetCubesGrid(tiles);
        Debug.Log("El tamaño del tablero es: " + tiles.Length);
        Debug.Log("El tamaño de los cubos son: " + cubes.Length);
    }

    // Update is called once per frame
    void Update ()
    {
       // ReGenerateGrid();
    }

    /// <summary>
    /// Genera el tablero inicial y llena la matriz de Tiles.
    /// </summary>
    /// <param name="tiles">La matriz de Tiles que representa el tablero.</param>
    private void GenerateDefaultArray()
    {
        for(int i = 0; i < grid_y; i++)
        {
            defaultBuffer[i] = (Random.Range(0,2));
        }
    }
    private void GenerateGrid(Tiles[,] tiles)
    {
        
        for(int x = 0; x < grid_x; x++)
        {
            for(int y = 0; y < grid_y; y++)
            {
                tiles[x, y] = new Tiles(x, y);
                int randNum = Random.Range(0, 2);
                tiles[x, y].SetIsDead(randNum  != 1 ? true : false);
            }
        }
    }

    /// <summary>
    /// Imprime la matriz del tablero en la consola.
    /// </summary>
    /// <param name="tiles">La matriz de Tiles que se va a imprimir.</param>

    private void PrintGrid(Tiles[,] tiles)
    {
        string printArray = " ";
        for (int x = 0; x < grid_x; x++)
        {
            for (int y = 0; y < grid_y; y++)
            {
                printArray += tiles[x, y].m_isDead ? '0' : '1';
                printArray += ',';
            }
            printArray += '\n';
        }
        Debug.Log(printArray);
    }

    /// <summary>
    /// Coloca los cubos en el tablero según el estado de Tiles.
    /// </summary>
    /// <param name="tiles">La matriz de Tiles que determina la posición de los cubos.</param>

    public void SetCubesGrid(Tiles[,] tiles)
    {
        for(int x =0; x < grid_x; x++)
        {
            for(int y=0; y < grid_y; y++)
            {                
                GameObject cube = Instantiate(tiles[x, y].m_isDead? cubeDead:cubeAlive, new Vector3(tiles[x, y].m_x,0, tiles[x, y].m_y), Quaternion.identity);
                Destroy(cube, timeInScreen);                
            }
        }
    }

    private void GenerateGridWithRules(Tiles[,] tiles)
    {

        for (int x = 0; x < grid_x; x++)
        {
            int[] tempDefault = new int[grid_y];

            for (int y = 0; y < grid_y; y++)
            {
                tiles[x, y] = new Tiles(x, y);
                if (y == 0 )
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
                if (y == grid_y-1)
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
                if (defaultBuffer[y + 1] == 1 & defaultBuffer[y -1] == 1 & defaultBuffer[y] == 1)
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
                if (defaultBuffer[y + 1] == 1 & defaultBuffer[y - 1] == 0 & defaultBuffer[y] == 1)
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
                if (defaultBuffer[y + 1] == 0 & defaultBuffer[y - 1] == 1 & defaultBuffer[y] == 1)
                {
                    tiles[x, y].SetIsDead(false);
                    tempDefault[y] = 0;
                    continue;
                }
                if (defaultBuffer[y + 1] == 0 & defaultBuffer[y - 1] == 0 & defaultBuffer[y] == 1)
                {
                    tiles[x, y].SetIsDead(false);
                    tempDefault[y] = 0;
                    continue;
                }
                if (defaultBuffer[y + 1] == 1 & defaultBuffer[y - 1] == 1 & defaultBuffer[y] == 0)
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
                if (defaultBuffer[y + 1] == 1 & defaultBuffer[y - 1] == 0 & defaultBuffer[y] == 0)
                {
                    tiles[x, y].SetIsDead(false);
                    tempDefault[y] = 0;
                    continue;
                }
                if (defaultBuffer[y + 1] == 0 & defaultBuffer[y - 1] == 1 & defaultBuffer[y] == 0)
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
                if (defaultBuffer[y + 1] == 0 & defaultBuffer[y - 1] == 0 & defaultBuffer[y] == 0)
                {
                    tiles[x, y].SetIsDead(true);
                    tempDefault[y] = 1;
                    continue;
                }
            }
            defaultBuffer = tempDefault;
        }
    }


    public void SetCubesGridFig1(Tiles[,] tiles)
    {
        for (int x = 0; x < grid_x; x++)
        {
            for (int y = 0; y < grid_y/2; y++)
            {
                GameObject cubeR = Instantiate(tiles[x, y].m_isDead ? cubeDead : cubeAlive, new Vector3(tiles[x, y].m_x, 0, tiles[x, y].m_y), Quaternion.identity);
                GameObject cubeL = Instantiate(tiles[x, y].m_isDead ? cubeDead : cubeAlive, new Vector3((tiles[x, y].m_x), 0, ((grid_y-1) - tiles[x, y].m_y)), Quaternion.identity);
                Destroy(cubeR, timeInScreen);
                Destroy(cubeL, timeInScreen);
            }
        }
    }

    /// <summary>
    /// Rellena el arreglo de cubos con los prefabs correspondientes a Tiles.
    /// </summary>

    private void FillArrayCubes()
    {   
        int i = 0;
        for (int x = 0; x < grid_x; x++)
        {
            for (int y = 0; y < grid_y; y++)
            {
                cubes[i] = tiles[x, y].m_isDead ? cubeDead : cubeAlive;
                i++;
            }
        }
    }

    /// <summary>
    /// Instancia los cubos en la escena, pero los mantiene desactivados.
    /// </summary>

    private void InstanceCubes()
    {
        foreach(GameObject c in cubes)
        {
             Instantiate(c, Vector3.zero, Quaternion.identity);

            c.SetActive(false);
        }
    }
    /// <summary>
    /// Activa los cubos y establece sus posiciones en la escena.
    /// </summary>
    private void TurnCubesAndSetPosition()
    {
        int i = 0;
        for (int x = 0; x < grid_x; x++)
        {
            for (int y = 0; y < grid_y; y++)
            {
                cubes[i].transform.position = new Vector3(tiles[x, y].m_x, tiles[x, y].m_y, 0);
                cubes[i].SetActive(true);
                i++;
            }
        }
    }
    /// <summary>
    /// Regenera el tablero y actualiza la visualización de los cubos cuando se presiona la tecla Espacio.
    /// </summary>
    public void ReGenerateGrid()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateGrid(tiles);
            PrintGrid(tiles);
            //SetCubesGrid(tiles);
            SetCubesGridFig1(tiles);
        }
    }

    /// <summary>
    /// Rutina que regenera el tablero de forma periódica y actualiza la visualización de los cubos.
    /// </summary>
    /// <returns>Espera una cantidad de tiempo y luego regenera el tablero.</returns>

    IEnumerator ReGenerateGridRotine()
    {
        while (true)
        {
           // GenerateGrid(tiles);
            GenerateGridWithRules(tiles);
            PrintGrid(tiles);
            SetCubesGrid(tiles);
           // SetCubesGridFig1(tiles);
            yield return new WaitForSeconds(timeInScreen);
        }
    }
}
