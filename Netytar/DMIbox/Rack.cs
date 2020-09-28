namespace Netytar
{
    internal static class Rack
    {
        private static NetytarDMIBox netytardmibox = new NetytarDMIBox();

        private static NetychordsDMIBox netychordsdmibox = new NetychordsDMIBox();


        public static NetytarDMIBox NetytarDMIBox { get => netytardmibox; set => netytardmibox = value; }
        public static NetychordsDMIBox NetychordsDMIBox { get => netychordsdmibox; set => netychordsdmibox = value; }
    }
}