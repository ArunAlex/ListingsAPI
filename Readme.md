# Create a User’s REST API server

Objective: Create a simple RESTful API using .NET that can be used to perform CRUD
operations for users' data.

## Table of Contents

- [UML](#uml-diagram)
- [Installation](#installation)
- [Running Unit Tests](#running-unit-tests)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## UML Diagram

Based on the ERD provided, the Users and SavedListings need to be modeled in a way that reflects the following relationships:

- A User can have zero or more Listings.
- Zero or more Users can save a Listing.

Here’s how the UML diagram can be structured:

```sh
+-------------+        +-------------------+        +------------+
|   Users     |        |   SavedListings   |        |   Listings |
+-------------+        +-------------------+        +------------+
| id (PK)     |1     * | user_id (PK,FK)   | *    1 | id (PK)    |
| name        |--------| listing_id (PK,FK)|--------| address    |
| email       |        | saved_at          |        | suburb     |
| passwordhash|        +-------------------+        | state      |
| created_at  |                                     | postcode   |
+-------------+                                     +------------+
```

The diagram depicts the following:
- A User can save zero or more Listings: This indicates a one-to-many relationship between User and SavedListing (the join table).
- A Listing can be saved by zero or more Users: This also indicates a one-to-many relationship between
Listing and SavedListing.
- SavedListing acts as the join table for the many-to-many relationship between Users and Listings.

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [SQLite](https://www.sqlite.org/download.html)
- [Git](https://git-scm.com/)

### Steps
Run the following steps using the command line prompt on windows or terminal on mac. 
Before performing the steps, set up the project directory and change to its root.

1. **Clone the repository:**

   ```sh
   git clone https://github.com/ArunAlex/ListingsAPI.git
   ```
   
2. **Navigate to the project directory:**

   ```sh
   cd ListingsAPI
   ```
   
3. **Restore the .NET dependencies:**

   ```sh
   dotnet restore
   ```
   
4. **Set up the SQLite database:**

   - Update the connection string in appsettings.json to use SQLite. It should look something like this:
      ```sh
      "ConnectionStrings": {
          "DefaultConnection": "Data Source=app.db"
      }
      ```
   - Apply migrations to create the database schema:
     ```sh
     dotnet ef database update --startup-project Listings.API
     ```
     Note: Refer https://www.nuget.org/packages/dotnet-ef to install dotnet-ef command
     
 5. **Run the application:**

    ```sh
    dotnet run --project Listings.API
    ```
 6. **Access the API:**

    Open your browser and navigate to http://localhost:5111/Swagger/index.html or https://localhost:7225/Swagger/index.html to access the API endpoints and test them.

## Running Unit Tests

1. **Navigate to the project directory:**

   ```sh
   cd ListingsAPI

2. **Run the unit tests:**

   ```sh
   dotnet test

## Usage

Another options to test the APIs is by using Postman. With the help of the Swagger Doc, we can enter the api url as below and execute them
- POST API
![image](https://github.com/user-attachments/assets/276d5f8a-e20f-404d-b24e-9358a5ec8aec)

- GET API
![image](https://github.com/user-attachments/assets/d9b2043b-a96d-4b85-9dbb-6f09543b64fe)

## Contributing

Guidelines for contributing to the project, including how to report issues and submit pull requests.

## License

Information about the project's license and any additional terms or conditions.
