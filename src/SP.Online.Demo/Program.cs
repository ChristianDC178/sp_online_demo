using System;
using System.Collections.Generic;


namespace SP.Online.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            SPConfiguration sPConfiguration = new SPConfiguration()
            {
                ClientSecret = "PmLL9XltXlFLaltyxK0zyNdtmeAMYBkdDZcRQ63pjcc=",
                ClientId = "75105401-4903-4852-bd8c-1737ba72fd2e",
                TenantId = "be803806-c5e6-4fe6-9f88-29e2a5b37146",
                SPUrl = "cdcsharepoint178.sharepoint.com",
                LibraryURL = "ChristianDC"
            };

            SPClient sPClient = new SPClient(sPConfiguration);

            sPClient.BeginSesion();

            List<SPFile> files = new List<SPFile>();

            foreach (var item in new string[] { "Folder_one", "Folder_two" }) //new string[] { "Folder_one", "Folder_two" })
            {
                files.AddRange(sPClient.GetFilesFromFolder(item));
            }

            //DownloadAll
            foreach (var item in files)
            {
                var str = sPClient.DownloadFile(item.Folder, item.Name);
            }

        }
    }
}
