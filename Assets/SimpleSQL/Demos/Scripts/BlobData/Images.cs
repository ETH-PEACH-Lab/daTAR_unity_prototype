namespace SimpleSQL.Demos
{
    using SimpleSQL;

    public class Images
    {
        [PrimaryKey]
        public int ID { get; set; }

        // blobs are stored as byte arrays
        public byte[] ImageData { get; set; }
    }
}