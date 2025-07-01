# ASP.NET Web Forms Registration System

A complete user registration and management system built using **ASP.NET Web Forms**, **C#**, and **SQL Server** with secure password handling and clean UI.

---

## ✨ Features

✔ User Registration with Name, Surname, Date of Birth, Email, and Password
✔ Passwords securely hashed using SHA256
✔ Required field validation with visible `*` indicators for empty fields
✔ GridView displays all registered users
✔ Edit functionality moves user details to form fields
✔ Password check required before updating data
✔ Delete functionality with confirmation popup for safety
✔ All database operations handled via a single Stored Procedure
✔ Clean, centered layout with responsive design

---

## 🗂️ Technologies Used

* ASP.NET Web Forms (C#)
* SQL Server (localdb)
* ADO.NET for database connectivity
* SQL Stored Procedure for secure operations
* SHA256 for password encryption
* Visual Studio IDE
* HTML & CSS for layout

---

## ⚙️ Setup Instructions

1. **Clone the Repository**

   ```bash
   git clone https://github.com/Biraj007/registration-form.git
   ```

2. **Open the Solution**
   Launch the `.sln` file in Visual Studio.

3. **Configure Database Connection**
   Update the connection string in `Registration.aspx.cs`:

   ```csharp
   string conString = "Data Source=(localdb)\\mylocodb;Initial Catalog=Registration;Integrated Security=True";
   ```

4. **Prepare the Database**

   * Run SQL commands to create tables:

     ```sql
     CREATE TABLE Reg_Table (
         id INT PRIMARY KEY IDENTITY(1,1),
         Name NVARCHAR(50),
         Surname NVARCHAR(50),
         DOB DATE,
         Email NVARCHAR(100)
     );

     CREATE TABLE Login_Details (
         login_id INT PRIMARY KEY IDENTITY(1,1),
         Email NVARCHAR(100) FOREIGN KEY REFERENCES Reg_Table(Email),
         Password NVARCHAR(200)
     );
     ```

   * Run the final Stored Procedure:

     ```sql
     CREATE PROCEDURE ManageUser
         @Action NVARCHAR(20),
         @id INT = NULL,
         @Name NVARCHAR(50) = NULL,
         @Surname NVARCHAR(50) = NULL,
         @DOB DATE = NULL,
         @Email NVARCHAR(100) = NULL,
         @Password NVARCHAR(200) = NULL
     AS
     BEGIN
         IF @Action = 'INSERT'
         BEGIN
             INSERT INTO Reg_Table (Name, Surname, DOB, Email) VALUES (@Name, @Surname, @DOB, @Email)
             INSERT INTO Login_Details (Email, Password) VALUES (@Email, @Password)
         END

         IF @Action = 'UPDATE'
         BEGIN
             UPDATE Reg_Table SET Name=@Name, Surname=@Surname, DOB=@DOB, Email=@Email WHERE id=@id
         END

         IF @Action = 'DELETE'
         BEGIN
             DELETE FROM Login_Details WHERE Email=(SELECT Email FROM Reg_Table WHERE id=@id)
             DELETE FROM Reg_Table WHERE id=@id
         END

         IF @Action = 'SELECT'
         BEGIN
             SELECT id, Name, Surname, DOB, Email FROM Reg_Table
         END

         IF @Action = 'GETONE'
         BEGIN
             SELECT id, Name, Surname, DOB, Email FROM Reg_Table WHERE id=@id
         END

         IF @Action = 'GETPASS'
         BEGIN
             SELECT Password FROM Login_Details WHERE Email=@Email
         END
     END
     ```

5. **Run the Project**
   Build and run the project to access your Registration System.

---

## 💡 How It Works

✅ Form requires all fields to be filled; `*` shows beside empty fields
✅ Password and Confirm Password must match during registration
✅ Passwords are hashed with SHA256 before storage
✅ Edit button moves data to form fields; Update allowed only with correct password
✅ Delete button shows confirmation popup
✅ Grid updates in real-time after every operation

---

## 📄 License

This project is for learning and demonstration purposes. Feel free to modify, enhance, or use parts of this project.
