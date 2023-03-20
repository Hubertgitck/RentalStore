# Rental Company

RentalCompany is a project i made in less than two weeks for a recruitment process. The task was to create a web app for a client who owns a rental company in Mallorca for renting Tesla cars.The technologies i uesed were .Net, .Net MVC, Entity Framework, Razor Pages, jQuery, bootstrap. The database I used is MS SQL.

<h3>The Features implemented are listed below:</h3>

- Admin control panel that allows processing orders and creating users with either employee or administrator privileges [admin]

- Order management panel that allows listing all orders and managing them [admin and employee]

- Product management panel that allows adding new cars and editing existing ones [admin and employee]

- Rental store management panel that allows editing stores related data like available cars, contact data [admin and employee]

- Users management is done with ASP Identity with a few tweaks, external login using Facebook

- Email system, after registration, sucesfull payment or password change, customer is getting an e-mail with relevant data

- payment processing using Stripe, if you want to test the payments, please use below data:<br>
email: test@test.com<br>
card number:  4242 4242 4242 4242<br>
MM / RR : 12/34 ; CVC: 123<br>
name: test<br>

<h3>Brief description</h3>

The user is available to choose from selected store's stock a tesla model which suits the best for his needs. After choosing a car, a booking window appears where he is able to chose from available days. Booking is possible for 3 months in advance (easly customizable). he calendar was implemented using jQuery datepicker and a custom function for highlighting dates, alerting the user if an unavailable day was selected, etc. Unavailable days are retrieved from the database based on the selected car and rental store, then they are grayed out. The customer can choose to return the rental car to any available store in the company. After clicking the "Book" button, the car is reserved, and the customer can proceed to payment or pay later. Booking data is available for updates before payment is done and when the order is not in "cancelled" or "refunded" status. If customer changes their mind, they may cancel the order and selected car is released. 

The payment system currently uses Stripe only, but thanks to the strategy pattern used when implementing the payment service, it will be easy to add new options in the future.

Below, you can find the database design. All of the tables were implemented in Entity Framework with a code-first approach. Data-related services are in the Infrastructure layer. I have implemented the unit of work and repository patterns for easier management and cleaner code.

![alt text](https://github.com/Hubertgitck/RentalStore/blob/release/database.png?raw=true)


Domain layer is based on services for each area. 

I provided a database initializer to make testing easier. The application is started with 4 Tesla models added, 4 rental stores, but only for 2 of them are there selected cars. If you want to check the rest, you will have to do it manually through the management panel. I also provided 2 test users.

If you want to test the admin panel, use the following data: 
admin@rental.com
Admin123*

For the customer stuff:
customer@domain.com
Test123*
