
namespace SP.Online.Demo
{
    public class SPConfiguration
    {

        public string GrantType => "client_credentials";

        public string SPUrl { get; set; }

        public string Resource
        {
            get
            {
                return $"00000003-0000-0ff1-ce00-000000000000/{SPUrl}@{TenantId}";
            }
        }

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AuthClientID
        {
            get
            {
                return $"{ClientId}@{TenantId}";
            }
        }

        public string AuthURL
        {
            get
            {
                return $@"https://accounts.accesscontrol.windows.net/{TenantId}/tokens/OAuth/2";
            }
        }

        public string LibraryURL { get; set; }

    }
}
