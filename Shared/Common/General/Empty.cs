namespace Shared.Common.General
{
    public class Empty
    {
        public Empty() { }

        public static Empty Default { get; } = new Empty();
    }
}
