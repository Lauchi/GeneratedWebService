if (Test-Path Host/Eventstore.db) {
	Remove-Item -Force GeneratedWebService/Eventstore.db
}
cd SqlAdapter/
dotnet ef migrations remove -s ../GeneratedWebService/
dotnet ef migrations add InitialMigration -s ../GeneratedWebService/
dotnet ef database update -s ../GeneratedWebService/
cd ..