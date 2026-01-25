namespace EtnasSoft.Foundation.Validation {

    public enum InvalidNumberPolicy {
        Ignore,
        Throw,
        ReturnDefault
    }

    public enum RangePolicy {
        Ignore,
        Clamp,
        Throw
    }

    public readonly struct ValidationPolicy {
        public readonly InvalidNumberPolicy InvalidNumber;
        public readonly RangePolicy ColorUnitRange;

        public ValidationPolicy(
            InvalidNumberPolicy invalidNumber,
            RangePolicy colorUnitRange
        ) {
            InvalidNumber = invalidNumber;
            ColorUnitRange = colorUnitRange;
        }

        public static ValidationPolicy None =>
            new ValidationPolicy(InvalidNumberPolicy.Ignore, RangePolicy.Ignore);

        public static ValidationPolicy Strict =>
            new ValidationPolicy(InvalidNumberPolicy.Throw, RangePolicy.Throw);

        public static ValidationPolicy Safe =>
            new ValidationPolicy(InvalidNumberPolicy.ReturnDefault, RangePolicy.Clamp);
    }
}
