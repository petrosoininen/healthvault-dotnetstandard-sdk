// Copyright(c) Microsoft Corporation.
// This content is subject to the Microsoft Reference Source License,
// see http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using Microsoft.HealthVault.Helpers;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// An approximation of a time.
    /// </summary>
    ///
    /// <remarks>
    /// An approximation of a time must have an hour and minute and can
    /// also optionally have seconds specified.
    /// </remarks>
    ///
    public class ApproximateTime
        : ItemBase,
            IComparable,
            IComparable<ApproximateTime>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ApproximateTime"/> class
        /// with default values.
        /// </summary>
        ///
        public ApproximateTime()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApproximateTime"/> class
        /// with the specified hour and minute.
        /// </summary>
        ///
        /// <param name="hour">
        /// The hour between 0 and 23.
        /// </param>
        ///
        /// <param name="minute">
        /// The minute between 0 and 59.
        /// </param>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="hour"/> parameter is less than 0 or greater
        /// than 23, or the <paramref name="minute"/> parameter is less than 0 or
        /// greater than 59.
        /// </exception>
        ///
        public ApproximateTime(int hour, int minute)
        {
            this.Hour = hour;
            this.Minute = minute;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApproximateTime"/> class
        /// with the specified hour, minute, and second.
        /// </summary>
        ///
        /// <param name="hour">
        /// The hour between 0 and 23.
        /// </param>
        ///
        /// <param name="minute">
        /// The minute between 0 and 59.
        /// </param>
        ///
        /// <param name="second">
        /// The second between 0 and 59.
        /// </param>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="hour"/> parameter is less than 0 or greater
        /// than 23, or the <paramref name="minute"/> or <paramref name="second"/>
        /// parameter is less than 0 or
        /// greater than 59.
        /// </exception>
        ///
        public ApproximateTime(int hour, int minute, int second)
            : this(hour, minute)
        {
            this.Second = second;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApproximateTime"/> class
        /// with the specified hour, minute, second, and millisecond.
        /// </summary>
        ///
        /// <param name="hour">
        /// The hour between 0 and 23.
        /// </param>
        ///
        /// <param name="minute">
        /// The minute between 0 and 59.
        /// </param>
        ///
        /// <param name="second">
        /// The second between 0 and 59.
        /// </param>
        ///
        /// <param name="millisecond">
        /// The millisecond between 0 and 999.
        /// </param>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="hour"/> parameter is less than 0 or greater
        /// than 23, or the <paramref name="minute"/>, or <paramref name="second"/>
        /// parameter is less than 0 or
        /// greater than 59, or <paramref name="millisecond"/> parameter is less than 0
        /// or greater than 999.
        /// </exception>
        ///
        public ApproximateTime(int hour, int minute, int second, int millisecond)
            : this(hour, minute, second)
        {
            this.Millisecond = millisecond;
        }

        /// <summary>
        /// Gets the current time including the hour, minute, and second.
        /// </summary>
        ///
        public static ApproximateTime Now
        {
            get
            {
                DateTime now = DateTime.Now;
                return new ApproximateTime(now.Hour, now.Minute, now.Second);
            }
        }

        /// <summary>
        /// Populates the data for the approximate time from the XML.
        /// </summary>
        ///
        /// <param name="navigator">
        /// The XML node representing the approximate time.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="navigator"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public override void ParseXml(XPathNavigator navigator)
        {
            Validator.ThrowIfNavigatorNull(navigator);

            XPathNavigator hourNav = navigator.SelectSingleNode("h");
            if (hourNav != null)
            {
                this.hour = hourNav.ValueAsInt;
            }

            XPathNavigator minuteNav = navigator.SelectSingleNode("m");
            if (minuteNav != null)
            {
                this.minute = minuteNav.ValueAsInt;
            }

            XPathNavigator secondNav = navigator.SelectSingleNode("s");
            if (secondNav != null)
            {
                this.second = secondNav.ValueAsInt;
            }

            XPathNavigator millisecondNav = navigator.SelectSingleNode("f");
            if (millisecondNav != null)
            {
                this.millisecond = millisecondNav.ValueAsInt;
            }
        }

        /// <summary>
        /// Writes the approximate time to the specified XML writer.
        /// </summary>
        ///
        /// <param name="nodeName">
        /// The name of the outer element for the approximate time.
        /// </param>
        ///
        /// <param name="writer">
        /// The XmlWriter to write the approximate time to.
        /// </param>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="nodeName"/> parameter is <b>null</b> or empty.
        /// </exception>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="writer"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public override void WriteXml(string nodeName, XmlWriter writer)
        {
            Validator.ThrowIfStringNullOrEmpty(nodeName, "nodeName");
            Validator.ThrowIfArgumentNull(writer, nameof(writer), Resources.WriteXmlNullWriter);

            // Having hour and minute null really means that we have
            // an unknown approximate time since both are required.
            if (this.hour != null && this.minute != null)
            {
                writer.WriteStartElement(nodeName);

                writer.WriteElementString(
                    "h", this.hour.Value.ToString(CultureInfo.InvariantCulture));

                writer.WriteElementString(
                    "m", this.minute.Value.ToString(CultureInfo.InvariantCulture));

                if (this.second != null)
                {
                    writer.WriteElementString(
                        "s",
                        ((int)this.second).ToString(CultureInfo.InvariantCulture));
                }

                if (this.millisecond != null)
                {
                    writer.WriteElementString(
                        "f",
                        ((int)this.millisecond).ToString(
                            CultureInfo.InvariantCulture));
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Gets or sets the hour of the time approximation.
        /// </summary>
        ///
        /// <returns>
        /// An integer representing the hour.
        /// </returns>
        ///
        /// <remarks>
        /// This value defaults to the current hour.
        /// </remarks>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="value"/> parameter is less than 0 or greater than 23
        /// when setting.
        /// </exception>
        ///
        public int Hour
        {
            get
            {
                return this.hour != null ? this.hour.Value : 0;
            }

            set
            {
                if (value < 0 || value > 23)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Hour), Resources.TimeHoursOutOfRange);
                }

                this.hour = value;
            }
        }

        private int? hour;

        /// <summary>
        /// Gets or sets the minute of the time approximation.
        /// </summary>
        ///
        /// <returns>
        /// An integer representing the minute.
        /// </returns>
        ///
        /// <remarks>
        /// This value defaults to the current minute.
        /// </remarks>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="value"/> parameter is less than 0 or greater than 59
        /// when setting.
        /// </exception>
        ///
        public int Minute
        {
            get
            {
                return this.minute != null ? this.minute.Value : 0;
            }

            set
            {
                if (value < 0 || value > 59)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Minute), Resources.TimeMinutesOutOfRange);
                }

                this.minute = value;
            }
        }

        private int? minute;

        /// <summary>
        /// Gets or sets the seconds of the time approximation.
        /// </summary>
        ///
        /// <returns>
        /// An integer representing the second.
        /// </returns>
        ///
        /// <remarks>
        /// If the number of seconds is unknown, the value can be set to
        /// <b>null</b>. This value defaults to <b>null</b>.
        /// </remarks>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="value"/> parameter is less than 0 or greater than 59
        /// when setting.
        /// </exception>
        ///
        public int? Second
        {
            get { return this.second; }

            set
            {
                if (value != null)
                {
                    if (value < 0 || value > 59)
                    {
                        throw new ArgumentOutOfRangeException(nameof(this.Second), Resources.TimeSecondsOutOfRange);
                    }
                }

                this.second = value;
            }
        }

        private int? second;

        /// <summary>
        /// Gets or sets the milliseconds of the time approximation.
        /// </summary>
        ///
        /// <returns>
        /// An integer representing the milliseconds.
        /// </returns>
        ///
        /// <remarks>
        /// If the number of milliseconds is unknown, the value can be set to <b>null</b>.
        /// This value defaults to <b>null</b>.
        /// </remarks>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="value"/> parameter is less than 0 or greater than 999
        /// when setting.
        /// </exception>
        ///
        public int? Millisecond
        {
            get { return this.millisecond; }

            set
            {
                // The following if statements cannot be combined with && becuase
                // it would throw an exception if value == null.
                if (value != null)
                {
                    if (value < 0 || value > 999)
                    {
                        throw new ArgumentOutOfRangeException(nameof(this.Millisecond), Resources.TimeMillisecondsOutOfRange);
                    }
                }

                this.millisecond = value;
            }
        }

        private int? millisecond;

        /// <summary>
        /// Gets a boolean indicating whether this ApproximateTime has a value in it.
        /// </summary>
        ///
        /// <returns>
        /// True if there is a value.
        /// </returns>
        ///
        /// <remarks>
        /// ApproximateTime instances are initialized into state without a value, but the Hour and Minute
        /// properties must still return numbers in that case. This property is used to tell the
        /// difference between an ApproximateTime that has no value and one where the hour and minute
        /// have been set to zero (ie 12:00 AM).
        /// </remarks>
        ///
        public bool HasValue => this.hour.HasValue && this.minute.HasValue;

        #region IComparable

        /// <summary>
        /// Compares the specified object to this ApproximateTime object.
        /// </summary>
        ///
        /// <returns>
        /// An integer representing the object order.
        /// </returns>
        ///
        /// <param name="obj">
        /// The object to be compared.
        /// </param>
        ///
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the
        /// objects being compared.
        /// </returns>
        ///
        /// <remarks>
        /// If the result is less than zero, the
        /// instance is less than <paramref name="obj"/>. If the result is zero
        /// the instance is equal to <paramref name="obj"/>. If the result is
        /// greater than zero, the instance is greater than
        /// <paramref name="obj"/>.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="obj"/> parameter is not an <see cref="ApproximateTime"/>
        /// object.
        /// </exception>
        ///
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            ApproximateTime hsTime = obj as ApproximateTime;
            if (hsTime == null)
            {
                throw new ArgumentException(Resources.TimeCompareToInvalidType, nameof(obj));
            }

            return this.CompareTo(hsTime);
        }

        /// <summary>
        /// Compares the specified object to this ApproximateDate object.
        /// </summary>
        ///
        /// <param name="other">
        /// The time to be compared.
        /// </param>
        ///
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the
        /// objects being compared.
        /// </returns>
        ///
        /// <remarks>
        /// If the result is less than zero, the
        /// instance is less than <paramref name="other"/>. If the result is zero
        /// the instance is equal to <paramref name="other"/>. If the result is
        /// greater than zero, the instance is greater than
        /// <paramref name="other"/>.
        /// </remarks>
        ///
        public int CompareTo(ApproximateTime other)
        {
            int result;
            if (other == null)
            {
                return 1;
            }

            if (this.Hour > other.Hour)
            {
                return 1;
            }

            if (this.Hour < other.Hour)
            {
                return -1;
            }

            if (this.Minute > other.Minute)
            {
                return 1;
            }

            if (this.Minute < other.Minute)
            {
                return -1;
            }

            result = CompareOptional(this.Second, other.Second);
            if (result != 0)
            {
                return result;
            }

            return CompareOptional(this.Millisecond, other.Millisecond);
        }

        internal static int CompareOptional(int? first, int? second)
        {
            if (first == null)
            {
                if (second != null)
                {
                    return -1;
                }
            }
            else
            {
                if (second == null)
                {
                    return 1;
                }

                if (first < second)
                {
                    return -1;
                }

                if (first > second)
                {
                    return 1;
                }
            }

            return 0;
        }

        #endregion IComparable

        #region Equals

        /// <summary>
        /// Gets a value indicating whether the specified object is equal to this object.
        /// </summary>
        ///
        /// <param name="obj">
        /// The object to be compared.
        /// </param>
        ///
        /// <returns>
        /// <b>true</b> if the <paramref name="obj"/> is a
        /// <see cref="ApproximateTime"/> object and the hour, minute,
        /// second, and millisecond exactly match; otherwise, <b>false</b>.
        /// </returns>
        ///
        /// <remarks>
        /// If an <see cref="ApproximateDate"/> is less specific than the other
        /// <see cref="ApproximateDate"/>, the less specific one is considered less than
        /// the more specific one, assuming that the current data is equal.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="obj"/> parameter is not an <see cref="ApproximateTime"/>
        /// object.
        /// </exception>
        ///
        public override bool Equals(object obj)
        {
            return this.CompareTo(obj) == 0;
        }

        /// <summary>
        /// See the base class documentation.
        /// </summary>
        ///
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Equals

        #region Operators

        /// <summary>
        /// Gets a value indicating whether the specified object is equal to
        /// the specified time.
        /// </summary>
        ///
        /// <param name="time">
        /// The time object to be compared.
        /// </param>
        ///
        /// <param name="obj">
        /// The second object to be compared.
        /// </param>
        ///
        /// <returns>
        /// <b>true</b> if <paramref name="time"/> has the hour, minute,
        /// second, and millisecond exactly match; otherwise, <b>false</b>.
        /// </returns>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="obj"/> parameter is not an
        /// <see cref="ApproximateTime"/> object.
        /// </exception>
        ///
        public static bool operator ==(ApproximateTime time, object obj)
        {
            if ((object)time == null)
            {
                return obj == null;
            }

            return time.Equals(obj);
        }

        /// <summary>
        /// Gets a value indicating whether the specified object is not equal
        /// to the specified
        /// time.
        /// </summary>
        ///
        /// <param name="time">
        /// The time object to be compared.
        /// </param>
        ///
        /// <param name="obj">
        /// The second object to be compared.
        /// </param>
        ///
        /// <returns>
        /// <b>false</b> if <paramref name="time"/> has the hour, minute,
        /// second, and millisecond exactly match.
        /// </returns>
        ///
        /// <remarks>
        /// If an <see cref="ApproximateDate"/> is less specific than the other
        /// <see cref="ApproximateDate"/>, the less specific one is considered less than
        /// the more specific one, assuming that the current data is equal.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="obj"/> parameter is not an <see cref="ApproximateTime"/> object.
        /// </exception>
        ///
        public static bool operator !=(ApproximateTime time, object obj)
        {
            if (time == null)
            {
                return obj != null;
            }

            return !time.Equals(obj);
        }

        /// <summary>
        /// Gets a value indicating whether the specified time is greater than the specified
        /// object.
        /// </summary>
        ///
        /// <param name="time">
        /// The time object to be compared.
        /// </param>
        ///
        /// <param name="obj">
        /// The second object to be compared.
        /// </param>
        ///
        /// <returns>
        /// <b>true</b> if <paramref name="time"/> has the hour, minute,
        /// second, and millisecond greater than that of
        /// <paramref name="obj"/>.
        /// </returns>
        ///
        /// <remarks>
        /// If an <see cref="ApproximateDate"/> is less specific than the other
        /// <see cref="ApproximateDate"/>, the less specific one is considered less than
        /// the more specific one, assuming that the current data is equal.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="obj"/> parameter is not an
        /// <see cref="ApproximateTime"/> object.
        /// </exception>
        ///
        public static bool operator >(ApproximateTime time, object obj)
        {
            if (time == null)
            {
                return obj != null;
            }

            return time.CompareTo(obj) > 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified time is less than the specified
        /// object.
        /// </summary>
        ///
        /// <param name="time">
        /// The time object to be compared.
        /// </param>
        ///
        /// <param name="obj">
        /// The second object to be compared.
        /// </param>
        ///
        /// <returns>
        /// <b>true</b> if <paramref name="time"/> has the hour, minute,
        /// second, and millisecond less than that of
        /// <paramref name="obj"/>.
        /// </returns>
        ///
        /// <remarks>
        /// If an <see cref="ApproximateDate"/> is less specific than the other
        /// <see cref="ApproximateDate"/>, the less specific one is considered less than
        /// the more specific one, assuming that the current data is equal.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// The <paramref name="obj"/> parameter is not an
        /// <see cref="ApproximateTime"/> object.
        /// </exception>
        ///
        public static bool operator <(ApproximateTime time, object obj)
        {
            if (time == null)
            {
                return obj != null;
            }

            return time.CompareTo(obj) < 0;
        }

        #endregion Operators

        internal string ToString(IFormatProvider formatProvider)
        {
            // If the default constructor is called, _hour is null, so we return empty.
            if (this.hour == null)
            {
                return string.Empty;
            }

            DateTimeFormatInfo formatInfo =
                (DateTimeFormatInfo)formatProvider.GetFormat(typeof(DateTimeFormatInfo));

            string format = formatInfo.LongTimePattern;

            DateTime dt = new DateTime(1900, 1, 1);
            dt = dt.AddHours(this.Hour);
            dt = dt.AddMinutes(this.Minute);

            if (this.Second != null)
            {
                dt = dt.AddSeconds(this.Second.Value);
            }

            if (this.Millisecond != null)
            {
                dt = dt.AddMilliseconds(this.Millisecond.Value);

                NumberFormatInfo numberFormatInfo =
                    (NumberFormatInfo)formatProvider.GetFormat(typeof(NumberFormatInfo));

                // There is no time pattern that includes milliseconds.  The hack around this is to
                // replace all occurrences of "ss" with "ss.ffff".
                format = format.Replace("ss", "ss" + numberFormatInfo.NumberDecimalSeparator + "fff");
            }

            return dt.ToString(format, formatProvider);
        }
    }
}