namespace TextBasedRPG.Interfaces
{
    public interface IDamageCalculator
    {
        int CalculateDMG(int sourceAtk, int targetDef, double critRate, double critDmg, out bool isCrit);
    }
    public class DamageCalculator : IDamageCalculator
    {
        public int CalculateDMG(int sourceAtk, int targetDef, double critRate, double critDmg, out bool isCrit)
        {
            double baseDmg = sourceAtk * 100.0 / (100.0 + targetDef);

            isCrit = Random.Shared.Next(0, 101) <= critRate;
            double newDmg = isCrit ? (baseDmg * critDmg / 100.0) : baseDmg;

            double deviation = newDmg * Random.Shared.Next(90, 111) / 100.0;

            int finalDamage = (int)Math.Round(deviation);   
            return Math.Max(1, finalDamage);
        }
    }
}
