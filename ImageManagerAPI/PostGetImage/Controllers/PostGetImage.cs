

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PostGetImage.Models;
using ImageProccesLayer;
using ImageDBConnection;

namespace PostGetImage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageUploadController : ControllerBase
{

    [HttpPost("UploadImageToExternalFolder", Name = "UploadImageToExternalFolder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImageToExternalFolder(
    IFormFile imageFile,
    string destinationFolder)
    {
        if (imageFile == null || imageFile.Length == 0)
            return BadRequest("Image file is required.");

        if (string.IsNullOrWhiteSpace(destinationFolder))
            return BadRequest("Destination folder is required.");

        if (!modFileAndDirecrty.VerfingDirectionByPath(destinationFolder))
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error happened."
            );

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
        string filePath = Path.Combine(destinationFolder, fileName);

        try
        {
            using var fileStream = new FileStream(
                filePath,
                FileMode.Create
            );

            await imageFile.CopyToAsync(fileStream);

            return Ok("Successfully uploaded.");
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error happened."
            );
        }
    }


    [HttpPost("UploadImageToDataBase_0", Name = "UploadImageToDataBase_0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImageToDataBase_0(
        IFormFile imageFile, 
        string forPerson)
    {

        if (imageFile == null || string.IsNullOrEmpty(forPerson)) 
            return BadRequest("the image file is null or empty!!");

        byte[]? bytesOfImageFile = null;

        using (var MemoryStream = new MemoryStream())
        {
            await imageFile.CopyToAsync(MemoryStream);
            bytesOfImageFile = MemoryStream.ToArray();
        }

        if (bytesOfImageFile == null) return StatusCode(StatusCodes.Status500InternalServerError);

        ImageDTO imageDTO = new ImageDTO()
        {
            ForPerson = forPerson,
            ImageName = imageFile.FileName,
            ExtentionFile = Path.GetExtension(imageFile.FileName),
            ImageByts = bytesOfImageFile
        };

        ImageProcces imageProcces = new ImageProcces(imageDTO);

        if (imageProcces.Save()) return Ok($"Succesfully this Image Id is {imageProcces.ImageId} .");
        else return StatusCode(StatusCodes.Status500InternalServerError);

    }

    [HttpPost("UploadImageToDataBase_1", Name = "UploadImageToDataBase_1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UploadImageToDataBase_1(ImageDTO imageFile)
    {

        if (imageFile == null || imageFile.ImageByts == null)
            return BadRequest("the image file is null or empty!!");

        ImageProcces imageProcces = new ImageProcces(imageFile);

        if (imageProcces.Save())
            return CreatedAtRoute("GetImageById", new { id = imageProcces.ImageId }, imageProcces.ImageId);
        else 
            return StatusCode(StatusCodes.Status500InternalServerError);

    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetImageById",Name = "GetImageById")]
    public IActionResult GetImageById(int ImageId)
    {
        
        if (ImageId <= 0) return BadRequest("the image id is <= 0");

        ImageDTO? imageDTO = ImageProcces.GetImageById(ImageId);

        if (imageDTO == null) return NotFound("the Data is not Found !!");

        return Ok(imageDTO);

    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteByID", Name = "DeleteByID")]
    public IActionResult DeleteByID(int ImageId)
    {

        if (ImageId <= 0) return BadRequest("the image id is <= 0");

        ImageDTO? imageDTO = ImageProcces.GetImageById(ImageId);

        if (imageDTO is null) return NotFound("This image is not found in Data base !");

        if (ImageProcces.Delete(ImageId))
            return Ok("The Image is Deleted Succesfully");
        else return StatusCode(500, "Internal Server Error Is Happen !");

    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateByID", Name = "UpdateByID")]
    public IActionResult UpdateByID(int ImageId, [FromBody] ImageDTO imageDTO)
    {

        if (imageDTO is null) return BadRequest("Should the ImageDto not null");

        if (ImageProcces.GetImageById(ImageId) is null) 
            return NotFound("This image is not found in Data base !");

        if (ImageProcces.Update(ImageId, imageDTO)) 
            return Ok("The Image is Updated Succesfully");
        else 
            return StatusCode(500, "Internal Server Error Is Happen !");

    }

}
