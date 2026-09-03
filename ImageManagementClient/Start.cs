using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
   
    internal static class Start
    {

        public static async Task MainStart()
        {
            await Start.StartMainLoopAsync();
        }

        static async Task StartMainLoopAsync()
        {

            do
            {

                Console.Clear();

                //this is wright the user choices .
                ImageUtility.PrintTheUserChoice();

                // this is the user choice .
                string choice = Console.ReadLine()!;

                switch (choice)
                {

                    case "0":
                        {
                            goto EndProcese;
                        }
                    case "1":
                        {

                            Console.Clear();

                            Console.Write("please inter image id : ");

                            ImageStrucher? image = null;

                            if (int.TryParse(Console.ReadLine(), out int id))
                                image = await Start.GetImageById(id);

                            if (image != null)
                                ImageUtility.PrintImageInfo(image);
                            else Console.WriteLine("the Image is null !");

                        }
                        break;
                    case "2":
                        {
                            Console.Clear();

                            Console.Write("please inter image id : ");

                            bool deletedImage = false;

                            if (int.TryParse(Console.ReadLine(), out int id))
                                deletedImage = await Start.DeleteImage(id);

                            if (deletedImage )Console.WriteLine("The deleted is succefully .");
                            else Console.WriteLine("The deleted is Do Not Happined !");

                        }
                        break;
                    case "3":
                        {

                            Console.Clear();

                            Console.Write("Enter person name: ");
                            string personName = Console.ReadLine() ?? string.Empty;

                            int? newImageId = await Start.AddImageToDataBase(personName);

                            if (newImageId is not null) 
                                Console.WriteLine($"the added is succefully and the new id is {newImageId}.");
                            else 
                                Console.WriteLine("the added is faild !!");
                                
                        }
                        break;
                    case "4":
                        {

                            Console.Clear();

                            bool? UpdatedIsDone = await Start.UpdateImageExisetInDataBase();

                            if (UpdatedIsDone != null) Console.WriteLine($"the Updated is succefully .");
                            else Console.WriteLine("the added is faild !!");

                        }
                        break;
                    case "5":
                        {
                            Console.Clear();
                            bool WillShowing = await Start.ShowImageToMeById();
                            if (WillShowing) Console.WriteLine("Waiting for Shoing the image .");
                            else Console.WriteLine("the image is not exist !!");
                        }
                        break;

                }

            } while (ImageUtility.Agian());

        EndProcese:
            {
                Console.WriteLine("EndProcese");
                return;
            }

        }

        public static async Task<ImageStrucher?> GetImageById(int id)
        {
            ImageStrucher? responseMessage = null;

            if (id <= 0) return null;

            try
            {
                using (var client = new HttpClient())
                {

                    string url = $@"https://localhost:7028/api/ImageUpload/GetImageById?ImageId={id}";

                    bool Succesfull = (await client.GetAsync(url)).StatusCode == System.Net.HttpStatusCode.OK;

                    if (Succesfull)
                        responseMessage = await client.GetFromJsonAsync<ImageStrucher>(url);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return responseMessage;
        }

        public static async Task<bool> DeleteImage(int id)
        {

            if (id <= 0) return false;

            bool IsDeleted = false;

            using (var client = new HttpClient())
            {

                string url = $@"https://localhost:7028/api/ImageUpload/DeleteByID?ImageId={id}";

                IsDeleted = (await client.DeleteAsync(url)).StatusCode == System.Net.HttpStatusCode.OK;

            }

            return IsDeleted;

        }

        public static async Task<int?> AddImageToDataBase(string forPerson)
        {

            int? NewImageId = null;

            ImageStrucher? imageStrucher = FilePicker.PickFile(forPerson);
            string jsonText = JsonSerializer.Serialize(imageStrucher);

            string url = "https://localhost:7028/api/ImageUpload/UploadImageToDataBase_1";

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.PostAsJsonAsync(url, imageStrucher);

                if ((int)response.StatusCode == 201)
                    if (int.TryParse(await response.Content.ReadAsStringAsync(), out int id))
                        NewImageId = id;
            }

            return NewImageId;

        }

        public static async Task<bool?> UpdateImageExisetInDataBase()
        {

            bool? UpdateImageExisetIsDone = null;

            ImageStrucher? imageStrucher = FilePicker.PickFile(string.Empty);
            string jsonText = JsonSerializer.Serialize(imageStrucher);

            Console.Write("Please Inter The Image Id How Is You Want Insted Off : ");
            string? idAsText = Console.ReadLine();
            int id = (int.TryParse(idAsText, out int _id)) ? _id : 0;

            string url = (id != 0) ? $"https://localhost:7028/api/ImageUpload/UpdateByID?ImageId={id}" : string.Empty;

            if (string.IsNullOrEmpty(url)) return UpdateImageExisetIsDone;

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.PutAsJsonAsync(url, imageStrucher);

                if ((int)response.StatusCode == 200)
                    UpdateImageExisetIsDone = true;
            }

            return UpdateImageExisetIsDone;

        }

        public static async Task<bool> ShowImageToMeById()
        {

            Console.Write("please inter image id : ");

            ImageStrucher? image = null;

            if (int.TryParse(Console.ReadLine(), out int id))
                image = await Start.GetImageById(id);

            if (image == null || image.ImageByts is null) return false;

            string tempFile = Path.Combine(Path.GetTempPath(), image.ImageName!);

            File.WriteAllBytes(tempFile, image.ImageByts);

            Process.Start(new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            });

            return true;

        }

    }

}
