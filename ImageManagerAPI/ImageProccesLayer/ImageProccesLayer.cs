
using ImageDBConnection;

namespace ImageProccesLayer
{

    public class ImageProcces
    {
        
        public int? ImageId { get; private set; }

        private enImageMode enImageMode { get; set; } = enImageMode.None;

        private ImageDTO imageDTO { get; set; }

        public ImageProcces(ImageDTO imageDTO , enImageMode mode = enImageMode.AddNew)
        {
            this.imageDTO = imageDTO;
            this.enImageMode = mode;
        }

        private bool AddNew()
        {
            if (imageDTO != null)
                this.ImageId = DBConnection.AddImage(this.imageDTO);
            return this.ImageId != null;
        }

        public bool Save()
        {

            switch (this.enImageMode)
            {
                case enImageMode.None:
                    return false;
                case enImageMode.AddNew:
                    if(this.AddNew())
                    {
                        this.enImageMode = enImageMode.Update;
                        return true;
                    }
                    break;
                case enImageMode.Update:
                    return false;
                default:
                    break;
            }

            return false;

        }

        public static ImageDTO? GetImageById(int imageId)
        {
            return DBConnection.GetImageById(imageId);
        }

        public static bool Delete(int  imageId)
        {
            return DBConnection.Delete(imageId);
        }

        public static bool Update(int ImageId, ImageDTO imageDTO)
        {
            return DBConnection.Update(ImageId, imageDTO);
        }

    }

}
