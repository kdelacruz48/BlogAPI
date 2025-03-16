using BlogAPI.Models.DTO;

namespace BlogAPI.Data
{
    public class PostStore
    {
        public static List<PostUpdateDTO> postList = new List<PostUpdateDTO> {
             new PostUpdateDTO{ Id = 1, Title = "test", Post = "test", UserName = "test" },
             new PostUpdateDTO{Id = 2, Title = "test2", Post = "test2", UserName = "test2"}
        };    

    }
}
