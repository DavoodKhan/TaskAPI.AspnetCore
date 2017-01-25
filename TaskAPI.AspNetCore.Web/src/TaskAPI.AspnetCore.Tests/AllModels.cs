using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI.AspnetCore.Tests
{
    public class User
    {
        /// <summary>
        /// Identity
        /// </summary>
        /// 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        //public int Id { get; set; }

        /// <summary>
        /// User unique Id
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// User email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// User created on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// User updated on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// True if user is deleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// True if user is active
        /// </summary>
        public bool? IsActive { get; set; }
    }

    public class Task
    {
        /// <summary>
        /// Identity
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Task List Unique Id
        /// </summary>
        public string TaskListId { get; set; }

        /// <summary>
        /// Task Unique Id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task due date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? DueOnUtc { get; set; }

        /// <summary>
        /// Task completed on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CompletedOnUtc { get; set; }

        /// <summary>
        /// True if task is completed 
        /// </summary>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// Task created on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Task updated on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// True if task is deleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// True if task is active
        /// </summary>
        public bool? IsActive { get; set; }
    }


    public class TaskList
    {
        /// <summary>
        /// Identity
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// User unique Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Task list unique Id
        /// </summary>
        public string TaskListId { get; set; }

        /// <summary>
        /// Task list title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task list created on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Task list updated on date
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// True list if task is deleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// True list if task is active
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
