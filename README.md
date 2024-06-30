



# Memory Game
---
## Project Description
This project is a memory game developed in C# using Windows Forms. The game consists of multiple levels, each with an increasing number of icons to match. Game data is stored in an SQLite database, including player information and game statistics.

## Project Structure
The project is organized into various forms (Windows Forms) and helper classes to interact with the database. Below is a description of each form and its interactions with the database.

### Main Forms

#### Registro.cs
- **Description:** Allows users to enter their name to register as players and start the game.
- **Functionalities:**
  - Captures and validates the player's name.
  - Inserts the player into the database.
  - Starts the game form (`Form1`) with the registered player's ID.


### Forms.cs (1 - 4 ) (Game)
- **Description:** Manages the logic of the memory game across all levels.
- **Functionalities:**
  - Handles click events on game icons dynamically based on level.
  - Verifies pairs and updates the game state accordingly.
  - Records score and attempts in the database specific to each level.
  - Transitions to the next level upon completion of current level's objectives.


#### Estadisticas.cs
- **Description:** Displays player statistics by level.
- **Functionalities:**
  - Loads and displays player results in a `DataGridView`.
  - Interacts with the database to fetch statistics.

## Working Methodology
We implemented agile methodology to optimize our work process and ensure the correct and functional delivery of the game. We adopted an iterative approach, organizing work into sprints lasting two weeks.

### Sprints
- **Sprint 1:** Focus on initial project setup, database design, and registration form development.
- **Sprint 2:** Game logic development, event handling, and statistics form design.

## Database
We use SQLite to store game data. The database includes the following tables:
- **Player:** Stores player information.
- **Level:** Records scores and attempts per level for each player.

## Instructions to Run the Project
1. Clone the repository.
   ```bash
   git clone <repository_url>
   ```
2. Open the project in Visual Studio.
3. Configure the SQLite database connection string.
4. Build and run the project.

## System Requirements
- Windows 10 or higher.
- .NET Framework 4.7.2 or higher.
- SQLite.

## Contributions
Contributions are welcome. To contribute, please follow these steps:
1. Fork the project.
2. Create a new branch (`git checkout -b feature/new-feature`).
3. Make your changes and commit (`git commit -am 'Add new feature'`).
4. Push the branch (`git push origin feature/new-feature`).
5. Open a Pull Request.

## Demo

https://www.loom.com/share/f6d65694db0c4adcba9b1a188979910e?sid=38ddd2a1-fbfb-4ec4-bd5a-44fb24e77c8a

---
