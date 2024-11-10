# SmartPulseTask

## Project Overview
This project, **SmartPulseTask**, is an ASP.NET Core MVC application designed to interact with EPİAŞ's API to fetch and process transaction data. It then calculates aggregated metrics such as total transaction amount, total transaction quantity, and weighted average price, grouping them by contract name and displaying them in a web interface.

## Project Structure and Architecture

The project adheres to a layered architecture, separating concerns across Controllers, Services, Entities, and Helpers.

### 1. Controllers
- **HomeController**: Handles error page navigation and displays error messages.
- **TransactionController**: Manages the core functionality by receiving parameters for date range and sorting. It calls services to fetch and process transaction data, then renders it in a view.

### 2. Services
Two services define the business logic and API interaction:
- **TransactionApiService**: Communicates with the EPİAŞ API. It authenticates using credentials, fetches transaction data, and handles errors.
- **TransactionCalculationService**: Processes raw transaction data by grouping records based on the contract name and calculating:
  - **Total Transaction Amount**: Calculated as the sum of \((price * quantity) / 10\) for each transaction within a contract.
  - **Total Transaction Quantity**: Calculated as the sum of \((quantity) / 10\) for each transaction within a contract.
  - **Weighted Average Price**: Determined by dividing the total transaction amount by the total transaction quantity.

### 3. Entities
Defines the data structure for objects used within the application:
- **TransactionHistoryResponse**: Wraps a list of transaction data items fetched from the API.
- **TransactionHistoryGipDataDto**: Represents individual transaction items.
- **TransactionResult**: Stores aggregated transaction metrics per contract.
- **TransactionViewModel**: Manages data for the view, including transactions, date range, and sorting options.

### 4. Helpers
- **ContractDateParser**: Extracts date and time information from the contract name to parse into a `DateTime` format.

### Configuration
The application settings (`appsettings.json`) store API configuration details such as credentials and URLs. These are injected into `TransactionApiService` for authentication and data fetching.

## Patterns and Principles Used

- **Dependency Injection (DI)**: Used extensively to manage dependencies between services and controllers, promoting testability and maintainability.
- **Repository Pattern**: Though not explicitly, a similar principle is applied by separating data access logic (`TransactionApiService`) from business logic (`TransactionCalculationService`).
- **Exception Handling**: Both services employ try-catch blocks to capture and log errors, ensuring that exceptions are managed and meaningful error messages are displayed.

## Views

- **Index View**: Displays a table of transaction results, allowing users to filter by date and sort by various fields. This view leverages Bootstrap for responsive design.
- **Error View**: Displays error messages when issues arise during API interaction or data processing.

## Technologies Used
- **ASP.NET Core MVC**: For building the web application and handling the MVC architecture.
- **Bootstrap**: For styling and responsive design.
- **Newtonsoft.Json**: For JSON parsing.
- **Microsoft.Extensions.Logging**: For logging application events.

## API Integration Details

### Authentication
The application uses the EPİAŞ API for transaction data. Authentication is handled by obtaining a ticket-granting ticket (TGT) using credentials stored in `appsettings.json`.

### Data Fetching
The `TransactionApiService` sends a POST request to fetch transaction data within a specified date range, handling response deserialization and error logging.

## Running the Project

To run this project:
1. Clone the repository.
2. Ensure your EPİAŞ API credentials are set in `appsettings.json`.
3. Start the application and navigate to `/Transaction/Index` to access the transaction history interface.

This project provides a robust starting point for integrating with EPİAŞ's transaction API, offering core features like data aggregation, error handling, and dynamic data presentation.
