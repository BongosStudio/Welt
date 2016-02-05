#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;

namespace Welt.Models
{
    public struct Maybe<TValue, TException> where TException : Exception
    {
        public TValue Value { get; private set; }
        public TException Error { get; private set; }

        public Maybe(TValue value, TException exception)
        {
            Value = value;
            Error = exception;
        } 
        
        public bool HasError => Error != null && Value == null;

        public void TrySetValue(Func<TValue> value)
        {
            try
            {

                Value = value.Invoke();
            }
            catch (Exception e)
            {
                Error = (TException) e;
            }
        }

        public static Maybe<TValue, TException> Check(Func<TValue> value)
        {
            var maybe = new Maybe<TValue, TException>();
            maybe.TrySetValue(value);
            return maybe;
        }

        public static implicit operator TValue(Maybe<TValue, TException> value)
        {
            return value.Value;
        }
    }
}