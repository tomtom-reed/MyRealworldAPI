/*
 * RealWorld Conduit API
 *
 * Conduit API documentation
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;

namespace RealworldApi.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class NewArticle : IEquatable<NewArticle>
    { 
        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [Required]

        [DataMember(Name="title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [Required]

        [DataMember(Name="description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Body
        /// </summary>
        [Required]

        [DataMember(Name="body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or Sets TagList
        /// </summary>

        [DataMember(Name="tagList")]
        public List<string> TagList { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NewArticle {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Body: ").Append(Body).Append("\n");
            sb.Append("  TagList: ").Append(TagList).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return  JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NewArticle)obj);
        }

        /// <summary>
        /// Returns true if NewArticle instances are equal
        /// </summary>
        /// <param name="other">Instance of NewArticle to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(NewArticle? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Title == other.Title ||
                    Title != null &&
                    Title.Equals(other.Title)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    Body == other.Body ||
                    Body != null &&
                    Body.Equals(other.Body)
                ) && 
                (
                    TagList == other.TagList ||
                    TagList != null &&
                    TagList.SequenceEqual(other.TagList)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Title != null)
                    hashCode = hashCode * 59 + Title.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (Body != null)
                    hashCode = hashCode * 59 + Body.GetHashCode();
                    if (TagList != null)
                    hashCode = hashCode * 59 + TagList.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(NewArticle left, NewArticle right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NewArticle left, NewArticle right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}