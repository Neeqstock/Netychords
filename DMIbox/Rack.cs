namespace Netychords
{
    internal static class Rack
    {

        private static NetychordsDMIBox netychordsdmibox = new NetychordsDMIBox();
        public static NetychordsDMIBox NetychordsDMIBox { get => netychordsdmibox; set => netychordsdmibox = value; }
    }
}