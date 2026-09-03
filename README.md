# Image Management System

A .NET 8 image management system that provides a Web API for storing and managing images in SQL Server, together with a Windows Console Client for interacting with the API.

The project demonstrates a layered architecture, RESTful API communication, SQL Server database access, image upload/download operations, and asynchronous HTTP communication between a client and server.

## 📌 Overview

The system allows users to:

* Upload images to the database
* Store image metadata
* Retrieve images by ID
* Delete images
* Update existing images
* Display stored images on the client machine
* Upload images to an external folder
* Communicate with the Web API through a Console Client

The project is divided into separate components to keep database access, image processing, API endpoints, and client functionality organized.

---

## 🏗️ Architecture

```text
                    ┌──────────────────────────┐
                    │   ImageManagementClient  │
                    │     Windows Console      │
                    └────────────┬─────────────┘
                                 │
                                 │ HTTP / JSON
                                 ▼
                    ┌──────────────────────────┐
                    │       Image Web API      │
                    │    ASP.NET Core .NET 8    │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │   ImageProccesLayer      │
                    │   Business / Processing   │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │     ImageDBConnection    │
                    │   SQL Server Data Access │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │        SQL Server        │
                    │        ImagesDB           │
                    └──────────────────────────┘
```

### Main Components

#### `ImageManagementClient`

Windows Console Client responsible for interacting with the API.

Main functionality includes:

* Displaying the application menu
* Selecting image files
* Sending HTTP requests
* Getting image information
* Adding images
* Updating images
* Deleting images
* Opening downloaded images

#### `PostGetImage`

ASP.NET Core Web API responsible for exposing HTTP endpoints for image operations.

The API uses controllers and Swagger/OpenAPI for development and testing.

#### `ImageProccesLayer`

Processing/business layer between the API and database layer.

It provides operations such as:

* Add image
* Get image
* Update image
* Delete image

#### `ImageDBConnection`

Database access layer responsible for communicating with SQL Server using `Microsoft.Data.SqlClient`.

It performs:

* `INSERT`
* `SELECT`
* `UPDATE`
* `DELETE`

operations against the `Images` table.

---

## ✨ Features

### Image Upload

Images can be selected from the client machine using a Windows file picker.

Supported formats currently include:

```text
.jpg
.jpeg
.png
.bmp
.gif
```

The image is converted to a byte array and can be stored in SQL Server.

### Image Metadata

The system stores information including:

* Image ID
* Image name
* Person associated with the image
* File extension
* Image bytes

Example model:

```csharp
public class ImageDTO
{
    public int ImageId { get; private set; }
    public string? ImageName { get; set; }
    public string? ExtentionFile { get; set; }
    public string? ForPerson { get; set; }
    public byte[]? ImageByts { get; set; }
}
```

### Image Retrieval

An image can be retrieved using its database ID.

The Console Client can then save the returned image bytes to a temporary file and open the image using the operating system's default image application.

### Image Deletion

Images can be deleted by their `ImageID`.

### Image Update

Existing image information and image bytes can be updated using the image ID.

### External Folder Upload

The API also supports uploading an image directly to an external folder on the server.

---

## 🛠️ Technologies

* **C#**
* **.NET 8**
* **ASP.NET Core Web API**
* **SQL Server**
* **Microsoft.Data.SqlClient**
* **HttpClient**
* **System.Text.Json**
* **Swagger / OpenAPI**
* **Windows Forms OpenFileDialog**
* **REST API**
* **Visual Studio**

---

## 📁 Project Structure

```text
ImageManagementSystem/
│
├── ImageManagementClient/
│   ├── FilePicker.cs
│   ├── ImageStrucher.cs
│   ├── ImageUtility.cs
│   ├── Program.cs
│   ├── Start.cs
│   └── ConsoleApp1.csproj
│
└── ImageManagerAPI/
    │
    ├── ImageDBConnection/
    │   ├── DBConnection.cs
    │   ├── ImageDTO.cs
    │   └── ImageDBConnection.csproj
    │
    ├── ImageProccesLayer/
    │   ├── ImageProccesLayer.cs
    │   ├── ModeImage.cs
    │   └── ImageProccesLayer.csproj
    │
    └── PostGetImage/
        ├── Controllers/
        │   └── PostGetImage.cs
        ├── Models/
        │   └── modFileAndDirecrty.cs
        ├── Program.cs
        └── PostGetImage.csproj
```

> Build output folders such as `bin`, `obj`, and Visual Studio `.vs` files should normally be excluded from the Git repository using `.gitignore`.

---

# 🔌 API Endpoints

The API is currently hosted during development at:

```text
https://localhost:7028
```

## Upload Image to Database

```http
POST /api/ImageUpload/UploadImageToDataBase_0
```

Parameters:

```text
IFormFile imageFile
string forPerson
```

This endpoint receives an image file and the person associated with the image.

---

## Upload Image DTO to Database

```http
POST /api/ImageUpload/UploadImageToDataBase_1
```

Receives an `ImageDTO` object.

Example JSON:

```json
{
  "imageId": 0,
  "imageName": "profile.jpg",
  "forPerson": "Ahmed",
  "imageByts": "...",
  "extentionFile": ".jpg"
}
```

> When sending the `byte[]` property as JSON, the byte array is represented using Base64 encoding.

---

## Get Image by ID

```http
GET /api/ImageUpload/GetImageById?ImageId={id}
```

Example:

```http
GET /api/ImageUpload/GetImageById?ImageId=1
```

Returns the image information and image bytes when the image exists.

---

## Delete Image

```http
DELETE /api/ImageUpload/DeleteByID?ImageId={id}
```

Example:

```http
DELETE /api/ImageUpload/DeleteByID?ImageId=1
```

---

## Update Image

```http
PUT /api/ImageUpload/UpdateByID?ImageId={id}
```

The endpoint accepts an `ImageDTO` in the request body.

---

## Upload Image to External Folder

```http
POST /api/ImageUpload/UploadImageToExternalFolder
```

Parameters:

```text
IFormFile imageFile
string destinationFolder
```

The server generates a unique filename using `Guid` before saving the image.

---

# 🗄️ Database

The project uses SQL Server with a database named:

```text
ImagesDB
```

The main table is:

```text
Images
```

The project currently expects the following connection string:

```text
Server=.;Database=ImagesDB;Trusted_Connection=True;TrustServerCertificate=True;
```

### Images Table

The application expects columns similar to:

```text
ImageID
ImageName
ForPerson
ExtentionFile
ImageBytes
```

A simplified SQL definition could be:

```sql
CREATE TABLE Images
(
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ImageName NVARCHAR(255) NULL,
    ForPerson NVARCHAR(255) NULL,
    ExtentionFile NVARCHAR(20) NULL,
    ImageBytes VARBINARY(MAX) NULL
);
```

---

# ⚙️ Installation

## 1. Clone the Repository

```bash
git clone <YOUR_REPOSITORY_URL>
cd <YOUR_REPOSITORY_FOLDER>
```

## 2. Requirements

Make sure the following are installed:

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 or another compatible .NET IDE
* Windows operating system for the Console Client's file-picker functionality

---

## 3. Create the Database

Create a SQL Server database:

```text
ImagesDB
```

Then create the `Images` table using the schema described above.

---

## 4. Configure the Connection String

Update the connection string in:

```text
ImageDBConnection/DBConnection.cs
```

For example:

```csharp
Server=.;
Database=ImagesDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

For a real application, the connection string should be moved to configuration such as `appsettings.json` or environment variables rather than hard-coded in the source code.

---

# ▶️ Running the Project

## Start the API

Open the solution containing the API projects and start the ASP.NET Core Web API project.

The API will run on the configured HTTPS address, for example:

```text
https://localhost:7028
```

Swagger can be used during development to test the endpoints.

## Start the Console Client

After the API is running, start:

```text
ImageManagementClient
```

The console application provides a menu similar to:

```text
╔══════════════════════════════════════════════╗
║              IMAGE MANAGEMENT                ║
╠══════════════════════════════════════════════╣
║                                              ║
║  [0]  Exit                                   ║
║  [1]  Get image information by ID            ║
║  [2]  Delete image by ID                     ║
║  [3]  Add image to database                  ║
║  [4]  Update image In database               ║
║  [5]  Show image from database by ID         ║
║                                              ║
╚══════════════════════════════════════════════╝
```

---

# 🔄 Client Workflow

A typical image upload workflow is:

```text
User
 │
 │ Select image
 ▼
FilePicker
 │
 │ Read image bytes
 ▼
ImageStrucher
 │
 │ HTTP request
 ▼
Image Web API
 │
 ▼
ImageProccesLayer
 │
 ▼
ImageDBConnection
 │
 ▼
SQL Server
```

For retrieving an image:

```text
SQL Server
     │
     ▼
ImageDBConnection
     │
     ▼
ImageProccesLayer
     │
     ▼
Web API
     │
     ▼
Console Client
     │
     ▼
Temporary File
     │
     ▼
Default Image Viewer
```

---

# 🧪 Example

### Add an image

1. Start the Web API.
2. Start the Console Client.
3. Select:

```text
[3] Add image to database
```

4. Enter the person's name.
5. Select an image file.
6. The client sends the image to the API.
7. The API stores the image in SQL Server.
8. The database-generated image ID is returned.

---

# 🔐 Security Notes

This project is currently intended as a learning/development project.

Before using it in production, consider implementing:

* Authentication
* Authorization
* Input validation
* File size limits
* File type/content validation
* Secure connection-string management
* Centralized exception handling
* Logging
* HTTPS certificate configuration
* Protection against malicious file uploads
* Proper API versioning
* Dependency Injection
* Configuration through `appsettings.json`
* Async database operations

In particular, avoid storing database credentials or connection strings directly in source code.

---

# 🚀 Possible Future Improvements

Planned or recommended improvements include:

* [ ] Rename classes and projects using consistent C# naming conventions
* [ ] Move the database connection string to configuration
* [ ] Use Dependency Injection
* [ ] Convert database methods to asynchronous operations
* [ ] Add authentication and authorization
* [ ] Add image validation
* [ ] Add maximum upload-size validation
* [ ] Improve API response models
* [ ] Use proper HTTP status codes consistently
* [ ] Add global exception handling
* [ ] Add logging
* [ ] Add automated tests
* [ ] Improve DTO naming
* [ ] Separate controllers, services, repositories, and DTOs more clearly
* [ ] Add API versioning
* [ ] Add pagination/search functionality for images

---

# 📚 Purpose

This project was developed as a practical exercise in building a multi-layered .NET application and understanding communication between:

```text
Console Client
       ↓
ASP.NET Core Web API
       ↓
Business / Processing Layer
       ↓
Database Access Layer
       ↓
SQL Server
```

It demonstrates how images can be transferred between a client and server, processed by an API, and persisted as binary data in SQL Server.

---

# 👤 Author

Developed as a C# / .NET learning project.

---

# 📄 License

This project is available for educational and personal development purposes.

If you intend to use or distribute the project commercially, add an appropriate open-source license such as MIT before publishing it.
