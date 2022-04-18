using NetTopologySuite.Geometries;
using SyntheticLife.Core.Energy;
using SyntheticLife.Core.LifeForm;
using SyntheticLife.Core.Map;

Console.WriteLine("Simulation Start!");

var sartingPopulation = 100;
var movementSpeed = 10;
var harvestRate = 10 * movementSpeed;
var minimumOffspringCost = 20 * movementSpeed;
var maxFoodSize = harvestRate * 1.5;
var startingEnergy = sartingPopulation * movementSpeed;
var energyPerYear = sartingPopulation * movementSpeed;
var searchAreaSize = 100;
var simulationLength = 1000;

var mapBounds = new Envelope(0, searchAreaSize * 10, 0, searchAreaSize * 10);

Random random = new (0);
var map = new EntityMap(mapBounds);
var behaviourEngine = new MoveToClosestEnergySourceBehaviourEngine(random, searchAreaSize);
var consumptionEngine = new ConsumeLargestConsumptionEngine();
var offspringCreationEngine = new OffspringCreationEngine();

var startingPopulation = new PopulationGenerator(random, movementSpeed, harvestRate, minimumOffspringCost).CreateCreatures(sartingPopulation, map.Bounds);
foreach (var creature in startingPopulation)
{
    map.AddEntity(creature);
}

var energyDistributor = new RandomEnergyDistributor(random, maxFoodSize);
var startingEnergySources = energyDistributor.CreateEnergySources(map, startingEnergy);

foreach (var energySource in startingEnergySources)
{
    map.AddEntity(energySource);
}

var day = 0;
while (day < simulationLength)
{
    Console.WriteLine(string.Format("Year {0}", day));
    var startOfYearEntites = map.MapEntities;
    var startOfYearCreatures = startOfYearEntites
        .Where(entity => typeof(ICreature)
        .IsInstanceOfType(entity))
        .Cast<ICreature>();
    var startOfYearEnergySources = startOfYearEntites
        .Where(entity => typeof(IEnergySource)
        .IsInstanceOfType(entity))
        .Cast<IEnergySource>();

    Console.WriteLine(
        string.Format(
            "{0} Creature totaling {1:0.00} energy.",
            startOfYearCreatures.Count(),
            startOfYearCreatures.Sum(creature => creature.Energy)));
    Console.WriteLine(
        string.Format(
            "{0} EnergySources totaling {1:0.00} energy.",
            startOfYearEnergySources.Count(),
            startOfYearEnergySources.Sum(creature => creature.Energy)));

    foreach (var creature in startOfYearCreatures)
    {
        behaviourEngine.SelectMovement(creature, map).ExecuteOrder(map);
        foreach (var order in consumptionEngine.ProssesCurrectPosition(creature, map))
        {
            order.ExecuteOrder(map);
        }

        foreach (var order in offspringCreationEngine.ProssesCurrectState(creature, map))
        {
            order.ExecuteOrder(map);
        }
    }

    foreach (var energySource in energyDistributor.CreateEnergySources(map, energyPerYear))
    {
        map.AddEntity(energySource);
    }

    day++;
}

Console.WriteLine("Simulation End!");