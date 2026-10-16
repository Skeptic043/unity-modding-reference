namespace Probe
{
    // Declared before Target, so its members precede Target's in the metadata tables.
    public class Earlier
    {
#if EXTRA
        // Unrelated update: one field, one method and one string literal.
        public int addedField;
        public void AddedMethod() => System.Console.WriteLine("added");
#endif
    }

    public class Target
    {
        private float useDistance = 1.5f;

        // Identical source in both builds.
        public bool Check(float distance)
        {
            System.Console.WriteLine("check");
            return distance <= useDistance;
        }
    }
}
