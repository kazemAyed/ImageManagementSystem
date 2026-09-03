using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    public static class ImageUtility
    {
        public static bool Agian()
        {
            Console.Write("\n\n\nAgian? [Y,N] : ");
            if (Console.ReadLine()?.ToLower() == "y") return true;
            else return false;
        }

        public static void PrintTheUserChoice()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║              IMAGE MANAGEMENT                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");

            Console.ResetColor();

            Console.WriteLine("║                                              ║");
            Console.WriteLine("║  [0]  Exit                                   ║");
            Console.WriteLine("║  [1]  Get image information by ID            ║");
            Console.WriteLine("║  [2]  Delete image by ID                     ║");
            Console.WriteLine("║  [3]  Add image to database                  ║");
            Console.WriteLine("║  [4]  Update image In database               ║");
            Console.WriteLine("║  [5]  Show image from database by ID         ║");
            Console.WriteLine("║                                              ║");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Console.ResetColor();

            Console.Write("\nSelect an option: ");
        }

        public static void PrintImageInfo(ImageStrucher? image)
        {
            if (image is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nImage not found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║                 IMAGE INFO                   ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");

            Console.ResetColor();

            Console.WriteLine($"║ Image ID       : {image.ImageId,-27} ║");
            Console.WriteLine($"║ Person         : {image.ForPerson,-27} ║");
            Console.WriteLine($"║ Image Name     : {image.ImageName,-27} ║");
            Console.WriteLine($"║ File Extension : {image.ExtentionFile,-27} ║");
            Console.WriteLine($"║ Image Size     : {image.ImageByts?.Length ?? 0,-27} ║");

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Console.ResetColor();
        }

        public static async Task GetImageInfo()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("          GET IMAGE BY ID");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.Write("Enter Image ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine();
                Console.WriteLine("❌ Invalid Image ID. Please enter a valid number.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("🔍 Searching for image...");

            ImageStrucher? image = await Start.GetImageById(id);

            Console.WriteLine();

            if (image is not null)
            {
                Console.WriteLine("✅ Image found!");
                Console.WriteLine("----------------------------------------");

                ImageUtility.PrintImageInfo(image);
            }
            else
            {
                Console.WriteLine("❌ Image not found.");
                Console.WriteLine($"No image exists with ID: {id}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }

        public static async Task DeleteImageInfo()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("            DELETE IMAGE");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.Write("Enter Image ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine();
                Console.WriteLine("❌ Invalid Image ID.");
                Console.WriteLine("Please enter a valid number.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                return;
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Image ID: {id}");
            Console.WriteLine("----------------------------------------");

            Console.Write("Are you sure you want to delete this image? (Y/N): ");

            string? confirmation = Console.ReadLine();

            if (!string.Equals(confirmation, "Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("ℹ️ Delete operation cancelled.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                return;
            }

            Console.WriteLine();
            Console.WriteLine("🔍 Deleting image...");

            bool deletedImage = await Start.DeleteImage(id);

            Console.WriteLine();

            if (deletedImage)
            {
                Console.WriteLine("✅ Image deleted successfully.");
                Console.WriteLine($"Image ID {id} has been deleted.");
            }
            else
            {
                Console.WriteLine("❌ Failed to delete image.");
                Console.WriteLine($"No image was deleted with ID: {id}");
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static async Task AddImage()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("             ADD NEW IMAGE");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.Write("Enter person name: ");

            string personName = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(personName))
            {
                Console.WriteLine();
                Console.WriteLine("❌ Person name cannot be empty.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                return;
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Person Name: {personName}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();

            Console.Write("Adding image...");

            int? newImageId = await Start.AddImageToDataBase(personName);

            Console.WriteLine();
            Console.WriteLine();

            if (newImageId is not null)
            {
                Console.WriteLine("✅ Image added successfully!");
                Console.WriteLine($"New Image ID: {newImageId}");
            }
            else
            {
                Console.WriteLine("❌ Failed to add image.");
                Console.WriteLine("The image could not be added to the database.");
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static async Task UpdateImage()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("             UPDATE IMAGE");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.WriteLine("🔄 Updating image...");
            Console.WriteLine();

            bool? isUpdated = await Start.UpdateImageExisetInDataBase();

            if (isUpdated == true)
            {
                Console.WriteLine("✅ Image updated successfully.");
            }
            else if (isUpdated == false)
            {
                Console.WriteLine("❌ Failed to update image.");
                Console.WriteLine("The image could not be updated.");
            }
            else
            {
                Console.WriteLine("⚠️ Update operation returned no result.");
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static async Task ShowImage()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("             SHOW IMAGE");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.WriteLine("🔍 Looking for the image...");
            Console.WriteLine();

            bool imageDisplayed = await Start.ShowImageToMeById();

            Console.WriteLine();

            if (imageDisplayed)
            {
                Console.WriteLine("✅ Image found successfully.");
                Console.WriteLine("🖼️ Displaying image...");
            }
            else
            {
                Console.WriteLine("❌ Image not found.");
                Console.WriteLine("The requested image does not exist.");
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

    }

}
