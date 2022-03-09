using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    int spikedamage = CONSTANTS.SPIKE_DAMAGE;

    private float spikeTimesec = CONSTANTS.SPIKE_DAMAGE_TIME;

    private float m_spikeTime;

    public float SpikerTimer 
    {
        get 
        {
            return m_spikeTime;
        }

        set 
        {
            if(value <= 0f) 
            {
                m_spikeTime = 0f;
                return;
            }

            m_spikeTime = value;
        }
    }

    private void Update()
    {
        SpikerTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (SpikerTimer <= 0f)
            {
                SpikerTimer = spikeTimesec;
                collision.gameObject.GetComponent<PlayerAttribute>().Hit(spikedamage);

            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
                collision.gameObject.GetComponent<PlayerAttribute>().Hit(spikedamage);
        }
    }

}
