namespace BlogService.API
{
    public class PostDataUpdateRequest : PostData, IPostUpdateRequest
    {
        public int PostId { get; set; }

        public PostDataUpdateRequest()
        {

        }

        public PostDataUpdateRequest(int id, string title, string body, string bodyPreview) : base(title, body, bodyPreview)
        {
            PostId = id;
        }
    }
}
