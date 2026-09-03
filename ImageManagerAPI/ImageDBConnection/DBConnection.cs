
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ImageDBConnection
{
    public static class DBConnection
    {

        private static string ConnectionString
        {
            get
            {
                return "Server=.;Database=ImagesDB;Trusted_Connection=True;TrustServerCertificate=True;";
            }
        }

        public static int? AddImage(ImageDTO imageDTO)
        {

            int? NewImageId = null;

            if(imageDTO == null)  return null;

            string? ConnectionString = DBConnection.ConnectionString;
            if (ConnectionString is null) return null;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                string? query =
                    @"
                        INSERT INTO Images (ImageName, ForPerson, ExtentionFile, ImageBytes)
                        VALUES (@ImageName, @ForPerson, @ExtentionFile, @ImageBytes);  
                        select SCOPE_IDENTITY();
                    ";

                if (query is null) return null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                   
                    //@ImageName, @ForPerson, @ImageBytes
                    command.Parameters.AddWithValue("@ImageName", imageDTO.ImageName);
                    command.Parameters.AddWithValue("@ForPerson", imageDTO.ForPerson);
                    command.Parameters.AddWithValue("@ExtentionFile", imageDTO.ExtentionFile);
                    command.Parameters.AddWithValue("@ImageBytes", imageDTO.ImageByts);

                    connection.Open();

                    object Result = command.ExecuteScalar();

                    if (Result != null) NewImageId = Convert.ToInt32(Result);
                    
                }

            }

            return NewImageId;

        }

        public static ImageDTO? GetImageById(int imageId)
        {

            if(imageId <= 0) return null;

            ImageDTO? imageDTO = null; 

            string ? ConnectionString = DBConnection.ConnectionString;
            if (ConnectionString is null) return null;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                string? query =
                    @"
                        select * 
                        from Images 
                        where Images.ImageID = @ImageID;
                    ";

                if (query is null) return null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@ImageID", imageId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imageDTO = new ImageDTO
                            (
                                Convert.ToInt32(reader["ImageID"]),
                                reader["ImageName"] as string,
                                reader["ForPerson"] as string,
                                reader["ImageBytes"] as byte[],
                                reader["ExtentionFile"] as string
                            );
                        }
                    }
                }
            }

            return imageDTO;

        }

        public static bool Delete(int imageId)
        {

            if (imageId <= 0) return false;

            bool isDelete = false;

            string? ConnectionString = DBConnection.ConnectionString;
            if (ConnectionString is null) return false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                string? query =
                    @"
                        DELETE FROM Images
                        WHERE ImageID = @ImageID; 
                    ";

                if (query is null) return false;

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@ImageID", imageId);

                    connection.Open();

                    isDelete = command.ExecuteNonQuery() == 1;

                }

                return isDelete;

            }
        }

        public static bool Update(int ImageId, ImageDTO imageDTO)
        {

            bool IsUpdated = false;

            if (imageDTO == null) return false;

            string? ConnectionString = DBConnection.ConnectionString;
            if (ConnectionString is null) return false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                string? query =
                    @"

                        UPDATE Images
                        SET
                            ImageName = @ImageName,
                            ForPerson = CASE
                                            WHEN @ForPerson <> 'UPDATE_InFo' THEN @ForPerson
                                            ELSE ForPerson
                                        END,
                            ExtentionFile = @ExtentionFile,
                            ImageBytes = @ImageBytes
                        WHERE ImageID = @ImageID;   

                    ";

                if (query is null) return false;

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    //@ImageName, @ForPerson, @ImageBytes
                    command.Parameters.AddWithValue("@ImageName", imageDTO.ImageName);
                    command.Parameters.AddWithValue("@ForPerson", imageDTO.ForPerson);
                    command.Parameters.AddWithValue("@ExtentionFile", imageDTO.ExtentionFile);
                    command.Parameters.AddWithValue("@ImageBytes", imageDTO.ImageByts);
                    command.Parameters.AddWithValue("@ImageID", ImageId);

                    connection.Open();

                    IsUpdated = command.ExecuteNonQuery() == 1;

                }

            }

            return IsUpdated;

        }

    }

}
