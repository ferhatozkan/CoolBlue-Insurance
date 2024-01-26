# CoolBlue Software Development Assignment

It includes the assignment done for the case created by CoolBlue. CoolBlue has written some of the code before. They have provided us the external Product API that we can use. The cases CoolBlue wants are as follows:

1.  **[BUGFIX]** The financial manager reported that when customers buy a laptop that costs less than € 500, insurance is not calculated, while it should be € 500.
2.  **[REFACTORING]** It looks like the already implemented functionality has some quality issues. Refactor that code, but be sure to maintain the same behavior.
3.  **[FEATURE 1]** Now we want to calculate the insurance cost for an order and for this, we are going to provide all the products that are in a shopping cart.
4.  **[FEATURE 2]** We want to change the logic around the insurance calculation. We received a report from our business analysts that digital cameras are getting lost more than usual. Therefore, if an order has one or more digital cameras, add € 500 to the insured value of the order.
5.  **[FEATURE 3]** As a part of this story we need to provide the administrators/back office staff with a new endpoint that will allow them to upload surcharge rates per product type. This surcharge will then need to be added to the overall insurance value for the product type.

:writing_hand: Feel free to add new surcharge rules since at the beggining there are no rules defined. Do not worry the calculations are also working without surcharge rules.

## GitHub Task Management: Issues and Pull Requests

I have created GitHub issues for each task with proper labels. Feel free to look at those tasks, as well as the pull requests that close those issues. I have provided clear descriptions and shared my comments on the pull request comments.

## Installation
1. Clone the repository
2. Make sure you are on the main branch and check out if necessary
3. Open the solution in Visual Studio 2022
4. Set the right *"Product Information API"* url in appsettings.json
5. And start the project! It will open a browser window displaying the Swagger UI

### Option 2:

1. Clone the repository
2. Make sure you are on the main branch and check out if necessary
3. Open terminal on directory
4. Run code below to run integration tests

```
docker-compose up integration-tests
```

5. This will also start Insurance Api (http://localhost:8081) and Product Api (http://localhost:8080)

## Coverage
To see the coverage resuls for the project's unit tests, follow these steps:
- Open Terminal at Insurance.Tests folder
- Run the command below
```dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=lcov.info```
- The coverage result table will appear on the terminal.

  
  

## Used Technologies
- FluentValidation
- Entity Framework Core InMemory
- Moq
- Swashbuckle
- Xunit

  

## Technical Structures
- Used chain of responsibility pattern for applying insurence cost by product calculation rules
- Used validator for request classes with FluentValidation
- Created API documentation with Swagger
- Created a filter for exception handling and used custom exception types
- Used Microsoft Logging library to write output logs
- Applied SOLID principles

  
## Assumptions
1. "Laptop" product type has been added twice. As the document emphasizes the need to add insurance to Laptop products, we accepted the one with the ID 21 as correct. The product type with the ID 841 has been incorrectly added. The data needs to be corrected to ensure uniqueness based on the Name column in the table. Thats why the operations have been done with the id field and not the name field.
2. Insurance Sales Price Ranges must be defined as unique and non-overlapping
3. For a product type, there is one surcharge rate. The surcharge controller does not allow the addition of two surcharge rates for the same product type

  
## Architectural Brainstorming
I observed that my class, which performs calculations based on the features given in the case, expands with each new feature. I noticed that I added new 'if' statements for each case. Therefore, I defined the cases that will be included in this calculation as rules. I ran these rules in a chain by arranging them in a logical order. In this chain logic, the rules are derived from a common abstraction. Each one operates with the same method but with different calculation implementations, impacting the result:

-  **[CHAIN 1]** Calculation based on the status of the canBeInsured field for the product type.
-  **[CHAIN 2]** Calculation based on the sale price range rules.
-  **[CHAIN 3]** Calculation based on specific types (such as smartphones or laptops).
-  **[CHAIN 4]** Calculation performed when there is a surcharge for a product.

[OUT OF CHAIN] In addition to these rules, an extra insurance is added for frequently lost items in the cart (only cart case not a product case), such as Digital Cameras. This case is for shopping carts only. If the requirement had more cases for cart based insurance calculations additional design patterns could have been applied. (Good Enough Solution)