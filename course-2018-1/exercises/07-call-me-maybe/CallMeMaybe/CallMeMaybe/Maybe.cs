﻿using System;

namespace CallMeMaybe
{
    public struct Maybe<T>
    {
        public static readonly Maybe<T> Nothing = new Maybe<T>();

        public bool HasValue { get; }

        private readonly T _value;
        public T Value => HasValue ? _value : throw new InvalidOperationException($"{typeof(Maybe<T>)} doesn't have value.");

        private Maybe(T value)
        {
            _value = value;
            HasValue = true;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return HasValue ? _value.ToString() : "null";
        }

        public static implicit operator Maybe<T>(T value)
        {
            //throw new NotImplementedException();
            return value != null ? new Maybe<T>(value) : Nothing;
        }

        #region LINQ syntax providers

        public Maybe<TResult> Select<TResult>(Func<T, TResult> map)
        {
            // обеспечит поддержку одинарного from
            //throw new NotImplementedException();
            return HasValue ? map(_value) : Maybe<TResult>.Nothing;
        }
        public Maybe<TResult> Select<TResult>(Func<T, Maybe<TResult>> maybeMap)
        {
            // обеспечит поддержку одинарного from
            //throw new NotImplementedException();
            return HasValue ? maybeMap(_value) : Maybe<TResult>.Nothing;
        }
        public Maybe<TResult> SelectMany<T2, TResult>(Func<T, Maybe<T2>> otherSelector, Func<T, T2, TResult> resultSelector)
        {
            // обеспечит поддержку цепочки from
            //throw new NotImplementedException();
            if (HasValue)
            {
                return otherSelector(_value).HasValue ? resultSelector(_value, otherSelector(_value)._value) : Maybe<TResult>.Nothing;
            }
            return Maybe<TResult>.Nothing;
        }
        public Maybe<TResult> SelectMany<T2, TResult>(Func<T, Maybe<T2>> otherSelector, Func<T, T2, Maybe<TResult>> maybeResultSelector)
        {
            // обеспечит поддержку цепочки from
            //throw new NotImplementedException();
            throw new NotImplementedException();
            if (HasValue)
            {
                return otherSelector(_value).HasValue ? maybeResultSelector(_value, otherSelector(_value)._value) : Maybe<TResult>.Nothing;
            }
            return Maybe<TResult>.Nothing;
        }
        public Maybe<T> Where(Predicate<T> predicate)
        {
            // обеспечит поддержку кляузы where
            //throw new NotImplementedException();
            return HasValue && predicate(_value) ? _value : Nothing;
        }

        #endregion

        #region Optional useful methods

        public static explicit operator T(Maybe<T> maybe)
        {
            return maybe.Value;
            //throw new NotImplementedException();
        }

        //public T GetValueOrDefault() => throw new NotImplementedException();
        public T GetValueOrDefault() => HasValue ? Value : default(T);
        //public T GetValueOrDefault(T defaultValue) => throw new NotImplementedException();       
        public T GetValueOrDefault(T defaultValue) => HasValue ? Value : defaultValue;

        public TResult SelectOrElse<TResult>(Func<T, TResult> map, Func<TResult> elseMap)
        {
            //throw new NotImplementedException();
            return HasValue ? map(_value) : elseMap();
        }

        public void Do(Action<T> doAction)
        {
            //throw new NotImplementedException();
            if (HasValue)
            {
                doAction(_value);
            }
        }
        public void DoOrElse(Action<T> doAction, Action elseAction)
        {
            //throw new NotImplementedException();
            if (HasValue)
            {
                doAction(_value);
            }
            else
            {
                elseAction();
            }
        }

        public T OrElse(Func<T> elseMap)
        {
            //throw new NotImplementedException();
            return HasValue ? _value : elseMap();
        }
        public void OrElse(Action elseAction)
        {
            //throw new NotImplementedException();
            if (!HasValue)
            {
                elseAction();
            }
        }

        #endregion
    }
}