using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DNA Digit - or Dit
/// </summary>
public enum Dit {
  G, // 00 - 0 yellow
  C, // 01 - 1 blue
  A, // 10 - 2 green
  T, // 11 - 3 red
}

/// <summary>
/// Hold stats of an organism and contains accessors for required stats
/// </summary>
public class DNA {
  public Dit bodyShape;
  public Dit colorR; // 0 64 128 192
  public Dit colorG;
  public Dit colorB;
  public Dit colorA; // // 30  -  190
  public Dit gunCount; // // 0 1 2 3
  public Dit gunAngle; //
  public Dit gunPower; // - bullet speed + damage + knockback (both you and enemy)
  public Dit gunCooldown;
  public Dit gunAccuracy;
  public Dit ammoRegen; // - 0 1 2 3
  public Dit gunBullets; // - 1 2 3 4
  public Dit bulletMass;
  public Dit cilia; // - speed
  public Dit jetCount; // 0 1 2 3
  public Dit jetPower; // - boost
  public Dit membraneThickness; // (+health -speed)
  public Dit density; // (+health -speed)
  public Dit frontSpikeCount; // - 0 1 2 4 - spikes do 1 hit per spike
  public Dit backSpikeCount; // - 0 1 2 4 - spikes do 1 hit per spike
  public Dit frontSpikeSize;
  public Dit backSpikeSize;
  public Dit frontSpikeAngle;
  public Dit backSpikeAngle;
  public Dit healthRegen; // - 0 1 2 3
  public Dit splitErrorRate; // - 1 2 3 4
  public Dit size; // - 1X 1.1X 1.3X 1.5X (health)
  public Dit foodVacuoles; // Number of max food vacuoles 3 6 9 12

  private readonly int[] colorMap = {0, 64, 128, 192};
  private readonly int[] alphaMap = {192, 112, 96, 64};
  private readonly float gunWeight = 4.0f;
  private readonly float jetWeight = 2.0f;
  private readonly float spikeWeight = 1.0f;
  private readonly float ciliaMult = 4.0f;
  private readonly float thicknessMult = 1.0f;
  private readonly float densityMult = 2.0f;
  private readonly float sizeMult = 2.0f;

  public DNA() {
    bodyShape = Dit.G;

    colorR = Dit.G; // 0 64 128 192
    colorG = Dit.G;
    colorB = Dit.G;
    colorA = Dit.G; // // 30  -  190

    gunCount = Dit.G; // // 0 1 2 3
    gunAngle = Dit.G; //
    gunPower = Dit.G; // - bullet speed + damage + knockback (both you and enemy)
    gunCooldown = Dit.G;
    gunAccuracy = Dit.G;
    ammoRegen = Dit.G; // - 0 1 2 3 - ammo and health generate when you have food globules
    gunBullets = Dit.G; // - number of bullets fired per shot 1 2 3 4
    bulletMass = Dit.G;

    cilia = Dit.G; // - speed
    jetCount = Dit.G;
    jetPower = Dit.G; // - boost

    membraneThickness = Dit.G; // (+health -speed)
    density = Dit.G; // (+health -speed)

    frontSpikeCount = Dit.G; // - 0 2 4 6 - spikes do 1 hit per spike
    frontSpikeSize = Dit.G;
    frontSpikeAngle = Dit.G;

    backSpikeCount = Dit.G; // - 0 1 2 4 - spikes do 1 hit per spike
    backSpikeSize = Dit.G;
    backSpikeAngle = Dit.G;

    healthRegen = Dit.G; // - 0 1 2 3
    splitErrorRate = Dit.G; // - 1 2 3 4
    size = Dit.G; // - 1X 1.1X 1.3X 1.5X (health + food storage)
    foodVacuoles = Dit.G; // Number of max food vacuoles 3 6 9 12
  }

  public void Shuffle() {
    bodyShape = GetRandom();
    colorR = GetRandom();
    colorG = GetRandom();
    colorB = GetRandom();
    colorA = GetRandom();
    gunCount = GetRandom();
    gunAngle = GetRandom();
    gunPower = GetRandom();
    gunCooldown = GetRandom();
    gunAccuracy = GetRandom();
    ammoRegen = GetRandom();
    gunBullets = GetRandom();
    bulletMass = GetRandom();
    cilia = GetRandom();
    jetCount = GetRandom();
    jetPower = GetRandom();
    membraneThickness = GetRandom();
    density = GetRandom();
    frontSpikeCount = GetRandom();
    frontSpikeSize = GetRandom();
    frontSpikeAngle = GetRandom();
    backSpikeCount = GetRandom();
    backSpikeSize = GetRandom();
    backSpikeAngle = GetRandom();
    healthRegen = GetRandom();
    splitErrorRate = GetRandom();
    size = GetRandom();
    foodVacuoles = GetRandom();
  }

  public float GetAcceleration() {
    return 1000.0f 
      + (1.0f + (float)cilia * ciliaMult)
      - (float)gunCount * gunWeight
      - (float)jetCount * jetWeight
      - (float)frontSpikeCount * spikeWeight * (float)frontSpikeSize
      - (float)backSpikeCount * spikeWeight * (float)backSpikeSize
      - (float)membraneThickness * thicknessMult
      - (float)density * densityMult
      - (float)size * sizeMult;
  }

  public float GetJetAcceleration() {
    return GetAcceleration() * 5.0f;
  }

  public Dit GetRandom() {
    float r = Random.Range(0.0f, 1.0f);
    if(r > 0 && r < 0.5) return Dit.G; // 50%
    if(r > 0.5 && r < 0.75) return Dit.C; // 25%
    if(r > 0.75 && r < 0.9) return Dit.A; // 15%
    return Dit.T; // 10%
  }

  public Dit Split(Dit own, Dit incoming) {
    float r = Random.Range(0.0f, 1.0f);
    if(r > 0 && r < 0.48) return own; // 48%
    if(r > 0.48 && r < 0.96) return incoming; // 48%
    return GetRandom(); // 4%
  }

  public Color GetColor() {
    return new Color(
      colorMap[(int)colorR] / 255.0f,
      colorMap[(int)colorG] / 255.0f,
      colorMap[(int)colorB] / 255.0f,
      alphaMap[(int)colorA] / 255.0f
    );
  }

  public string ColoredString(Dit d) {
    switch(d) {
      case Dit.T: return "<color=red>T</color>";
      case Dit.G: return "<color=yellow>G</color>";
      case Dit.C: return "<color=blue>C</color>";
      default: return "<color=green>A</color>";
    }
  }

  public void Absorb(DNA incoming) {
    bodyShape = Split(bodyShape, incoming.bodyShape);
    colorR = Split(colorR, incoming.colorR);
    colorG = Split(colorG, incoming.colorG);
    colorB = Split(colorB, incoming.colorB);
    colorA = Split(colorA, incoming.colorA);
    gunCount = Split(gunCount, incoming.gunCount);
    gunAngle = Split(gunAngle, incoming.gunAngle);
    gunPower = Split(gunPower, incoming.gunPower);
    gunCooldown = Split(gunCooldown, incoming.gunCooldown);
    gunAccuracy = Split(gunAccuracy, incoming.gunAccuracy);
    ammoRegen = Split(ammoRegen, incoming.ammoRegen);
    gunBullets = Split(gunBullets, incoming.gunBullets);
    bulletMass = Split(bulletMass, incoming.bulletMass);
    cilia = Split(cilia, incoming.cilia);
    jetCount = Split(jetCount, incoming.jetCount);
    jetPower = Split(jetPower, incoming.jetPower);
    membraneThickness = Split(membraneThickness, incoming.membraneThickness);
    density = Split(density, incoming.density);
    frontSpikeCount = Split(frontSpikeCount, incoming.frontSpikeCount);
    frontSpikeSize = Split(frontSpikeSize, incoming.frontSpikeSize);
    frontSpikeAngle = Split(frontSpikeAngle, incoming.frontSpikeAngle);
    backSpikeCount = Split(backSpikeCount, incoming.backSpikeCount);
    backSpikeSize = Split(backSpikeSize, incoming.backSpikeSize);
    backSpikeAngle = Split(backSpikeAngle, incoming.backSpikeAngle);
    healthRegen = Split(healthRegen, incoming.healthRegen);
    splitErrorRate = Split(splitErrorRate, incoming.splitErrorRate);
    size = Split(size, incoming.size);
    foodVacuoles = Split(foodVacuoles, incoming.foodVacuoles);
  }

  public string ToColoredString() {
    return ColoredString(bodyShape) +
      ColoredString(colorR) +
      ColoredString(colorG) +
      ColoredString(colorB) +
      ColoredString(colorA) +
      ColoredString(gunCount) +
      ColoredString(gunAngle) +
      ColoredString(gunPower) +
      ColoredString(gunCooldown) +
      ColoredString(gunAccuracy) +
      ColoredString(ammoRegen) +
      ColoredString(gunBullets) +
      ColoredString(bulletMass) +
      ColoredString(cilia) +
      ColoredString(jetCount) +
      ColoredString(jetPower) +
      ColoredString(membraneThickness) +
      ColoredString(density) +
      ColoredString(frontSpikeCount) +
      ColoredString(frontSpikeSize) +
      ColoredString(frontSpikeAngle) +
      ColoredString(backSpikeCount) +
      ColoredString(backSpikeSize) +
      ColoredString(backSpikeAngle) +
      ColoredString(healthRegen) +
      ColoredString(splitErrorRate) +
      ColoredString(size) +
      ColoredString(foodVacuoles);
  }
}
