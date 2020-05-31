using System;
using System.Collections.Generic;


namespace SP.Online.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                SPConfiguration sPConfiguration = new SPConfiguration()
                {
                    ClientSecret = "YsshbJsH1UPgeRd07UVKN8xNH3o5Sc6dlrnxhsNmc0k=",
                    ClientId = "3b9e6c50-065e-45e6-9cc1-05dd9ef1657d",
                    TenantId = "90295537-9508-4e55-bcc6-eb7d75692b82",
                    SPUrl = "cdc178202005.sharepoint.com",
                    LibraryURL = "ChristianDC"
                };

                SPClient sPClient = new SPClient(sPConfiguration);

                if (sPClient.BeginSesion())
                {

                    Console.WriteLine("Signin succesful !");
                    Console.WriteLine($"The access token: {sPClient.AuthToken}");

                    Console.WriteLine("Listing files from the folders");
                    List<SPFile> files = new List<SPFile>();


                    foreach (var item in new string[] { "Folder_one", "Folder_two" }) //new string[] { "Folder_one", "Folder_two" })
                    {
                        files.AddRange(sPClient.GetFilesFromFolder(item));
                    }

                    files.ForEach((f) => { Console.WriteLine($"File: {f.Folder}/{f.Name}"); });

                    Console.WriteLine("Downloading the files to a local folder");



                    //DownloadAll
                    foreach (var item in files)
                    {
                        var str = sPClient.DownloadFile(item.Folder, item.Name);
                    }


                    Console.WriteLine("Done !");
                }
                else
                {
                    Console.WriteLine("Signin failed !!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ups , there is an error: {ex.Message}");
            }

            finally
            {
                Console.ReadKey();
            }

        }

    }


}
