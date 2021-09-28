using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SomeExampleFunctions.Models
{
    public class Integration
    {
        public Integration()
        {
            CreatedAt = DateTime.Now;
            Status = IntegrationStatus.RECEIVED;
        }

        [BsonId]
        [JsonProperty("objectId")]
        public ObjectId ObjectId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Key]
        [Required]
        [Column(TypeName = "varchar(25)")]
        public string MessageId { get; set; }

        [Required]
        [Column(TypeName = "varchar(25)")]
        public string CorrelationId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string JobName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string MessageBodyFilePath { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string ExceptionMessage { get; set; }

        public IntegrationStatus Status { get; set; }
    }

    public enum IntegrationStatus
    {
        RECEIVED = 0,
        SUCCESS = 1,
        FAILURE = -1,
    }
}
