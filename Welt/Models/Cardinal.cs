#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Welt.Types;

#endregion

namespace Welt.Models
{

    public enum Cardinal
    {
        N, S, E, W, NE, NW, SE, SW
    }

    public static class Cardinals
    {
        //TODO N is +1
        public static SignedVector3I N = new SignedVector3I(0, 0, -1);
        public static SignedVector3I NE = new SignedVector3I(+1, 0, -1);
        public static SignedVector3I E = new SignedVector3I(+1, 0, 0);
        public static SignedVector3I SE = new SignedVector3I(+1, 0, +1);
        public static SignedVector3I S = new SignedVector3I(0, 0, +1);
        public static SignedVector3I SW = new SignedVector3I(-1, 0, +1);
        public static SignedVector3I W = new SignedVector3I(-1, 0, 0);
        public static SignedVector3I NW = new SignedVector3I(-1, 0, -1);

        public static SignedVector3I VectorFrom(Cardinal c)
        {
            switch (c)
            {
                case Cardinal.N: return N;
                case Cardinal.NE: return NE;
                case Cardinal.E: return E;
                case Cardinal.SE: return SE;
                case Cardinal.S: return S;
                case Cardinal.SW: return SW;
                case Cardinal.W: return W;
                case Cardinal.NW: return NW;
            }
            throw new NotImplementedException("unknown cardinal direction" + c);
        }

        public static SignedVector3I OppositeVectorFrom(Cardinal c)
        {
            switch (c)
            {
                case Cardinal.N: return S;
                case Cardinal.NE: return SW;
                case Cardinal.E: return W;
                case Cardinal.SE: return NW;
                case Cardinal.S: return N;
                case Cardinal.SW: return NE;
                case Cardinal.W: return E;
                case Cardinal.NW: return SE;
                default:
                    break;
            }
            throw new NotImplementedException("unknown cardinal direction " + c);
        }

        public static Cardinal CardinalFrom(int x, int z)
        {
            var v = new SignedVector3I(x, 0, z);
            return CardinalFrom(v);
        }

        public static Cardinal CardinalFrom(SignedVector3I v)
        {

            if (v == N) return Cardinal.N;
            if (v == NE) return Cardinal.NE;
            if (v == E) return Cardinal.E;
            if (v == SE) return Cardinal.SE;
            if (v == S) return Cardinal.S;
            if (v == SW) return Cardinal.SW;
            if (v == W) return Cardinal.W;
            if (v == NW) return Cardinal.NW;

            throw new NotImplementedException("vector " + v + " does not map to a cardinal direction");
        }

        public static Cardinal[] Adjacents(Cardinal from) {
            switch (from)
            {
                case Cardinal.N: return new Cardinal[] {Cardinal.E,Cardinal.W};
                case Cardinal.NE: return new Cardinal[] { Cardinal.S, Cardinal.W, Cardinal.E, Cardinal.N };
                case Cardinal.E: return new Cardinal[] { Cardinal.N, Cardinal.S };
                case Cardinal.SE: return new Cardinal[] { Cardinal.N, Cardinal.W, Cardinal.E, Cardinal.S };
                case Cardinal.S: return new Cardinal[] { Cardinal.E, Cardinal.W };
                case Cardinal.SW: return new Cardinal[] { Cardinal.N, Cardinal.E, Cardinal.W, Cardinal.S };
                case Cardinal.W: return new Cardinal[] { Cardinal.N, Cardinal.S };
                case Cardinal.NW: return new Cardinal[] { Cardinal.S, Cardinal.E, Cardinal.N, Cardinal.W };
                default:
                    break;
            }
            throw new NotImplementedException("unknown cardinal direction " + from);
            
        }

    }

}