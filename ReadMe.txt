01) Install EF Power Tools on VS 2015, you'll need this to check to see if EF will generate the
desired DB schema from your classes. After installation, you can right-click on your context class (NinjaContext),
go to Entity Framework > View Entity Data Model (Read-only). You must set the project where your context classes
reside as the startup project.
http://thedatafarm.com/data-access/installing-ef-power-tools-into-vs2015/

02) FK Properties make working in disconnected apps much easier!

03) Creating databases from your EF Model using Code First Migrations

04) Database Migration evolving the database schema as the model changes. EF has API to help you with this

05) Go to Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
	a) change "Default project" to NinjaDomain.DataModel
	b) type "enable-migrations" command into Package Manager Console

06) Code First Database Migrations is a 3 step process
	a) Define/Change Model
	b) Create a Migration File
	c) Apply Migration to Database

07) Here are example steps to add a migration
	a) Go to PMC and type "add-migration [migration name]" command, e.g., "add-migration Initial"
	b) Go to PMC and type "update-database" (runs against DB) 
	   or "update-database -verbose" (runs against DB and prints what's happening behind the scenes)
	   or "update-database -script" (generates scripts), this will look for latest migrations file

08) Here is an example of modifying your model to add a birthdate
	a) Modify your model by adding birthdate
	b) Go To PMC and type "add-migration [migration name]" command, e.g., "add-migration AddBirthdate"
	c) Go To PMC and type "update-database" command, use any of the flags desired

09) Here are the steps to create your models DB first with EDMX
	a) Create a new class library project
	b) Add ADO.NET Entity Data Model to your project
	c) Select EF Designer from database option and follow steps

10) Here are the steps to create your models DB first
	a) Create a new class library project
	b) Add ADO.NET Entity Data Model to your project
	c) Select Code First from database option and follow steps

11) For performance reasons, in disconnect apps like mvc, use the asnotracking method to save ef from tracking entities

12) To use DB scripts to deploy, use the command Update-Database -Script -SourceMigration: $InitialDatabase 
	This will roll all the way back to an empty database, creates SQL script starting from initial database
	Or you can use Update-Database -Script -SourceMigration: $InitialDatabase -TargetMigration: AddPostAbstract to specify source and target
	This will create a SQL script starting from source migration snapshot to target migration