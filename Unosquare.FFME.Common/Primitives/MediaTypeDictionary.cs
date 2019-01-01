﻿namespace Unosquare.FFME.Primitives
{
    using Shared;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a very simple dictionary for MediaType keys
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public sealed class MediaTypeDictionary<TValue>
        : Dictionary<MediaType, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeDictionary{TValue}"/> class.
        /// </summary>
        public MediaTypeDictionary()
            : base(Enum.GetValues(typeof(MediaType)).Length)
        {
            // placeholder
        }

        /// <summary>
        /// Gets or sets the item with the specified key.
        /// return the default value of the value type when the key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The item</returns>
        public new TValue this[MediaType key]
        {
            get { return ContainsKey(key) == false ? default(TValue) : base[key]; }
            internal set { base[key] = value; }
        }
    }
}
