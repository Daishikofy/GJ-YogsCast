﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pots : MonoBehaviour, Interactable
{
    [SerializeField]
    Attributes.Biome biome;
    [SerializeField]
    float waterGrowthBoost = 1;
    [SerializeField]
    float wateredTime = 10;

    float timeBeforeHarvest;
    float timeSincePlanted;

    bool isOccupied;
    bool isWatered;
    bool isReadyForHarvest;
    Plant plant;
    //DEBUG : Implémenter l'objet plante
    public void OnInteraction(Player player)
    {
        var selectedObject = player.selectedObject;
        if (selectedObject != null)
        {
            if (selectedObject.isType(typeof(Seed).Name))//Planting
            {
                player.selectedObject.deSelected();
                player.setSelectedObject(null);
                Seed seed = (Seed)selectedObject;
                plant = Instantiate(seed.getPlantPrefab(), this.transform.position + Vector3.up * 0.5f, Quaternion.identity, this.transform).GetComponent<Plant>();
                Debug.Log("Pot: You just planted a " + selectedObject.getName());
            }
            if (selectedObject.isType(typeof(WaterCan).Name) && !isWatered)
            {
                Debug.Log("Pot: I like water");//Wtering
                WaterCan waterCan = (WaterCan)selectedObject;
                waterCan.use(this);       
            }
        }
        else if (selectedObject == null && isReadyForHarvest)
        {
            Debug.Log("Pot: It is harvest time!");//Harvesting
            //If the plant is ready, harvest
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isOccupied || isReadyForHarvest) return;
        if (timeSincePlanted >= timeBeforeHarvest)
        {
            isReadyForHarvest = true;
            return;
        }
        timeSincePlanted += Time.deltaTime * waterGrowthBoost;
    }

    private void resetPot()
    {
        setReadyForHarvest(false);
        plant = null;
        setWater(false);
    }

    private void setReadyForHarvest(bool value)
    {
        if (value)
        {
            isReadyForHarvest = true;
            plant.isReadyHarvest();
        }
        else
        {
            timeSincePlanted = 0f;
            isReadyForHarvest = false;
        }
    }

    private void Harvest()
    {
        //Pass the animal and the biome and get a prefab back
        //Instanciate the prefab and desable it
        //Put the animal in the hands of the player
    }

    public void setWater(bool value)
    {
        isWatered = value;
        if (value)          
            StartCoroutine(waterCoolDown());     
        else          
            StopCoroutine(waterCoolDown());
        
    }
    private IEnumerator waterCoolDown()
    {
        isWatered = true;
        Debug.Log("Pot: I have water!");
        float cooldown = wateredTime;
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Pot: I need water!");
        isWatered = false;    
    }
}