namespace Shared.Common.General
{
    public class Empty
    {
        private Empty() { }

        public static Empty Default { get; } = new Empty();
    }
}
