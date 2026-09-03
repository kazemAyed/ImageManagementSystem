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

    }

}
