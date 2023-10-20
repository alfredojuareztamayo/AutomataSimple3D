using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles 
{
    // Start is called before the first frame update
    public float m_x { get; private set; }
    public float m_y { get; private set; }
    public bool m_isDead { get; private set; }

    public Tiles(float x, float y) {
        m_x = x;
        m_y = y;
    }

    public void SetIsDead(bool isDead)
    {
        m_isDead = isDead;
    }
}
