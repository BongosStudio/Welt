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
        N, S, E, W, Ne, Nw, Se, Sw, None
    }

    public static class Cardinals
    {
        //TODO N is +1
        public static SignedVector3I N = new SignedVector3I(0, 0, -1);
        public static SignedVector3I Ne = new SignedVector3I(+1, 0, -1);
        public static SignedVector3I E = new SignedVector3I(+1, 0, 0);
        public static SignedVector3I Se = new SignedVector3I(+1, 0, +1);
        public static SignedVector3I S = new SignedVector3I(0, 0, +1);
        public static SignedVector3I Sw = new SignedVector3I(-1, 0, +1);
        public static SignedVector3I W = new SignedVector3I(-1, 0, 0);
        public static SignedVector3I Nw = new SignedVector3I(-1, 0, -1);

        public static SignedVector3I VectorFrom(Cardinal c)
        {
            switch (c)
            {
                case Cardinal.N: return N;
                case Cardinal.Ne: return Ne;
                case Cardinal.E: return E;
                case Cardinal.Se: return Se;
                case Cardinal.S: return S;
                case Cardinal.Sw: return Sw;
                case Cardinal.W: return W;
                case Cardinal.Nw: return Nw;
            }
            throw new NotImplementedException("unknown cardinal direction" + c);
        }

        public static SignedVector3I OppositeVectorFrom(Cardinal c)
        {
            switch (c)
            {
                case Cardinal.N: return S;
                case Cardinal.Ne: return Sw;
                case Cardinal.E: return W;
                case Cardinal.Se: return Nw;
                case Cardinal.S: return N;
                case Cardinal.Sw: return Ne;
                case Cardinal.W: return E;
                case Cardinal.Nw: return Se;
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
            if (v == Ne) return Cardinal.Ne;
            if (v == E) return Cardinal.E;
            if (v == Se) return Cardinal.Se;
            if (v == S) return Cardinal.S;
            if (v == Sw) return Cardinal.Sw;
            if (v == W) return Cardinal.W;
            if (v == Nw) return Cardinal.Nw;

            throw new NotImplementedException("vector " + v + " does not map to a cardinal direction");
        }

        public static Cardinal[] Adjacents(Cardinal from) {
            switch (from)
            {
                case Cardinal.N: return new Cardinal[] {Cardinal.E,Cardinal.W};
                case Cardinal.Ne: return new Cardinal[] { Cardinal.S, Cardinal.W, Cardinal.E, Cardinal.N };
                case Cardinal.E: return new Cardinal[] { Cardinal.N, Cardinal.S };
                case Cardinal.Se: return new Cardinal[] { Cardinal.N, Cardinal.W, Cardinal.E, Cardinal.S };
                case Cardinal.S: return new Cardinal[] { Cardinal.E, Cardinal.W };
                case Cardinal.Sw: return new Cardinal[] { Cardinal.N, Cardinal.E, Cardinal.W, Cardinal.S };
                case Cardinal.W: return new Cardinal[] { Cardinal.N, Cardinal.S };
                case Cardinal.Nw: return new Cardinal[] { Cardinal.S, Cardinal.E, Cardinal.N, Cardinal.W };
                default:
                    break;
            }
            throw new NotImplementedException("unknown cardinal direction " + from);
            
        }

    }

}