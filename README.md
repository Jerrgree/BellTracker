# Instructions

## To run
1. Clone this git repository
2. Create a `secret.json` file in the main BellTracker application folow.
3. In the `secret.json` file, add the key "BellContext" with a connection string pointing to an accessible SQL Server Instance
4. In the package manager console, create an Entity Framework migration by typing `add-migration <migrationName>` to stage your new database
5. In the package manager console, type `update-database` to publish your database.
6. You can now run the project by running the BellTracker application
