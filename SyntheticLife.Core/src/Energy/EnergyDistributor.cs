using Ardalis.GuardClauses;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public class RandomEnergyDistributor : IEnergyDistributor
    {
        private Random RandomNumberGenerator { get; }
        private double MaxSize { get; }

        public RandomEnergyDistributor(Random random, double maxSize)
        {
            Guard.Against.Negative(maxSize);

            RandomNumberGenerator = random;
            MaxSize = maxSize;
        }

        public IEnumerable<IEnergySource> CreateEnergySources(IEntityMap map, double energyAmount)
        {
            Guard.Against.Negative(energyAmount);
            var energyList = new List<IEnergySource>();

            while (energyAmount > 0)
            {
                var location = map.Bounds.RandomEnvelopeInside(RandomNumberGenerator, 1);
                if (energyAmount >= MaxSize)
                {
                    energyList.Add(new EnergySource(location, MaxSize));
                    energyAmount -= MaxSize;
                }
                else
                {
                    energyList.Add(new EnergySource(location, energyAmount));
                    energyAmount = 0;
                }
            }

            return energyList;
        }
    }
}
