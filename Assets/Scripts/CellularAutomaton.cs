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
    int ruleNum;
    

    public TMP_InputField ruleInputField;
    public Button evolutionButton;

    // Start is called before the first frame update
    void Start()
    {
        ruleSet = new int[8];
        grid = new bool[80];
        InitialCondition(true);
        InitializeInputField();
    }


    /// <summary>
    /// Initializes the UI components and assigns click event to the evolutionButton.
    /// </summary>
    void InitializeInputField()
    {
        evolutionButton.onClick.AddListener(ButtomOnClickToEvolve);
    }

    /// <summary>
    /// Event handler for the evolutionButton click event.
    /// </summary>
    void ButtomOnClickToEvolve()
    {
        if (int.TryParse(ruleInputField.text, out int inputRuleNum))
        {
            ruleNum = Mathf.Clamp(inputRuleNum, 0, 255);
            ToBinary(ruleNum);
            ResetEverthing();
            EvolveAutomata();
        }
    }

    /// <summary>
    /// Resets the current generation and destroys existing ACE
    /// game objects.
    /// </summary>
    void ResetEverthing()
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

    /// <summary>
    /// Evolves the cellular automaton generation based on the defined rules.
    /// </summary>
    void EvolveAutomata()
    {
        for (int genIndex = 1; genIndex < limitGrid - 1; genIndex++)
        {
            bool[] buffer = new bool[80];
            for (int j = 0; j < grid.Length; j++)
            {
                CreateEntities(j, grid[j], genIndex);
            }
            bool res;
            //ClearBuff();
            for (int i = 0; i < grid.Length; i++)
            {
                if (i == 0)
                {
                    res = DetermineState(grid[grid.Length - 1], grid[i], grid[i + 1]);
                }
                else if (i == grid.Length - 1)
                {
                    res = DetermineState(grid[i - 1], grid[i], grid[0]);
                }
                else
                {
                    res = DetermineState(grid[i - 1], grid[i], grid[i + 1]);
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
    bool DetermineState(bool a, bool b, bool c)
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
    void ToBinary(int number)
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
    /// <param name="index">Index of the cell in the generation.</param>
    /// <param name="state">State of the cell (alive or dead).</param>
    /// <param name="genIndex">Index of the current generation.</param>
    void CreateEntities(int index, bool state, int genIndex)
    {
        Vector3 position = new Vector3(index, -genIndex, 0f);
        GameObject entity = Instantiate(entityPrefab, position, Quaternion.identity);
        Renderer ren = entity.GetComponent<Renderer>();
        ren.material.color = state ? Color.black : Color.white;
    }
}
