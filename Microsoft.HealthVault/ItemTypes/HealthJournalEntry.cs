// Copyright (c) Microsoft Corporation.  All rights reserved.
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.HealthVault.Clients;
using Microsoft.HealthVault.Exceptions;
using Microsoft.HealthVault.Helpers;
using Microsoft.HealthVault.Thing;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// Information related to a health journal entry.
    /// </summary>
    ///
    public class HealthJournalEntry : ThingBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="HealthJournalEntry"/> class with default values.
        /// </summary>
        ///
        /// <remarks>
        /// This item is not added to the health record until the <see cref="IThingClient.CreateNewThingsAsync{ThingBase}(Guid, ICollection{ThingBase})"/> method is called.
        /// </remarks>
        ///
        public HealthJournalEntry()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HealthJournalEntry"/> class
        /// specifying mandatory values.
        /// </summary>
        ///
        /// <remarks>
        /// This item is not added to the health record until the <see cref="IThingClient.CreateNewThingsAsync{ThingBase}(Guid, ICollection{ThingBase})"/> method is called.
        /// </remarks>
        ///
        /// <param name="when">
        /// The date and time associated with the journal entry.
        /// </param>
        /// <param name="content">
        /// The text content of this health journal entry.
        /// </param>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="content"/> is <b>null</b>, empty or contains only whitespace.
        /// </exception>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="when"/> is <b>null</b>.
        /// </exception>
        ///
        public HealthJournalEntry(
            ApproximateDateTime when,
            string content)
            : base(TypeId)
        {
            When = when;
            Content = content;
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
            new Guid("21d75546-8717-4deb-8b17-a57f48917790");

        /// <summary>
        /// Populates this <see cref="HealthJournalEntry"/> instance from the data in the specified XML.
        /// </summary>
        ///
        /// <param name="typeSpecificXml">
        /// The XML to get the HealthJournalEntry data from.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="typeSpecificXml"/> parameter is <b>null</b>.
        /// </exception>
        ///
        /// <exception cref="InvalidOperationException">
        /// If the first node in <paramref name="typeSpecificXml"/> is not
        /// a HealthJournalEntry node.
        /// </exception>
        ///
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            Validator.ThrowIfArgumentNull(typeSpecificXml, nameof(typeSpecificXml), Resources.ParseXmlNavNull);

            XPathNavigator itemNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode("health-journal-entry");

            Validator.ThrowInvalidIfNull(itemNav, Resources.HealthJournalEntryUnexpectedNode);

            _when = new ApproximateDateTime();
            _when.ParseXml(itemNav.SelectSingleNode("when"));
            _content = itemNav.SelectSingleNode("content").Value;
            _category = XPathHelper.GetOptNavValue<CodableValue>(itemNav, "category");
        }

        /// <summary>
        /// Writes the XML representation of the HealthJournalEntry into
        /// the specified XML writer.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XML writer into which the HealthJournalEntry should be
        /// written.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writer"/> parameter is <b>null</b>.
        /// </exception>
        ///
        /// <exception cref="ThingSerializationException">
        /// If <see cref="When"/> is <b>null</b>.
        /// If <see cref="Content"/> is <b>null</b> or empty or contains only whitespace.
        /// </exception>
        ///
        public override void WriteXml(XmlWriter writer)
        {
            Validator.ThrowIfWriterNull(writer);

            Validator.ThrowSerializationIfNull(_when, Resources.WhenNullValue);

            if (string.IsNullOrEmpty(_content) || string.IsNullOrEmpty(_content.Trim()))
            {
                throw new ThingSerializationException(Resources.HealthJournalEntryContentMandatory);
            }

            writer.WriteStartElement("health-journal-entry");

            _when.WriteXml("when", writer);
            writer.WriteElementString("content", _content);
            XmlWriterHelper.WriteOpt(writer, "category", _category);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the date and time associated with the journal entry.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public ApproximateDateTime When
        {
            get
            {
                return _when;
            }

            set
            {
                Validator.ThrowIfArgumentNull(value, nameof(value), Resources.WhenNullValue);
                _when = value;
            }
        }

        private ApproximateDateTime _when;

        /// <summary>
        /// Gets or sets the text content of this health journal entry.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="value"/> parameter is null, empty or only whitespace.
        /// </exception>
        ///
        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                Validator.ThrowIfStringNullOrEmpty(value, "Content");
                Validator.ThrowIfStringIsWhitespace(value, "Content");
                _content = value;
            }
        }

        private string _content;

        /// <summary>
        /// Gets or sets the category of the health journal entry.
        /// </summary>
        ///
        /// <remarks>
        /// The category can be used to group related journal entries together. For example, 'mental health'.
        /// If there is no information about category, the value should be set to <b>null</b>.
        /// </remarks>
        ///
        public CodableValue Category
        {
            get { return _category; }

            set { _category = value; }
        }

        private CodableValue _category;

        /// <summary>
        /// Gets a string representation of the HealthJournalEntry.
        /// </summary>
        ///
        /// <returns>
        /// A string representation of the HealthJournalEntry.
        /// </returns>
        ///
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(200);

            result.Append(_content);

            if (_category != null)
            {
                result.Append(" ");
                result.Append(Resources.OpenParen);
                result.Append(_category.Text);
                result.Append(Resources.CloseParen);
            }

            return result.ToString();
        }
    }
}
