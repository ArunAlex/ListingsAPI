# Create a User’s REST API server

Objective: Create a simple RESTful API using .NET that can be used to perform CRUD
operations for users' data.

## Table of Contents

- [UML](#uml diagram)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## UML Diagram

Based on the ERD provided, the Users and SavedListings need to be modeled in a way that reflects the following relationships:

- A User can have zero or more Listings.
- Zero or more Users can save a Listing.

Here’s how the UML diagram could be structured:
+-------------+        +-------------------+        +------------+
|   Users     |        |   SavedListings   |        |   Listings |
+-------------+        +-------------------+        +------------+
| id (PK)     |1     * | user_id (PK,FK)   | *    1 | id (PK)    |
| name        |--------| listing_id (PK,FK)|--------| address    |
| email       |        | saved_at          |        | suburb     |
| passwordhash|        +-------------------+        | state      |
| created_at  |                                     | postcode   |
+-------------+                                     +------------+

- Users to SavedListings: One user can save many listings (1 to many).
- Listings to SavedListings: One listing can be saved by many users (1 to many).

## Installation

Instructions on how to install and set up the project.

## Usage

Instructions on how to use the project, including any examples or screenshots.

## Contributing

Guidelines for contributing to the project, including how to report issues and submit pull requests.

## License

Information about the project's license and any additional terms or conditions.
