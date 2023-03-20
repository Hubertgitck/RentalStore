# RentalCompany

RentalCompany is a project i made in less then two weeks for a recrutation process. The task was to create web app for a client who owns rental company in Mallorca for renting Tesla cars. Technologies i uesed were .Net, .Net MVC, Entity Framework, Razor Pages, jQuery. Database i used is MS SQL.

<h3>Features implemented are listed below:</h3>

- admin control panel that allows processing orders and creating users with either employee or administrator privileges [admin]

- order management panel that allows listing all orders and managing them [admin and employee]

- product management panel that allows adding new Cars and editing existing ones [admin and employee]

- rental store management panel that allows editing stores related data like available cars, contact data [admin and employee]

- users management is done with ASP Identity with few tweaks, external login using Facebook

- payment processing using Stripe, if you want to test the payments, please use below data:<br>
email: test@test.com<br>
card number:  4242 4242 4242 4242<br>
MM / RR : 12/34 ; CVC: 123<br>
name: test<br>

<h3>Brief description</h3>

User is available to choose from selected store's stock a tesla model which suits the best for his needs. After chosing a car, booking window appears where he is able to chose from available days. Booking is possible for 3 months in advance (easly customizable). Calendar was implemented using jQuery datepicker and custom function for highlighting dates, alerting user if unavailable day was selected etc. Unavailable days are retrieved from database based on selected car and rental store, then they are grayed out. Customer can chose return rental store from all available stores in company. After clicking the "Book" button, the car is reserved and customer can proceed to payment or pay later. Booking data is avaible for updates before payment is done and when order is not in "cancelled" or "refunded" status. If customer changes his mind, he may cancel the order and selected car is released. 

Payment system is using Stripe only for now but thanks to strategy pattern used when implementing payment service, it will be easy to add new options in the future.

Below u can find database design. All of the tables were implemented in Entity Framework with code-first aproach. Data related services are in Infrastructure layer. I have implemented unit of work and repository patterns for easier management and cleaner code. 

![alt text](https://github.com/Hubertgitck/RentalStore/blob/release/database.png?raw=true)


Domain layer is based on services for each area. 

I provided datbase intializer to make testing easier. Application is started with 4 tesla models added, 4 rental stores but only for 2 of them there are selected cars!! If you want to check the rest, you will have to do it manually through management panel. I also provided 2 test users.

If you want to test the admin panel, use the following data: 
admin@rental.com
Admin123*

For the customer stuff:
customer@domain.com
Test123*
