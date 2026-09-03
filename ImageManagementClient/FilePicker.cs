using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class FilePicker
    {

        public static ImageStrucher? PickFile(string forperson)
        {
            ImageStrucher? imageStrucher = null;

            Thread staThread = new Thread(() =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                   
                    imageStrucher = new ImageStrucher();

                    openFileDialog.Title = "Select an Image";
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        byte[] imageBytes = File.ReadAllBytes(filePath);

                        imageStrucher.ImageName = Path.GetFileName(filePath);
                        imageStrucher.ForPerson = $"{forperson}";
                        imageStrucher.ExtentionFile = Path.GetExtension(filePath);
                        imageStrucher.ImageByts = imageBytes ?? null;

                    }
                };

            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            return imageStrucher;

        }

    }

}
