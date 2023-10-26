using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellularAutomaton : MonoBehaviour
{
    [SerializeField] GameObject entityPrefab;
    int[] ruleSet;
    bool[] grid;
    int limitGrid = 30;
    int ruleNumerator;
    

    public TMP_InputField ruleInputField;
    public Button evolutionButton;

    // Start is called before the first frame update
    void Start()
    {
        ruleSet = new int[8];
        grid = new bool[80];
        initialCondition(true);
        initializeInputField();
    }


    /// <summary>
    /// Initializes the UI components and assigns click event to the evolutionButton.
    /// </summary>
    void initializeInputField()
    {
        evolutionButton.onClick.AddListener(buttomOnClickToEvolve);
    }

    /// <summary>
    /// Event handler for the evolutionButton click event.
    /// </summary>
    void buttomOnClickToEvolve()
    {
        if (int.TryParse(ruleInputField.text, out int inputRuleNum))
        {
            ruleNumerator = Mathf.Clamp(inputRuleNum, 0, 255);
            toBinary(ruleNumerator);
            resetEverthing();
            evolveAutomata();
        }
    }

    /// <summary>
    /// Resets the current generation and destroys existing ACE
    /// game objects.
    /// </summary>
    void resetEverthing()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = false;
        }
        grid[grid.Length / 2] = true;
        GameObject[] ACE = GameObject.FindGameObjectsWithTag("Entity");
        for (int i = 0; i < ACE.Length; i++)
        {
            Destroy(ACE[i]);
        }
    }

    /// <summary>
    /// Sets the initial condition for the generation.
    /// </summary>
    /// <param name="isSimple">Flag indicating whether to use a simple initial condition or randomize.</param>
    void initialCondition(bool isSimple)
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

    /// <summary>
    /// Evolves the cellular automaton generation based on the defined rules.
    /// </summary>
    void evolveAutomata()
    {
        for (int genIndex = 1; genIndex < limitGrid - 1; genIndex++)
        {
            bool[] buffer = new bool[80];
            for (int j = 0; j < grid.Length; j++)
            {
                createEntities(j, grid[j], genIndex);
            }
            bool res;
            //ClearBuff();
            for (int i = 0; i < grid.Length; i++)
            {
                if (i == 0)
                {
                    res = determineState(grid[grid.Length - 1], grid[i], grid[i + 1]);
                }
                else if (i == grid.Length - 1)
                {
                    res = determineState(grid[i - 1], grid[i], grid[0]);
                }
                else
                {
                    res = determineState(grid[i - 1], grid[i], grid[i + 1]);
                }
                buffer[i] = res;
            }
            
            grid = buffer; // Actualiza grid con los nuevos valores
        }
    }

    /// <summary>
    /// Applies the defined rules to determine the state of a cell in the next generation.
    /// </summary>
    /// <param name="a">Left neighbor state.</param>
    /// <param name="b">Current cell state.</param>
    /// <param name="c">Right neighbor state.</param>
    /// <returns>Returns true if the cell should be alive in the next generation, false otherwise.</returns>
    bool determineState(bool a, bool b, bool c)
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

    /// <summary>
    /// Converts a decimal number to binary and stores it in the ruleSet array.
    /// </summary>
    /// <param name="number">Decimal number to convert.</param>
    void toBinary(int number)
    {
        
        for (int i = 7; i >= 0; i--)
        {
            ruleSet[i] = number % 2;
            number = number / 2;
        }
        Debug.Log("Reglas: " + string.Join(",", ruleSet));
    }


    /// <summary>
    /// Instantiates cell game objects based on their state.
    /// </summary>
    /// <param name="id">Index of the cell in the generation.</param>
    /// <param name="fase">State of the cell (alive or dead).</param>
    /// <param name="index">Index of the current generation.</param>
    void createEntities(int id, bool fase, int index)
    {
        GameObject entity = Instantiate(entityPrefab, new Vector3(id, -index, 0f), Quaternion.identity);
        entity.GetComponent<Renderer>().material.color = fase ? Color.black : Color.green;
    }

 
}
