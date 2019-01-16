# My_Repo
Download This Zip 
Go to Ajinkya_Repository\Controllers\HomeController.cs

If uses Local Server then Go to email and click on less security so it will help you to send mail to user 

After opening of HomeController.cs go to action SendVerificationLinkEmail and insert your gmail password here var pass = "YourPassword";
then enter your email address here var fromEmail = new MailAddress("YourMailID@gmail.com", "Verification Mail");

go and run your sql server management studio
get script.sql file from downloaded zip it will help you to create database

then go to project folder and see Web.config file in that file change your sql server name it will help you to connect your database with this project

<add name="Ajinkya_RepositoryEntities" connectionString="metadata=res://*/Models.Ajinkya_RepositoryModel.csdl|res://*/Models.Ajinkya_RepositoryModel.ssdl|res://*/Models.Ajinkya_RepositoryModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=LENOVO-PC;initial catalog=Ajinkya_Repository;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />

Now configuration is completed just run this project.

Thank You.
