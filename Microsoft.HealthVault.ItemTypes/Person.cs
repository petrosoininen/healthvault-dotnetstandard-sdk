// Copyright(c) Microsoft Corporation.
// This content is subject to the Microsoft Reference Source License,
// see http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Xml;
using System.Xml.XPath;
using Microsoft.HealthVault.Helpers;
using Microsoft.HealthVault.Thing;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// Represents a health record item type that encapsulates non-identifying
    /// information about a person.
    /// </summary>
    ///
    public class Person : HealthRecordItem
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Person"/> class with default values.
        /// </summary>
        ///
        /// <remarks>
        /// The item is not added to the health record until the
        /// <see cref="HealthRecordAccessor.NewItem(HealthRecordItem)"/> method
        /// is called.
        /// </remarks>
        ///
        public Person()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Person"/> class with the
        /// specified name.
        /// </summary>
        ///
        /// <param name="name">
        /// The name of the person.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> parameter
        /// is <b>null</b>.
        /// </exception>
        ///
        public Person(Name name)
            : base(TypeId)
        {
            this.Item.Name = name;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Person"/> class with the
        /// specified name and type.
        /// </summary>
        ///
        /// <param name="name">
        /// The name of the person.
        /// </param>
        ///
        /// <param name="personType">
        /// The type of the person, such as emergency contact, provider, and
        /// so on.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> or <paramref name="personType"/>
        /// parameter is <b>null</b>.
        /// </exception>
        ///
        public Person(Name name, CodableValue personType)
            : base(TypeId)
        {
            this.Name = name;
            this.PersonType = personType;
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
            new Guid("25c94a9f-9d3d-4576-96dc-6791178a8143");

        /// <summary>
        /// Populates this <see cref="Person"/> instance from the data in the XML.
        /// </summary>
        ///
        /// <param name="typeSpecificXml">
        /// The XML to get the person data from.
        /// </param>
        ///
        /// <exception cref="InvalidOperationException">
        /// The first node in <paramref name="typeSpecificXml"/> is not
        /// a person node.
        /// </exception>
        ///
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            XPathNavigator personNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode("person");

            Validator.ThrowInvalidIfNull(personNav, "PersonUnexpectedNode");

            // <person>
            this.item.ParseXml(personNav);
        }

        /// <summary>
        /// Writes the person data to the specified XmlWriter.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XmlWriter to write the person data to.
        /// </param>
        ///
        public override void WriteXml(XmlWriter writer)
        {
            // <person>
            this.item.WriteXml("person", writer);
        }

        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="Name"/> value.
        /// </value>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b> during set.
        /// </exception>
        ///
        public Name Name
        {
            get { return this.Item.Name; }
            set { this.Item.Name = value; }
        }

        /// <summary>
        /// Gets or sets the organization the person belongs to.
        /// </summary>
        ///
        /// <value>
        /// A string representing the organization.
        /// </value>
        ///
        /// <remarks>
        /// Set the value to <b>null</b> if the organization should not be stored.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="value"/> contains only whitespace.
        /// </exception>
        ///
        public string Organization
        {
            get { return this.Item.Organization; }

            set
            {
                Validator.ThrowIfStringIsWhitespace(value, "Organization");
                this.Item.Organization = value;
            }
        }

        /// <summary>
        /// Gets or sets the professional training for the provider.
        /// </summary>
        ///
        /// <value>
        /// A string representing the training.
        /// </value>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="value"/> contains only whitespace.
        /// </exception>
        ///
        public string ProfessionalTraining
        {
            get { return this.Item.ProfessionalTraining; }

            set
            {
                Validator.ThrowIfStringIsWhitespace(value, "ProfessionalTraining");
                this.Item.ProfessionalTraining = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID number for the person in the organization.
        /// </summary>
        ///
        /// <value>
        /// A string representing the ID number.
        /// </value>
        ///
        /// <remarks>
        /// Set the value to <b>null</b> if the ID should not be stored.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="value"/> contains only whitespace.
        /// </exception>
        ///
        public string PersonId
        {
            get { return this.Item.PersonId; }

            set
            {
                Validator.ThrowIfStringIsWhitespace(value, "PersonId");
                this.Item.PersonId = value;
            }
        }

        /// <summary>
        /// Gets or sets the contact information.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="ContactInfo"/> representing the information.
        /// </value>
        ///
        /// <remarks>
        /// Set the value to <b>null</b> if the contact information should not be
        /// stored.
        /// </remarks>
        ///
        public ContactInfo ContactInformation
        {
            get { return this.Item.ContactInformation; }
            set { this.Item.ContactInformation = value; }
        }

        /// <summary>
        /// Gets or sets the type of person, such as provider, emergency contact,
        /// and so on.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="CodableValue"/> representing the person type.
        /// </value>
        ///
        public CodableValue PersonType
        {
            get { return this.Item.PersonType; }
            set { this.Item.PersonType = value; }
        }

        /// <summary>
        /// Gets or sets the person information.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="PersonItem"/> representing the information.
        /// </value>
        ///
        private PersonItem Item
        {
            get { return this.item; }
            set { this.item = value; }
        }

        private PersonItem item = new PersonItem();

        /// <summary>
        /// Gets a string representation of the person item.
        /// </summary>
        ///
        /// <returns>
        /// A string representation of the person item.
        /// </returns>
        ///
        public override string ToString()
        {
            string result = this.Name.ToString();

            if (this.PersonType != null)
            {
                result =
                    string.Format(
                        ResourceRetriever.GetResourceString(
                            "PersonToStringFormat"),
                        this.Name.ToString(),
                        this.PersonType.Text);
            }

            return result;
        }
    }
}
