
namespace SP.Online.Demo
{
    public class SPFileResponse
    {
        public D d { get; set; } = new D();

    }

    public class SPFileCollectionResponse
    {

        public SPFileCollectionResponse()
        {
            d = new D();
        }

        public D d { get; set; }

    }

    public class SPFileObjectResponse
    {

        public SPFile d { get; set; }

    }

}
