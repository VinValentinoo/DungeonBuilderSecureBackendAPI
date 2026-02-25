namespace MySecureBackend.WebApi.Models
{
    public class Object2D
    {
        public int Id { get; set; }
        public int EnvironmentId { get; set; }

        public string ObjectType { get; set; } = null;
        public int PosX { get; set; }
        public int PosY { get; set; }

        public float Rotation { get; set; }
        public float Scale { get; set; }
    }
}
