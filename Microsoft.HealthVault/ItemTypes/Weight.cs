// Copyright (c) Microsoft Corporation.  All rights reserved.
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using Microsoft.HealthVault.Clients;
using Microsoft.HealthVault.Exceptions;
using Microsoft.HealthVault.Helpers;
using Microsoft.HealthVault.Thing;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// Represents a thing type that encapsulates a person's weight.
    /// </summary>
    ///
    public class Weight : ThingBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Weight"/> class with default
        /// values.
        /// </summary>
        ///
        /// <remarks>
        /// The item is not added to the health record until the <see cref="IThingClient.CreateNewThingsAsync{ThingBase}(Guid, ICollection{ThingBase})"/> method is called.
        /// </remarks>
        ///
        public Weight()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Weight"/> class with the
        /// specified date/time and weight.
        /// </summary>
        ///
        /// <param name="when">
        /// The date/time when the weight measurement occurred.
        /// </param>
        ///
        /// <param name="weight">
        /// The person's weight.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="when"/> or <paramref name="weight"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public Weight(HealthServiceDateTime when, WeightValue weight)
            : base(TypeId)
        {
            When = when;
            Value = weight;
        }

        /// <summary>
        /// Retrieves the unique identifier for the item type.
        /// </summary>
        ///
        /// <value>
        /// A GUID.
        /// </value>
        ///
        public static new readonly Guid TypeId =
            new Guid("3d34d87e-7fc1-4153-800f-f56592cb0d17");

        /// <summary>
        /// Populates this <see cref="Weight"/> instance from the data in the XML.
        /// </summary>
        ///
        /// <param name="typeSpecificXml">
        /// The XML to get the weight data from.
        /// </param>
        ///
        /// <exception cref="InvalidOperationException">
        /// The first node in <paramref name="typeSpecificXml"/> is not
        /// a weight node.
        /// </exception>
        ///
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            XPathNavigator weightNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode("weight");

            Validator.ThrowInvalidIfNull(weightNav, Resources.WeightUnexpectedNode);

            _when = new HealthServiceDateTime();
            _when.ParseXml(weightNav.SelectSingleNode("when"));

            _value = new WeightValue();
            _value.ParseXml(weightNav.SelectSingleNode("value"));
        }

        /// <summary>
        /// Writes the weight data to the specified XmlWriter.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XmlWriter to write the weight data to.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writer"/> is <b>null</b>.
        /// </exception>
        ///
        /// <exception cref="ThingSerializationException">
        /// The <see cref="Value"/> property has not been set.
        /// </exception>
        ///
        public override void WriteXml(XmlWriter writer)
        {
            Validator.ThrowIfWriterNull(writer);
            Validator.ThrowSerializationIfNull(_value, Resources.WeightValueNotSet);

            // <weight>
            writer.WriteStartElement("weight");

            // <when>
            _when.WriteXml("when", writer);

            _value.WriteXml("value", writer);

            // </weight>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets a string representation of the object.
        /// </summary>
        ///
        /// <returns>
        /// <see cref="Value"/> if set; otherwise, or String.Empty.
        /// </returns>
        ///
        public override string ToString()
        {
            string result = string.Empty;

            if (Value != null)
            {
                result = Value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the date/time when the weight measurement
        /// was taken.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="HealthServiceDateTime"/> for the weight measurement.
        /// The default value is the current year, month, and day.
        /// </value>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public HealthServiceDateTime When
        {
            get { return _when; }

            set
            {
                Validator.ThrowIfArgumentNull(value, nameof(When), Resources.WhenNullValue);
                _when = value;
            }
        }

        private HealthServiceDateTime _when = new HealthServiceDateTime();

        /// <summary>
        /// Gets or sets the person's weight.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="WeightValue"/>.
        /// </value>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b> during set.
        /// </exception>
        ///
        public WeightValue Value
        {
            get { return _value; }

            set
            {
                Validator.ThrowIfArgumentNull(value, nameof(Value), Resources.WeightValueMandatory);
                _value = value;
            }
        }

        private WeightValue _value = new WeightValue();
    }
}
