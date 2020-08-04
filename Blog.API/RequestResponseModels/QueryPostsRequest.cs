namespace BlogService.API
{
    public class QueryPostsRequest
    {
        public int[]? PostIds { get; set; }

        public QueryPostsRequest()
        {

        }

        public QueryPostsRequest(int[]? postIds)
        {
            PostIds = postIds;
        }

        public static QueryPostsRequest QueryAll() => new QueryPostsRequest();
        public static QueryPostsRequest QueryByIds(params int[] postIds) => new QueryPostsRequest(postIds);
    }
}
