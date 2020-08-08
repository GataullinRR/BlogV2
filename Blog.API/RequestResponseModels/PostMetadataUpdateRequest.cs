namespace BlogService.API
{
    public class PostMetadataUpdateRequest : IPostUpdateRequest
    {
        public int PostId { get; set; }
        public bool? IsHidden { get; set; }
        public bool? IsDeleted { get; set; }

        public PostMetadataUpdateRequest()
        {

        }

        public PostMetadataUpdateRequest(int id, bool? isHidden, bool? isDeleted)
        {
            PostId = id;
            IsHidden = isHidden;
            IsDeleted = isDeleted;
        }
    }
}
