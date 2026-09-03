using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDBConnection
{

    public class ImageDTO
    {

        public int ImageId { get; private set; }

        public string? ImageName {  get; set; }

        public string? ExtentionFile {  get; set; }

        public string? ForPerson { get; set; }

        public byte[]? ImageByts { get; set; }


        public ImageDTO() { }

        public ImageDTO(int imageId, string? imageName, string? forPerson, byte[]? imageByts, string? extentionFile)
        {
            this.ImageId = imageId;
            this.ImageName = imageName;
            this.ForPerson = forPerson;
            this.ImageByts = imageByts;
            this.ExtentionFile = extentionFile;
        }

    }

}
