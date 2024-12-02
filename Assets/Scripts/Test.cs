using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Test : MonoBehaviour
{
	[SerializeField] GameObject cardprefab;

	[SerializeField] GameObject panel;
	private void Start()
	{
        for (int i = 0; i < 5; i++)
        {
			SpawnRandomCard();
        }
    }

	void SpawnRandomCard()
	{
		Card obj = Instantiate(cardprefab).GetComponent<Card>();

		obj.transform.parent = panel.transform;

		obj.SetVehicle(new Ucak());
		obj.ShowCard();
	}
}

