﻿namespace AllOverIt.Formatters.Objects
{
    /// <summary>Represents the ability to format values serialized by the <see cref="ObjectPropertySerializer"/>.</summary>
    public interface IFormattableObjectPropertyFilter
    {
        /// <summary>Provides the ability to format a value generated by the <see cref="ObjectPropertySerializer"/>.</summary>
        /// <param name="value">The value that can be formatted.</param>
        /// <returns>The replacement, formatted, value to be serialized.</returns>
        string OnFormatValue(string value);
    }
}