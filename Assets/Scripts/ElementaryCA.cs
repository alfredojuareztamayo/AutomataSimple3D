using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryCA : MonoBehaviour
{
    [SerializeField] GameObject cubeAlive, cubeDead;
    int[] ruleSet;
    bool[] grid, buffer;
    
    int posX = 0;
    GameObject[] cubes;
    public int limitEvole;
    public int grid_y;
    public int ruleBinary;
    public bool initialCondition;

    // Start is called before the first frame update
    void Start()
    {
        cubes = new GameObject[grid_y*limitEvole];
        ruleSet = new int[8];
        grid = new bool[grid_y];
        buffer = new bool[grid_y];
        InitialCondition(initialCondition);
        ToBinary(ruleBinary);
        //Evolve();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitialCondition(bool isSimple)
    {
        if (isSimple)
        {
            grid[grid.Length / 2] = true;
        }
        else
        {
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = Random.Range(0, 2) == 1;
            }
        }
    }

    public void StartEvolveButtom()
    {
        StartCoroutine(StartEvolve());
    }

    public void Evolve()
    {
    
            bool res = false;
            for (int i = 0; i < grid.Length; i++)
            {
                if (i == 0)
                {
                    res = Rules(grid[grid.Length - 1], grid[i], grid[i + 1]);
                }
                else if (i == grid.Length - 1)
                {
                    res = Rules(grid[i - 1], grid[i], grid[0]);
                }
                else
                {
                    res = Rules(grid[i - 1], grid[i], grid[i + 1]);
                }
                buffer[i] = res;
            }
            grid = buffer;
            PrintGrid();
            ClearBuff();
            
        
    }


    void PrintGrid()
    {
        int posZ = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i] == false)
            {
                GameObject cube = Instantiate(cubeDead, new Vector3(posX, 0, posZ), Quaternion.identity);
                cubes[i] = cube;
                posZ++;
            }
            else
            {
                GameObject cube = Instantiate(cubeAlive, new Vector3(posX, 0, posZ), Quaternion.identity);
                cubes[i] = cube;
                posZ++;
            }
        }
        posX++;
        
    }

    void ClearBuff()
    {
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = false;
        }
    }
    bool Rules(bool a, bool b, bool c)
    {
        if (a && b && c)
        {
            return ruleSet[0] == 1;
        }
        if (a && b && !c)
        {
            return ruleSet[1] == 1;
        }
        if (a && !b && c)
        {
            return ruleSet[2] == 1;
        }
        if (a && !b && !c)
        {
            return ruleSet[3] == 1;
        }
        if (!a && b && c)
        {
            return ruleSet[4] == 1;
        }
        if (!a && b && !c)
        {
            return ruleSet[5] == 1;
        }
        if (!a && !b && c)
        {
            return ruleSet[6] == 1;
        }
        if (!a && !b && !c)
        {
            return ruleSet[7] == 1;
        }
        return false;
    }

    void ToBinary(int number)
    {
        int[] numberarray = new int[8];
        for (int i = 0; i < number; i++)
        {
            ruleSet[i] = number % 2;
            number = number / 2;
        }
    }

    IEnumerator StartEvolve()
    {

        while (true)
        {
            Evolve();
            yield return new WaitForSeconds(2);
        }
    }
}