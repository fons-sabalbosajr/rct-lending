using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace rct_lmis.ADMIN_SECTION
{
    public class Announcement
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Content")]
        public string Content { get; set; }

        [BsonElement("PostedDate")]
        public BsonDateTime PostedDate { get; set; }
    }
}