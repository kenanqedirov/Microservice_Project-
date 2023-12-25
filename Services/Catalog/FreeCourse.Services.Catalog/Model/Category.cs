using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Model
{
    public class Category
    {
        [BsonId]     // Key olmağın göstərir
        [BsonRepresentation(BsonType.ObjectId)]     // ObjectId tipine cevirir
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
