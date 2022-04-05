using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public Orbiter planetPrefab;
    public Orbiter wishPrefab;
    public OrbitBody sun;

    public int numberOfShells;
    public float baseHeight;
    public float minShellSize;
    public float maxShellSize;

    [Range(0.0f, 1.0f)] public float spawnChance;
    public float firstLayerSpawnChance;

    public float minSpeed;
    public float maxSpeed;

    public float minPlanetDiameter;
    public float minPlanetSpacing;
    public float maxPlanetSpacing;

    public float wishProbability;
    public float wishSpeed;
    public float wishHeight;

    private void Awake() {
        float shellInnerRadius = sun.radius + baseHeight;
        for (int shellNumber = 0; shellNumber < numberOfShells; shellNumber++) {
            float shellThickness = Random.Range(minShellSize, maxShellSize);

            float circumference = 2 * Mathf.PI * shellInnerRadius;
            float shellSpeed = Random.Range(minSpeed, maxSpeed);
            float direction = Random.Range(0.0f, 1.0f) > 0.5 ? 1.0f : -1.0f;

            for(float lastOccupied = 0.0f; lastOccupied <= circumference;) {
                float maxDiameter = Mathf.Min(circumference - lastOccupied - minPlanetSpacing, shellThickness);
                if(maxDiameter < minPlanetDiameter) {
                    break;
                }
                float planetDiameter = Random.Range(minPlanetDiameter, shellThickness);
                float spawnPosition = lastOccupied + planetDiameter/2.0f + Random.Range(minPlanetSpacing, maxPlanetSpacing);
                lastOccupied = spawnPosition + planetDiameter / 2.0f;

                float heightInShell = Random.Range(planetDiameter/2.0f, shellThickness-(planetDiameter/2.0f));

                float chance = shellNumber == 0 ? firstLayerSpawnChance : spawnChance;
                if (Random.Range(0.0f, 1.0f) > chance) {
                    continue;
                }
                Orbiter newPlanet = Instantiate(planetPrefab.gameObject).GetComponent<Orbiter>();
                newPlanet.transform.localScale = Vector3.one * planetDiameter;
                newPlanet.objectToOrbit = sun;
                newPlanet.angle = (spawnPosition/shellInnerRadius)*Mathf.Rad2Deg;
                newPlanet.orbitSpeed = shellSpeed * direction;
                newPlanet.orbitHeightAboveSurface = shellInnerRadius + heightInShell - sun.radius;
                newPlanet.GetComponentInChildren<Rotator>().spinRate = Random.onUnitSphere * Random.Range(-10.0f, 10.0f);

                if(Random.Range(0.0f, 1.0f) < wishProbability) {
                    Orbiter newWish = Instantiate(wishPrefab.gameObject).GetComponent<Orbiter>();
                    newWish.objectToOrbit = newPlanet.GetComponent<OrbitBody>();
                    float wDir = Random.Range(0.0f, 1.0f) > 0.5 ? 1.0f : -1.0f;
                    newWish.orbitSpeed = wishSpeed * wDir;
                    newWish.orbitHeightAboveSurface = wishHeight;
                    newWish.GetComponentInChildren<Rotator>().spinRate = Random.onUnitSphere * Random.Range(-10.0f, 10.0f);
                }
            }

            shellInnerRadius += shellThickness + minPlanetSpacing;
        }
    }

    private void OnValidate() {
        minPlanetDiameter = Mathf.Clamp(minPlanetDiameter, 0.0f, minShellSize);
    }
}
