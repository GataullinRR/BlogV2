namespace BlogService.API
{
    public class PostUpdateData : PostData
    {
        public int Id { get; set; }

        public PostUpdateData()
        {

        }

        public PostUpdateData(int id, string title, string body, string bodyPreview) : base(title, body, bodyPreview)
        {
            Id = id;
        }
    }
}
