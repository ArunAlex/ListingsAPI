# Create a User’s REST API server

Objective: Create a simple RESTful API using .NET that can be used to perform CRUD
operations for users' data.

## Table of Contents

- [UML](#uml-diagram)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## UML Diagram

Based on the ERD provided, the Users and SavedListings need to be modeled in a way that reflects the following relationships:

- A User can have zero or more Listings.
- Zero or more Users can save a Listing.

Here’s how the UML diagram can be structured:

![Screenshot 2024-10-07 at 1 00 06 pm](https://github.com/user-attachments/assets/19ca0463-e084-4fab-a308-e15851c5ae8f)

The diagram depicts the following:
- A User can save zero or more Listings: This indicates a one-to-many relationship between User and SavedListing (the join table).
- A Listing can be saved by zero or more Users: This also indicates a one-to-many relationship between
Listing and SavedListing.
- SavedListing acts as the join table for the many-to-many relationship between Users and Listings.

## Installation

Instructions on how to install and set up the project.

## Usage

Instructions on how to use the project, including any examples or screenshots.

## Contributing

Guidelines for contributing to the project, including how to report issues and submit pull requests.

## License

Information about the project's license and any additional terms or conditions.
